using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

namespace GOAP
{
    public class Planner
    { 
           // Junta e sobrescreve propriedades.
            public static State Next(State original, Action action)
            {
                // Cria uma nova lista com os valores atuais do estado original
                var resultProperties = new List<(string, object)>(original.GetState());
                var resultState = new State(resultProperties);

                // Para cada efeito da a��o, atualiza ou insere o valor correspondente
                foreach (var effect in action.GetEffects())
                {
                    bool found = false;

                    // Procura na lista para ver se a chave j� existe
                    for (int i = 0; i < resultProperties.Count; i++)
                    {
                        if (resultProperties[i].Item1 == effect.Item1)
                        {
                            // Atualiza o valor se a chave j� existir
                            resultProperties[i] = (effect.Item1, effect.Item2);
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        resultState.Set(effect.Item1, effect.Item2);
                    }
                }

                return resultState;
            }

        /**
            * Retorna o estado que, se uma dada a��o for executada, leva ao estado desejado.
         */
        public static State Previous(Action action, State target)
        {
            // Cria uma nova lista baseada no estado atual
            var resultProperties = new List<(string, object)>(target.GetState());
            var resultState = new State(resultProperties);

            // Atualiza ou insere as pr�-condi��es no estado resultante
            foreach (var precond in action.GetPreconds())
            {
                bool found = false;

                // Procura na lista para ver se a chave j� existe
                for (int i = 0; i < resultProperties.Count; i++)
                {
                    if (resultProperties[i].Item1 == precond.Item1)
                    {
                        // Atualiza o valor se a chave j� existir
                        resultProperties[i] = (precond.Item1, precond.Item2);
                        found = true;
                        break;
                    }
                }

                // Se a chave n�o existir, adiciona como um novo item
                if (!found)
                {
                    resultState.Set(precond.Item1, precond.Item2);
                }
            }

            return resultState;
        }


        /**
         * Verifica se as a��es de um plano transforma um estado inicial (current) em um estado
        * objetivo (goal). A verifica��o � feita a partir das precondi��es de cada a��o e os
        * seus efeitos (transforma��es no estado).
        */
        public static bool IsValidPlan(State initial, State goal, List<Action> plan)
            {
                var result = plan.Aggregate(initial, (state, action) =>
                {
                    return Match(action.Precond, state) ? Next(state, action) : null;
                });
                return Match(goal, result);
            }

        private static bool Match(State a, State b)
        {
            // Itera por todas as propriedades do estado `a`
            foreach (var (key, value) in a.GetState())
            {
                // Tenta obter o valor correspondente no estado `b`
                var bValue = b.Get(key);

                // Se a chave existe em `b`, verifica se os valores s�o iguais
                if (bValue != null && !Equals(bValue, value))
                {
                    return false; // Se os valores n�o coincidem, retorna falso
                }
            }
            // Se todas as condi��es s�o atendidas, retorna verdadeiro
            return true;
        }


        private static bool PartialMatch(State a, State b)
        {
            // Itera sobre todas as chaves e valores de `a`
            foreach (var (key, value) in a.GetState())
            {
                // Verifica se `b` cont�m a chave e se os valores s�o iguais
                var bValue = b.Get(key);
                if (bValue != null && Equals(bValue, value))
                {
                    return true; // Se houver pelo menos um par chave-valor igual, retorna true
                }
            }
            return false; // Caso contr�rio, retorna false
        }


        private static int Distance(State a, State b)
        {
            // Cria uma lista com as chaves comuns entre `a` e `b`
            var commonKeys = new List<string>();
            foreach (var (key, _) in a.GetState())
            {
                if (b.Get(key) != null) // Adiciona a chave se existir em `b`
                {
                    commonKeys.Add(key);
                }
            }

            // Conta quantas chaves t�m valores iguais em `a` e `b`
            var equalCount = commonKeys.Count(key => Equals(a.Get(key), b.Get(key)));

            // Calcula a dist�ncia como o n�mero m�nimo de chaves nos dois estados menos o n�mero de chaves iguais
            var minKeyCount = System.Math.Min(a.GetState().Count, b.GetState().Count);
            return minKeyCount - equalCount;
        }


        /** N� do grafo utilizado pelo A*. Encapsula um estado. */
        private class Node
            {
                public State State { get; }
                public int GCost { get; set; }
                public int HCost { get; set; }
                public Edge From { get; set; }

                public Node(State state, int gCost = int.MaxValue, int hCost = int.MaxValue)
                {
                    State = state;
                    GCost = gCost;
                    HCost = hCost;
                }
            }

        /** Aresta dirigida do grafo utilizado pelo A*. */
        private class Edge
            {
                public Action Action { get; }
                public Node Origin { get; }
                public Node Target { get; }

                public Edge(Action action, Node origin, Node target)
                {
                    Action = action;
                    Origin = origin;
                    Target = target;
                }
            }

        /**
        * Cria uma sequ�ncia de a��es voltando no caminho do A*, do n� atual at� o inicial.
        */
        private static List<Action> CreatePath(Node node)
            {
                var path = new List<Action>();
                while (node.From != null)
                {
                    path.Add(node.From.Action);
                    node = node.From.Origin;
                }
                return path;
            }

        /**
        * Cria um plano (sequ�ncia de a��es) para levar um estado inicial a um estado final
        * a partir de um conjunto de a��es poss�veis.
        * @param initial Estado inicial.
        * @param goal    Estado final
        * @param actions Conjunto de a��es poss�veis.
        * @returns       Sequ�ncia de a��es para levar `initial` a `goal`.
        */
        public static List<Action> DefinePlan(State initial, State goal, List<Action> actions)
            {
                int FCost(Node node) => node.GCost + node.HCost;
                Node PriorFunc(Node a, Node b) => FCost(a) < FCost(b) ? a : b;

                var queue = new PQueue<Node>(PriorFunc);
                var closed = new List<Node>();

                queue.Push(new Node(goal, 0, Distance(goal, initial)));

                while (!queue.IsEmpty())
                {
                    var currNode = queue.Pop();
                    if (Match(currNode.State, initial))
                    {
                        return CreatePath(currNode);
                    }

                    closed.Add(currNode);

                    foreach (var action in actions.Where(action => PartialMatch(action.Effects, currNode.State)))
                    {
                        var prevState = Previous(action, currNode.State);
                        if (closed.All(n => !Match(n.State, prevState)))
                        {
                            var prevNode = queue.Find(node => Match(node.State, prevState)) ?? new Node(prevState); //ERROR
                            if (!queue.Contains(prevNode)) queue.Push(prevNode);

                            var gCost = currNode.GCost + action.Cost;
                            var hCost = Distance(prevState, initial);

                            if (gCost + hCost < FCost(prevNode))
                            {
                                prevNode.GCost = gCost;
                                prevNode.HCost = hCost;
                                prevNode.From = new Edge(action, currNode, prevNode);
                            }
                        }
                    }
                }

                return new List<Action>();
            }

        }
}

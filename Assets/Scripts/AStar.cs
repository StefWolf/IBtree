using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar: MonoBehaviour
{
    public class Node
    {
        public Vector2 position;
        public float gCost;  // Custo de g
        public float hCost;  // Custo de h (heur�stica)
        public float fCost => gCost + hCost;  // Fun��o de avalia��o f(n) = g(n) + h(n)
        public Node parent;
        public bool isBlocked;

        public Node(Vector2 position, bool isBlocked)
        {
            this.position = position;
            gCost = Mathf.Infinity;
            hCost = Mathf.Infinity;
            parent = null;
            this.isBlocked = isBlocked;
        }
    }

    public Vector2Int start = new Vector2Int(0, 0);  // Ponto de in�cio
    public Vector2Int target = new Vector2Int(5, 5); // Ponto de destino

    private List<Vector2> GetNeighbours(Vector2 nodePosition)
    {
        // Retorna os pontos vizinhos
        return FindFirstObjectByType<NPCController>().FindNeighboursPoints(nodePosition);
    }

    private float CalculateManhattanDistance(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    public List<Vector2> AStarAlgorithm(Vector2 start, Vector2 target)
    {
        // Lista de n�s abertos (a serem avaliados)
        List<Node> openList = new List<Node>();
        
        // Lista de n�s fechados (j� avaliados)
        HashSet<Node> closedList = new HashSet<Node>();

        Node startNode = new Node(start, false);
        startNode.gCost = 0;
        startNode.hCost = CalculateManhattanDistance(start, target); // calcula o custo do n� inicial at� o objetivo

        openList.Add(startNode); // adiciona o primeiro n� � lista aberta

        while (openList.Count > 0) // enquanto a lista n�o for vazia
        {
            // Ordenar pelo menor fCost
            openList.Sort((a, b) => a.fCost.CompareTo(b.fCost));

            Node currentNode = openList[0]; // pega o n� de menor custo 

            // Se atingirmos o alvo, podemos reconstruir o caminho
            if (currentNode.position == target)
            {
                List<Vector2> path = new List<Vector2>();
                while (currentNode != null)
                {
                    path.Add(currentNode.position);
                    currentNode = currentNode.parent;
                }

                path.Reverse();
                return path;
            }

            openList.Remove(currentNode); // remove o n� da lista j� que ele n�o � o alvo ainda 
            closedList.Add(currentNode); // adiciona ele aos n�s j� avaliados 

            // Verifica os vizinhos
            foreach (Vector2 neighbourPos in GetNeighbours(currentNode.position))
            {
                // add verifica��o para ver se o vizinho � uma parede ou n�o (a fazer ainda)
                Node neighbourNode = new Node(neighbourPos, false);

                if (closedList.Contains(neighbourNode))
                    continue;

                float tentativeGCost = currentNode.gCost + 1;  // Suponha que o custo entre n�s vizinhos � 1 (movimento)
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateManhattanDistance(neighbourPos, target);
                    neighbourNode.parent = currentNode;

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // Se n�o encontrar um caminho
        return null;
    }
}

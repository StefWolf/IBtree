using System;
using System.Collections.Generic;
using System.Linq;

public static class StateController
{
    // Define State como uma lista de propriedades
    public static bool Match(List<(string, object)> a, List<(string, object)> b)
    {
        // Verifica se todas as chaves em 'a' casam com os valores em 'b'
        return a.All(pairA =>
            !b.Any(pairB => pairB.Item1 == pairA.Item1) || // Não existe no estado 'b'
            b.Any(pairB => pairB.Item1 == pairA.Item1 && Equals(pairA.Item2, pairB.Item2)) // Os valores coincidem
        );
    }

    public static bool PartialMatch(List<(string, object)> a, List<(string, object)> b)
    {
        // Verifica se pelo menos uma propriedade em 'a' possui o mesmo valor em 'b'
        return b.Any(pairB => a.Any(pairA => pairA.Item1 == pairB.Item1 && Equals(pairA.Item2, pairB.Item2)));
    }

    public static bool IsEqual(List<(string, object)> a, List<(string, object)> b)
    {
        // Verifica se possuem o mesmo número de propriedades
        if (a.Count != b.Count) return false;

        // Verifica se todos os pares (chave, valor) em 'a' existem e são iguais em 'b'
        return a.All(pairA => b.Any(pairB => pairA.Item1 == pairB.Item1 && Equals(pairA.Item2, pairB.Item2)));
    }

    public static int Distance(List<(string, object)> s1, List<(string, object)> s2)
    {
        // Chaves comuns
        var commonKeys = s1.Select(pair => pair.Item1).Intersect(s2.Select(pair => pair.Item1)).ToList();

        // Quantidade de chaves com valores iguais
        var equalCount = commonKeys.Count(key =>
            s1.Any(pair => pair.Item1 == key && Equals(pair.Item2, s2.First(pair2 => pair2.Item1 == key).Item2))
        );

        // Calcula a distância como o número total de propriedades menos as iguais
        var minKeyCount = Math.Min(s1.Count, s2.Count);
        return minKeyCount - equalCount;
    }
}

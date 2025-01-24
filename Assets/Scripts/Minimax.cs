using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimax : MonoBehaviour
{
    public float distanciaPerseguir = 5f;
    public float distanciaPatrulhar = 10f;

    public NPCController.State MinimaxExecute()
    {
        int melhorAção = MinimaxAlgorithm();
        return ExecutarAção(melhorAção);
    }

    // Função Minimax para decidir a melhor ação
    private int MinimaxAlgorithm()
    {
        // Definir os estados possíveis e suas avaliações
        // 0 = Patrulhando, 1 = Perseguindo, 2 = Atacando e 3 = Retornando

        int pontuacaoPatrulhando = AvaliarEstado(GetComponent<NPCController>().StatePatrulhando());
        int pontuacaoPerseguindo = AvaliarEstado(GetComponent<NPCController>().StatePerseguindo());
        int pontuacaoRetornando = AvaliarEstado(GetComponent<NPCController>().StateRetornando());
        int pontuaçãoAtacando = AvaliarEstado(GetComponent<NPCController>().StateAtacando());

        // Decidir qual ação tem a melhor pontuação
        if (pontuacaoPerseguindo >= pontuacaoPatrulhando && pontuacaoPerseguindo >= pontuacaoRetornando && pontuacaoPerseguindo >= pontuaçãoAtacando)
            return 1; // Perseguir
        else if (pontuacaoRetornando >= pontuacaoPatrulhando && pontuacaoRetornando >= pontuacaoPerseguindo && pontuacaoRetornando >= pontuaçãoAtacando)
            return 3; // Retornar
        else if(pontuaçãoAtacando >= pontuacaoPatrulhando && pontuaçãoAtacando >= pontuacaoPerseguindo && pontuaçãoAtacando >= pontuacaoRetornando)
            return 2; // atacar
        else
            return 0; // Patrulhar
    }

    // Função para avaliar o estado com base em algumas condições
    private int AvaliarEstado(NPCController.State estado)
    {
        switch (estado)
        {
            case NPCController.State.Patrulhando:
                if (GetComponent<NPCController>().life >= 50)
                    return 4; // player proximo a você, mas você tá com a vida boa. Patrulhar pode ser perigoso mas sem risco de morte alto ainda
                else
                    return 0; // player está proximo a você e você está com a vida baixa, não é uma boa patrulhar, alto risco de morte
            case NPCController.State.Perseguindo:
                if (GetComponent<NPCController>().life > 60 && GetComponent<NPCController>().player.GetComponent<PlayerController>().life < 30)
                    return 10; // player proximo a você, você tá com a vida boa e o player com a vida baixa. Boa ideia persegui-lo
                else if (GetComponent<NPCController>().life == GetComponent<NPCController>().player.GetComponent<PlayerController>().life) // equilibrado
                    return 5;
                else if (GetComponent<NPCController>().life < 30 && GetComponent<NPCController>().player.GetComponent<PlayerController>().life > 60)
                    return 0; // player com a vida alta e você com a vida baixa. Não é uma boa persegui-lo agora
                else
                    return 0;
            case NPCController.State.Retornando:
                if (GetComponent<NPCController>().life < 30) // vida muito baixa, melhor voltar para se recuperar
                    return 10;
                else if (GetComponent<NPCController>().life > 30 && GetComponent<NPCController>().life <= 50) // pode ser uma boa voltar para se recuperar, mas não é urgente
                    return 5;
                else // > 50
                    return 0; // a vida tá boa, não tem por que retornar
            case NPCController.State.Atacando:
                if (GetComponent<NPCController>().life > 50)
                    return 10; // player está perto e sua vida tá boa, é uma boa atacar
                else
                    return 5; // player está perto e sua vida está na metade, pode ser arriscado atacar, mas o risco de morte ainda não é alto
            default:
                return 0;
        }
    }

    // Função para executar a ação escolhida pelo Minimax
    private NPCController.State ExecutarAção(int acao)
    {
        switch (acao)
        {
            case 0:
                return NPCController.State.Patrulhando;
            case 1:
                return NPCController.State.Perseguindo;
            case 2:
                return NPCController.State.Atacando;
            case 3:
                return NPCController.State.Retornando;
        }
        return NPCController.State.Patrulhando;
    }
}

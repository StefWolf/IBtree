using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimax : MonoBehaviour
{
    public float distanciaPerseguir = 5f;
    public float distanciaPatrulhar = 10f;

    public NPCController.State MinimaxExecute()
    {
        int melhorA��o = MinimaxAlgorithm();
        return ExecutarA��o(melhorA��o);
    }

    // Fun��o Minimax para decidir a melhor a��o
    private int MinimaxAlgorithm()
    {
        // Definir os estados poss�veis e suas avalia��es
        // 0 = Patrulhando, 1 = Perseguindo, 2 = Atacando e 3 = Retornando

        int pontuacaoPatrulhando = AvaliarEstado(GetComponent<NPCController>().StatePatrulhando());
        int pontuacaoPerseguindo = AvaliarEstado(GetComponent<NPCController>().StatePerseguindo());
        int pontuacaoRetornando = AvaliarEstado(GetComponent<NPCController>().StateRetornando());
        int pontua��oAtacando = AvaliarEstado(GetComponent<NPCController>().StateAtacando());

        // Decidir qual a��o tem a melhor pontua��o
        if (pontuacaoPerseguindo >= pontuacaoPatrulhando && pontuacaoPerseguindo >= pontuacaoRetornando && pontuacaoPerseguindo >= pontua��oAtacando)
            return 1; // Perseguir
        else if (pontuacaoRetornando >= pontuacaoPatrulhando && pontuacaoRetornando >= pontuacaoPerseguindo && pontuacaoRetornando >= pontua��oAtacando)
            return 3; // Retornar
        else if(pontua��oAtacando >= pontuacaoPatrulhando && pontua��oAtacando >= pontuacaoPerseguindo && pontua��oAtacando >= pontuacaoRetornando)
            return 2; // atacar
        else
            return 0; // Patrulhar
    }

    // Fun��o para avaliar o estado com base em algumas condi��es
    private int AvaliarEstado(NPCController.State estado)
    {
        switch (estado)
        {
            case NPCController.State.Patrulhando:
                if (GetComponent<NPCController>().life >= 50)
                    return 4; // player proximo a voc�, mas voc� t� com a vida boa. Patrulhar pode ser perigoso mas sem risco de morte alto ainda
                else
                    return 0; // player est� proximo a voc� e voc� est� com a vida baixa, n�o � uma boa patrulhar, alto risco de morte
            case NPCController.State.Perseguindo:
                if (GetComponent<NPCController>().life > 60 && GetComponent<NPCController>().player.GetComponent<PlayerController>().life < 30)
                    return 10; // player proximo a voc�, voc� t� com a vida boa e o player com a vida baixa. Boa ideia persegui-lo
                else if (GetComponent<NPCController>().life == GetComponent<NPCController>().player.GetComponent<PlayerController>().life) // equilibrado
                    return 5;
                else if (GetComponent<NPCController>().life < 30 && GetComponent<NPCController>().player.GetComponent<PlayerController>().life > 60)
                    return 0; // player com a vida alta e voc� com a vida baixa. N�o � uma boa persegui-lo agora
                else
                    return 0;
            case NPCController.State.Retornando:
                if (GetComponent<NPCController>().life < 30) // vida muito baixa, melhor voltar para se recuperar
                    return 10;
                else if (GetComponent<NPCController>().life > 30 && GetComponent<NPCController>().life <= 50) // pode ser uma boa voltar para se recuperar, mas n�o � urgente
                    return 5;
                else // > 50
                    return 0; // a vida t� boa, n�o tem por que retornar
            case NPCController.State.Atacando:
                if (GetComponent<NPCController>().life > 50)
                    return 10; // player est� perto e sua vida t� boa, � uma boa atacar
                else
                    return 5; // player est� perto e sua vida est� na metade, pode ser arriscado atacar, mas o risco de morte ainda n�o � alto
            default:
                return 0;
        }
    }

    // Fun��o para executar a a��o escolhida pelo Minimax
    private NPCController.State ExecutarA��o(int acao)
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

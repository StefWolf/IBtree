using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimax : NPCController
{
    //public Transform[] pontosDePatrulha; // Lista de pontos para patrulha
    private int pontoAtualIndex = 0;
    public Transform player;
    //public float alcanceVisao = 10f;
    //public float velocidade = 3f;

    public float distanciaPerseguir = 6f;
    public float distanciaPatrulhar = 12f;

    //private State estadoAtual = State.Patrulhando;

    void Update()
    {
        int melhorAção = MinimaxAlgorithm();
        ExecutarAção(melhorAção);
    }

    // Função Minimax para decidir a melhor ação
    private int MinimaxAlgorithm()
    {
        // Definir os estados possíveis e suas avaliações
        // 0 = Patrulhando, 1 = Perseguindo, 2 = Retornando

        int pontuacaoPatrulhando = AvaliarEstado(State.Patrulhando);
        int pontuacaoPerseguindo = AvaliarEstado(State.Perseguindo);
        int pontuacaoRetornando = AvaliarEstado(State.Retornando);

        // Decidir qual ação tem a melhor pontuação
        if (pontuacaoPerseguindo >= pontuacaoPatrulhando && pontuacaoPerseguindo >= pontuacaoRetornando)
            return 1; // Perseguir
        else if (pontuacaoRetornando >= pontuacaoPatrulhando && pontuacaoRetornando >= pontuacaoPerseguindo)
            return 2; // Retornar
        else
            return 0; // Patrulhar
    }

    // Função para avaliar o estado com base em algumas condições
    private int AvaliarEstado(State estado)
    {
        switch (estado)
        {
            case State.Patrulhando:
                if (Vector2.Distance(transform.position, player.position) >= distanciaPatrulhar)
                    return 10; // player está longe, pode patrulhar em segurança
                else if (Vector2.Distance(transform.position, player.position) <= distanciaPerseguir && life >= 70)
                    return 5; // player proximo a você e você tá com a vida boa, patrulhar pode ser perigoso
                else
                    return 0; // player está proximo a você e você está com a vida baixa, não é uma boa patrulhar
            case State.Perseguindo:
                if (life > 60 && player.GetComponent<PlayerController>().life < 30) // vida boa, player com vida baixa
                    return 10;
                else if (life == player.GetComponent<PlayerController>().life) // equilibrado
                    return 5;
                else if (life < 30 && player.GetComponent<PlayerController>().life > 70) // player com vida boa, você com vida muito baixa
                    return 0;
                else
                    return 0;
            case State.Retornando:
                if (life <= 30) // vida muito baixa, melhor voltar para se recuperar
                    return 10;
                else if (life > 30 && life <= 50) // pode ser uma boa voltar para se recuperar
                    return 5;
                else // > 50
                    return 0; // a vida tá boa, não tem por que retornar
            default:
                return 0;
        }
    }

    // Função para executar a ação escolhida pelo Minimax
    private void ExecutarAção(int acao)
    {
        switch (acao)
        {
            case 0:
                Patrulhar();
                break;
            case 1:
                PerseguirJogador();
                break;
            case 2:
                RetornarAOPontoDePatrulha();
                break;
        }
    }
}

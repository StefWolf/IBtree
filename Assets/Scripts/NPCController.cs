using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public enum State
    {
        Patrulhando,
        Perseguindo,
        Retornando
    }

    public Transform[] pontosDePatrulha; // Lista de pontos para patrulha
    private int targetIndex = 0; // Índice do alvo atual
    public Transform player; // Referência ao jogador

    public float alcanceVisao = 10f; // Distância máxima para perceber o jogador
    public float velocidade = 3f; // Velocidade de movimento

    public State estadoAtual = State.Patrulhando;
    public int life = 100;
    private float menorDistancia = 99999f;
    private int pontoAtualIndex = 0; // Índice do ponto de patrulha atual

    public List<Vector2> FindNeighboursPoints(Vector2 currentPoint)
    {
        List<Vector2> neighbours = new List<Vector2>();
        foreach(Transform point in pontosDePatrulha)
        {
            if((Vector2)point.position != currentPoint)
                neighbours.Add(point.position);
        }
        return neighbours;
    }
    public void Patrulhar()
    {
        for(int ii=0; ii < pontosDePatrulha.Length; ii++)
        {
            if (Vector2.Distance(pontosDePatrulha[ii].position, player.position) < menorDistancia)
            {
                menorDistancia = Vector2.Distance(pontosDePatrulha[ii].position, player.position);
                targetIndex = ii;
            }
        }
        // envia o ponto de patrulha mais perto do player para ser o objetivo do NPC, retornando a lista de pontos até esse ponto
        List<Vector2> path = FindFirstObjectByType<AStar>().AStarAlgorithm(transform.position, pontosDePatrulha[targetIndex].position);
        
        if (Vector3.Distance(transform.position, path[pontoAtualIndex]) < 0.5f) // caso tenha chegado no ponto de patrulha, vai para outro
            pontoAtualIndex++;
        else
            MoverEmDirecao(path[pontoAtualIndex]); // move-se em direção ao ponto de patrulha atual

        // muda o estado para perseguir caso o player esteja no raio de visão
        if (Vector3.Distance(transform.position, player.position) <= alcanceVisao)
        {
            estadoAtual = State.Perseguindo;
        }
    }

    public void PerseguirJogador()
    {
        MoverEmDirecao(player.position);

        if (Vector3.Distance(transform.position, player.position) > alcanceVisao)
        {
            estadoAtual = State.Retornando;
        }
    }

    public void RetornarAOPontoDePatrulha()
    {
        MoverEmDirecao(pontosDePatrulha[pontoAtualIndex].position);

        if (Vector3.Distance(transform.position, pontosDePatrulha[pontoAtualIndex].position) < 0.5f)
        {
            estadoAtual = State.Patrulhando;
        }
    }

    public void MoverEmDirecao(Vector3 destino)
    {
        Vector3 direcao = (destino - transform.position).normalized;
        transform.position += direcao * velocidade * Time.deltaTime;
    }
}

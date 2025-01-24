using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPCController : MonoBehaviour
{
    public enum State
    {
        Patrulhando,
        Perseguindo,
        Atacando,
        Retornando
    }

    public Transform[] patrolPoints; // Lista de pontos para patrulha
    public Transform player; // Referência ao jogador

    public float alcanceVisao = 6f; // Distância máxima para perceber o jogador
    public float velocidade = 5f; // Velocidade de movimento
    public LayerMask playerLayer;  // Camada onde o jogador est�
    public float visionAngle = 45f;  // �ngulo do cone

    public float maxAcceleration = 10f;
    public float mass = 1f;
    public float stopDistance = 1f;
    private Vector3 velocity;

    public State estadoAtual = State.Patrulhando;
    public int life = 100;
    private int currentPointIndex = 0; // Índice do ponto de patrulha atual


    public State StatePatrulhando()
    {
        return State.Patrulhando;
    }

    public State StatePerseguindo()
    {
        return State.Perseguindo;
    }

    public State StateRetornando()
    {
        return State.Retornando;
    }
    public State StateAtacando()
    {
        return State.Atacando;
    }


    private void Update()
    {
        State bestMove = GetComponent<Minimax>().MinimaxExecute();
        estadoAtual = bestMove;

        switch (estadoAtual)
        {
            case State.Patrulhando:
                Patrulhar();
                break;
            case State.Perseguindo:
                PerseguirPlayer();
                break;
            case State.Atacando:
                AtacarPlayer();
                break;
            case State.Retornando:
                RetornarAOPontoDePatrulha();
                break;
            default: 
                break;
        }
    }

    public void Patrulhar()
    {
        if (Vector3.Distance(transform.position, patrolPoints[currentPointIndex].position) < 1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            Seek(patrolPoints[currentPointIndex].position);
        }
        else
            Seek(patrolPoints[currentPointIndex].position);
    }

    public void PerseguirPlayer()
    {
        Seek(player.position);
    }

    public void AtacarPlayer()
    {
        player.GetComponent<PlayerController>().life -= 5;
    }

    public void RetornarAOPontoDePatrulha()
    {
        Seek(patrolPoints[currentPointIndex].position);
    }

    private void Seek(Vector3 target)
    {
        if (target == null)
            return;

        Vector3 desiredVelocity = (target - transform.position).normalized * velocidade;

        Vector3 steering = (desiredVelocity - velocity) / mass;

        steering = Vector3.ClampMagnitude(steering, maxAcceleration);

        velocity += steering * Time.deltaTime;

        velocity = Vector3.ClampMagnitude(velocity, velocidade);

        transform.position += velocity * Time.deltaTime;

        if (Vector3.Distance(transform.position, target) > stopDistance)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

}

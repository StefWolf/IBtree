using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade de movimento
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        // Obt�m o componente Rigidbody2D do jogador
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Captura as entradas do teclado (AWSD ou setas direcionais)
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D ou Setas Esquerda/Direita
        movement.y = Input.GetAxisRaw("Vertical");   // W/S ou Setas Cima/Baixo

        // Normaliza o vetor de movimento para evitar velocidade maior ao andar em diagonal
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // Move o jogador usando o Rigidbody2D
        rb.velocity = movement * moveSpeed;
    }
}

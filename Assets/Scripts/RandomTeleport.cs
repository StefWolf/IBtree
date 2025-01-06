using Unity.VisualScripting;
using UnityEngine;

public class RandomTeleport : MonoBehaviour
{
    // Limites da região
    public float xMin = -10f;
    public float xMax = 10f;
    public float yMin = -10f;
    public float yMax = 10f;

    // Método para teletransportar o objeto
    void Teleport()
    {
        float randomX = Random.Range(xMin, xMax);
        float randomY = Random.Range(yMin, yMax);

        transform.position = new Vector3(randomX, randomY);
        Debug.Log($"Teletransportado para: {transform.position}");
    }

    void OnCollisionEnter2D(Collision2D col) {

        Debug.Log("Achou o ponto");

        if(col.gameObject.CompareTag("NPCWander")){
            Teleport();
        }
    }
}

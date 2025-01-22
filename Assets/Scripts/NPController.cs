using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class NPController : MonoBehaviour
{
    public Transform topPoint;
    public Transform bottomPoint;
    public float speed = 2f; // Velocidade do inimigo

    // Configura��es do cone de vis�o
    public float visionDistance = 5f; // Alcance do cone
    public float visionAngle = 45f;  // �ngulo do cone
    public LayerMask playerLayer;  // Camada onde o jogador est�
    public MeshFilter visionMeshFilter;

    private Mesh visionMesh;
    private Vector3 targetPosition; // Pr�ximo ponto para o inimigo ir

    private Sequence vigiarZona;

    private Sequence perseguirPlayer;

    public NPC npc = new NPC();

    void Start()
    {
        GerarArvoreDeComportamento();
        targetPosition = topPoint.position;

        visionMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = visionMesh;
        DrawVisionCone();
    }

    void Update()
    {
        if (vigiarZona.Execute(npc) == TaskStatus.SUCCESS)
        {
            if (perseguirPlayer.Execute(npc) == TaskStatus.SUCCESS)
            {
                FindFirstObjectByType<GameController>().GameOver();
            }
        }
    }

    void GerarArvoreDeComportamento()
    {
        Action leaf1 = new Action(MoverNoLimiteDaZona);
        Condition leaf2 = new Condition(EstaNoPontoDeVisao);
        Action leaf3 = new Action(LocalizarPlayer);

        Action leaf4 = new Action(MoverAteOPlayer);
        Condition leaf5 = new Condition(ChegouNoPlayer);
        Action leaf6 = new Action(CapturarPlayer);

        List<Task> listSequence1 = new List<Task>();
        List<Task> listSequence2 = new List<Task>();

        listSequence1.Add(leaf1);
        listSequence1.Add(leaf2);
        listSequence1.Add(leaf3);
        listSequence2.Add(leaf4);
        listSequence2.Add(leaf5);
        listSequence2.Add(leaf6);


        vigiarZona = new Sequence(listSequence1);
        perseguirPlayer = new Sequence(listSequence2);
    }

    TaskStatus MoverNoLimiteDaZona(NPC npc)
    {
        Debug.Log("Movendo no limite da zona...");
        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = targetPosition == topPoint.position ? bottomPoint.position : topPoint.position;

            if (transform.eulerAngles.z == 0) {
                transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        return TaskStatus.SUCCESS;
    }

    bool EstaNoPontoDeVisao(NPC npc)
    {
        Debug.Log("Verificando se est� no ponto de vis�o....");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, visionDistance, playerLayer);

        foreach (var hit in hits)
        {
            Vector2 directionToPlayer = (hit.transform.position - transform.position).normalized;

            float angleToPlayer = Vector2.Angle(transform.up, directionToPlayer);
            if (angleToPlayer < visionAngle / 2f)
            {
                return true;
            }
        }

        return false;
    }

    TaskStatus LocalizarPlayer(NPC npc)
    {
        Debug.Log("Localizando o player...");

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            float distancia = Vector3.Distance(transform.position, player.transform.position);
            return TaskStatus.SUCCESS;
        }
        Debug.LogWarning("Player n�o encontrado!");
        return TaskStatus.FAILURE;
    }

    TaskStatus MoverAteOPlayer(NPC npc)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 direcao = (player.transform.position - transform.position).normalized;
            transform.position += direcao * (speed * 4) * Time.deltaTime;
            return TaskStatus.SUCCESS;
        }

        return TaskStatus.FAILURE;
    }

    bool ChegouNoPlayer(NPC npc)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            float distancia = Vector3.Distance(transform.position, player.transform.position);
            if (distancia < 0.5f)
            {
                Debug.Log("Inimigo chegou no jogador!");
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    TaskStatus CapturarPlayer(NPC npc)
    {
        Debug.Log("Capturei ele!");
        return TaskStatus.SUCCESS;
    }

    void DrawVisionCone()
    {
        int rayCount = 50;
        float angleStep = visionAngle / rayCount;
        float startAngle = -visionAngle / 2f;
        float endAngle = visionAngle / 2f;

        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;

        Vector3 baseDirection = transform.up;

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = startAngle + angleStep * i;
            float radian = Mathf.Deg2Rad * angle;
   
            Vector3 direction = Quaternion.Euler(0, 0, angle) * baseDirection;
            vertices[i + 1] = direction * visionDistance;
        }

        for (int i = 0; i < rayCount; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        visionMesh.Clear();
        visionMesh.vertices = vertices;
        visionMesh.triangles = triangles;
        visionMesh.RecalculateNormals();
    }

}


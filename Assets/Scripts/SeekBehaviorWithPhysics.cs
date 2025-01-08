using UnityEngine;

public class SeekBehaviorWithPhysics : MonoBehaviour
{
    public Transform target;   
    public float maxSpeed = 5f;     
    public float maxAcceleration = 10f; 
    public float mass = 1f;           
    public float stopDistance = 1f;

    public float visionDistance = 5f; // Alcance do cone
    public float visionAngle = 45f;  // �ngulo do cone
    public LayerMask playerLayer;  // Camada onde o jogador est�
    public MeshFilter visionMeshFilter;

    private Mesh visionMesh;

    private Vector3 velocity;        

    void Start() {
        visionMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = visionMesh;
        DrawVisionCone();
    }

    void Update()
    {
        Seek();
        if(EstaNoPontoDeVisao()) {
            Debug.Log("Game Over");
            FindFirstObjectByType<GameController>().GameOver();
        }
    }

    private void Seek() {
        if (target == null)
            return;

        Vector3 desiredVelocity = (target.position - transform.position).normalized * maxSpeed;

        Vector3 steering = (desiredVelocity - velocity) / mass;

        steering = Vector3.ClampMagnitude(steering, maxAcceleration);

        velocity += steering * Time.deltaTime;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position += velocity * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) > stopDistance)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
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

    bool EstaNoPontoDeVisao()
    {
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
}


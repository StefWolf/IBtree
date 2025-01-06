using UnityEngine;

public class SeekBehaviorWithPhysics : MonoBehaviour
{
    public Transform target;   
    public float maxSpeed = 5f;     
    public float maxAcceleration = 10f; 
    public float mass = 1f;           
    public float stopDistance = 1f;

    private Vector3 velocity;        

    void Update()
    {
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
            transform.rotation = Quaternion.LookRotation(velocity.normalized);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}


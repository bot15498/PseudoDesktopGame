using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullIn : MonoBehaviour
{
    public Transform player; // The target (e.g., player)
    public float flightTime = 2f; // Time it takes to reach the player

    private Rigidbody rb;
    private AiBehaviorBase aiBehavior;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        aiBehavior = GetComponent<AiBehaviorBase>();
    }


    void Update()
    {

    }

    public void LaunchTowardsPlayer()
    {
        if (player == null || rb == null)
        {
            Debug.LogWarning("Player or Rigidbody is not assigned!");
            return;
        }

        // Get starting and target positions
        Vector3 startPos = transform.position;
        Vector3 endPos = player.position;

        // Calculate initial velocity
        Vector3 velocity = CalculateLaunchVelocity(startPos, endPos, flightTime);

        // Make the ai stop doing what it's doing for a bit
        if (aiBehavior != null)
        {
            aiBehavior.stunState = EnemyAiStunState.Stun;
            aiBehavior.agent.isStopped = true;
            rb.isKinematic = false;
            Vector3 force = CalculateLaunchForce(startPos, endPos, flightTime);
            Debug.Log(force);
            rb.AddForce(force, ForceMode.Impulse);
            StartCoroutine(aiBehavior.DelayStunStateChange(EnemyAiStunState.Normal, 0.2f));
        }
        else
        {
            rb.velocity = velocity;
            // Ensure gravity is enabled
            rb.useGravity = true;
        }
    }

    Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 target, float time)
    {
        // Displacement between the start and target
        Vector3 displacement = target - start;

        // Separate horizontal and vertical components
        Vector3 horizontalDisplacement = new Vector3(displacement.x, 0, displacement.z);
        float horizontalDistance = horizontalDisplacement.magnitude;

        float verticalDisplacement = displacement.y;

        // Calculate horizontal and vertical velocities
        float horizontalVelocity = horizontalDistance / time;
        float verticalVelocity = (verticalDisplacement / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

        // Combine into a single velocity vector
        Vector3 horizontalDirection = horizontalDisplacement.normalized;
        Vector3 velocity = horizontalDirection * horizontalVelocity;
        velocity.y = verticalVelocity;

        return velocity;
    }

    Vector3 CalculateLaunchForce(Vector3 start, Vector3 target, float time)
    {
        // Get velocity vector
        Vector3 targetVelocity = CalculateLaunchVelocity(start, target, time);

        // Modify based on masss
        return targetVelocity * rb.mass * 3f;
    }
}

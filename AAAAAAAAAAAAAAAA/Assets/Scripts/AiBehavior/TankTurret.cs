using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TankTurret : AiBehaviorBase
{
    public float timeToReachPlayer = 3f;
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPoint;
    private EnemyHealth enemyHealth;
    private bool curlock = false;

    new void Start()
    {
        // Make it so melee on this unit doesn't drop anything.
        enemyHealth = GetComponent<EnemyHealth>();
        enemyHealth.bypassMeleeExecution = true;
        base.Start();
    }

    new void Update()
    {
        base.Update();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsObjectInAttackRange()
    {
        // If you can see the player then fire away.
        return true;
    }

    public override void Attack()
    {
        if (!curlock)
        {
            StartCoroutine(DoAttack());
        }
    }


    private IEnumerator DoAttack()
    {
        curlock = true;
        for (int i = 0; i < 3; i++)
        {
            Vector3 vel = CalculateLaunchVelocity(bulletSpawnPoint.transform.position, player.transform.position, timeToReachPlayer);
            GameObject bul = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            bul.GetComponent<Rigidbody>().velocity = vel;
            yield return new WaitForSeconds(0.5f);
        }
        curlock = false;
        yield return null;
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
}

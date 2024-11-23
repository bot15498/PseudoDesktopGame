using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankVampire : AiBehaviorBase
{
    public float attackRange = 4f;
    public int attackDamage = 7;
    public float knockbackForce = 5f;
    public float turretAttackRate = 0.5f;
    public LayerMask attackMask;

    new void Start()
    {
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
        // For turret that is on the tank's back, see TankTurret.cs
        return CanSeePlayer() && DistanceToPlayer() <= attackRange;
    }

    public override void Attack()
    {
        // Detect enemies within the attack range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, attackMask);

        foreach (Collider player in hitColliders)
        {
            // Apply damage
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }

            // Apply knockback
            AiBehaviorBase aiBehavior = player.GetComponent<AiBehaviorBase>();
            Rigidbody rb = player.GetComponent<Rigidbody>();
            Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
            knockbackDirection.y = 0; // Prevent movement along the Y axis
            rb.velocity = Vector3.zero;
            if (aiBehavior != null)
            {
                aiBehavior.stunState = EnemyAiStunState.Stun;
                aiBehavior.agent.isStopped = true;
                rb.isKinematic = false;
                rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
                StartCoroutine(aiBehavior.DelayStunStateChange(EnemyAiStunState.Normal, 0.2f));
            }
            else if (rb != null)
            {
                rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
            }
        }
    }
}

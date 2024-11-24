using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeVampireAttackState
{
    Idle,
    MeleeAttack,
    StartTeleport,
    EndTeleport
}

public class MeleeVampire : AiBehaviorBase
{
    public float attackRange = 2f;
    public int attackDamage = 5;
    public float knockbackForce = 5f;
    public float teleportAttackChance = 0.15f;
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
        return CanSeePlayer() && DistanceToPlayer() <= attackRange;
    }

    public override void Attack()
    {
        if(Random.Range(0,1f) < teleportAttackChance)
        {
            // Do special teleport attack
        }
        else
        {
            // Detect enemies within the attack range
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, attackMask);

            foreach (Collider player in hitColliders)
            {
                if(player.gameObject == gameObject)
                {
                    // myself,
                    continue;
                }

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
}

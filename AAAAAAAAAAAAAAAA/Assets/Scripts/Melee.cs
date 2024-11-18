using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public float attackRange = 2f; // Range of the melee attack
    public int attackDamage = 10; // Damage dealt by the attack
    public float knockbackForce = 5f; // Force of the knockback
    public LayerMask enemyLayer; // Layer to identify enemies

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Left mouse button for attack
        {
            PerformMeleeAttack();
        }
    }

    void PerformMeleeAttack()
    {
        // Detect enemies within the attack range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            // Apply damage
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }

            // Apply knockback
            Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                Vector3 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the attack range in the Scene view for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

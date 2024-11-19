using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public float attackRange = 2f; // Range of the melee attack
    public int attackDamage = 10; // Damage dealt by the attack
    public float knockbackForce = 5f; // Force of the knockback
    public LayerMask enemyLayer; // Layer to identify enemies
    public Vector3 offset;
    public Transform playertransform;
    public Animator anim;
    public float meleecooldown;
    float timer;
    bool canmelee;


    private void Start()
    {
        canmelee = true;
        timer = 0;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canmelee == true) // Left mouse button for attack
        {

            PerformMeleeAttack();
            anim.Play("MeleeAttack",-1,0f);
            canmelee = false;
        }

        if (canmelee == false)
        {
            timer += Time.deltaTime;
        }
        if(timer >= meleecooldown)
        {
            canmelee = true;
            timer = 0;
        }

    }

    void PerformMeleeAttack()
    {
        // Detect enemies within the attack range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position , attackRange, enemyLayer);

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
                Vector3 knockbackDirection = (enemy.transform.position - playertransform.position).normalized;
                knockbackDirection.y = 0; // Prevent movement along the Y axis
                enemyRb.velocity = Vector3.zero;
                enemyRb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
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

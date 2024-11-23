using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float lifeTime = 2f;          // How long the projectile exists
    public float knockbackForce = 10f;  // Knockback force applied to enemies
    public int damage;          // Damage dealt to enemies

    // Define the three colors to choose from
    public Color color1 = Color.red;
    public Color color2 = Color.green;
    public Color color3 = Color.blue;
    private TrailRenderer trailRenderer;



    private void Start()
    {
        Destroy(gameObject, lifeTime); // Destroy projectile after lifetime

        // Get the TrailRenderer component attached to this GameObject
        trailRenderer = GetComponent<TrailRenderer>();

        if (trailRenderer == null)
        {
            Debug.LogError("TrailRenderer component not found on the GameObject.");
            return;
        }

        // Randomize the trail color at the start
        RandomizeTrailColor();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is an enemy
        
            // Apply knockback if the enemy has a Rigidbody
        

        Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
        if (enemyRb != null)
        {
            Vector3 knockbackDirection = (collision.transform.position - transform.position).normalized;
            knockbackDirection.y = 0; // Prevent movement along the Y axis
            enemyRb.velocity = Vector3.zero;
            enemyRb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
        }


        // Apply damage (assuming enemy has a health component)
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        

        // Destroy the projectile on collision
        Destroy(gameObject);
    }



    public void RandomizeTrailColor()
    {
        // Pick a random color from the three defined options
        Color[] colors = { color1, color2, color3 };
        Color randomColor = colors[Random.Range(0, colors.Length)];

        // Apply the chosen color to the TrailRenderer
        trailRenderer.startColor = randomColor; // Starting color of the trail
        trailRenderer.endColor = randomColor;   // Ending color of the trail
    }
}

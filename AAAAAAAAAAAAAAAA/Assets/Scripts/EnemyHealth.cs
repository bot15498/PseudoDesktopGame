using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHealth : MonoBehaviour
{

    public int healthDrops;
    public int AmmoDrops;
    public int AbilityDrops;

    public GameObject healthdrop;
    public GameObject ammodrop;
    public GameObject abilitydrop;

    public float dbnoTimer;
    bool dbno;
    public int maxHealth;
    [SerializeField]
    private int currentHealth;

    public float minForce = 1f;

    [Tooltip("The maximum force applied to each pickup.")]
    public float maxForce = 3f;

    [Tooltip("The upward bias for the force applied.")]
    public float upwardBias = 0.5f;
    private Collider thiscolluider;
    Rigidbody rb;
    private AiBehaviorBase aiBehavior;



    void Start()
    {
        dbno = false;
        currentHealth = maxHealth;
        thiscolluider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        aiBehavior = GetComponent<AiBehaviorBase>();
    }

    private void Update()
    {
        if (dbno == true)
        {
            // Waiting for melee execution to kill us.
            aiBehavior.agent.isStopped = true;
            rb.isKinematic = false;
        }
    }


    public void MeleeDamage(int damage)
    {
        if (damage >= currentHealth)
        {
            meleeExecution();
        }
        if (damage < currentHealth)
        {
            currentHealth -= damage;
        }
    }





    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        // Debug.Log($"Enemy took {damage} damage. Remaining health: {currentHealth}");
        if (currentHealth <= 0)
        {
            dbno = true;
            aiBehavior.stunState = EnemyAiStunState.Stagger;
        }

    }



    void Die()
    {
        //Debug.Log("Enemy died!");
        Destroy(gameObject);
    }



    void meleeExecution()
    {
        thiscolluider.enabled = false;
        rb.isKinematic = true;
        //do death animation and destroy object after spawning drops


        for (int i = 0; i < healthDrops; i++)
        {
            GameObject pickup = Instantiate(healthdrop, transform.position, Quaternion.identity);


            // Add a Rigidbody for physics
            Rigidbody rb = pickup.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = pickup.AddComponent<Rigidbody>();
            }

            Vector3 randomDirection = new Vector3(
              Random.Range(-1f, 1f),
              Random.Range(0.5f, 1f) * upwardBias, // Upward bias
              Random.Range(-1f, 1f)
          ).normalized;
            float forceMagnitude = Random.Range(minForce, maxForce);
            rb.AddForce(randomDirection * forceMagnitude, ForceMode.Impulse);
        }

        for (int i = 0; i < AmmoDrops; i++)
        {
            GameObject pickup = Instantiate(ammodrop, transform.position, Quaternion.identity);


            // Add a Rigidbody for physics
            Rigidbody rb = pickup.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = pickup.AddComponent<Rigidbody>();
            }

            Vector3 randomDirection = new Vector3(
              Random.Range(-1f, 1f),
              Random.Range(0.5f, 1f) * upwardBias, // Upward bias
              Random.Range(-1f, 1f)
          ).normalized;
            float forceMagnitude = Random.Range(minForce, maxForce);
            rb.AddForce(randomDirection * forceMagnitude, ForceMode.Impulse);
        }

        for (int i = 0; i < AbilityDrops; i++)
        {
            GameObject pickup = Instantiate(abilitydrop, transform.position, Quaternion.identity);


            // Add a Rigidbody for physics
            Rigidbody rb = pickup.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = pickup.AddComponent<Rigidbody>();
            }

            Vector3 randomDirection = new Vector3(
              Random.Range(-1f, 1f),
              Random.Range(0.5f, 1f) * upwardBias, // Upward bias
              Random.Range(-1f, 1f)
          ).normalized;
            float forceMagnitude = Random.Range(minForce, maxForce);
            rb.AddForce(randomDirection * forceMagnitude, ForceMode.Impulse);
        }


        Die();
    }



}


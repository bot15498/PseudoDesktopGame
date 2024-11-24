using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHealth : MonoBehaviour
{
    public bool bypassMeleeExecution; 
    public int healthDrops;
    public int AmmoDrops;
    public int AbilityDrops;

    public GameObject healthdrop;
    public GameObject ammodrop;
    public GameObject abilitydrop;

    private float maxdbnoTime = 3f;
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
    public AudioClip enemyhurtclip;
    public AudioClip[] deathclips;
    public AudioClip dbnoclip;



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
            aiBehavior.chaseState = EnemyAiChaseState.AttackingButStayingStillForever;
            rb.isKinematic = false;
            rb.drag = 999f;

            if(dbnoTimer > maxdbnoTime)
            {
                Die();
            }
            dbnoTimer += Time.deltaTime;
        }
    }


    public void MeleeDamage(int damage)
    {
        if (damage < currentHealth)
        {
            currentHealth -= damage;
        }
        else
        {
            if(bypassMeleeExecution)
            {
                // just die
                Die();
            }
            else
            {
                meleeExecution();
            }
        }
    }





    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        AudioSource.PlayClipAtPoint(enemyhurtclip, transform.position, 0.5f);
        // Debug.Log($"Enemy took {damage} damage. Remaining health: {currentHealth}");
        if (currentHealth <= 0)
        {
            if (bypassMeleeExecution)
            {
                // Just going to die.
                Die();

            }
            else
            {
                // Start timer for melee execution
                dbno = true;
                AudioSource.PlayClipAtPoint(dbnoclip, transform.position, 0.1f);
                aiBehavior.stunState = EnemyAiStunState.Stagger;
            }
        }

    }



    void Die()
    {
        //Debug.Log("Enemy died!");
        AudioSource.PlayClipAtPoint(deathclips[Random.Range(0,5)], transform.position, 0.7f);
        FindObjectOfType<EnemyManager>().enemies.Remove(aiBehavior);

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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShotgunBlast : MonoBehaviour
{
    public int MaxCharges;
    public int CurrentCharges;
    public float fireDelay;
    private float timer;

    [Header("Shotgun Settings")]
    public GameObject projectilePrefab; // The projectile prefab to instantiate
    public Transform firePoint;        
    public int projectileCount = 8;     
    public float spreadAngle = 30f;     
    public float projectileSpeedMax; 
    public float projectileSpeedMin;
    public float cooldown;
    private float timer2;

    public PlayerMovement pm;
    public float knockbackForce = 5f;           
    public float knockbackDuration = 0.2f;      
    public CameraShake cs;
    bool cooldown2;
    public Animator anim;
    public TextMeshProUGUI ChargeText;
    public AudioClip blastclip;
    AudioSource asource;


    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        cooldown2 = false;
        asource = GetComponent<AudioSource>();
    }

    public void addCharges(int chargestoadd)
    {

        if (CurrentCharges != MaxCharges && CurrentCharges + chargestoadd < MaxCharges)
        {
            CurrentCharges += chargestoadd;
        }
        if(CurrentCharges + chargestoadd >= MaxCharges)
        {
            CurrentCharges = MaxCharges;
        }
        
    }
    void Update()
    {
        ChargeText.text = "x" + CurrentCharges;

        if (Input.GetKeyDown(KeyCode.Q)) // Left mouse button
        {
            if (CurrentCharges > 0 && cooldown2 == false)
            {
                cooldown2 = true;
                CurrentCharges -= 1;
                asource.clip = blastclip;
                asource.Play(); 
                anim.Play("Shotgun_Fire", -1, 0f);
                StartCoroutine(ExampleCoroutine());
            }
            Debug.Log("shotgun is fired");
        }

        if(cooldown2 == true)
        {
            timer2 += Time.deltaTime;

        }

        if(timer2 >= cooldown)
        {
            cooldown2 = false;
            timer2 = 0;
        }

        
    }

    void FireShotgun()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            // Generate a random direction uniformly within the cone
            Vector3 randomDirection = GetRandomCircularConeDirection(firePoint.forward, spreadAngle);

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(randomDirection));
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = randomDirection * Random.Range(projectileSpeedMin, projectileSpeedMax);
            }
        }
    }

    Vector3 GetRandomCircularConeDirection(Vector3 forward, float angle)
    {
        // Convert the angle to radians and compute the cosine of the maximum spread angle
        float maxRadians = Mathf.Deg2Rad * angle / 2;
        float cosMaxAngle = Mathf.Cos(maxRadians);

        // Randomly choose a cosine value between the forward direction (1) and the cone edge (cosMaxAngle)
        float randomCosAngle = Random.Range(cosMaxAngle, 1f);

        // Randomly choose an azimuthal angle (full circle around the forward vector)
        float randomAzimuth = Random.Range(0, 2 * Mathf.PI);

        // Compute the sine of the chosen angle for correct distribution
        float randomSinAngle = Mathf.Sqrt(1 - randomCosAngle * randomCosAngle);

        // Use spherical coordinates to calculate the direction
        Vector3 randomDirection = new Vector3(
            randomSinAngle * Mathf.Cos(randomAzimuth),
            randomSinAngle * Mathf.Sin(randomAzimuth),
            randomCosAngle
        );

        // Rotate the direction to align with the forward vector
        return Quaternion.LookRotation(forward) * randomDirection;
    }

    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.


        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(fireDelay);
        FireShotgun();
        cs.addShake(0.2f);
        Vector3 knockbackDirection = -firePoint.forward * knockbackForce;
       

        pm.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration, true);

    }

    
}

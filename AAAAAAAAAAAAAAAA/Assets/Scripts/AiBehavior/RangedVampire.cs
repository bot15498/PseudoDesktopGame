using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedVampire : AiBehaviorBase
{
    public float adjustShootDirection = 90f;
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPoint;
    public bool predictPlayerLocation = false;
    public Vector3 playerPredictionDisplace = new Vector3(0f, -0.5f, 0f);
    private Vector3 previousPlayerLocation;

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
        if (player != null)
        {
            previousPlayerLocation = player.transform.position;
        }
        base.FixedUpdate();
    }

    public override bool IsObjectInAttackRange()
    {
        return CanSeePlayer();
    }

    public override void Attack()
    {
        GameObject bul = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        // Rotate to point to player center
        Vector3 direction = player.transform.position + playerPredictionDisplace - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        bul.transform.rotation = rotation;
    }

    public override void FacePlayer()
    {
        if (chaseState != EnemyAiChaseState.Idle && predictPlayerLocation && player != null)
        {
            // Try to have some prediction
            float bulletSpeed = bulletPrefab.GetComponent<Bullet>().speed * Time.fixedDeltaTime;

            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            float timeToReachPlayerNominal = distanceToPlayer / bulletSpeed;
            // Shouldn't this be curr - prev???????
            Vector3 playerVelocity = (previousPlayerLocation - player.transform.position) / Time.fixedDeltaTime;
            Vector3 predictedPosition = playerVelocity * timeToReachPlayerNominal + player.transform.position;

            Vector3 direction = predictedPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
        }
        else
        {
            // Do the base logic
            base.FacePlayer();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedVampire : AiBehaviorBase
{
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPoint;
    public bool predictPlayerLocation = false;
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
        if(player != null)
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
        Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
    }

    public override void FacePlayer()
    {
        if(!predictPlayerLocation || player == null)
        {
            // Do the base logic
            base.FacePlayer();
        }
        else
        {
            // Try to have some prediction
            float bulletSpeed = bulletPrefab.GetComponent<Bullet>().speed * Time.fixedDeltaTime;

            float distanceToPlayer = Vector3.Distance(transform.position,player.transform.position);
            float timeToReachPlayerNominal = distanceToPlayer / bulletSpeed;
            // Shouldn't this be curr - prev???????
            Vector3 playerVelocity = (previousPlayerLocation - player.transform.position) / Time.fixedDeltaTime;
            Vector3 predictedPosition = playerVelocity * timeToReachPlayerNominal + player.transform.position;
            Debug.Log($"Player velocity: {playerVelocity}");
            Debug.Log($"time to reach player: {timeToReachPlayerNominal}");
            Debug.Log($"Predicted player position: {predictedPosition}");
            Debug.Log($"Player Position: {player.transform.position}");

            Vector3 direction = predictedPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
        }
    }
}

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

    public new void FacePlayer()
    {
        if(!predictPlayerLocation || player == null)
        {
            // Do the base logic
            base.FacePlayer();
        }
        else
        {
            // Try to have some prediction
            float bulletSpeed = bulletPrefab.GetComponent<Bullet>().speed;

            float distanceToPlayer = Vector3.Distance(transform.position,player.transform.position);
            float timeToReachPlayerNominal = distanceToPlayer / bulletSpeed;
            Vector3 playerVelocity = (player.transform.position - previousPlayerLocation) / Time.fixedDeltaTime;
            Vector3 predictedPosition = playerVelocity * timeToReachPlayerNominal + transform.position;

            Vector3 direction = predictedPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
        }
    }
}

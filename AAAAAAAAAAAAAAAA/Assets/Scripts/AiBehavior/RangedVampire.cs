using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedVampire : AiBehaviorBase
{
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPoint;
    public bool predictPlayerLocation = false;

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

    public override void Attack()
    {
        Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
    }

    public new void FacePlayer()
    {
        if(!predictPlayerLocation)
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
            Vector3 predictedPosition = agent.velocity * timeToReachPlayerNominal + transform.position;

            Vector3 direction = predictedPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
        }
    }
}

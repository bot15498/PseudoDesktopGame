using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurret : AiBehaviorBase
{
    public GameObject bulletPrefab;
    public GameObject bulletSpawnPoint;
    private EnemyHealth enemyHealth;

    new void Start()
    {
        // Make it so melee on this unit doesn't drop anything.
        enemyHealth = GetComponent<EnemyHealth>();
        enemyHealth.bypassMeleeExecution = true;
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

    public override bool IsObjectInAttackRange()
    {
        // If you can see the player then fire away.
        return CanSeePlayer();
    }

    public override void Attack()
    {
        Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
    }
}

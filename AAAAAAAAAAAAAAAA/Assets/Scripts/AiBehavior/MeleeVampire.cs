using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeVampireAttackState
{
    Idle,
    MeleeAttack,
    StartTeleport,
    EndTeleport
}

public class MeleeVampire : AiBehaviorBase
{
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
        
    }
}

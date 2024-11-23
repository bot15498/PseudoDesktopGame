using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedVampire : AiBehaviorBase
{
    public GameObject buleltPrefab;
    public float meleeRange = 1f;

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
        throw new System.NotImplementedException();
    }

    public bool IsInMeleeRange()
    {
        return false;
    }
}

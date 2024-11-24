using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Propagandist : AiBehaviorBase
{
    PropagandistManager manager;

    new void Start()
    {
        manager = FindObjectOfType<PropagandistManager>();
        if(manager != null)
        {
            manager.propagang.Add(this);
        }
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
        return false;
    }

    public override void Attack()
    {
        // do nothing.
    }

    public bool IsVisibleByPlayer()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool toreturn = screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1 && screenPoint.z >= 0 && CanSeePlayer();
        if(stunState == EnemyAiStunState.Normal)
        {
            return toreturn;
        }
        else
        {
            return false;
        }
    }

    //private int IncrementPropagandaIndex()
    //{
    //    propagandaImageIndex = (propagandaImageIndex + 1) % propagandaImages.Count;
    //    return propagandaImageIndex;
    //}

    //private float CalculateOpacity(float timestep)
    //{
    //    float toreturn = -Mathf.Cos(timestep / propagandaIntervalSec * Mathf.PI * 2) / 2 + 0.25f;
    //    if (toreturn <= 0.1f)
    //    {
    //        // If we are close enough, swap images
    //        IncrementPropagandaIndex();
    //        propagandaImage.sprite = propagandaImages[propagandaImageIndex];
    //        toreturn = 0;
    //    }
    //    return toreturn;
    //}
}

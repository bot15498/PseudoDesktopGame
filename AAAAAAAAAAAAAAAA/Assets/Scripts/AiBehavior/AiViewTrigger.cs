using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiViewTrigger : MonoBehaviour
{
    private SphereCollider trigger;
    public AiBehaviorBase behaviourBase;

    public float CapsuleRadius
    {
        get
        {
            if (trigger != null)
            {
                return trigger.radius;
            }
            else
            {
                return 0f;
            }
        }
        set
        {
            if (trigger == null)
            {
                CreateCapsuleTrigger(value);
            }
            trigger.radius = value;
        }
    }

    private void Awake()
    {
        behaviourBase = gameObject.GetComponentInParent<AiBehaviorBase>();
    }

    public SphereCollider CreateCapsuleTrigger(float radius)
    {
        trigger = gameObject.AddComponent<SphereCollider>();
        trigger.isTrigger = true;
        trigger.radius = radius;
        CapsuleRadius = radius;
        return trigger;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Notify the ai controller that we can see the bullet
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            behaviourBase.canSeeBullet = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            behaviourBase.canSeeBullet = false;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    if (trigger != null)
    //    {
    //        Gizmos.DrawWireSphere(transform.position, trigger.radius);
    //    }
    //}
}

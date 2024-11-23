using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAvoidanceCircle : MonoBehaviour
{
    private CapsuleCollider trigger;
    public AiBehaviorBase behaviourBase;

    private void Awake()
    {
        behaviourBase = gameObject.GetComponentInParent<AiBehaviorBase>();
    }

    public float CapsuleRadius
    {
        get
        {
            if(trigger != null)
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
            if(trigger == null)
            {
                CreateCapsuleTrigger(value);
            }
            trigger.radius = value;
        }
    }

    public CapsuleCollider CreateCapsuleTrigger(float radius)
    {
        trigger = gameObject.AddComponent<CapsuleCollider>();
        trigger.isTrigger = true;
        trigger.radius = radius;
        CapsuleRadius = radius;
        return trigger;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Notify the ai controller that we need to move to spread out.
        if(other.tag == "Enemy" && !other.isTrigger && behaviourBase != null)
        {
            behaviourBase.agentsInAvoidanceCircle.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" && !other.isTrigger && behaviourBase.agentsInAvoidanceCircle.Contains(other.transform))
        {
            behaviourBase.agentsInAvoidanceCircle.Remove(other.transform);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if(trigger != null)
        {
            Gizmos.DrawWireSphere(transform.position, trigger.radius);
        }
    }
}

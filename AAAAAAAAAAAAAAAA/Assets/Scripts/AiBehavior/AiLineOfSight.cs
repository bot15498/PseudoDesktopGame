using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class AiLineOfSight : MonoBehaviour
{
    private BoxCollider trigger;
    public AiBehaviorBase behaviourBase;

    private void Awake()
    {
        behaviourBase = gameObject.GetComponentInParent<AiBehaviorBase>();
    }
    public float LineOfSightWidth
    {
        get
        {
            if (trigger != null)
            {
                return trigger.size.x;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (trigger == null)
            {
                CreateLineOfSightTrigger(value);
            }
            SetLineOfSightWidth(value);
        }
    }

    public BoxCollider CreateLineOfSightTrigger(float width, float maxLength = 10f)
    {
        trigger = gameObject.AddComponent<BoxCollider>();
        trigger.isTrigger = true;
        trigger.size = new Vector3(maxLength, 1, width);
        trigger.center = new Vector3(maxLength / 2, 0, 0);
        return trigger;
    }

    private void SetLineOfSightWidth(float width)
    {
        trigger.size = new Vector3(trigger.size.x, trigger.size.y, width);
        trigger.center = new Vector3(trigger.size.x / 2, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Notify the ai controller that we need to move so we have good line of sight.
        if (other.tag == "Enemy" && !other.isTrigger && behaviourBase != null)
        {
            behaviourBase.agentsInLineOfSight.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy" && !other.isTrigger && behaviourBase.agentsInLineOfSight.Contains(other.transform))
        {
            behaviourBase.agentsInLineOfSight.Remove(other.transform);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if (trigger != null)
        {
            Gizmos.matrix = trigger.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(trigger.center, trigger.size);
        }
    }
}

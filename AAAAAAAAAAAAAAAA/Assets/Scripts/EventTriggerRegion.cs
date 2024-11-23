using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTriggerRegion : MonoBehaviour
{
    public UnityEvent OnEnemyEnter;
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnExit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            OnPlayerEnter.Invoke();
        }
        else if(other.gameObject.tag == "Enemy")
        {
            OnEnemyEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        OnExit.Invoke();
    }
}

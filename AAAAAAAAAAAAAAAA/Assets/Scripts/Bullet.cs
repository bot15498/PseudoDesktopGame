using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public float timeoutTimeSec = 10f;
    [SerializeField]
    private float aliveTime = 0f;
    Rigidbody rb;
    bool hasHit;
    Collider thiscollide;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed);
        hasHit = true;
        thiscollide = gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
            
        
        if (timeoutTimeSec > 0f && aliveTime >= timeoutTimeSec)
        {
            Destroy(gameObject);
        }
        if(hasHit == true)
        {
            
        }
        aliveTime += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //thiscollide.enabled = false;
        rb.velocity = Vector3.zero;
        hasHit = true;
        rb.useGravity = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class childtest : MonoBehaviour
{

    public GameObject test;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.parent = test.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHeadAtPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(player.transform);
    }
}

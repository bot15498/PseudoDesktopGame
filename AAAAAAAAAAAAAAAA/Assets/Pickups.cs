using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    // Start is called before the first frame update

    public Gun gun;
    public ShotgunBlast blast;
    public int chargestoadd;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log(collision.gameObject.tag);

       
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "AmmoPickup")
        {
            Destroy(hit.gameObject);
            //playpickup sound
            gun.addAmmo();
        }
        if (hit.gameObject.tag == "ShellPickup")
        {
            Destroy(hit.gameObject);
            blast.addCharges(chargestoadd);
        }
    }

}

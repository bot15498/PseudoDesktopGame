using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    // Start is called before the first frame update

    public Gun gun;
    public ShotgunBlast blast;
    public PlayerHealth healther;
    public int chargestoadd;
    public int healthToadd;

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
        if (hit.gameObject.tag == "HealthPickup" && healther.health < healther.maxHealth)
        {
            Destroy(hit.gameObject);
            healther.health = Mathf.Min(healther.health + healthToadd, healther.maxHealth);
        }
    }

}

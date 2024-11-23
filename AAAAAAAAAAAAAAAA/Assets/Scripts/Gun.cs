using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform gunBarrel;
    public GameObject bullet;
    public int ammocountMax;
    public int ammocount;
    public int reserveAmmoMax;
    public int reserveAmmoCurrent;
    public int ammoPerPickup;

    public float firerate;
    float fireratetimer;
    public float reloadtimer;
    public float timer;
    bool isreloading;
    bool canfire;
    public Animator anim;
    public TextMeshProUGUI ammotext;
    //public Animator anim;
    public GameObject reloadText;


    void Start()
    {
        ammocount = ammocountMax;
        timer = 0;
        fireratetimer = 0;
        isreloading = false;
        canfire = true;
       // reloadText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ammotext.text = ammocount.ToString() + "/" + reserveAmmoCurrent.ToString();
        if (ammocount <= 3 && isreloading == false)
        {
            reloadText.SetActive(true);
        }


       // ammotext.text = ammocount.ToString() + "/" + ammocountMax.ToString();
        if (Input.GetKey(KeyCode.Mouse0) && ammocount > 0 && isreloading == false && canfire == true)
        {
            Instantiate(bullet, gunBarrel.position, gunBarrel.rotation);
            ammocount -= 1;
            anim.Play("Crossbow_fire", -1, 0f);
            
            Debug.Log("Shoot");
            canfire = false;

        }
        if (Input.GetKey(KeyCode.Mouse0) && ammocount > 0 && isreloading == false)
        {
           
        }
        else if (isreloading == false)
        {
            //anim.Play("Idle");
        }


        if ((ammocount == 0 && reserveAmmoCurrent != 0) || ((Input.GetKeyDown(KeyCode.R) && ammocount != ammocountMax))&& reserveAmmoCurrent != 0)
        {

            anim.Play("Crossbow_reload");
            isreloading = true;
            reloadText.SetActive(false);
        }


        if (isreloading == true)
        {
            timer += Time.deltaTime;
        }

        if (timer >= reloadtimer)
        {
            isreloading = false;
            if(ammocountMax - ammocount <= reserveAmmoCurrent)
            {
                reserveAmmoCurrent -= (ammocountMax - ammocount);
                ammocount = ammocountMax;

            }else if(ammocountMax - ammocount > reserveAmmoCurrent)
            {
                ammocount += reserveAmmoCurrent;
                reserveAmmoCurrent = 0;
            }

            timer = 0;
            
        }


        if (canfire == false)
        {
            fireratetimer += Time.deltaTime;
        }

        if (fireratetimer >= firerate)
        {
            canfire = true;
            fireratetimer = 0;
        }
    }

    public void addAmmo()
    {
        if(reserveAmmoCurrent < reserveAmmoMax)
        {
            if(reserveAmmoCurrent + ammoPerPickup <= reserveAmmoMax)
            {
                reserveAmmoCurrent += ammoPerPickup;
            }
            else
            {
                if(reserveAmmoCurrent + ammoPerPickup > reserveAmmoMax)
                {
                    reserveAmmoCurrent = reserveAmmoMax;
                }
            }
        }
    }
}

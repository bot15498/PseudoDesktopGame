using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int health = 100;
    public TextMeshProUGUI hptext;
    public Image hpbar;
    public float onDeathStaticFadeInTime = 2f;
    public PauseMenu pauseMenu;
    public Animator playerAnimator;
    public AnimationClip playerDeathAnimation; 
    private bool died = false;

    public delegate void OnDeath(GameObject player);
    public static OnDeath onDeath;
    public delegate void OnHit(GameObject player);
    public static OnHit onHit;

    public AudioClip hitsound;
    public AudioClip hitsound2;
  

    void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        hptext.text = $"HP:{health}";
        hpbar.fillAmount = (float)health / maxHealth;
    }

    public void TakeDamage(int damage)
    {
        Random.Range(1, 2);
        if (Random.Range(1, 2) == 1)
        {
            AudioSource.PlayClipAtPoint(hitsound, transform.position, 0.1f);
        }
        else
        {
            AudioSource.PlayClipAtPoint(hitsound2, transform.position, 0.1f);
        }
        health = Mathf.Max(health - damage, 0);
        


        if (!died && health <= 0)
        {
            died = true;
           
            onHit?.Invoke(gameObject);
            Die();

        }
    }

    public void Die()
    {
        Debug.Log("You died!!!!!!!!!");
        StartCoroutine(pauseMenu.FadeInStatic(onDeathStaticFadeInTime));;
        onDeath?.Invoke(gameObject);
    }
}

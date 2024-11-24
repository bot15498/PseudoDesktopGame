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
        health = Mathf.Max(health - damage, 0); 
        if(!died && health <= 0)
        {
            died = true;
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("You died!!!!!!!!!");
        StartCoroutine(pauseMenu.FadeInStatic(onDeathStaticFadeInTime));;
    }
}

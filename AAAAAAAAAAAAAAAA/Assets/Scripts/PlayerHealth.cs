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
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("You died!!!!!!!!!");
    }
}

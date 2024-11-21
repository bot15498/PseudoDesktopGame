using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class EnemyHealth : MonoBehaviour
    {

     public float dbnoTimer;
     public int maxHealth;
     private int currentHealth;





        void Start()
        {
            currentHealth = maxHealth;
        }

        public void MeleeDamage(int damage)
    {
        currentHealth -= damage;
    } 

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            Debug.Log($"Enemy took {damage} damage. Remaining health: {currentHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            Debug.Log("Enemy died!");
            Destroy(gameObject);
        }
    }


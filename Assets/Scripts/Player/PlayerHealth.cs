using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        //Hurt animation
        currentHealth -= damage;
        Debug.Log("Daños");
        if(currentHealth <= 0 )
        {
            Die();
        }
    }

    void Die()
    {
        //Die animation
        //Game over menu
        Debug.Log("Player Death");
    }

    //public void Knockback(GameObject sender)
    //{
    //    Vector2 direction = (transform.position - sender.transform.position).normalized;
    //    rb2D.AddForce(direction * knockbackImpulse, ForceMode2D.Impulse);
    //}
}

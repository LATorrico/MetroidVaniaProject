using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    Rigidbody2D rb2D;
    GameObject player;
    PlayerHealth playerHealth;
    PlayerMovement playerMovement;

    [SerializeField] private float fireballSpeed;
    [SerializeField] private int damage;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();

        Vector2 direction = player.transform.position - transform.position;
        rb2D.velocity = direction.normalized * fireballSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerHealth.TakeDamage(damage);
            playerMovement.KnockBack(transform);
            //collision effect
            Destroy(gameObject);
        }
    }
}
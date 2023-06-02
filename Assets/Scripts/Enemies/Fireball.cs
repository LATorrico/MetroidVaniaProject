using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    Rigidbody2D rb2D;
    GameObject player;

    [SerializeField] private float fireballSpeed;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb2D.velocity = new Vector2(direction.x, direction.y).normalized * fireballSpeed;
    }
}

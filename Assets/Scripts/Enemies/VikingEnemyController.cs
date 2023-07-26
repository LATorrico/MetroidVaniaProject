using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VikingEnemyController : MonoBehaviour
{
    Rigidbody2D rb2D;
    Animator anim;
    GameObject player;

    PlayerDetect playerDetect;
    PlayerHealth playerHealth;
    PlayerMovement playerMovement;

    [SerializeField] private float distanceDetectY;
    [SerializeField] private float attackDistance;

    Vector2 distanceToPlayer;

    [Header("Patrol")]
    [Space(5)]
    [SerializeField] private Transform rayPoint;
    [SerializeField] private Transform checkPlayerPoint;

    [Space(10)]

    [SerializeField] private float rayDownDistance;
    [SerializeField] private float rayForwardDistance;
    [SerializeField] private float rayPlayerDetectDistance;

    [Space(10)]

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;

    [Space(10)]

    [SerializeField] private float moveSpeed;
    [SerializeField] private float waitTime;

    bool checkDown;
    bool checkForward;
    bool checkPlayer;
    bool isAttacking;

    [Header("Attack")]
    [Space(5)]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector2 attackRange;

    [Space(10)]

    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private int damage;
    
    float timer;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerDetect = GetComponent<PlayerDetect>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        distanceToPlayer = transform.position - player.transform.position;

        CheckGround();
        anim.SetFloat("Walk", Math.Abs(rb2D.velocity.x));

        //Follow player when detect
        if (playerDetect.playerDetect && Math.Abs(distanceToPlayer.y) <= distanceDetectY)
        {
            FollowPlayer();
        }

        //Attack
        timer += Time.deltaTime;
        if (checkPlayer && Math.Abs(distanceToPlayer.x) <= attackDistance)
        {
            rb2D.velocity = Vector3.zero;
            if (timer > timeBetweenAttacks)
            {
                timer = 0;
                Attack();
            }
        }
        else
        {
            isAttacking = false;
        }

        //Patrolling
        if (!isAttacking && Math.Abs(distanceToPlayer.x) > attackDistance)
        {
            rb2D.velocity = new Vector2(moveSpeed, rb2D.velocity.y);
            if (!checkDown || checkForward)
            {
                StartCoroutine("Patrolling");
            }
        }
    }

    private void FollowPlayer()
    {
        //Go to player position
        if(!checkPlayer)
        {
            Flip();
        } 
    }

    private void Attack()
    {
        isAttacking = true;
        //Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        
        anim.SetTrigger("Attack");
    }

    private void Damage()
    {
        Collider2D hitPlayer = Physics2D.OverlapBox(attackPoint.position, attackRange, 90, playerLayer);

        if (hitPlayer != null)
        {
            playerHealth.TakeDamage(damage);
            playerMovement.KnockBack(transform);
        }
    }

    IEnumerator Patrolling()
    {
        rb2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(waitTime);
        Flip();
        StopCoroutine("Patrolling");
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
        moveSpeed *= -1f;
    }

    void CheckGround()
    {
        checkDown = Physics2D.Raycast(rayPoint.position, Vector2.down, rayDownDistance, groundLayer);
        checkForward = Physics2D.Raycast(rayPoint.position, new Vector2(transform.localScale.x, 0), rayForwardDistance, groundLayer);
        checkPlayer = Physics2D.Raycast(checkPlayerPoint.position, new Vector2(transform.localScale.x, 0), rayPlayerDetectDistance, playerLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayPoint.transform.position, rayPoint.transform.position + Vector3.down * rayDownDistance);
        Gizmos.DrawLine(rayPoint.transform.position, rayPoint.transform.position + new Vector3(transform.localScale.x * rayForwardDistance, 0));
        Gizmos.DrawLine(checkPlayerPoint.transform.position, checkPlayerPoint.transform.position + new Vector3(transform.localScale.x * rayPlayerDetectDistance, 0));
        Gizmos.DrawWireCube(attackPoint.position, attackRange);
    }
}

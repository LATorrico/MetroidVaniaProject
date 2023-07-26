using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Animator anim;

    [SerializeField] private Transform attackPoint;
    [Space(5)]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackRate;
    [Space(5)]
    [SerializeField] private LayerMask enemyLayers;

    [Space(10)]

    [SerializeField] private int damage;

    [HideInInspector] public bool isAttacking;
    float nextAttackTime;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if(!isAttacking)
                {
                    isAttacking = true;
                    Attack();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }
        }
    }

    private void Attack()
    {
        anim.SetBool("Attack", true);
        anim.SetBool("IsAttacking", isAttacking);
    }

    private void Damage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("IsAttacking", isAttacking);
        anim.SetBool("Attack", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
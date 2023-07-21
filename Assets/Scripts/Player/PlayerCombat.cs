using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Animator anim;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange, attack1Damage;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private bool combatEnabled;
    [SerializeField] private float attackRate;
    [SerializeField] private int damage;

    public bool isAttacking;
    float nextAttackTime;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("CanAttack", combatEnabled);
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
        anim.SetBool("Attack1", true);
        anim.SetBool("IsAttacking", isAttacking);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("IsAttacking", isAttacking);
        anim.SetBool("Attack1", false);
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
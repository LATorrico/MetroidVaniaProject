using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat2 : MonoBehaviour
{
    //Alternate between 2 attacks. Animator controller needs to be modified.
    Animator anim;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float inputTimer, attackRange, attack1Damage;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private bool combatEnabled;
    bool gotInput, isAttacking, isFirstAttack;

    float lastInputTime = Mathf.NegativeInfinity;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("CanAttack", combatEnabled);
    }
    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }

    private void CheckCombatInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (combatEnabled)
            {
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }

    private void CheckAttacks()
    {
        if (gotInput)
        {
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                anim.SetBool("Attack1", true);
                anim.SetBool("FirstAttack", isFirstAttack);
                anim.SetBool("IsAttacking", isAttacking);
            }
        }

        if (Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
        }
    }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Damage" + enemy.name);
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
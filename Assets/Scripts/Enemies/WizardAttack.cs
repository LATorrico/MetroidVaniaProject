using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAttack : MonoBehaviour
{
    PlayerDetect playerDetect;
    Animator anim;
    GameObject player;

    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform fireballInstantiatePoint;

    [Space(5)]

    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float fireballSpeed;

    float timer;

    private void Start()
    {
        playerDetect = GetComponent<PlayerDetect>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Flip();

        timer += Time.deltaTime;
        
        if (playerDetect.playerDetect)
        {
            if (timer > timeBetweenAttacks)
            {
                timer = 0;
                Attack();
            }
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
    }

    void InstantiateFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, fireballInstantiatePoint.position, fireballInstantiatePoint.rotation);
        Destroy(fireball, 1f);
    }

    void Flip()
    {
        Vector3 direction = transform.position - player.transform.position;
        if (direction.x > 0)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
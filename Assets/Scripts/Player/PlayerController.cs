using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerCombat playerCombat;
    Animator anim;

    [SerializeField] private float knockBackTime = 1f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();  
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Update()
    {
        if (playerMovement.knockBacked)
        {
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerCombat>().enabled = false;
            StartCoroutine(UnKnockBack());
        }
        anim.SetBool("Hurt", playerMovement.knockBacked);
    }

    IEnumerator UnKnockBack()
    {
        yield return new WaitForSeconds(knockBackTime);
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerCombat>().enabled = true;
        playerMovement.knockBacked = false;
    }
}

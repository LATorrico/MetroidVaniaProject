using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackTrigger : MonoBehaviour
{
    PlayerMovement playerMovement;
    GameObject player;

    [SerializeField] private Transform kBCollider;
    [SerializeField] private Vector2 rangeCollider;
    [SerializeField] private LayerMask playerLayer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        Collider2D playerCollider = Physics2D.OverlapBox(kBCollider.position, rangeCollider, 90, playerLayer);
        if(playerCollider != null)
        {
            playerMovement.KnockBack(transform);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(kBCollider.position, rangeCollider);
    }
}

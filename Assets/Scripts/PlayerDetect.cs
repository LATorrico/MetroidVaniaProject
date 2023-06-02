using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
    [SerializeField] private float distanceToPlayerDetect;
    public bool playerDetect;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position ,player.transform.position) <= distanceToPlayerDetect)
        {
            playerDetect = true;
        }
        else
        {
            playerDetect = false;
        }
    }
}

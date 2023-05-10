using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb2D;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    float horizontal;
    bool isGrounded;

    [SerializeField] private Transform checkGroundPoint;
    [SerializeField] private float radOfCircle;
    [SerializeField] private LayerMask whatIsGround;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        Jump();
    }

    private void FixedUpdate()
    {
        rb2D.velocity = new Vector2(horizontal * moveSpeed, rb2D.velocity.y);
    }

    void Jump()
    {
        CheckGround();
        //Para el salto si sueltas el botón
        if(Input.GetButtonUp("Jump") && rb2D.velocity.y > 0)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        }
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2D.AddForce(new Vector2(rb2D.velocity.x, jumpForce), ForceMode2D.Impulse);
        }
    }

    void Dash()
    {

    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(checkGroundPoint.position, radOfCircle, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(checkGroundPoint.position, radOfCircle);
    }
}

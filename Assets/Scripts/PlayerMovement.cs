using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb2D;
    Animator anim;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    private float horizontal;
    private bool facingRight;

    [Header("CheckGround")]
    [SerializeField] private Transform checkGroundPoint;
    [SerializeField] private float radOfCircle;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCoolDown;
    private bool canDash = true;
    private bool isDashing;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //if (isDashing)
        //{
        //    return;
        //}

        horizontal = Input.GetAxisRaw("Horizontal");
        Jump();
        SetAnimationState();

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if(isDashing)
        {
            return;
        }

        rb2D.velocity = new Vector2(horizontal * moveSpeed, rb2D.velocity.y);

        if (horizontal < 0 && !facingRight) Flip();
        if(horizontal > 0 && facingRight) Flip();
    }

    void Jump()
    {
        CheckGround();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb2D.gravityScale;
        rb2D.gravityScale = 0f;
        rb2D.velocity = new Vector2(dashSpeed * transform.localScale.x, 0f);
        anim.SetTrigger("Dashing");

        yield return new WaitForSeconds(dashTime);

        rb2D.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(checkGroundPoint.position, radOfCircle, whatIsGround);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    void SetAnimationState()
    {
        anim.SetFloat("Run", Mathf.Abs(horizontal));
        anim.SetBool("IsGrounded", !isGrounded);
        anim.SetFloat("VelocityY", rb2D.velocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(checkGroundPoint.position, radOfCircle);
    }
}

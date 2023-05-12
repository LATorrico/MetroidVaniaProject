using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb2D;
    Animator anim;
    Collider2D col;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    private float horizontal;
    private bool facingRight;
    float originalGravity;

    [Header("CheckGround")]
    [SerializeField] private Transform checkGroundPoint;
    [SerializeField] private Transform checkWallPoint;
    [SerializeField] private Transform checkGrabPoint;
    [SerializeField] private float radOfCircle;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    private bool isWalling;
    private bool isGrab;

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
        col = GetComponent<Collider2D>();
        originalGravity = rb2D.gravityScale;
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        Jump();
        ClimbOrRelease();
        SetAnimationState();

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded && canDash)
        {
            StartCoroutine(Dash());
        }

        if(isWalling && !isGrounded && !isGrab)
        {
            //rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            rb2D.velocity = new Vector2(0f, 0f);
            rb2D.gravityScale = 0f;
            horizontal = 0f;
        }
    }

    private void FixedUpdate()
    {
        if(!isDashing)
        {
            rb2D.velocity = new Vector2(horizontal * moveSpeed, rb2D.velocity.y);
        }

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

    void ClimbOrRelease()
    {
        if (Input.GetKeyDown(KeyCode.W) && !isGrounded && isWalling && !isGrab ||
            Input.GetButtonDown("Jump") && !isGrounded && isWalling && !isGrab)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.1f);
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            rb2D.gravityScale = originalGravity;
            isGrab = true;
        }

        if (Input.GetKeyDown(KeyCode.S) && !isGrounded && isWalling && !isGrab)
        {
            transform.position = new Vector2(transform.position.x + (-0.1f * transform.localScale.x), transform.position.y - 0.5f);
            rb2D.gravityScale = originalGravity;
            isGrab = true;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        //float originalGravity = rb2D.gravityScale;
        rb2D.gravityScale = 0f;
        col.enabled = false;
        rb2D.velocity = new Vector2(dashSpeed * transform.localScale.x, 0f);

        yield return new WaitForSeconds(dashTime);

        col.enabled = true;
        rb2D.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(checkGroundPoint.position, radOfCircle, whatIsGround);
        isWalling = Physics2D.OverlapCircle(checkWallPoint.position, radOfCircle, whatIsGround);
        isGrab = Physics2D.OverlapCircle(checkGrabPoint.position, radOfCircle, whatIsGround);
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
        anim.SetBool("Dashing", isDashing);
        anim.SetBool("IsGrab", isGrab);
        anim.SetBool("IsWalling", isWalling);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(checkGroundPoint.position, radOfCircle);
        Gizmos.DrawSphere(checkWallPoint.position, radOfCircle);
        Gizmos.DrawSphere(checkGrabPoint.position, radOfCircle);
    }
}

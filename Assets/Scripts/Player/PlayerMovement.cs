using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb2D;
    Animator anim;
    Collider2D col;
    PlayerCombat playerCombat;

    [SerializeField] private float moveSpeed;

    private float horizontal;
    private float vertical;
    private bool facingRight;
    float originalGravity;

    [Header("Jump System")]
    [Space(5)]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;

    [Space(5)]

    [SerializeField] private float jumpMultiplier;
    [SerializeField] private float fallMultiplier;
    Vector2 vecGravity;
    bool isJumping;
    float jumpCounter;

    [Header("CheckGround")]
    [Space(5)]
    [SerializeField] private Transform checkGroundPoint;
    [SerializeField] private Transform checkWallPoint;
    [SerializeField] private Transform checkGrabPoint;
    [SerializeField] private Transform checkBorderPlatform;

    [Space(10)]

    [SerializeField] private float radOfGroundCircle;
    [SerializeField] private float radOfWallingCircle;

    [Space(10)]

    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    private bool isWalling;
    private bool isGrab;
    private bool isBorderPlatform;

    [Header("Dash")]
    [Space(5)]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCoolDown;

    [Space(10)]

    [SerializeField] private Collider2D dashCollider;
    private bool canDash = true;
    private bool isDashing;

    [Header("Ladder")]
    [Space(5)]
    [SerializeField] private float onLadderSpeed;
    private bool onLadder = false;

    [Space(10)]

    [Header("KnockBack")]
    [Space(5)]
    [SerializeField] private Transform center;
    [SerializeField] private float knockBackVel;
    public bool knockBacked;



    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        originalGravity = rb2D.gravityScale;
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        CheckGround();
        Jump();
        ClimbOrRelease();
        SetAnimationState();

        //Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded && canDash)
        {
            StartCoroutine(Dash());
        }

        //Grab
        if(isWalling && !isGrounded && !isGrab && !isBorderPlatform)
        {
            rb2D.velocity = new Vector2(0f, 0f);
            rb2D.gravityScale = 0f;
            horizontal = 0f;
        }

        //Jump system
        if (rb2D.velocity.y < 0)
        {
            rb2D.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }
        if (rb2D.velocity.y > 0 && isJumping)
        {
            jumpCounter += Time.deltaTime;
            if (jumpCounter > jumpTime) isJumping = false;

            float t = jumpCounter / jumpTime;
            float currentJumpM = jumpMultiplier;

            if (t > 0.5f)
            {
                currentJumpM = jumpMultiplier * (1 - t);
            }

            rb2D.velocity += vecGravity * currentJumpM * Time.deltaTime;
        } 
    }

    private void FixedUpdate()
    {
        //Move
        if(!isDashing && playerCombat.isAttacking == false && !knockBacked)
        {
            rb2D.velocity = new Vector2(horizontal * moveSpeed, rb2D.velocity.y);
        }

        //Flip
        if (horizontal < 0 && !facingRight) Flip();
        if (horizontal > 0 && facingRight) Flip();

        //Ladder
        if (onLadder)
        {
            if (vertical != 0)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, vertical * onLadderSpeed);
                rb2D.gravityScale = 0f;
            }
            else if (vertical == 0 && !isGrounded)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
            }
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            isJumping = true;
            jumpCounter = 0;
        }
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpCounter = 0;

            if (rb2D.velocity.y > 0)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * 0.6f);
            }
        }
    }

    //Climb or release when player is grab
    void ClimbOrRelease()
    {
        if (Input.GetKeyDown(KeyCode.W) && !isGrounded && isWalling && !isGrab && !isBorderPlatform ||
            Input.GetButtonDown("Jump") && !isGrounded && isWalling && !isGrab && !isBorderPlatform)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f);
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            rb2D.gravityScale = originalGravity;
            isGrab = true;
        }

        if (Input.GetKeyDown(KeyCode.S) && !isGrounded && isWalling && !isGrab && !isBorderPlatform)
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
        rb2D.gravityScale = 0f;
        col.enabled = false;
        dashCollider.isTrigger = false;
        rb2D.velocity = new Vector2(dashSpeed * transform.localScale.x, 0f);

        yield return new WaitForSeconds(dashTime);

        col.enabled = true;
        dashCollider.isTrigger = true;
        rb2D.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    public void KnockBack(Transform t)
    {
        var dir = center.position - t.position;
        knockBacked = true;
        rb2D.velocity = dir.normalized * knockBackVel;
        Debug.Log("Knockback");
    }

    //For platform trigger when the player is on ladder
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            onLadder = true;
            if(vertical != 0)
                other.GetComponent<LadderController>().onLadderPlatform.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Ladder") && onLadder)
        {
            rb2D.gravityScale = originalGravity;
            other.GetComponent<LadderController>().onLadderPlatform.isTrigger = false;
            onLadder = false;
        }
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(checkGroundPoint.position, radOfGroundCircle, whatIsGround);
        isWalling = Physics2D.OverlapCircle(checkWallPoint.position, radOfWallingCircle, whatIsGround);
        isGrab = Physics2D.OverlapCircle(checkGrabPoint.position, radOfWallingCircle, whatIsGround);
        isBorderPlatform = Physics2D.OverlapCircle(checkBorderPlatform.position, radOfWallingCircle, whatIsGround);
    }


    //Flip player
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    //Player animations
    void SetAnimationState()
    {
        anim.SetFloat("Run", Mathf.Abs(horizontal));
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("Dashing", isDashing);
        anim.SetBool("IsGrab", isGrab);
        anim.SetBool("IsWalling", isWalling);
        anim.SetBool("IsBorderPlatform", isBorderPlatform);
        anim.SetBool("OnLadder", onLadder);
        anim.SetFloat("LadderVelocityY", Mathf.Abs(vertical));
        anim.SetFloat("VelocityY", rb2D.velocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkGroundPoint.position, radOfGroundCircle);
        Gizmos.DrawWireSphere(checkWallPoint.position, radOfWallingCircle);
        Gizmos.DrawWireSphere(checkGrabPoint.position, radOfWallingCircle);
        Gizmos.DrawWireSphere(checkBorderPlatform.position, radOfWallingCircle);
    }
}
using System.Collections;
using UnityEngine;

public class PlayerJumping : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private bool isFacingRight = true;

    private bool isJumping;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [SerializeField] private float perfectJumpTime = 0.1f;
    //[SerializeField] private float 
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    Rigidbody2D rb;

    //Jump related Counters
    float jumpBufferCounter = 0;
    float jumpTime = 0;
    float timeOnGround = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        //if (IsGrounded())
        //{
        //    coyoteTimeCounter = coyoteTime;
        //}
        //else
        //{
        //    coyoteTimeCounter -= Time.deltaTime;
        //}

        StartGroundedTimer();
        StartJumpBufferCounter();
        VariableJumping();

        //if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

        //    coyoteTimeCounter = 0f;
        //}

        //Flip();
    }

    private void VariableJumping()
    {
        if (IsGrounded() && jumpBufferCounter > 0)
        {
            float jumpMultiplier = 1f;
            if (jumpTime != 0)
            {
                if (jumpTime > 0 && jumpTime < perfectJumpTime)
                {
                    //Debug.Log($"<color=magenta>PERFECT : {jumpTime.ToString("F3")}</color>");
                    jumpMultiplier = 3f;
                }
                else if (jumpTime > perfectJumpTime && jumpTime < perfectJumpTime*3)
                {
                    //Debug.Log($"<color=cyan>GREAT : {jumpTime.ToString("F3")}</color>");
                    jumpMultiplier = 2f;
                }
                else if (jumpTime > perfectJumpTime*3)
                {
                    //Debug.Log($"<color=yellow>OK : {jumpTime.ToString("F3")}</color>");
                    jumpMultiplier = 1f;
                }

            }
            else
            {
                if (timeOnGround > 0 && timeOnGround < perfectJumpTime)
                {
                    //Debug.Log($"<color=magenta>PERFECT : {timeOnGround.ToString("F3")}</color>");
                    jumpMultiplier = 3f;
                }
                else if (timeOnGround > perfectJumpTime && timeOnGround < perfectJumpTime*3)
                {
                    //Debug.Log($"<color=cyan>GREAT : {timeOnGround.ToString("F3")}</color>");
                    jumpMultiplier = 2f;
                }
                else if (timeOnGround > perfectJumpTime*3)
                {
                    //Debug.Log($"<color=yellow>OK : {timeOnGround.ToString("F3")}</color>");
                    jumpMultiplier = 1;
                }
            }

            //Jumps to a specific jump height
            rb.velocity = new Vector2(0, Mathf.Sqrt(-2.0f * rb.gravityScale * Physics2D.gravity.y * jumpHeight * jumpMultiplier));

            jumpBufferCounter = 0;
            isJumping = true;
        }
    }

    private void StartJumpBufferCounter()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
            jumpTime = 0;
        }
        else
        {
            if (jumpBufferCounter >= 0)
                jumpBufferCounter -= Time.deltaTime;
            jumpTime += Time.deltaTime;
        }
    }

    private void StartGroundedTimer()
    {
        if (IsGrounded())
        {
            isJumping = false;
            if (timeOnGround < jumpBufferTime)
            {
                timeOnGround += Time.deltaTime;
            }
        }
        else
        {
            timeOnGround = 0;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

}
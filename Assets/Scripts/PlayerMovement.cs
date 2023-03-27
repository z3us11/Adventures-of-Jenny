using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] VariableJoystick joystick;
    [SerializeField] private float perfectJumpTime = 0.1f;
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform[] groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float horizontalSpeed = 8f;
    [SerializeField] private float wallSlidingSpeed = 5f;


    //Wall Sliding and Jumping
    [Header("Wall Jump")]
    private bool isWallJumping;
    private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    private float horizontal;
    private bool isFacingRight = true;


    private bool isJumping;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    Rigidbody2D rb;
    SpriteRenderer playerColor;

    //Jump related Counters
    float jumpBufferCounter = 0;
    float jumpTime = 0;
    float timeOnGround = 0;
    float jumpMultiplier = 1f;

    //float distanceFromGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerColor = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Application.targetFrameRate = 60;
        //distanceFromGround = GetComponent<Collider2D>().bounds.extents.y;
        
        HorizontalMovement();
        StartGroundedTimer();
        StartJumpBufferCounter();
        VariableJumping();
        //WallSliding();
        //WallJump();

        //if (IsGrounded())
        //{
        //    coyoteTimeCounter = coyoteTime;
        //}
        //else
        //{
        //    coyoteTimeCounter -= Time.deltaTime;
        //}

        //if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

        //    coyoteTimeCounter = 0f;
        //}

        if (!IsWallSliding())
            Flip();
    }

    private void HorizontalMovement()
    {
        horizontal = joystick.Horizontal;
        Vector2 newPos = new Vector2(horizontal * horizontalSpeed * Time.deltaTime, 0);
        transform.Translate(newPos);
        //rb.velocity = new Vector2(horizontal * horizontalSpeed * Time.deltaTime, rb.velocity.y);
    }

    private void VariableJumping()
    {
        if (IsGrounded() && jumpBufferCounter > 0)
        {
            if (jumpTime != 0)
            {
                if (jumpTime > 0 && jumpTime < perfectJumpTime)
                {
                    if (jumpMultiplier < 4)
                        jumpMultiplier++;
                    Debug.Log($"<color=magenta>PERFECT : {jumpTime.ToString("F3")}</color> |  {jumpMultiplier}");
                }
                else if (jumpTime > perfectJumpTime && jumpTime < perfectJumpTime*3)
                {
                    jumpMultiplier = 2f;
                    Debug.Log($"<color=cyan>GREAT : {jumpTime.ToString("F3")}</color> |  {jumpMultiplier}");
                }
                else if (jumpTime > perfectJumpTime*3)
                {
                    jumpMultiplier = 1f;
                    Debug.Log($"<color=yellow>OK : {jumpTime.ToString("F3")}</color> | {jumpMultiplier}");
                }

            }
            //After touching ground logic
            else
            {
                if (timeOnGround > 0 && timeOnGround < perfectJumpTime)
                {
                    if (jumpMultiplier < 4)
                        jumpMultiplier++;
                    Debug.Log($"<color=magenta>PERFECT : {timeOnGround.ToString("F3")}</color> | {jumpMultiplier}");
                }
                else if (timeOnGround > perfectJumpTime && timeOnGround < perfectJumpTime*3)
                {
                    jumpMultiplier = 2f;
                    Debug.Log($"<color=cyan>GREAT : {timeOnGround.ToString("F3")}</color> | {jumpMultiplier}" );
                }
                else if (timeOnGround > perfectJumpTime*3)
                {
                    jumpMultiplier = 1;
                    Debug.Log($"<color=yellow>OK : {timeOnGround.ToString("F3")}</color> | {jumpMultiplier}");
                }
            }

            //Jumps to a specific jump height
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(-2.0f * rb.gravityScale * Physics2D.gravity.y * jumpHeight * jumpMultiplier));

            jumpBufferCounter = 0;
            isJumping = true;
        }
    }

    private void StartJumpBufferCounter()
    {
        //if (Input.GetButtonDown("Jump"))
        if(joystick.JoystickReleased)
        {
            jumpBufferCounter = jumpBufferTime;
            jumpTime = 0;
            joystick.JoystickReleased = false;
        }
        else
        {
            if (jumpBufferCounter >= 0)
                jumpBufferCounter -= Time.deltaTime;
            jumpTime += Time.deltaTime;
            
            //if (jumpTime > 0 && jumpTime < perfectJumpTime)
            //{
            //    playerColor.color = Color.magenta;
            //}
            //else if (jumpTime > perfectJumpTime && jumpTime < perfectJumpTime * 3)
            //{
            //    playerColor.color = Color.cyan;
            //}
            //else if (jumpTime > perfectJumpTime * 3)
            //{
            //    playerColor.color = Color.yellow;
            //}
            //Debug.Log(jumpTime);
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

            //if (timeOnGround > 0 && timeOnGround < perfectJumpTime)
            //{
            //    playerColor.color = Color.magenta;
            //}
            //else if (timeOnGround > perfectJumpTime && timeOnGround < perfectJumpTime * 3)
            //{
            //    playerColor.color = Color.blue;
            //}
            //else if (timeOnGround > perfectJumpTime * 3)
            //{
            //    playerColor.color = Color.yellow;
            //}

            //if(!joystick.JoystickReleased)
            //    horizontal = joystick.Horizontal;
        }
        else
        {
            timeOnGround = 0;
            //if (joystick.Horizontal != 0)
            //    horizontal = joystick.Horizontal;
        }
    }

    private void FixedUpdate()
    {
    }

    private bool IsGrounded()
    {
        //return Physics2D.Raycast(transform.position, Vector2.down, distanceFromGround);
        if (rb.velocity.y > 0)
            return false;

        foreach(var ground in groundCheck)
        {
            if(Physics2D.OverlapCircle(ground.position, 0.2f, groundLayer))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsFacingFront()
    {
        foreach (var ground in groundCheck)
        {
            if (Physics2D.OverlapCircle(ground.position, 0.2f, wallLayer))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsWallSliding()
    {
        if (IsFacingFront() && !IsGrounded() && horizontal != 0)
            return true;
        else  
            return false;
    }

    private void WallSliding()
    {
        if(IsWallSliding())
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
    }

    private void WallJump()
    {
        if (IsWallSliding())
        {
            isWallJumping = false;
            wallJumpingDirection = (transform.localScale.x < 0) ? -1 : 1;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0 && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
        
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
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
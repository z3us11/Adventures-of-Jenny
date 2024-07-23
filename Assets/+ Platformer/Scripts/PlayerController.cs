using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public bool canPlayerMove = true;
        [Header("Ability Unlocks")]
        public bool canLedgeGrab;
        public bool canWallJump;
        public bool canWallRun;
        public bool canSprint;

        float xInput = 0;
        float yInput = 0;
        bool isSprintPressed = false;
        bool isJumpPressed = false;
        [Header("Movement")]
        [SerializeField] float walkVelocity = 5f;
        [SerializeField] float walkAcceleration = 10f;
        [SerializeField] float sprintVelocity;
        [SerializeField] float sprintAcceleration;
        float velocity = 0;
        float acceleration = 0;
        float currentFrameVelocity = 0;
        [Header("Jump")]
        [SerializeField] float jumpHeight;
        [SerializeField] float jumpMultiplier;
        [SerializeField] float maxFallingVelocity;
        float jumpButtonPressedTimer;
        bool isJumpingFromLedgeOrWall = false;
        bool isPerfectJump;
        [SerializeField] float jumpBuffer;
        float jumpBufferCounter;
        [SerializeField] float coyoteTime;
        float coyoteTimeCounter;
        
        [SerializeField] float perfectJumpTime;
        [SerializeField] float perfectJumpTimeWindow;
        [SerializeField] float jumpButtonPressThreshold;
        [SerializeField] private Transform[] groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [Header("Ledge Grab")]
        [SerializeField] Transform upperCheck;
        [SerializeField] Transform lowerCheck;
        [SerializeField] float ledgeGrabDistance;
        [SerializeField] LayerMask ledgeLayer;
        bool isLedgeGrabbing;
        bool isTouchingLedge;
        bool isClimbingFromLedge;
        public bool IsTouchingLedge { get { return isTouchingLedge; } set { isTouchingLedge = value; } }
        Ledge currentLedge;
        public Ledge CurrentLedge { get { return currentLedge; } set { currentLedge = value; } }
        [Header("Wall Slide")]
        [SerializeField] Transform wallCheck;
        [SerializeField] LayerMask wallLayer;
        [SerializeField] float wallSlidingVelocity;
        bool isScaleSet = false;
        bool isWallRunning = false;
        [Space]
        [Header("Effects")]
        [SerializeField] Transform playerGround;
        [SerializeField] Transform playerWall;
        [SerializeField] GameObject grassParticle;
        [SerializeField] GameObject grassParticlePerfectJump;
        [Space]
        [SerializeField] GameObject playerShadow;
        [SerializeField] GameObject jumpEffectRing;
        [SerializeField] GameObject jumpEffectSprite;
        [SerializeField] GameObject jumpEffectParticle;

        bool isSpawningWalkParticles = false;
        bool isJumping = false;
        [Header("Camera")]
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        [SerializeField] CinemachineVirtualCamera mapCamera;
        [SerializeField] int cameraZoomedInSize;
        [SerializeField] int cameraZoomedOutSize;
        [SerializeField] float cameraZoomSpeed;
        Vector3 cameraStartPositionBeforePanning;
        bool isPlayerPanningCamera = false;
        bool startedLookingX = false;
        bool startedLookingY = false;
        bool startedLookingBack = false;
        bool lookBackComplete = false;
        bool isMapButtonPressed = false;
        [Header("UI")]
        public AbilityUnlockPanel abilityUnlockPanel;
        public Toggle mobileControlsToggle;
        [Space]
        [Header("Other Scripts")]
        public FlowerCollection flowerCollection;
        public StaminaConfidence staminaConfidence;
        public MobileControls mobileControls;
        [Space]
        [SerializeField] Transform visuals;
        [SerializeField] Animator[] playerAnims;

        bool isWalking = false;
        bool isSprinting = false;
        bool isGrounded = false;
        bool isSprintingWhileJumping = false;

        float rightXInput = 0;
        float rightYInput = 0;

        private Rigidbody2D rb;
        private Animator perfectJumpAnim;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            CheckForMobileControls();
        }

        void OnMoveX(InputValue inputValue)
        {
            xInput = inputValue.Get<float>();
        }

        void OnMoveY(InputValue inputValue)
        {
            yInput = inputValue.Get<float>();
        }

        void OnSprint(InputValue inputValue)
        {
            isSprintPressed = inputValue.Get<float>() != 0 ? true : false;
        }

        void OnJump(InputValue inputValue)
        {
            isJumpPressed = inputValue.Get<float>() != 0 ? true : false;
        }

        void OnZoom(InputValue inputValue)
        {
            isPlayerPanningCamera = inputValue.Get<float>() != 0 ? true : false;
        }

        void OnMap(InputValue inputValue)
        {
            isMapButtonPressed = inputValue.Get<float>() != 0 ? true : false;
            
            if(isMapButtonPressed)
                Map(true);
            //else
            //{
            //    virtualCamera.gameObject.SetActive(true);
            //    mapCamera.gameObject.SetActive(false);
            //}
        }

        public void Map(bool open)
        {
            if (open)
            {
                virtualCamera.gameObject.SetActive(false);
                mapCamera.gameObject.SetActive(true);
            }
            else
            {
                virtualCamera.gameObject.SetActive(true);
                mapCamera.gameObject.SetActive(false);
            }
        }

        void OnMoveRightX(InputValue inputValue)
        {
            rightXInput = inputValue.Get<float>();
        }

        void OnMoveRightY(InputValue inputValue)
        {
            rightYInput = inputValue.Get<float>();
        }

        private void CheckForMobileControls()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Application.targetFrameRate = 60;

                mobileControlsToggle.isOn = true;
                mobileControlsToggle.gameObject.SetActive(false);
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                mobileControlsToggle.isOn = false;
                mobileControlsToggle.gameObject.SetActive(false);
            }
            else if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                mobileControlsToggle.isOn = false;
            }
        }

        private void FixedUpdate()
        {
            if (!canPlayerMove)
                return;
            if (virtualCamera.Follow == null)
                return;

            MoveWithAcceleration();
        }

        private void MoveWithAcceleration()
        {
            if (isLedgeGrabbing)
                return;

            // Apply acceleration
            float targetVelocity = xInput * velocity;
            float currentVelocity = rb.velocity.x;
            float deltaVelocity = targetVelocity - currentVelocity;

            // Clamp the delta velocity to acceleration


            if (Mathf.Sign(xInput) != Mathf.Sign(rb.velocity.x))
                deltaVelocity *= 2f;
            deltaVelocity = Mathf.Clamp(deltaVelocity, -acceleration * Time.fixedDeltaTime, acceleration * Time.fixedDeltaTime);

            // Apply the force to the rigidbody
            rb.AddForce(new Vector2(deltaVelocity, 0), ForceMode2D.Impulse);
            currentFrameVelocity = rb.velocity.x;

            if (Mathf.Abs(rb.velocity.x) < 0.1f)
                rb.velocity = new Vector2(0, rb.velocity.y);

            if (rb.velocity.y < maxFallingVelocity)
                rb.velocity = new Vector2(rb.velocity.x, maxFallingVelocity);

        }

        private void Update()
        {
            if(mobileControls.gameObject.activeSelf)
            {
                xInput = mobileControls.onScreenXInput;
                yInput = mobileControls.onScreenYInput;
                isJumpPressed = mobileControls.onScreenJumpBtnPressed;
                isSprintPressed = mobileControls.onScreenSprintBtnPressed;

                mobileControls.sprintBtn.gameObject.SetActive(canSprint);
            }

            if (!canPlayerMove)
                return;

            playerShadow.transform.localScale = visuals.transform.localScale;

            if (!isLedgeGrabbing)
            {
                MovementValuesIfSprinting();
                LedgeGrab();

                //Animations
                ChangeWalkingState();
                ChangeJumpState();

                //Wall Slide
                WallSliding();
                WallRun();

                //Effects
                StartCoroutine(WalkParticles(playerGround));
            }

            if (isTouchingLedge)
            {
                if (IsFacingFront() && !IsGrounded())
                {
                    //GrabLedge(currentLedge);
                }
                else
                {
                    if (yInput < 0)
                    {
                        if (currentLedge != null)
                        {
                            if (currentLedge.transform.localPosition.x < 0)
                                visuals.transform.localScale = new Vector3(1, 1, 1);
                            else if (currentLedge.transform.localPosition.x > 0)
                                visuals.transform.localScale = new Vector3(-1, 1, 1);
                            GrabLedge(currentLedge);
                        }
                    }
                }
            }

            if (isLedgeGrabbing)
            {
                StartCoroutine(DropFromLedgeGrab());
                StartCoroutine(ClimbFromLedgeGrab());
            }

            //Jumping
            UpdateJumpBufferCounter();
            Jump();
            if (rb.velocity.y < 0 && !IsWallSliding())
                StartCoroutine(IsJumping());
            UpdateCoyoteTimeCounter();
            jumpEffectParticle.SetActive(jumpEffectSprite.GetComponent<SpriteRenderer>().enabled);

            //Camera
            ZoomCamera();
            /*
            PanCamera();
            */
        }


        void MovementValuesIfSprinting()
        {
            if (isSprintPressed)
            {
                if (!canSprint)
                    return;

                if (!IsGrounded())
                {
                    if (isSprintingWhileJumping)
                    {
                        velocity = sprintVelocity;
                        acceleration = sprintAcceleration;

                    }
                    else
                    {
                        velocity = walkVelocity;
                        acceleration = walkAcceleration;
                    }
                }
                else
                {
                    velocity = sprintVelocity;
                    acceleration = sprintAcceleration;
                }

            }
            else
            {
                velocity = walkVelocity;
                acceleration = walkAcceleration;
            }


            if (Mathf.Abs(rb.velocity.x) > walkVelocity)
            {
                acceleration = sprintAcceleration;
            }
        }

        public void UpdateCoyoteTimeCounter()
        {
            if(IsGrounded())
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                if(coyoteTimeCounter > 0)
                    coyoteTimeCounter -= Time.deltaTime;
                else
                    coyoteTimeCounter = 0;
            }
        }

        public void UpdateJumpBufferCounter()
        {
            if(!isJumpPressed && jumpButtonPressedTimer > 0.01f)
                jumpBufferCounter = jumpBuffer;
            else
            {
                if (jumpBufferCounter > 0)
                    jumpBufferCounter -= Time.deltaTime;
                else
                    jumpBufferCounter = 0;
            }

        }

        bool CanJump()
        {
            //return IsGrounded(); 

            if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
            {
                return true;

            }
            else
                return false;
        }


        void Jump()
        {
            if (isJumpPressed)
            {
                jumpEffectRing.SetActive(true);
                if (jumpButtonPressedTimer < jumpButtonPressThreshold)
                {
                    jumpButtonPressedTimer += Time.deltaTime;
                    if (jumpButtonPressedTimer <= perfectJumpTime)
                    {
                        float scale = jumpEffectRing.transform.localScale.x;
                        scale = Remap(jumpButtonPressedTimer, 0, perfectJumpTime, 2, 0);
                        jumpEffectRing.transform.localScale = new Vector3(scale, scale);
                    }

                    if (jumpButtonPressedTimer > (perfectJumpTime - perfectJumpTimeWindow) && jumpButtonPressedTimer < (perfectJumpTime + perfectJumpTimeWindow))
                    {
                        jumpEffectSprite.GetComponent<SpriteRenderer>().enabled = true;
                        visuals.GetComponent<SpriteRenderer>().enabled = false;
                        Time.timeScale = 0.75f;
                        if(staminaConfidence.GetStaminConfidenceValue() > 75)
                        {
                            jumpButtonPressedTimer = perfectJumpTime;
                        }
                            
                        //jumpEffectSprite.GetComponent<SpriteRenderer>().sprite = visuals.GetComponent<SpriteRenderer>().sprite;
                    }
                    else
                    {
                        if(jumpButtonPressedTimer > (perfectJumpTime + perfectJumpTimeWindow))
                        {
                            jumpEffectSprite.GetComponent<SpriteRenderer>().enabled = false;
                            visuals.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        Time.timeScale = 1f;
                    }

                }
                else
                {
                    //isJumpPressed = false;
                    jumpButtonPressedTimer = jumpButtonPressThreshold;
                    jumpEffectRing.SetActive(false);
                    jumpEffectSprite.GetComponent<SpriteRenderer>().enabled = false;
                    visuals.GetComponent<SpriteRenderer>().enabled = true;
                    Time.timeScale = 1f;
                }
            }
            else
            {
                isPerfectJump = false;
                if (jumpButtonPressedTimer > 0.01f)
                {
                    if (jumpButtonPressedTimer > (perfectJumpTime - perfectJumpTimeWindow) && jumpButtonPressedTimer < (perfectJumpTime + perfectJumpTimeWindow))
                    {
                        isPerfectJump = true;
                        jumpButtonPressedTimer = perfectJumpTime;
                        jumpMultiplier = 1;
                    }
                    else
                    {
                        jumpMultiplier = (perfectJumpTime - Mathf.Abs(jumpButtonPressedTimer - perfectJumpTime)) / perfectJumpTime;

                    }

                    if (jumpButtonPressedTimer > perfectJumpTime && jumpMultiplier < 0.75f)
                        jumpMultiplier = 1f;
                    else if (jumpMultiplier < 0.25f)
                        jumpMultiplier = 0.25f;


                    //float jumpAccuracy = Mathf.Abs(jumpButtonPressedTimer - perfectJumpTime);
                    //if (jumpAccuracy > jumpButtonPressThreshold - perfectJumpTime - 0.1f)
                    //    jumpAccuracy = perfectJumpTime - perfectJumpTimeWindow;
                    //if (jumpAccuracy < perfectJumpTimeWindow)
                    //    jumpMultiplier = 1;
                    //else
                    //    jumpMultiplier = Remap(jumpAccuracy, perfectJumpTimeWindow, perfectJumpTime, 1, 0.25f);

                    //if (jumpButtonPressedTimer > perfectJumpTime)
                    //    jumpButtonPressedTimer = perfectJumpTime;

                    if (CanJump())
                    {
                        jumpButtonPressedTimer = 0;
                        coyoteTimeCounter = 0;
                        jumpBufferCounter = 0;
                        ApplyJumpForce();
                    }
                    //else
                    //{
                    //    if(!isJumping)
                    //    {
                    //        jumpEffectSprite.SetActive(false);
                    //        visuals.GetComponent<SpriteRenderer>().enabled = true;
                    //    }
                    //}

                    if (canWallJump)
                    {
                        if (!IsGrounded())
                        {
                            if (isLedgeGrabbing)
                            {
                                ReleaseLedge();
                                ApplyJumpForce(false);
                            }
                            else if (IsWallSliding())
                            {
                                ApplyJumpForce(false);
                            }
                        }
                    }
                }

                StartCoroutine(ResetJumpButtonPressedTimer());

                jumpEffectRing.SetActive(false);
                Time.timeScale = 1f;

                //jumpEffectSprite.SetActive(false);
            }
        }

        IEnumerator ResetJumpButtonPressedTimer()
        {
            yield return new WaitForSeconds(0.1f);
            jumpButtonPressedTimer = 0;
        }

        void ApplyJumpForce(bool isNormalJump = true)
        {
            StartCoroutine(IsJumping());
            if (isSprintPressed)
                isSprintingWhileJumping = true;
            else
                isSprintingWhileJumping = false;

            if (isNormalJump)
            {
                //rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(-2.0f * rb.gravityScale * Physics2D.gravity.y * jumpHeight * jumpMultiplier * (jumpButtonPressedTimer / perfectJumpTime)));
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(-2.0f * rb.gravityScale * Physics2D.gravity.y * jumpHeight * jumpMultiplier));
                SpawnGrassParticle(playerGround);
            }
            else
            {
                StartCoroutine(JumpFromLedgeOrWall());
                SpawnGrassParticle(playerWall, false, true);
            }
        }

        IEnumerator IsJumping()
        {
            if (isJumping)
                yield break;
            yield return new WaitForSeconds(0.1f);
            isJumping = true;
        }

        IEnumerator JumpFromLedgeOrWall()
        {
            isJumpingFromLedgeOrWall = true;
            float jumpVelocity = Mathf.Sqrt(-2.0f * rb.gravityScale * Physics2D.gravity.y * jumpHeight);
            rb.velocity = new Vector2(visuals.transform.localScale.x * -jumpVelocity / 1.65f, jumpVelocity / 1.25f);
            yield return new WaitForSeconds(0.1f);
            isJumpingFromLedgeOrWall = false;
        }


        private void WallSliding()
        {
            if (IsWallSliding())
            {
                if (!isScaleSet)
                {
                    var scaleX = visuals.transform.localScale.x;
                    visuals.transform.localScale = new Vector3(scaleX, 1, 1);
                }
                if (isJumping)
                {
                    isJumping = false;
                    jumpEffectSprite.GetComponent<SpriteRenderer>().enabled = false;
                    visuals.GetComponent<SpriteRenderer>().enabled = true;
                }
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingVelocity, float.MaxValue));
                StartCoroutine(WalkParticles(playerWall));
            }
        }

        private void WallRun()
        {
            if (!canWallRun)
                return;
            if (isLedgeGrabbing)
                return;
            if (staminaConfidence.GetStaminConfidenceValue() == 0)
                return;

            if (IsWallSliding() && xInput != 0)
            {
                if (Mathf.Sign(xInput) == Mathf.Sign(visuals.transform.localScale.x))
                {
                    isWallRunning = true;
                    rb.velocity = new Vector2(rb.velocity.x, walkVelocity);
                    StartCoroutine(WalkParticles(playerWall));

                }
            }
            else
            {
                isWallRunning = false;
            }
        }


        void LedgeGrab()
        {
            if (!canLedgeGrab)
                return;
            if (isLedgeGrabbing)
                return;

            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position, Vector2.right * visuals.localScale.x, ledgeGrabDistance, ledgeLayer);
            if (hit.collider != null)
            {
                GrabLedge(hit.transform.GetComponent<Ledge>());
            }
        }

        private void GrabLedge(Ledge ledge)
        {
            if (!canLedgeGrab)
                return;
            if (isLedgeGrabbing)
                return;
            if (isClimbingFromLedge)
                return;
            if (isJumpingFromLedgeOrWall)
                return;

            Debug.Log("Grabbing Ledge");
            yInput = 0;
            isTouchingLedge = false;
            isLedgeGrabbing = true;
            for(int i = 0; i < playerAnims.Length; i++)
                playerAnims[i].SetBool(nameof(isLedgeGrabbing), true);
            transform.DOMove(ledge.transform.position - new Vector3(visuals.transform.localScale.x * 0.1f, 0.10f), 0.1f);
            //transform.DOMove(ledge.transform.position, 0f);
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            if (isJumping)
            {
                isJumping = false;
                jumpEffectSprite.GetComponent<SpriteRenderer>().enabled = false;
                visuals.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        void ReleaseLedge()
        {
            Debug.Log("Releasing Ledge");
            rb.gravityScale = 4;
            isLedgeGrabbing = false;
            for(int i = 0; i < playerAnims.Length; i++)
                playerAnims[i].SetBool(nameof(isLedgeGrabbing), false);
            currentLedge = null;
        }

        IEnumerator DropFromLedgeGrab()
        {
            yield return new WaitForSeconds(0.1f);
            if (yInput < 0)
                ReleaseLedge();
        }

        IEnumerator ClimbFromLedgeGrab()
        {
            if (yInput > 0)
            {
                Debug.Log("Climbing");
                isClimbingFromLedge = true;
                ReleaseLedge();
                rb.velocity = new Vector2(0, 15);
                yield return new WaitForSeconds(0.1f);
                rb.velocity = new Vector2(visuals.transform.localScale.x * 5, 0);
                isClimbingFromLedge = false;
            }
        }

        public bool IsGrounded()
        {
            if (isLedgeGrabbing)
            {
                return isGrounded = false;
            }
            //return Physics2D.Raycast(transform.position, Vector2.down, distanceFromGround);
            //if (rb.velocity.y > 0)
            //    return isGrounded = false;

            foreach (var ground in groundCheck)
            {
                //if (Physics2D.OverlapCircle(ground.position, 0.2f, groundLayer))
                var groundHit = Physics2D.Raycast(ground.transform.position, Vector2.down, 0.2f, groundLayer);
                if (groundHit.collider != null)
                {
                    if (isJumping)
                    {
                        isJumping = false;
                        jumpEffectSprite.GetComponent<SpriteRenderer>().enabled = false;
                        visuals.GetComponent<SpriteRenderer>().enabled = true;
                    }
                    return isGrounded = true;
                }
            }
            return isGrounded = false;
        }

        

        private bool IsWalking()
        {
            return isWalking = (Math.Abs(rb.velocity.x) > 1f) ? true : false;
        }

        private bool IsSprinting()
        {
            isSprinting = Mathf.Abs(rb.velocity.x) > walkVelocity + 1f ? true : false;
            //if (afterLedgeGrab)
            //    Debug.Log(Mathf.Abs(rb.velocity.x));
            if (isSprinting && staminaConfidence.GetStaminConfidenceValue() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsWallSliding()
        {
            if (IsFacingFront() && !IsGrounded())
            {
                isScaleSet = true;
                return true;
            }
            else
            {
                isScaleSet = false;
                return false;
            }
        }

        private bool IsFacingFront()
        {
            if (Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallLayer))
            {
                return true;
            }

            //foreach (var ground in groundCheck)
            //{
            //    if (Physics2D.OverlapCircle(ground.position, 0.1f, wallLayer))
            //    {
            //        return true;
            //    }
            //}
            return false;
        }


        //Animations
        private void ChangeWalkingState()
        {
            for(int i = 0; i < playerAnims.Length; i++)
                playerAnims[i].SetBool(nameof(isWalking), IsWalking());
            for(int i = 0; i < playerAnims.Length; i++)
                playerAnims[i].SetBool(nameof(isWalking), IsWalking());
            for(int i = 0; i < playerAnims.Length; i++)
                playerAnims[i].speed = IsSprinting() ? 1.5f : 1.0f;

            if (IsWallSliding())
                return;

            if (rb.velocity.x > 1f)
            {
                visuals.localScale = new Vector3(1, 1, 1);
            }
            else if (rb.velocity.x < -1f)
            {
                visuals.localScale = new Vector3(-1, 1, 1);
            }
        }

        private void ChangeJumpState()
        {
            if (rb.velocity.y > 0.1f || rb.velocity.y < -0.1f)
                for(int i = 0; i < playerAnims.Length; i++)
                    playerAnims[i].SetFloat("yVelocity", rb.velocity.y);
            else
                for(int i = 0; i < playerAnims.Length; i++)
                    playerAnims[i].SetFloat("yVelocity", 0);

            if (IsGrounded())
            {
                for(int i = 0; i < playerAnims.Length; i++)
                    playerAnims[i].SetInteger("velocity", 0);
                for(int i = 0; i < playerAnims.Length; i++)
                    playerAnims[i].SetInteger("velocity", 0);
            }
            else
            {
                if (rb.velocity.y > 0)
                {
                    for(int i = 0; i < playerAnims.Length; i++)
                        playerAnims[i].SetInteger("velocity", 1);
                    for(int i = 0; i < playerAnims.Length; i++)
                        playerAnims[i].SetInteger("velocity", 1);

                }
                else if (rb.velocity.y < 0)
                {
                    for(int i = 0; i < playerAnims.Length; i++)
                        playerAnims[i].SetInteger("velocity", -1);
                    for(int i = 0; i < playerAnims.Length; i++)
                        playerAnims[i].SetInteger("velocity", -1);
                }
            }

            for(int i = 0; i < playerAnims.Length; i++)
                playerAnims[i].SetBool(nameof(IsGrounded), IsGrounded());
            for(int i = 0; i < playerAnims.Length; i++)
                playerAnims[i].SetBool(nameof(IsWallSliding), IsWallSliding());
        }


        public void UnlockAbility(AbilityType ability)
        {
            if (ability == AbilityType.LedgeGrab)
            {
                canLedgeGrab = true;
            }
            else if (ability == AbilityType.WallJump)
            {
                canWallJump = true;
            }
            else if (ability == AbilityType.WallRun)
            {
                canWallRun = true;
            }
            else if (ability == AbilityType.Sprint)
            {
                canSprint = true;
            }
        }

        //Effects
        private void SpawnGrassParticle(Transform location, bool isWalkParticle = false, bool isWallParticle = false)
        {
            GameObject particle;
            if (isPerfectJump)
            {
                particle = Instantiate(grassParticlePerfectJump, location.position, grassParticle.transform.rotation);
                staminaConfidence.UpdateStaminaConfidence(15);
            }
            else
            {
                particle = Instantiate(grassParticle, location.position, grassParticle.transform.rotation);
                if (jumpMultiplier == 1f && !isWalkParticle)
                    staminaConfidence.UpdateStaminaConfidence(-5f);

            }
            particle.transform.localPosition += new Vector3(0, 0.5f);
            if (isWalkParticle)
            {
                var particleSystem = particle.GetComponent<ParticleSystem>();
                particleSystem.startSpeed = 2;
                particleSystem.gravityModifier = 0.5f;
                var emission = particleSystem.emission;
                emission.SetBursts(new ParticleSystem.Burst[]{
                new ParticleSystem.Burst(0.0f, 2)});
                if (isWallParticle)
                {
                    particle.transform.Rotate(new Vector3(0, -visuals.transform.localScale.x * 90, visuals.transform.localScale.x * 90));
                    particleSystem.startSpeed = 1;
                }
            }
            else
            {
                if(isWallParticle)
                {
                    var particleSystem = particle.GetComponent<ParticleSystem>();
                    particle.transform.Rotate(new Vector3(0, -visuals.transform.localScale.x * 90, visuals.transform.localScale.x * 90));
                    particleSystem.startSpeed = 1;

                }
            }
            //particle.transform.localRotation = new Quaternion(-90, 0, 0, 0);
            Destroy(particle, 1);
        }

        private IEnumerator WalkParticles(Transform spawnPoint)
        {
            if (spawnPoint == playerGround)
            {
                if (isWalking && IsGrounded() && !isSpawningWalkParticles)
                {
                    isSpawningWalkParticles = true;
                    SpawnGrassParticle(spawnPoint, true);
                    if(IsSprinting())
                        staminaConfidence.UpdateStaminaConfidence(-5f);
                    yield return new WaitForSeconds(0.25f);
                    isSpawningWalkParticles = false;
                }
            }
            else if (spawnPoint == playerWall)
            {
                if (IsWallSliding() && !isSpawningWalkParticles)
                {
                    isSpawningWalkParticles = true;
                    SpawnGrassParticle(spawnPoint, true, true);
                    if(isWallRunning)
                        staminaConfidence.UpdateStaminaConfidence(-5f);
                    yield return new WaitForSeconds(0.35f);
                    isSpawningWalkParticles = false;
                }
            }

        }

        //Camera 
        void ZoomCamera()
        {
            float panSpeed = cameraZoomSpeed;
            if (isPlayerPanningCamera)
                panSpeed *= 5;

            if (IsSprinting() || isPlayerPanningCamera)
            {
                if (virtualCamera.m_Lens.OrthographicSize <= cameraZoomedOutSize)
                {
                    virtualCamera.m_Lens.OrthographicSize += Time.deltaTime * panSpeed;
                }
            }
            else
            {
                if (virtualCamera.m_Lens.OrthographicSize >= cameraZoomedInSize)
                {
                    virtualCamera.m_Lens.OrthographicSize -= Time.deltaTime * panSpeed * 5;
                }
                else
                {
                    if (!startedLookingX && !startedLookingY && lookBackComplete)
                        virtualCamera.Follow = transform;
                }
            }
        }

        private void PanCamera()
        {
            if (rightXInput == 0 && rightYInput == 0)
            {
                LookAtPlayer();
            }

            if (rb.velocity.magnitude < 0.5f)
            {
                if (rightXInput != 0)
                {
                    if (rightXInput < 0)
                        LookLeft();
                    else if (rightXInput > 0)
                        LookRight();
                }
                if (rightYInput != 0)
                {
                    if (rightYInput > 0)
                        LookUp();
                    else if (rightYInput < 0)
                        LookDown();
                }
            }


            if (lookBackComplete)
            {
                lookBackComplete = false;
                startedLookingBack = false;
            }
        }

        void LookAtPlayer()
        {
            if (!startedLookingX && !startedLookingY)
            {

                if (virtualCamera.Follow == transform)
                {
                    return;
                }
                if (!startedLookingBack)
                {
                    startedLookingBack = true;
                    lookBackComplete = false;
                    virtualCamera.transform.DOMove(cameraStartPositionBeforePanning, 0.5f).OnComplete(() => lookBackComplete = true);
                }
                if (lookBackComplete)
                {
                    virtualCamera.Follow = transform;
                }

            }

        }

        void LookLeft()
        {
            if (startedLookingX || Mathf.Abs(Vector2.Distance(virtualCamera.transform.position, transform.position)) > 4)
                return;
            startedLookingX = true;
            virtualCamera.Follow = null;
            lookBackComplete = false;
            cameraStartPositionBeforePanning = virtualCamera.transform.position;
            virtualCamera.transform.DOMoveX(transform.localPosition.x - 5, 0.5f).OnComplete(() => startedLookingX = false);
        }

        void LookRight()
        {
            if (startedLookingX || Mathf.Abs(Vector2.Distance(virtualCamera.transform.position, transform.position)) > 4)
                return;
            startedLookingX = true;
            virtualCamera.Follow = null;
            lookBackComplete = false;
            cameraStartPositionBeforePanning = virtualCamera.transform.position;
            virtualCamera.transform.DOMoveX(transform.localPosition.x + 5, 0.5f).OnComplete(() => startedLookingX = false);
        }


        void LookUp()
        {
            if (startedLookingY || Mathf.Abs(Vector2.Distance(virtualCamera.transform.position, transform.position)) > 4)
                return;
            startedLookingY = true;
            virtualCamera.Follow = null;
            lookBackComplete = false;
            cameraStartPositionBeforePanning = virtualCamera.transform.position;
            virtualCamera.transform.DOMoveY(transform.localPosition.y + 5, 0.5f).OnComplete(() => startedLookingY = false);
        }

        void LookDown()
        {
            if (startedLookingY || Mathf.Abs(Vector2.Distance(virtualCamera.transform.position, transform.position)) > 4)
                return;
            startedLookingY = true;
            virtualCamera.Follow = null;
            lookBackComplete = false;
            cameraStartPositionBeforePanning = virtualCamera.transform.position;
            virtualCamera.transform.DOMoveY(transform.localPosition.y - 5, 0.5f).OnComplete(() => startedLookingY = false);
        }




        //Utility
        float Remap(float x, float in_min, float in_max, float out_min, float out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;

        }


    }
}


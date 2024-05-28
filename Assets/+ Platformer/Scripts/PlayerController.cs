using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
        float jumpButtonPressedTimer;
        bool isJumpingFromLedgeOrWall = false;
        [SerializeField] float perfectJumpTime;
        [SerializeField] float perfectJumpTimeWindow;
        [SerializeField] float jumpButtonPressThreshold;
        [SerializeField] private Transform[] groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [Header("Ledge Grab")]
        [SerializeField] float ledgeGrabDistance;
        [SerializeField] LayerMask ledgeLayer;
        bool isLedgeGrabbing;
        bool isTouchingLedge;
        bool isClimbingFromLedge;
        public bool IsTouchingLedge { get { return isTouchingLedge; } set { isTouchingLedge = value; } }
        Ledge currentLedge;
        public Ledge CurrentLedge { get { return  currentLedge; } set {  currentLedge = value; } }
        [Header("Wall Slide")]
        [SerializeField] Transform wallCheck;
        [SerializeField] LayerMask wallLayer;
        [SerializeField] float wallSlidingVelocity;
        bool isScaleSet = false;
        [Space]
        [Header("Effects")]
        [SerializeField] Transform playerGround;
        [SerializeField] Transform playerWall;
        [SerializeField] GameObject grassParticle;
        [Space]
        [SerializeField] GameObject jumpEffectRing;
        [SerializeField] GameObject jumpEffectSprite;
        bool isSpawningWalkParticles = false;
        [Header("Camera")]
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        [SerializeField] int cameraZoomedInSize;
        [SerializeField] int cameraZoomedOutSize;
        [SerializeField] float cameraPanSpeed;
        bool isPlayerPanningCamera = false;
        [Header("UI")]
        public AbilityUnlockPanel abilityUnlockPanel;
        [Space]
        [Header("Other Scripts")]
        public FlowerCollection flowerCollection;

        bool isWalking = false;
        bool isSprinting = false;
        bool isGrounded = false;
        bool isSprintingWhileJumping = false;

        float rightYInput = 0;
        bool startedLooking = false;

        private Rigidbody2D rb;
        private Transform visuals;
        private Animator anim;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            visuals = transform.GetChild(0);
            anim = visuals.GetComponent<Animator>();
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

        void OnMoveRightY(InputValue inputValue)
        {
            rightYInput = inputValue.Get<float>();
        }

        private void FixedUpdate()
        {
            if (!canPlayerMove)
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
        }

        private void Update()
        {
            if(!canPlayerMove)
                return;

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
                if(IsFacingFront() && !IsGrounded())
                {
                    //GrabLedge(currentLedge);
                }
                else
                {
                    if(yInput < 0)
                    {
                        if (currentLedge.transform.localPosition.x < 0)
                            visuals.transform.localScale = new Vector3(1, 1, 1);
                        else if(currentLedge.transform.localPosition.x > 0)
                            visuals.transform.localScale = new Vector3(-1, 1, 1);

                        GrabLedge(currentLedge);
                    }
                }
            }

            if (isLedgeGrabbing)
            {
                StartCoroutine(DropFromLedgeGrab());
                StartCoroutine(ClimbFromLedgeGrab());
            }

            //Jumping
            Jump();

            //Camera
            PanCamera();

            if (rightYInput == 0)
            {
                LookAtPlayer();
                startedLooking = false;
            }
            else
            {
                if (rightYInput > 0)
                    LookUp();
                else if (rightYInput < 0)
                    LookDown();
                startedLooking = true;
            }

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

        void Jump()
        {
            if (isJumpPressed)
            {
                jumpEffectRing.SetActive(true);
                if (jumpButtonPressedTimer < jumpButtonPressThreshold)
                {
                    jumpButtonPressedTimer += Time.deltaTime;
                    if(jumpButtonPressedTimer <= perfectJumpTime)
                    {
                        float scale = jumpEffectRing.transform.localScale.x;
                        scale = Remap(jumpButtonPressedTimer, 0, perfectJumpTime, 2, 0);
                        jumpEffectRing.transform.localScale = new Vector3(scale, scale);
                    }

                    if(jumpButtonPressedTimer > (perfectJumpTime - perfectJumpTimeWindow) && jumpButtonPressedTimer < (perfectJumpTime + perfectJumpTimeWindow))
                    {
                        jumpEffectSprite.SetActive(true);
                        jumpEffectSprite.GetComponent<SpriteRenderer>().sprite = visuals.GetComponent<SpriteRenderer>().sprite;
                    }
                    else
                    {
                        jumpEffectSprite.SetActive(false);
                    }

                }
                else
                {
                    isJumpPressed = false;
                    jumpButtonPressedTimer = jumpButtonPressThreshold;
                    jumpEffectRing.SetActive(false);
                    jumpEffectSprite.SetActive(false);
                }
            }
            else
            {
                if (jumpButtonPressedTimer > 0.01f)
                {
                    if (jumpButtonPressedTimer > (perfectJumpTime - perfectJumpTimeWindow) && jumpButtonPressedTimer < (perfectJumpTime + perfectJumpTimeWindow))
                    {
                        jumpButtonPressedTimer = perfectJumpTime;
                        jumpMultiplier = 1;
                    }
                    else
                    {
                        jumpMultiplier = (perfectJumpTime - Mathf.Abs(jumpButtonPressedTimer - perfectJumpTime)) / perfectJumpTime;
                        
                    }

                    if (jumpButtonPressedTimer > perfectJumpTime && jumpMultiplier < 0.75f)
                        jumpMultiplier = 0.75f;
                    else if (jumpMultiplier < 0.25f)
                        jumpMultiplier = 0.25f;

                    Debug.Log(Mathf.Abs(jumpButtonPressedTimer - perfectJumpTime)+ " | " + jumpMultiplier);

                    //float jumpAccuracy = Mathf.Abs(jumpButtonPressedTimer - perfectJumpTime);
                    //if (jumpAccuracy > jumpButtonPressThreshold - perfectJumpTime - 0.1f)
                    //    jumpAccuracy = perfectJumpTime - perfectJumpTimeWindow;
                    //if (jumpAccuracy < perfectJumpTimeWindow)
                    //    jumpMultiplier = 1;
                    //else
                    //    jumpMultiplier = Remap(jumpAccuracy, perfectJumpTimeWindow, perfectJumpTime, 1, 0.25f);

                    //if (jumpButtonPressedTimer > perfectJumpTime)
                    //    jumpButtonPressedTimer = perfectJumpTime;

                    if (IsGrounded())
                        ApplyJumpForce();

                    if(canWallJump)
                    {
                        if(!IsGrounded())
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
                jumpButtonPressedTimer = 0;
                jumpEffectRing.SetActive(false);
                jumpEffectSprite.SetActive(false);
            }
        }

        void ApplyJumpForce(bool isNormalJump = true)
        {
            if (isSprintPressed)
                isSprintingWhileJumping = true;
            else
                isSprintingWhileJumping = false;

            if(isNormalJump)
            {
                //rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(-2.0f * rb.gravityScale * Physics2D.gravity.y * jumpHeight * jumpMultiplier * (jumpButtonPressedTimer / perfectJumpTime)));
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(-2.0f * rb.gravityScale * Physics2D.gravity.y * jumpHeight * jumpMultiplier));
                SpawnGrassParticle(playerGround);
            }
            else
            {
                StartCoroutine(JumpFromLedgeOrWall());
            }
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
                if(!isScaleSet)
                {
                    var scaleX = visuals.transform.localScale.x;
                    visuals.transform.localScale = new Vector3(scaleX, 1, 1);
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

            if(IsWallSliding() && xInput != 0)
            {
                if(Mathf.Sign(xInput) == Mathf.Sign(visuals.transform.localScale.x))
                {
                    rb.velocity = new Vector2(rb.velocity.x, walkVelocity);
                    StartCoroutine(WalkParticles(playerWall));
                }
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
            if(isJumpingFromLedgeOrWall)
                return;

            Debug.Log("Grabbing Ledge");
            yInput = 0;
            isTouchingLedge = false;
            isLedgeGrabbing = true;
            anim.SetBool(nameof(isLedgeGrabbing), true);
            transform.DOMove(ledge.transform.position - new Vector3(0, 0.10f), 0.2f);
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }

        void ReleaseLedge()
        {
            Debug.Log("Releasing Ledge");
            rb.gravityScale = 4;
            isLedgeGrabbing = false;
            anim.SetBool(nameof(isLedgeGrabbing), false);
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
            if(yInput > 0)
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
                return isGrounded = false;
            //return Physics2D.Raycast(transform.position, Vector2.down, distanceFromGround);
            //if (rb.velocity.y > 0)
            //    return isGrounded = false;

            foreach (var ground in groundCheck)
            {
                if (Physics2D.OverlapCircle(ground.position, 0.2f, groundLayer))
                {
                    return isGrounded = true;
                }
            }
            return isGrounded = false;
        }


        private bool IsWalking()
        {
            isWalking = (Math.Abs(rb.velocity.x) > 0.1f) ? true : false;
            if(isWalking)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsSprinting()
        {
            isSprinting = Mathf.Abs(rb.velocity.x) > walkVelocity ? true : false;
            //if (afterLedgeGrab)
            //    Debug.Log(Mathf.Abs(rb.velocity.x));
            if(isSprinting)
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
            //if (Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallLayer))
            //{
            //    return true;
            //}

            foreach (var ground in groundCheck)
            {
                if (Physics2D.OverlapCircle(ground.position, 0.1f, wallLayer))
                {
                    return true;
                }
            }
            return false;
        }


        //Animations
        private void ChangeWalkingState()
        {
            anim.SetBool(nameof(isWalking), IsWalking());
            anim.speed = IsSprinting() ? 1.5f : 1.0f;

            if (IsWallSliding())
                return;

            if (rb.velocity.x > 0f)
            {
                visuals.localScale = new Vector3(1, 1, 1);
            }
            else if (rb.velocity.x < 0f)
            {
                visuals.localScale = new Vector3(-1, 1, 1);
            }
        }

        private void ChangeJumpState()
        {
            if(rb.velocity.y > 0.1f || rb.velocity.y < -0.1f)
                anim.SetFloat("yVelocity", rb.velocity.y);
            else
                anim.SetFloat("yVelocity", 0);

            if (IsGrounded())
            {
                anim.SetInteger("velocity", 0);
            }
            else
            {
                if (rb.velocity.y > 0)
                {
                    anim.SetInteger("velocity", 1);

                }
                else if (rb.velocity.y < 0)
                {
                    anim.SetInteger("velocity", -1);
                }
            }

            anim.SetBool(nameof(IsGrounded), IsGrounded());
            anim.SetBool(nameof(IsWallSliding), IsWallSliding());
        }


        public void UnlockAbility(AbilityType ability)
        {
            if(ability == AbilityType.LedgeGrab)
            {
                canLedgeGrab = true;
            }
            else if (ability == AbilityType.WallJump)
            {
                canWallJump = true;
            }
            else if(ability == AbilityType.WallRun)
            {
                canWallRun = true;
            }
            else if(ability == AbilityType.Sprint)
            {
                canSprint = true;
            }
        }

        //Effects
        private void SpawnGrassParticle(Transform location, bool isWalkParticle = false, bool isWallParticle = false)
        {
            var particle = Instantiate(grassParticle, location.position, grassParticle.transform.rotation);
            particle.transform.localPosition += new Vector3(0, 0.5f);
            if (isWalkParticle)
            {
                var particleSystem = particle.GetComponent<ParticleSystem>();
                particleSystem.startSpeed = 2;
                particleSystem.gravityModifier = 0.5f;
                var emission = particleSystem.emission;
                emission.SetBursts(new ParticleSystem.Burst[]{
                new ParticleSystem.Burst(0.0f, 2)});
                if(isWallParticle)
                {
                    particle.transform.Rotate(new Vector3(0, -visuals.transform.localScale.x * 90, visuals.transform.localScale.x * 90));
                    particleSystem.startSpeed = 1;
                }
            }
            //particle.transform.localRotation = new Quaternion(-90, 0, 0, 0);
            Destroy(particle, 1);
        }

        private IEnumerator WalkParticles(Transform spawnPoint)
        {
            if(spawnPoint == playerGround)
            {
                if (isWalking && IsGrounded() && !isSpawningWalkParticles)
                {
                    isSpawningWalkParticles = true;
                    SpawnGrassParticle(spawnPoint, true);
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
                    yield return new WaitForSeconds(0.35f);
                    isSpawningWalkParticles = false;
                }
            }

        }

        //Camera 
        void PanCamera()
        {
            float panSpeed = cameraPanSpeed;
            if (isPlayerPanningCamera)
                panSpeed *= 5;

            if(IsSprinting() || isPlayerPanningCamera)
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
            }
        }

        void LookAtPlayer()
        {
            if (!startedLooking)
                return;
            virtualCamera.Follow = transform;
        }

        void LookUp()
        {
            if (startedLooking)
                return;
            virtualCamera.Follow = null;
            virtualCamera.transform.DOMoveY(virtualCamera.transform.localPosition.y + 4, 0.5f);
        }

        void LookDown()
        {
            if (startedLooking)
                return;
            virtualCamera.Follow = null;
            virtualCamera.transform.DOMoveY(virtualCamera.transform.localPosition.y - 4, 0.5f);
        }



        //Utility
        float Remap(float x, float in_min, float in_max, float out_min, float out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;

        }


    }
}


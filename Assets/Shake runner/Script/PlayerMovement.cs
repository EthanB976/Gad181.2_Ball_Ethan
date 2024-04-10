using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
    public class PlayerMovement : MonoBehaviour
    {

        [Header("Movement")]
        private float moveSpeed;
        public float walkSpeed;
        public float sprintSpeed;
        public float slideSpeed;
        public float climbSpeed;
        public float wallRunSpeed;
        public float dashSpeed;

        private float desiredMoveSpeed;
        private float lasrDesiredMoveSpeed;

        public float groundDrag;

        public float jumpForce;
        public float jumpCoolDown;
        public float airMulitplier;
        bool readyToJump;

        [Header("Crouching")]
        public float crouchSpeed;
        public float crouchYScale;
        private float startYScale;

        [Header("keybinds")]
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode sprintKey = KeyCode.LeftShift;
        public KeyCode crouchKey = KeyCode.LeftControl;

        [Header("Ground Check")]
        public float playerHeight;
        public LayerMask whatIsGround;
        public bool grounded;
        public Transform groundCheck;
        public float groundDistance = 0.4f;

        [Header("Slope Handling")]
        public float maxSlopeAngle;
        private RaycastHit slopeHit;
        private bool exitingSlope;

        public Transform orientation;

        float horizontalInput;
        float verticalInput;

        public bool sliding;
        public bool wallrunning;
        public bool climbing;
        public bool dashing;

        Vector3 moveDirection;

        Rigidbody rb;

        public MovementState state;

        public enum MovementState
        {
            walking,
            sprinting,
            wallrunning,
            climbing,
            crouching,
            dashing,
            sliding,
            air
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            readyToJump = true;

            startYScale = transform.localScale.y;
        }

        private void StateHandler()
        {
            // Mode - Dashing
            if (dashing)
            {
                state = MovementState.dashing;
                moveSpeed = dashSpeed;
            }

            // Mode - Climbing
            else if (climbing)
            {
                state = MovementState.climbing;
                desiredMoveSpeed = climbSpeed;
            }

            // Mode - Wallrunning
            else if (wallrunning)
            {
                state = MovementState.wallrunning;
                desiredMoveSpeed = wallRunSpeed;
            }

            // Mode - Sliding
            else if (sliding)
            {
                state = MovementState.sliding;

                if(OnSlope() && rb.velocity.y < 0.1f)
                {
                    desiredMoveSpeed = slideSpeed;
                }
                else
                {
                    desiredMoveSpeed = sprintSpeed;
                }
            }

            // Mode - crouching
            else if(Input.GetKey(crouchKey))
            {
                state = MovementState.crouching;
                desiredMoveSpeed = crouchSpeed;
            }

            // Mode - Sprinting
            else if(grounded && Input.GetKey(sprintKey))
            {
                state = MovementState.sprinting;
                desiredMoveSpeed = sprintSpeed;
            }

            // Mode - Wlaking
            else if (grounded)
            {
                state = MovementState.walking;
                desiredMoveSpeed = walkSpeed;
            }

            // Mode - Air
            else
            {
                state = MovementState.air;
            }

            // check if desiredMoveSpeed has chnaged drastically
            if(Mathf.Abs(desiredMoveSpeed - lasrDesiredMoveSpeed) > 4f && moveSpeed != 0)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }

            lasrDesiredMoveSpeed = desiredMoveSpeed;
        }

        private IEnumerator SmoothlyLerpMoveSpeed()
        {
            // smoothly lerp movement speed to desired value
            float time = 0;
            float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
            float startValue = moveSpeed;

            while (time < difference)
            {
                moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
                time += Time.deltaTime;
                yield return null;
            }

            moveSpeed = desiredMoveSpeed;
        }

        private void FixedUpdate() 
        {
            MovePlayer();
        }
        private void Update()
        {
            // ground check
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
            grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

            MyInput();
            SpeedControl();
            StateHandler();

            // handle drag
            if(state == MovementState.walking || state == MovementState.sprinting || state == MovementState.crouching)
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = 0;
            }
        }

        private void MyInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            
            //when to jump
            if(Input.GetKey(jumpKey) && readyToJump && grounded)
            {
                readyToJump = false;

                Jump();

                // continuously jump
                Invoke(nameof(ResetJump), jumpCoolDown);

                
            }

            // start couching
            if (Input.GetKeyDown(crouchKey))
            {
                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            }

            // stop crouch
            if (Input.GetKeyUp(crouchKey))
            {
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            }
        }

        private void MovePlayer()
        {
            //calculate movement direction
            moveDirection = orientation.forward *verticalInput + orientation.right *horizontalInput;

            // on slope
            if (OnSlope() && !exitingSlope)
            {
                rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

                if(rb.velocity.y > 0)
                {
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
            }

            // on ground 
            else if (grounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            
            //in air
            else if (!grounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMulitplier, ForceMode.Force);
            }

            // turn gravity off while on slope
            if (!wallrunning)
            {
                rb.useGravity = !OnSlope();
            }
        }

        private void SpeedControl()
        {

            // limiting speed on slope 
            if (OnSlope())
            {
                if (rb.velocity.magnitude > moveSpeed)
                {
                    rb.velocity = rb.velocity.normalized * moveSpeed;
                }
            }

            // limiting speed on ground or in air
            else
            {
                Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                // limit velocity if needed
                if (flatVel.magnitude > moveSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * moveSpeed;
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                }
            }
            
        }

        private void Jump()
        {

            exitingSlope = true;

            // reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, 0F, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        }

        private void ResetJump()
        {
            readyToJump = true;

            exitingSlope = false;
        }

        public bool OnSlope()
        {
            if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        public Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
        }
    }
}


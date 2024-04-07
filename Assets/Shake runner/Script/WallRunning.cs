using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
    public class WallRunning : MonoBehaviour
    {

        [Header("Wallrunning")]
        public LayerMask whatIsWall;
        public LayerMask whatIsGround;
        public float wallRunForce;
        public float maxWallRunTime;
        public float wallRunTimer;

        [Header("Input")]
        private float horizontalInput;
        private float verticalInput;

        [Header("Detection")]
        public float wallCheckDistance;
        public float minJumpHeight;
        private RaycastHit leftWallHit;
        private RaycastHit rightWallHit;
        private bool wallLeft;
        private bool wallRight;

        [Header("References")]
        public Transform orientation;
        private PlayerMovement pm;
        private Rigidbody rb;

        private void Start ()
        {
            rb = GetComponent<Rigidbody>();
            pm = GetComponent<PlayerMovement>();
        }

        private void Update ()
        {
            CheckForWall();
            StateMachine();
        }

        private void FixedUpdate()
        {
            if (pm.wallrunning)
            {
                WallRunningMovement();
            }
        }
        private void CheckForWall()
        {
            wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
            wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
        }

        private bool AboveGround()
        {
            return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
        }
        
        private void StateMachine()
        {
            // getting Inputs
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            // State 1 - WallRunning
            if((wallLeft || wallRight) && verticalInput > 0 && AboveGround())
            {
                // start wallrun here
                if(!pm.wallrunning)
                {
                    StartWallRun();
                }

                // State 3 - None
                else
                {
                    if(pm.wallrunning)
                    {
                        StopWallRun();
                    }
                }
            }
        }

        private void StartWallRun()
        {
            pm.wallrunning = true;
        }

        private void WallRunningMovement()
        {
            rb.useGravity = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

            Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

            // forward force
            rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
        }

        private void StopWallRun()
        {
            pm.wallrunning = false;
        }
    }
}

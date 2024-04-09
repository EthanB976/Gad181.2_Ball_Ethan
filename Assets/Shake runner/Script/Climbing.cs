using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
    public class Climbing : MonoBehaviour
    {

        [Header("References")]
        public Transform orientation;
        public Rigidbody rb;
        public PlayerMovement pm;
        public LayerMask whatIsWall;

        [Header("Climbing")]
        public float climbSpeed;
        public float maxClimbTime;
        public float climbTimer;

        private bool climbing;

        [Header("Dectection")]
        public float detectionLength;
        public float spherecastRadius;
        public float maxWalllookAngle;
        private float wallLookAngle;

        private RaycastHit frontWallHit;
        private bool wallFront;

        private void Update()
        {
            WallCheck();
            StateMachine();

            if(climbing)
            {
                ClimbingMovement();
            }
        }

        private void StateMachine()
        {
            // State 1 - Climbing
            if (wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWalllookAngle)
            {
                if (!climbing && climbTimer > 0)
                {
                    StartClimbing();
                }

                // timer
                if (climbTimer > 0)
                {
                    climbTimer -= Time.deltaTime;
                }

                if (climbTimer < 0)
                {
                    StopClimbing();
                }
            }

            // State 3 - None
            else
            {
                if (climbing)
                {
                    StopClimbing();
                }
            }
        }

        private void WallCheck()
        {
            wallFront = Physics.SphereCast(transform.position, spherecastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
            wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

            if (pm.grounded)
            {
                climbTimer = maxClimbTime;
            }
        }

        private void StartClimbing()
        {
            climbing = true;
            pm.climbing = true;
        }

        private void ClimbingMovement()
        {
            rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
        }

        private void StopClimbing()
        {
            climbing = false;
            pm.climbing = false;
        }
    }

}

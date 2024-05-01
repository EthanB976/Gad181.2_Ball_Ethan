using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EB
{
    public class Sliding : MonoBehaviour
    {

        [Header("References")]
        public Transform orientation;
        public Transform playObj;
        private Rigidbody rb;
        private PlayerMovement pm;

        [Header("Sliding")]
        public float maxSlideTime;
        public float slideForce;
        private float slideTimer;

        public float slideYScale;
        private float startYScale;

        [Header("Input")]
        public KeyCode slideKey = KeyCode.LeftControl;
        private float horizontalinput;
        private float verticalinput;

        public bool purchaseSlide = false;

        private void Start ()
        {
            rb = GetComponent<Rigidbody> ();
            pm = GetComponent<PlayerMovement> ();

            startYScale = playObj.localScale.y;
        }

        private void Update ()
        {
            horizontalinput = Input.GetAxisRaw("Horizontal");
            verticalinput = Input.GetAxisRaw("Vertical");

            if(Input.GetKeyDown(slideKey) && (horizontalinput != 0 || verticalinput !=0))
            {
                StartSlide();
            }

            if (Input.GetKeyUp(slideKey) && pm.sliding)
            {
                StopSlide();
            }
        }

        private void FixedUpdate()
        {
            if (pm.sliding)
            {
                SlidingMovement();
            }
        }

        private void StartSlide()
        {
            if (purchaseSlide == true)
            {
                pm.sliding = true;

                playObj.localScale = new Vector3(playObj.localScale.x, slideYScale, playObj.localScale.z);

                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

                slideTimer = maxSlideTime;
            }
        }

        private void SlidingMovement()
        {
            if (purchaseSlide == true)
            {
                Vector3 inputDirection = orientation.forward * verticalinput + orientation.right * horizontalinput;

                // sliding normal
                if (!pm.OnSlope() || rb.velocity.y > -0.1f)
                {
                    rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

                    slideTimer -= Time.deltaTime;
                }

                // sliding down a slope
                else
                {
                    rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
                }

                if (slideTimer <= 0)
                {
                    StopSlide();
                }
            }
        }

        private void StopSlide()
        {
            if (purchaseSlide == true)
            {

                pm.sliding = false;

                playObj.localScale = new Vector3(playObj.localScale.x, startYScale, playObj.localScale.z);
            }
        }

        public void ShopSliding()
        {
            purchaseSlide = true;
        }
    }
}


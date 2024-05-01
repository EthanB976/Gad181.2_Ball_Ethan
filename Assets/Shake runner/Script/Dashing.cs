using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
    public class Dashing : MonoBehaviour
    {

        [Header("References")]
        public Transform orientation;
        public Transform playerCam;
        private Rigidbody rb;
        private PlayerMovement pm;

        [Header("Dashing")]
        public float dashForce;
        public float dashUpwardForce;
        public float dashDuration;

        [Header("CoolDown")]
        public float dashCd;
        private float dashCdTimer;

        [Header("Input")]
        public KeyCode dashKey = KeyCode.E;

        public bool purchaseDash = false;
        void Start ()
        {
            rb = GetComponent<Rigidbody>();
            pm = GetComponent<PlayerMovement>();
        }

        private void Update ()
        {
            if (Input.GetKeyDown(dashKey))
            {
                Dash();
                Debug.Log("Pressed E");
            }

            if (dashCdTimer > 0)
            {
                dashCdTimer -= Time.deltaTime;
            }
        }

        public void Dash()
        {
            if (purchaseDash == true)
            {
                if (dashCdTimer > 0)
                {
                    return;
                }

                else
                {
                    dashCdTimer = dashCd;
                }

                pm.dashing = true;

                Vector3 forecToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;

                delayedForceToApply = forecToApply;
                Invoke(nameof(DelayedDashForce), 0.025f);

                Invoke(nameof(ResetDash), dashDuration);
            }
        }

        public void ShopDash()
        {
            purchaseDash = true;
        }

        private Vector3 delayedForceToApply;
        private void DelayedDashForce()
        {
            rb.AddForce(delayedForceToApply, ForceMode.Force);
        }

        private void ResetDash()
        {
            pm.dashing = false;
        }
    }
}


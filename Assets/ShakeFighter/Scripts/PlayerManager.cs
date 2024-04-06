using EB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EB
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerLocomotion playerLocomotion;

        public bool isInteracting;

        [Header("player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool isUsingRighthand;
        public bool isUsingLefthand;


        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            playerLocomotion.HandleJumping();


        }


        void Update()
        {
            isInteracting = anim.GetBool("isInteracting");
            anim.SetBool("isInAir", true);
            isUsingRighthand = anim.GetBool("isUsingRightHand");
            isUsingLefthand = anim.GetBool("isUsingLeftHand");

            float delta = Time.deltaTime;

            inputHandler.TickInput(delta);

            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        }



        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }


        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            isSprinting = inputHandler.b_Input;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.jump_Input = false;

            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }



    }
}

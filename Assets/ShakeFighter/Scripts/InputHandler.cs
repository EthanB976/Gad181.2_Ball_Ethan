using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EB
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool rb_Input;
        public bool rt_Input;
        public bool jump_Input;
        public bool lockOnInput;

        public bool rollFlag;
        public bool sprintFlag;
        public float rollInputTimer;
        public bool lockOnFlag;
        

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        CameraHandler cameraHandler;
        AnimatorHandler animatorHandler;
        

        Vector2 movementInput;
        Vector2 cameraInput;


        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }


        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovment.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovment.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.Jump.performed += inputActions => jump_Input = true;
                inputActions.PlayerActions.RB.performed += inputActions => rb_Input = true;
                inputActions.PlayerActions.RT.performed += inputActions => rt_Input = true;
                inputActions.PlayerActions.LockOn.performed += inputActions => lockOnInput = true;
            }

            inputActions.Enable();
        }


        private void OnDisable()
        {
            inputActions.Disable();
        }


        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackinput(delta);
            HandleLockOnInput();


        }



        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }



        private void HandleRollInput(float delta)
        {
            b_Input = inputActions.PlayerActions.Roll.IsPressed();



            if (b_Input)
            {
                rollInputTimer += delta;
                sprintFlag = true;
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }
                rollInputTimer = 0;
            }
        }



        private void HandleAttackinput(float delta)
        {
            


            //RB Input handles the right hand weapon's light attack
            if (rb_Input)
            {
                playerAttacker.HandleLightAttack(playerInventory.rightweapon);
                animatorHandler.anim.SetBool("isUsingRightHand", true);
            }

            if (rt_Input)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightweapon);
                animatorHandler.anim.SetBool("isUsingRightHand", true);
            }
        }

       
        private void HandleLockOnInput()
        {
            if (lockOnInput && lockOnFlag == false)
            {
                cameraHandler.ClearLockOnTargets();
                lockOnInput = false;
                lockOnFlag = true;
                cameraHandler.HandleLockOn();
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOnInput && lockOnFlag)
            {
                lockOnInput = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }
        }


    }

}

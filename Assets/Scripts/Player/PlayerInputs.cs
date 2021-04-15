using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class PlayerInputs : MonoBehaviour
    {
        private PlayerActionControl inputActions;
        private PlayerController playerController;

        [Header("Jump")]
        [SerializeField] private float jumpHoldDuration = .15f;
        [SerializeField] private float minimalJumpTime = .15f;
        [SerializeField] private float jumpBufferTime = .2f;
        [SerializeField] private int airJumps = 1;

        [Header("Dash")]
        [SerializeField] private int airDashes = 1;

        private float jumpHoldTime = 0;
        private float jumpBufferTimer = 0;
        private int airJumpCounter = 0;
        private int airDashCounter = 0;

        private Vector2 movementDirection;
        private Vector2 dashDirectionCache;
        private bool jumpHolded = false;
        private bool hasJumped = false;
        private bool hasPressedJump = false;
        private bool jumpBuffer = false;
        private bool dashPerfomed = false;

        private void Awake()
        {
            inputActions = new PlayerActionControl();
            playerController = GetComponent<PlayerController>();

            inputActions.Land.Move.performed += ctx => movementDirection = ctx.ReadValue<Vector2>();
            inputActions.Land.Move.canceled += ctx => movementDirection = Vector2.zero;

            inputActions.Land.Jump.performed += ctx => Jump(ctx);
            inputActions.Land.Jump.canceled += ctx => Jump(ctx);

            inputActions.Land.Dash.performed += ctx => Dash(ctx);
            inputActions.Land.Dash.canceled += ctx => Dash(ctx);
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }
        private void OnDisable()
        {
            inputActions.Disable();
        }

        void Update()
        {
            if (jumpHoldTime >= jumpHoldDuration) jumpHolded = false;
            else jumpHoldTime += Time.deltaTime;

            if(jumpBufferTimer < jumpBufferTime && hasPressedJump && CanJump()) ConfirmJump();
            else jumpBufferTimer += Time.deltaTime;
        }
        
        public void Jump(CallbackContext ctx)
        {
            if (ctx.performed)
            {
                jumpBufferTimer = 0;
                hasPressedJump = true;
                jumpBuffer = true;
            }

            else if (ctx.canceled)
            {
                jumpHolded = false;
                jumpBuffer = false;
                StartCoroutine(JumpReleasedDelay(minimalJumpTime - jumpHoldTime));
            }
        }

        private void ConfirmJump()
        {
            jumpHolded = jumpBuffer;
            hasJumped = true;
            hasPressedJump = false;
            jumpBufferTimer = 0;
            jumpHoldTime = 0;
        }

        IEnumerator JumpReleasedDelay(float time)
        {
            yield return new WaitForSeconds(time);
            hasJumped = false;
        }

        private bool CanJump()
        {
            if (playerController.GetGroundCollision() || playerController.GetWallSliding()) return true;
            else if (airJumpCounter < airJumps)
            {
                airJumpCounter++;
                return true;
            }

            return false;
        }

        private void Dash(CallbackContext ctx)
        {
            if (canDash() && ctx.performed)
            {
                dashPerfomed = true;
                dashDirectionCache = movementDirection;
            }
        }

        private bool canDash()
        {
            if (playerController.GetGroundCollision()) return true;
            else if (airDashCounter < airDashes)
            {
                airDashCounter++;
                return true;
            }

            return false;
        }

        public void ResetInputCounters()
        {
            if (dashPerfomed) return;

            airJumpCounter = 0;
            airDashCounter = 0;
        }

        public void ActivatePlayerInputs()
        {
            dashPerfomed = false;
            inputActions.Enable();
        }

        public void DectivatePlayerInputs()
        {
            inputActions.Disable();
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public Vector2 GetMovementDirection()
        {
            return movementDirection;
        }

        public Vector2 GetDashDirectionCache()
        {
            return dashDirectionCache;
        }

        public bool GetJumpHoldValue()
        {
            return jumpHolded;
        }

        public bool GetHasJumped()
        {
            return hasJumped;
        }

        public bool GetDashPerfomed()
        {
            return dashPerfomed;
        }
    }
}

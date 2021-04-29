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
        [SerializeField] private float dashBufferTime = .2f;
        [SerializeField] private int airDashes = 1;

        private float jumpHoldTime = 0;
        private float jumpBufferTimer = 0;
        private float dashBufferTimer = 0;
        private int airJumpCounter = 0;
        private int airDashCounter = 0;

        public Vector2 movementDirection { get; private set; }
        public Vector2 dashDirectionCache { get; private set; }
        public bool jumpHolded { get; private set; } = false;
        public bool hasJumped { get; private set; } = false;
        public bool dashPerfomed { get; private set; } = false;

        private bool hasPressedJump = false;
        private bool jumpBuffer = false;
        private bool hasPressedDash = false;
        private bool hasPressedAttack = false;

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

            inputActions.Land.Attack.performed += ctx => Attack(ctx);
            inputActions.Land.Attack.canceled += ctx => Attack(ctx);
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

            if (dashBufferTimer < dashBufferTime && hasPressedDash && canDash()) ConfirmDash();
            else dashBufferTimer += Time.deltaTime;
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

        private void ConfirmDash()
        {
            dashBufferTimer = 0;
            dashPerfomed = true;
            hasPressedDash = false;
            dashDirectionCache = movementDirection;
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
            if (ctx.performed)
            {
                dashBufferTimer = 0;
                hasPressedDash = true;
            }
        }

        private bool canDash()
        {
            if (playerController.GetDashDelayTimer() > 0) return false;

            if (playerController.GetGroundCollision()) return true;
            else if (airDashCounter < airDashes)
            {
                airDashCounter++;
                return true;
            }

            return false;
        }

        public void Attack(CallbackContext ctx)
        {
            if (ctx.performed)
            {
                hasPressedAttack = true;
            }
        }

        public void ResetInputCounters()
        {
            if (dashPerfomed) return;

            airJumpCounter = 0;
            airDashCounter = 0;
        }

        public void ActivatePlayerInputs(bool value)
        {
            if (value)
            {
                dashPerfomed = false;
                inputActions.Enable();
            }
            else inputActions.Disable();
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public bool GetHasPressedAttack()
        {
            bool attack = hasPressedAttack;
            hasPressedAttack = false;

            return attack;
        }
    }
}

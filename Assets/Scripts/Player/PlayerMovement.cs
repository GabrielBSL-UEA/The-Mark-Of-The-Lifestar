using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D rb;
        private PlayerController playerController;
        private Vector2 dashDirecton;

        private int facingDirection = 1;
        private float dashTimer = 0;
        private float wallJumpDirection = 0;
        private float wallJumpXImpulseTimer = Mathf.Infinity;
        private bool isWallJumping = false;
        private bool antiWallJumpOnFirstContactWhilePressingJump = false;

        [Header("General")]
        [SerializeField] private float horizontalSpeed = 20f;
        [SerializeField] private float jumpForce = 15f;
        [SerializeField] private float deadZone = .25f;

        [Header("Dash")]
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashDuration;

        [Header("Fall")]
        [SerializeField] private float fallVelocityLimit;

        [Header("WallSlide")]
        [SerializeField] private float wallSlideSpeed;
        [SerializeField] private float wallJumpXForce;
        [SerializeField] private float wallJumpXImpulseTime;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerController = GetComponent<PlayerController>();
        }

        public void MovementInterpreter(Vector2 movementDirection, bool jumpHolded, bool jumpPressed, bool dashPerfomed, bool wallSliding)
        {
            if(jumpPressed && !wallSliding) antiWallJumpOnFirstContactWhilePressingJump = false;
            
            else if (!jumpPressed && wallSliding) antiWallJumpOnFirstContactWhilePressingJump = true;

            if (!wallSliding) FallControl();
            else WallSlideControl (jumpPressed, movementDirection.x);

            if (dashPerfomed && !wallSliding) Dash();
            else
            {
                Jump(jumpHolded, jumpPressed);
                Move(movementDirection);
            }
        }

        private void FallControl()
        {
            if (rb.velocity.y < (fallVelocityLimit * -1)) rb.velocity = new Vector2(rb.velocity.x, -fallVelocityLimit);
        }

        private void WallSlideControl(bool jumpPressed, float direction)
        {
            if (jumpPressed && !isWallJumping && antiWallJumpOnFirstContactWhilePressingJump)
            {
                wallJumpXImpulseTimer = 0;
                isWallJumping = true;

                if (direction > 0) wallJumpDirection = 1;
                else wallJumpDirection = -1;
            }
            else
            {
                if (!jumpPressed) isWallJumping = false;
            }

            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }

        private void Dash()
        {
            if(dashTimer == 0)
            {
                dashDirecton = playerController.GetDashDirection();
            }
            if (dashTimer > dashDuration)
            {
                playerController.EnablePlayerInputs();
                dashTimer = 0;
            }
            else
            {
                dashTimer += Time.fixedDeltaTime;

                if (Mathf.Abs(dashDirecton.x) < deadZone && Mathf.Abs(dashDirecton.y) < deadZone)
                    rb.velocity = Vector2.right * facingDirection * dashSpeed;

                else
                {
                    if (Mathf.Abs(dashDirecton.x) >= 2 * Mathf.Abs(dashDirecton.y))
                    {
                        if (dashDirecton.x > 0) rb.velocity = Vector2.right * dashSpeed;
                        else rb.velocity = Vector2.left * dashSpeed;
                    }
                    else if (Mathf.Abs(dashDirecton.y) >= 2 * Mathf.Abs(dashDirecton.x))
                    {
                        if (dashDirecton.y > 0) rb.velocity = Vector2.up * dashSpeed;
                        else rb.velocity = Vector2.down * dashSpeed;
                    }
                    else
                    {
                        float x, y;

                        if (dashDirecton.x > 0) x = 1;
                        else x = -1;
                        if (dashDirecton.y > 0) y = 1;
                        else y = -1;

                        rb.velocity = new Vector2(x / Mathf.Sqrt(2), y / Mathf.Sqrt(2)) * dashSpeed;
                    }
                }
            }
        }

        private void Move(Vector2 movementDirection)
        {
            if(wallJumpXImpulseTimer > wallJumpXImpulseTime)
            {
                if (movementDirection.x > 0) facingDirection = 1;
                else if (movementDirection.x < 0) facingDirection = -1;

                if (Mathf.Abs(movementDirection.x) > deadZone) rb.velocity = new Vector2(horizontalSpeed * facingDirection, rb.velocity.y);
                else rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                Debug.Log(wallJumpDirection);
                rb.velocity = new Vector2(wallJumpXForce * -wallJumpDirection, rb.velocity.y);
                wallJumpXImpulseTimer += Time.fixedDeltaTime;
            }
        }

        private void Jump(bool jumpHolded, bool jumpPressed)
        {
            if (jumpHolded) rb.velocity = Vector2.up * jumpForce;
            else if (rb.velocity.y > 0 && !jumpPressed) rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }
}

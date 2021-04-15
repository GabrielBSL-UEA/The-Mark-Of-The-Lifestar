using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public enum DashAllDirections
        {
            Horiz,
            Horiz_Verti,
            Horiz_Verti_Diago
        }

        private Rigidbody2D rb;
        private PlayerController playerController;
        private Vector2 dashDirecton;

        private int facingDirection = 1;
        private float dashTimer = 0;
        private float wallJumpDirection = 0;
        private float wallJumpXImpulseTimer = Mathf.Infinity;
        private bool antiWallJumpOnFirstContactWhilePressingJump = false;
        private bool reverseDash = false;

        private bool isDashing = false;
        private bool isWallSliding = false;
        private bool isWallJumping = false;

        [Header("General")]
        [SerializeField] private float horizontalSpeed = 20f;
        [SerializeField] private float jumpForce = 15f;

        [Header("Dash")]
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashDuration;
        [SerializeField] private DashAllDirections dashAllDirections;

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

        private void Update()
        {
            AnimationCall();
        }

        private void AnimationCall()
        {
            if (isDashing) playerController.PlayAnimation(AnimationsList.p_dash_start);
            else if (isWallSliding) playerController.PlayAnimation(AnimationsList.p_wall_slide);
            else if (rb.velocity.y > 0) playerController.PlayAnimation(AnimationsList.p_jump);
            else if (rb.velocity.y < 0) playerController.PlayAnimation(AnimationsList.p_jump_to_fall);
            else if (rb.velocity.x != 0) playerController.PlayAnimation(AnimationsList.p_run);
            else playerController.PlayAnimation(AnimationsList.p_idle);
        }

        public void movementTranslator(Vector2 movementDirection, bool jumpHolded, bool jumpPressed, bool dashPerfomed, bool wallSliding)
        {
            if(jumpPressed && !wallSliding) antiWallJumpOnFirstContactWhilePressingJump = false;
            else if (!jumpPressed && wallSliding) antiWallJumpOnFirstContactWhilePressingJump = true;

            if (wallSliding) WallSlideControl(jumpPressed, movementDirection.x);
            else FallControl();

            if (dashPerfomed) Dash(wallSliding);
            else
            {
                Jump(jumpHolded, jumpPressed);
                Move(movementDirection);
            }
        }

        private void FallControl()
        {
            isWallSliding = false;

            if (rb.velocity.y < (fallVelocityLimit * -1)) rb.velocity = new Vector2(rb.velocity.x, -fallVelocityLimit);
        }

        private void WallSlideControl(bool jumpPressed, float direction)
        {
            isWallSliding = true;

            if (jumpPressed && !isWallJumping && antiWallJumpOnFirstContactWhilePressingJump)
            {
                playerController.ResetInputCounters();
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

        private void Dash(bool wallSliding)
        {
            isDashing = true;

            if (dashTimer == 0)
            {
                playerController.DisablePlayerInputs();
                dashDirecton = playerController.GetDashDirection();
                reverseDash = wallSliding;
            }
            
            if (dashTimer > dashDuration)
            {
                isDashing = false;
                playerController.EnablePlayerInputs();
                dashTimer = 0;
            }
            else
            {
                dashTimer += Time.fixedDeltaTime;

                if (Mathf.Abs(dashDirecton.x) < playerController.GetDeadZone() && Mathf.Abs(dashDirecton.y) < playerController.GetDeadZone())
                    rb.velocity = Vector2.right * facingDirection * dashSpeed;
                
                else if (reverseDash) rb.velocity = Vector2.right * -facingDirection * dashSpeed;

                else
                {
                    //Cálculo para o dash em apenas duas direções (Horizontal)
                    if (dashAllDirections == DashAllDirections.Horiz)
                    {
                        if (dashDirecton.x > 0) rb.velocity = Vector2.right * dashSpeed;
                        else rb.velocity = Vector2.left * dashSpeed;
                    }

                    //Cálculo para o dash em quatro direções (Horizontal e Vertical)
                    else if (dashAllDirections == DashAllDirections.Horiz_Verti)
                    {
                        if (Mathf.Abs(dashDirecton.x) >= Mathf.Abs(dashDirecton.y))
                        {
                            if (dashDirecton.x > 0) rb.velocity = Vector2.right * dashSpeed;
                            else rb.velocity = Vector2.left * dashSpeed;
                        }
                        else
                        {
                            if (dashDirecton.y > 0) rb.velocity = Vector2.up * dashSpeed;
                            else rb.velocity = Vector2.down * dashSpeed;
                        }
                    }

                    //Cálculo para o dash em oito direções (Horizontal, Vertical e Diagonal)
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
        }

        private void Move(Vector2 movementDirection)
        {
            if(wallJumpXImpulseTimer > wallJumpXImpulseTime)
            {
                if (transform.localScale.x > 0) facingDirection = 1;
                else if (transform.localScale.x < 0) facingDirection = -1;

                if (Mathf.Abs(movementDirection.x) > playerController.GetDeadZone()) rb.velocity = new Vector2(horizontalSpeed * facingDirection, rb.velocity.y);
                else rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
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

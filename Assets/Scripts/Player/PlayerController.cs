using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactible;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float deadZone = .4f;

        PlayerInputs playerInputs;
        PlayerHealth playerHealth;
        PlayerCombat playerCombat;
        PlayerMovement playerMovement;
        PlayerCollision playerCollision;
        PlayerAnimations playerAnimations;

        private void Awake()
        {
            playerAnimations = GetComponent<PlayerAnimations>();
            playerCollision = GetComponent<PlayerCollision>();
            playerMovement = GetComponent<PlayerMovement>();
            playerInputs = GetComponent<PlayerInputs>();
            playerHealth = GetComponent<PlayerHealth>();
            playerCombat = GetComponent<PlayerCombat>();
        }

        private void FixedUpdate()
        {
            if (!playerHealth.isAlive)
            {
                PlayAnimation(PlayerAnimationsList.p_death);
                return;
            }
            if (playerCombat.enabled)
            {
                playerCombat.AttackInterpreter(
                    playerInputs.GetHasPressedAttack(),
                    playerHealth.isStunned);
            }

            if (playerMovement.enabled)
            {
                playerMovement.MovementTranslator(
                    playerInputs.movementDirection,
                    playerInputs.jumpHolded,
                    playerInputs.hasJumped,
                    playerInputs.dashPerfomed,
                    playerCollision.isWallSliding,
                    playerCombat.isAttacking,
                    playerHealth.isStunned);
            }
        }

        public void ResetInputCounters()
        {
            playerInputs.ResetInputCounters();
        }

        public void ResetPlayerAttack(bool value)
        {
            playerCombat.ResetAttack(value);
        }

        public void PlayAnimation(PlayerAnimationsList animation)
        {
            playerAnimations.Play(animation);
        }

        public void EnablePlayerInputs(bool value)
        {
            playerInputs.ActivatePlayerInputs(value);
        }

        public void EnablePlayerMovements(bool value)
        {
            if (playerMovement.enabled == value) return;
            playerMovement.enabled = value;
        }

        public void MakePlayerBlink(bool value)
        {
            playerAnimations.StartBlink(value);
        }

        public void ForcePlayerFlip()
        {
            playerAnimations.ForceFlip();
        }

        public void SetHitReciever(bool value)
        {
            GetComponent<HitReciever>().SetCanRecieveHit(value);
        }

        //-----------------------------------------------------------------
        //**********              Animation Calls                **********
        //-----------------------------------------------------------------

        public void EnableAttackBuffer()
        {
            playerCombat.ActivateAttackBuffer();
        }

        public void RegisterAttackHits()
        {
            playerCombat.DetectHits();
        }

        public void AttackTransitionStart()
        {
            playerCombat.OnTransitionStart();
        }

        public void AttackTransitionEnds()
        {
            playerCombat.OnTransitionEnd();
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public Vector2 GetMovementInputs()
        {
            return playerInputs.movementDirection;
        }

        public Vector2 GetDashDirection()
        {
            return playerInputs.dashDirectionCache;
        }

        public float GetLastAgressorDirection()
        {
            return playerHealth.agressorDirection;
        }

        public float GetFacingDirection()
        {
            return playerMovement.facingDirection;
        }

        public float GetDashDelayTimer()
        {
            return playerMovement.dashDelayTimer;
        }

        public bool GetDashPerfomed()
        {
            return playerInputs.dashPerfomed;
        }

        public bool GetDashState()
        {
            return playerMovement.isDashing;
        }

        public bool GetPlayerIsAttacking()
        {
            return playerCombat.isAttacking;
        }

        public bool GetWallSliding()
        {
            return playerCollision.isWallSliding;
        }

        public bool GetGroundCollision()
        {
            return playerCollision.isGrounded;
        }

        public float GetDeadZone()
        {
            return deadZone;
        }
    }
}

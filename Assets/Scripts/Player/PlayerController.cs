using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (playerCombat.enabled)
            {
                playerCombat.RecieveAttackInput(
                    playerInputs.GetHasPressedAttack(),
                    playerMovement.GetIsDashing());
            }

            if (playerMovement.enabled)
            {
                playerMovement.MovementTranslator(
                    playerInputs.GetMovementDirection(),
                    playerInputs.GetJumpHoldValue(),
                    playerInputs.GetHasJumped(),
                    playerInputs.GetDashPerfomed(),
                    playerCollision.GetIsWallSliding(),
                    playerCombat.GetIsAttacking());
            }
        }

        public void ResetInputCounters()
        {
            playerInputs.ResetInputCounters();
        }

        public void PlayAnimation(AnimationsList animation)
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

        public void AttackTransitionEnds()
        {
            playerCombat.OnTransitionEnd();
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public Vector2 GetMovementInputs()
        {
            return playerInputs.GetMovementDirection();
        }

        public Vector2 GetDashDirection()
        {
            return playerInputs.GetDashDirectionCache();
        }

        public float GetDashDelayTimer()
        {
            return playerMovement.GetDashDelayRemaining();
        }

        public bool GetPlayerIsAttacking()
        {
            return playerCombat.GetIsAttacking();
        }

        public bool GetWallSliding()
        {
            return playerCollision.GetIsWallSliding();
        }

        public bool GetGroundCollision()
        {
            return playerCollision.GetIsGrounded();
        }

        public float GetDeadZone()
        {
            return deadZone;
        }
    }
}

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
            if (!playerHealth.GetIsAlive())
            {
                PlayAnimation(PlayerAnimationsList.p_death);
                return;
            }
            if (playerCombat.enabled)
            {
                playerCombat.AttackInterpreter(
                    playerInputs.GetHasPressedAttack(),
                    playerHealth.GetIsStunned());
            }

            if (playerMovement.enabled)
            {
                playerMovement.MovementTranslator(
                    playerInputs.GetMovementDirection(),
                    playerInputs.GetJumpHoldValue(),
                    playerInputs.GetHasJumped(),
                    playerInputs.GetDashPerfomed(),
                    playerCollision.GetIsWallSliding(),
                    playerCombat.GetIsAttacking(),
                    playerHealth.GetIsStunned());
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
            return playerInputs.GetMovementDirection();
        }

        public Vector2 GetDashDirection()
        {
            return playerInputs.GetDashDirectionCache();
        }

        public float GetLastAgressorDirection()
        {
            return playerHealth.GetAgressorDirection();
        }

        public float GetFacingDirection()
        {
            return playerMovement.GetFacingDirection();
        }

        public float GetDashDelayTimer()
        {
            return playerMovement.GetDashDelayRemaining();
        }

        public bool GetDashPerfomed()
        {
            return playerInputs.GetDashPerfomed();
        }

        public bool GetDashState()
        {
            return playerMovement.GetIsDashing();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
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
            if (playerMovement.enabled)
            {
                playerMovement.movementTranslator(
                    playerInputs.GetMovementDirection(),
                    playerInputs.GetJumpHoldValue(),
                    playerInputs.GetHasJumped(),
                    playerInputs.GetDashPerfomed(),
                    playerCollision.GetIsWallSliding());
            }
        }

        public void ResetInputCounters()
        {
            playerInputs.ResetInputCounters();
        }

        public void EnablePlayerInputs()
        {
            playerInputs.ActivatePlayerInputs();
        }

        public void PlayAnimation(AnimationsList animation)
        {
            playerAnimations.Play(animation);
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

        public bool GetWallSliding()
        {
            return playerCollision.GetIsWallSliding();
        }

        public bool GetGroundCollision()
        {
            return playerCollision.GetIsGrounded();
        }
    }
}

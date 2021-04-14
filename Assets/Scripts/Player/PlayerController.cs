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

        private void Awake()
        {
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
                playerMovement.MovementInterpreter(
                    playerInputs.GetMovementDirection(),
                    playerInputs.GetJumpHoldValue(),
                    playerInputs.GetHasJumped(),
                    playerInputs.GetDashPerfomed(),
                    playerCollision.GetIsWallSliding());
            }
        }

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

        public void ResetAirJumps()
        {
            playerInputs.ResetCountersOnGround();
        }

        public void EnablePlayerInputs()
        {
            playerInputs.ActivatePlayerInputs();
        }
    }
}

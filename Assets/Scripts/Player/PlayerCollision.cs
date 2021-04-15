using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerCollision : MonoBehaviour
    {
        private PlayerController playerController;
        private BoxCollider2D boxCol2D;

        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private float coyoteTime = .05f;
        [SerializeField] private float collisionDetectionOffset;

        private float coyoteTimer = 0;
        private bool isGrounded;
        private bool isWallSliding;

        private void Awake()
        {
            boxCol2D = GetComponent<BoxCollider2D>();
            playerController = GetComponent<PlayerController>();
        }

        private void FixedUpdate()
        {
            if (!IsGrounded())
            {
                if(coyoteTimer >= coyoteTime)isGrounded = IsGrounded();
                else
                {
                    coyoteTimer += Time.fixedDeltaTime;
                    isGrounded = true;
                }
            }
            else
            {
                isGrounded = IsGrounded();
                coyoteTimer = 0;
            }

            isWallSliding = IsWallSliding();
        }

        private bool IsGrounded()
        {
            RaycastHit2D rayHit = Physics2D.BoxCast(boxCol2D.bounds.center, boxCol2D.bounds.size - new Vector3(0f, boxCol2D.bounds.extents.y, 0f), 
                0f, Vector2.down, boxCol2D.bounds.extents.y + collisionDetectionOffset, groundLayerMask);

            bool collisionDetected = rayHit.collider != null;

            if (collisionDetected && !isGrounded) playerController.ResetInputCounters();

            return collisionDetected;
        }

        private bool IsWallSliding()
        {
            Vector2 playerMovement = playerController.GetMovementInputs();

            RaycastHit2D rayHit = Physics2D.BoxCast(boxCol2D.bounds.center, boxCol2D.bounds.size - new Vector3(boxCol2D.bounds.extents.x, 1f, 0f),
                0f, new Vector2(playerMovement.x, 0), boxCol2D.bounds.extents.x + collisionDetectionOffset, groundLayerMask);

            bool collisionDetected = rayHit.collider != null;

            if (collisionDetected && !isGrounded && Mathf.Abs(playerMovement.x) > 0) return true;
            
            return false;
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public bool GetIsGrounded()
        {
            return isGrounded;
        }

        public bool GetIsWallSliding()
        {
            return isWallSliding;
        }
    }
}

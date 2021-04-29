using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerCollision : MonoBehaviour
    {
        private PlayerController playerController;
        private BoxCollider2D boxCol2D;
        private Rigidbody2D rb;

        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private float coyoteTime = .15f;
        [SerializeField] private float collisionDetectionOffset = .1f;

        private float coyoteTimer = 0;
        public bool isGrounded { get; private set; }
        public bool isWallSliding { get; private set; }
        private bool canCoyote = true;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            boxCol2D = GetComponent<BoxCollider2D>();
            playerController = GetComponent<PlayerController>();
        }

        private void FixedUpdate()
        {
            isWallSliding = IsWallSliding();

            if (rb.velocity.y >= .1f) canCoyote = false;
            else if (Mathf.Approximately(rb.velocity.y, 0) && isGrounded) canCoyote = true;

            if (!IsGrounded())
            {
                if (coyoteTimer >= coyoteTime || !canCoyote) isGrounded = false;
                else isGrounded = true;
                
                coyoteTimer += Time.fixedDeltaTime;
            }
            else
            {
                isGrounded = true;
                coyoteTimer = 0;
            }
        }

        private bool IsGrounded()
        {
            RaycastHit2D rayHit = Physics2D.BoxCast(boxCol2D.bounds.center, boxCol2D.bounds.size - new Vector3(0f, boxCol2D.bounds.extents.y, 0f), 
                0f, Vector2.down, boxCol2D.bounds.extents.y + collisionDetectionOffset, groundLayerMask);

            bool collisionDetected = rayHit.collider != null;

            if (collisionDetected && isGrounded && rb.velocity.y <= 0) playerController.ResetInputCounters();
            
            return collisionDetected;
        }

        private bool IsWallSliding()
        {
            Vector2 playerMovement = playerController.GetMovementInputs();

            RaycastHit2D rayHit = Physics2D.BoxCast(boxCol2D.bounds.center, boxCol2D.bounds.size - new Vector3(boxCol2D.bounds.extents.x, 1f, 0f),
                0f, new Vector2(playerMovement.x, 0), boxCol2D.bounds.extents.x + collisionDetectionOffset, groundLayerMask);

            bool collisionDetected = rayHit.collider != null;
            
            if (collisionDetected && !IsGrounded() && rb.velocity.y < 0 && (Mathf.Abs(playerMovement.x) > 0 || isWallSliding)) return true;
            
            return false;
        }
    }
}

using UnityEngine;

namespace Player
{
    public class PlayerCollision : MonoBehaviour
    {
        private PlayerController _playerController;
        private BoxCollider2D _boxCol2D;
        private Rigidbody2D _rb;

        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private float coyoteTime = .15f;
        [SerializeField] private float collisionDetectionOffset = .1f;

        private float _coyoteTimer;
        public bool IsGrounded { get; private set; }
        public bool IsWallSliding { get; private set; }
        private bool _canCoyote = true;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _boxCol2D = GetComponent<BoxCollider2D>();
            _playerController = GetComponent<PlayerController>();
        }

        private void FixedUpdate()
        {
            IsWallSliding = CheckIfPlayerIsWallSliding();

            if (_rb.velocity.y >= .1f) _canCoyote = false;
            else if (Mathf.Approximately(_rb.velocity.y, 0) && IsGrounded) _canCoyote = true;

            if (!CheckIfPlayerIsGrounded())
            {
                if (_coyoteTimer >= coyoteTime || !_canCoyote) IsGrounded = false;
                else IsGrounded = true;
                
                _coyoteTimer += Time.fixedDeltaTime;
            }
            else
            {
                IsGrounded = true;
                _coyoteTimer = 0;
            }
        }

        private bool CheckIfPlayerIsGrounded()
        {
            var bounds = _boxCol2D.bounds;
            RaycastHit2D rayHit = Physics2D.BoxCast(bounds.center, bounds.size - new Vector3(0f, bounds.extents.y, 0f), 
                0f, Vector2.down, bounds.extents.y + collisionDetectionOffset, groundLayerMask);

            bool collisionDetected = rayHit.collider != null;

            if (collisionDetected && IsGrounded && _rb.velocity.y <= 0) _playerController.ResetInputCounters();
            
            return collisionDetected;
        }

        private bool CheckIfPlayerIsWallSliding()
        {
            Vector2 playerMovement = _playerController.GetMovementInputs();

            var bounds = _boxCol2D.bounds;
            RaycastHit2D rayHit = Physics2D.BoxCast(bounds.center, bounds.size - new Vector3(bounds.extents.x, 1f, 0f),
                0f, new Vector2(playerMovement.x, 0), bounds.extents.x + collisionDetectionOffset, groundLayerMask);

            bool collisionDetected = rayHit.collider != null;
            
            if (collisionDetected && !CheckIfPlayerIsGrounded() && _rb.velocity.y < 0 && (Mathf.Abs(playerMovement.x) > 0 || IsWallSliding)) return true;
            
            return false;
        }
    }
}

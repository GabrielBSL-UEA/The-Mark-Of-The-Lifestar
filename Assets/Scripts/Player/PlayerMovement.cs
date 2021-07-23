using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private enum DashAllDirections
        {
            Horiz,
            HorizVerti,
            HorizVertiDiago
        }

        private Rigidbody2D _rb;
        private PlayerController _playerController;
        private Vector2 _dashDirection;

        public int FacingDirection { get; private set; } = 1;
        public float DashDelayTimer { get; private set; }
        public bool IsDashing { get; private set; }

        private float _dashTimer;
        private float _wallJumpDirection;
        private float _wallJumpXImpulseTimer = Mathf.Infinity;
        private bool _antiWallJumpOnFirstContactWhilePressingJump;
        private bool _reverseDash;

        private bool _isStunned;
        private bool _isAttacking;
        private bool _isWallSliding;
        private bool _isWallJumping;

        [Header("General")]
        [SerializeField] private float horizontalSpeed = 16f;
        [SerializeField] private float jumpForce = 30f;

        [Header("Dash")]
        [SerializeField] private float dashSpeed = 40f;
        [SerializeField] private float dashDuration = .15f;
        [SerializeField] private float dashDelay = .2f;
        [SerializeField] private DashAllDirections dashAllDirections;

        [Header("Fall")]
        [SerializeField] private float fallVelocityLimit = 30f;

        [Header("WallSlide")]
        [SerializeField] private float wallSlideSpeed = 6f;
        [SerializeField] private float wallJumpXForce = 18f;
        [SerializeField] private float wallJumpXImpulseTime = .13f;

        [Header("Hit")]
        [SerializeField] private float hitImpulseX = 30;
        [SerializeField] private float hitImpulseY = 15;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerController = GetComponent<PlayerController>();
        }

        private void FixedUpdate()
        {
            if (DashDelayTimer > 0) DashDelayTimer -= Time.fixedDeltaTime;
            if (IsDashing) _dashTimer += Time.fixedDeltaTime;
        }

        public void MovementTranslator(Vector2 movementDirection, bool jumpHeld, bool jumpPressed, bool dashPerformed, bool wallSliding, bool attacking, bool stunned)
        {
            _isStunned = stunned;
            if (stunned)
            {
                HitImpulse();
                return;
            }

            _isAttacking = attacking;
            if (_isAttacking)
            {
                if (dashPerformed) _playerController.ResetPlayerAttack(true);
                else return;
            }

            if(jumpPressed && !wallSliding) _antiWallJumpOnFirstContactWhilePressingJump = false;
            else if (!jumpPressed && wallSliding) _antiWallJumpOnFirstContactWhilePressingJump = true;

            if (wallSliding) WallSlideControl(jumpPressed, movementDirection.x);
            else FallControl();

            if (dashPerformed) Dash(wallSliding);
            else
            {
                Jump(jumpHeld, jumpPressed);
                Move(movementDirection);
            }

            AnimationCall();
        }

        private void AnimationCall()
        {
            if (_isStunned) _playerController.PlayAnimation(PlayerAnimationsList.p_hurt);
            else if (!_isAttacking || IsDashing)
            {
                if (IsDashing) _playerController.PlayAnimation(PlayerAnimationsList.p_dash_start);
                else if (_isWallSliding) _playerController.PlayAnimation(PlayerAnimationsList.p_wall_slide);
                else if (_rb.velocity.y > .1f) _playerController.PlayAnimation(PlayerAnimationsList.p_jump);
                else if (_rb.velocity.y < -.1f) _playerController.PlayAnimation(PlayerAnimationsList.p_jump_to_fall);
                else if (_rb.velocity.x != 0) _playerController.PlayAnimation(PlayerAnimationsList.p_run);
                else _playerController.PlayAnimation(PlayerAnimationsList.p_idle);
            }
        }

        private void HitImpulse()
        {
            _rb.velocity = new Vector2(hitImpulseX * _playerController.GetLastAggressorsDirection(), hitImpulseY);
            _playerController.ResetInputCounters();
        }

        private void FallControl()
        {
            _isWallSliding = false;

            if (_rb.velocity.y < (-fallVelocityLimit)) _rb.velocity = new Vector2(_rb.velocity.x, -fallVelocityLimit);
        }

        private void WallSlideControl(bool jumpPressed, float direction)
        {
            _isWallSliding = true;

            if (jumpPressed && !_isWallJumping && _antiWallJumpOnFirstContactWhilePressingJump)
            {
                _wallJumpXImpulseTimer = 0;

                if (direction > 0) _wallJumpDirection = 1;
                else _wallJumpDirection = -1;
            }
            else
            {
                if (!jumpPressed) _isWallJumping = false;
            }

            _playerController.ResetInputCounters();
            _rb.velocity = new Vector2(_rb.velocity.x, -wallSlideSpeed);
        }

        private void Dash(bool wallSliding)
        {
            IsDashing = true;

            if (_dashTimer == 0)
            {
                _playerController.EnablePlayerInputs(false);
                _dashDirection = _playerController.GetDashDirection();
                _reverseDash = wallSliding;

                if (_reverseDash) _playerController.ForcePlayerFlip();
            }
            
            if (_dashTimer > dashDuration)
            {
                IsDashing = false;
                DashDelayTimer = dashDelay;
                _playerController.EnablePlayerInputs(true);
                _dashTimer = 0;
            }
            else
            {

                if (Mathf.Abs(_dashDirection.x) < _playerController.GetDeadZone() && Mathf.Abs(_dashDirection.y) < _playerController.GetDeadZone())
                    _rb.velocity = Vector2.right * (FacingDirection * dashSpeed);
                
                else if (_reverseDash) _rb.velocity = Vector2.right * (-FacingDirection * dashSpeed);

                else
                {
                    //Two direction calculation
                    if (dashAllDirections == DashAllDirections.Horiz)
                    {
                        if (_dashDirection.x > 0) _rb.velocity = Vector2.right * dashSpeed;
                        else _rb.velocity = Vector2.left * dashSpeed;
                    }

                    //Four direction calculation
                    else if (dashAllDirections == DashAllDirections.HorizVerti)
                    {
                        if (Mathf.Abs(_dashDirection.x) >= Mathf.Abs(_dashDirection.y))
                        {
                            if (_dashDirection.x > 0) _rb.velocity = Vector2.right * dashSpeed;
                            else _rb.velocity = Vector2.left * dashSpeed;
                        }
                        else
                        {
                            if (_dashDirection.y > 0) _rb.velocity = Vector2.up * dashSpeed;
                            else _rb.velocity = Vector2.down * dashSpeed;
                        }
                    }

                    //Eight direction calculation
                    else if (dashAllDirections == DashAllDirections.HorizVertiDiago)
                    {
                        if (Mathf.Abs(_dashDirection.x) >= 2 * Mathf.Abs(_dashDirection.y))
                        {
                            if (_dashDirection.x > 0) _rb.velocity = Vector2.right * dashSpeed;
                            else _rb.velocity = Vector2.left * dashSpeed;
                        }
                        else if (Mathf.Abs(_dashDirection.y) >= 2 * Mathf.Abs(_dashDirection.x))
                        {
                            if (_dashDirection.y > 0) _rb.velocity = Vector2.up * dashSpeed;
                            else _rb.velocity = Vector2.down * dashSpeed;
                        }
                        else
                        {
                            float x, y;

                            if (_dashDirection.x > 0) x = 1;
                            else x = -1;
                            if (_dashDirection.y > 0) y = 1;
                            else y = -1;

                            _rb.velocity = new Vector2(x / Mathf.Sqrt(2), y / Mathf.Sqrt(2)) * dashSpeed;
                        }
                    }
                }
            }
        }

        private void Move(Vector2 movementDirection)
        {
            if(_wallJumpXImpulseTimer > wallJumpXImpulseTime)
            {
                if (transform.localScale.x > 0) FacingDirection = 1;
                else if (transform.localScale.x < 0) FacingDirection = -1;

                if (Mathf.Abs(movementDirection.x) > _playerController.GetDeadZone()) _rb.velocity = new Vector2(horizontalSpeed * FacingDirection, _rb.velocity.y);
                else _rb.velocity = new Vector2(0, _rb.velocity.y);
            }
            else
            {
                _rb.velocity = new Vector2(wallJumpXForce * -_wallJumpDirection, _rb.velocity.y);
                _wallJumpXImpulseTimer += Time.fixedDeltaTime;
            }
        }

        private void Jump(bool jumpHolded, bool jumpPressed)
        {
            if (jumpHolded) _rb.velocity = Vector2.up * jumpForce;
            else if (_rb.velocity.y > 0 && !jumpPressed) _rb.velocity = new Vector2(_rb.velocity.x, 0);
        }
    }
}

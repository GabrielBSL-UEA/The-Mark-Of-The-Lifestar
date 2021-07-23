using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class PlayerInputs : MonoBehaviour
    {
        private PlayerActionControl _inputActions;
        private PlayerController _playerController;

        [Header("Jump")]
        [SerializeField] private float jumpHoldDuration = .15f;
        [SerializeField] private float minimalJumpTime = .1f;
        [SerializeField] private float jumpBufferTime = .2f;
        [SerializeField] private int airJumps = 1;

        [Header("Dash")]
        [SerializeField] private float dashBufferTime = .2f;
        [SerializeField] private int airDashes = 1;

        private float _jumpHoldTime;
        private float _jumpBufferTimer;
        private float _dashBufferTimer;
        private int _airJumpCounter;
        private int _airDashCounter;

        public Vector2 MovementDirection { get; private set; }
        public Vector2 DashDirectionCache { get; private set; }
        public bool JumpHeld { get; private set; }
        public bool HasJumped { get; private set; }
        public bool DashPerformed { get; private set; }

        private bool _hasPressedJump;
        private bool _jumpBuffer;
        private bool _hasPressedDash;
        private bool _hasPressedAttack;

        private void Awake()
        {
            _inputActions = new PlayerActionControl();
            _playerController = GetComponent<PlayerController>();

            _inputActions.Land.Move.performed += ctx => MovementDirection = ctx.ReadValue<Vector2>();
            _inputActions.Land.Move.canceled += ctx => MovementDirection = Vector2.zero;

            _inputActions.Land.Jump.performed += Jump;
            _inputActions.Land.Jump.canceled += Jump;

            _inputActions.Land.Dash.performed += Dash;
            _inputActions.Land.Dash.canceled += Dash;

            _inputActions.Land.Attack.performed += Attack;
            _inputActions.Land.Attack.canceled += Attack;
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }
        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void Update()
        {
            if (_jumpHoldTime >= jumpHoldDuration) JumpHeld = false;
            else _jumpHoldTime += Time.deltaTime;

            if(_jumpBufferTimer < jumpBufferTime && _hasPressedJump && CanJump()) ConfirmJump();
            else _jumpBufferTimer += Time.deltaTime;

            if (_dashBufferTimer < dashBufferTime && _hasPressedDash && CanDash()) ConfirmDash();
            else _dashBufferTimer += Time.deltaTime;
        }

        private void Jump(CallbackContext ctx)
        {
            if (ctx.performed)
            {
                _jumpBufferTimer = 0;
                _hasPressedJump = true;
                _jumpBuffer = true;
            }

            else if (ctx.canceled)
            {
                JumpHeld = false;
                _jumpBuffer = false;
                StartCoroutine(JumpReleasedDelay(minimalJumpTime - _jumpHoldTime));
            }
        }

        private void ConfirmJump()
        {
            JumpHeld = _jumpBuffer;
            HasJumped = true;
            _hasPressedJump = false;
            _jumpBufferTimer = 0;
            _jumpHoldTime = 0;
        }

        private void ConfirmDash()
        {
            _dashBufferTimer = 0;
            DashPerformed = true;
            _hasPressedDash = false;
            DashDirectionCache = MovementDirection;
        }

        IEnumerator JumpReleasedDelay(float time)
        {
            yield return new WaitForSeconds(time);
            HasJumped = false;
        }

        private bool CanJump()
        {
            if (_playerController.GetGroundCollision() || _playerController.GetWallSliding()) return true;
            else if (_airJumpCounter < airJumps)
            {
                _airJumpCounter++;
                return true;
            }

            return false;
        }

        private void Dash(CallbackContext ctx)
        {
            if (!ctx.performed) return;
            
            _dashBufferTimer = 0;
            _hasPressedDash = true;
        }

        private bool CanDash()
        {
            if (_playerController.GetDashDelayTimer() > 0) return false;

            if (_playerController.GetGroundCollision()) return true;
            else if (_airDashCounter < airDashes)
            {
                _airDashCounter++;
                return true;
            }

            return false;
        }

        private void Attack(CallbackContext ctx)
        {
            if (ctx.performed)
            {
                _hasPressedAttack = true;
            }
        }

        public void ResetInputCounters()
        {
            if (DashPerformed) return;

            _airJumpCounter = 0;
            _airDashCounter = 0;
        }

        public void ActivatePlayerInputs(bool value)
        {
            if (value)
            {
                DashPerformed = false;
                _inputActions.Enable();
            }
            else _inputActions.Disable();
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public bool GetHasPressedAttack()
        {
            bool attack = _hasPressedAttack;
            _hasPressedAttack = false;

            return attack;
        }
    }
}

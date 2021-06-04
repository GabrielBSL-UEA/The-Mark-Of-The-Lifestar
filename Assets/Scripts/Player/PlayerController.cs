using UnityEngine;
using Interactable;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float deadZone = .4f;

        PlayerInputs _playerInputs;
        PlayerHealth _playerHealth;
        PlayerCombat _playerCombat;
        PlayerMovement _playerMovement;
        PlayerCollision _playerCollision;
        PlayerAnimations _playerAnimations;

        private void Awake()
        {
            _playerAnimations = GetComponent<PlayerAnimations>();
            _playerCollision = GetComponent<PlayerCollision>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerInputs = GetComponent<PlayerInputs>();
            _playerHealth = GetComponent<PlayerHealth>();
            _playerCombat = GetComponent<PlayerCombat>();
        }

        private void FixedUpdate()
        {
            if (!_playerHealth.IsAlive)
            {
                PlayAnimation(PlayerAnimationsList.p_death);
                return;
            }
            if (_playerCombat.enabled)
            {
                _playerCombat.AttackInterpreter(
                    _playerInputs.GetHasPressedAttack(),
                    _playerHealth.IsStunned);
            }

            if (_playerMovement.enabled)
            {
                _playerMovement.MovementTranslator(
                    _playerInputs.MovementDirection,
                    _playerInputs.JumpHolded,
                    _playerInputs.HasJumped,
                    _playerInputs.DashPerfomed,
                    _playerCollision.IsWallSliding,
                    _playerCombat.IsAttacking,
                    _playerHealth.IsStunned);
            }
        }

        public void ResetInputCounters()
        {
            _playerInputs.ResetInputCounters();
        }

        public void ResetPlayerAttack(bool value)
        {
            _playerCombat.ResetAttack(value);
        }

        public void PlayAnimation(PlayerAnimationsList playerAnimationsList)
        {
            _playerAnimations.Play(playerAnimationsList);
        }

        public void EnablePlayerInputs(bool value)
        {
            _playerInputs.ActivatePlayerInputs(value);
        }

        public void EnablePlayerMovements(bool value)
        {
            if (_playerMovement.enabled == value) return;
            _playerMovement.enabled = value;
        }

        public void MakePlayerBlink(bool value)
        {
            _playerAnimations.StartBlink(value);
        }

        public void ForcePlayerFlip()
        {
            _playerAnimations.ForceFlip();
        }

        public void SetHitReceiver(bool value)
        {
            GetComponent<HitReceiver>().SetCanReceivedHit(value);
        }

        //-----------------------------------------------------------------
        //**********              Animation Calls                **********
        //-----------------------------------------------------------------

        public void EnableAttackBuffer()
        {
            _playerCombat.ActivateAttackBuffer();
        }

        public void RegisterAttackHits()
        {
            _playerCombat.DetectHits();
        }

        public void AttackTransitionStart()
        {
            _playerCombat.OnTransitionStart();
        }

        public void AttackTransitionEnds()
        {
            _playerCombat.OnTransitionEnd();
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public Vector2 GetMovementInputs()
        {
            return _playerInputs.MovementDirection;
        }

        public Vector2 GetDashDirection()
        {
            return _playerInputs.DashDirectionCache;
        }

        public float GetLastAggressorsDirection()
        {
            return _playerHealth.AggressorDirection;
        }

        public float GetFacingDirection()
        {
            return _playerMovement.FacingDirection;
        }

        public float GetDashDelayTimer()
        {
            return _playerMovement.DashDelayTimer;
        }

        public bool GetDashPerfomed()
        {
            return _playerInputs.DashPerfomed;
        }

        public bool GetDashState()
        {
            return _playerMovement.IsDashing;
        }

        public bool GetPlayerIsAttacking()
        {
            return _playerCombat.IsAttacking;
        }

        public bool GetWallSliding()
        {
            return _playerCollision.IsWallSliding;
        }

        public bool GetGroundCollision()
        {
            return _playerCollision.IsGrounded;
        }

        public float GetDeadZone()
        {
            return deadZone;
        }
    }
}

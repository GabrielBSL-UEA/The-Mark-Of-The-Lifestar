using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Interactable;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        PlayerController _playerController;
        Rigidbody2D _rb;

        [Header("Hit")]
        [SerializeField] private Transform[] hitDetectors;
        [SerializeField] private float hitRange = .5f;
        [SerializeField] private LayerMask hitLayer;

        [Header("Attack")]
        [SerializeField] private float attackDamage = 5f;
        [SerializeField] private float attackDelay = .2f;
        [SerializeField] private float stunForce = 3f;

        private readonly PlayerAnimationsList[] _attackAnimations =
        {
            PlayerAnimationsList.p_attack_1,
            PlayerAnimationsList.p_attack_2
        };

        public bool IsAttacking { get; private set; }
        private float _gravityCache;
        private bool _attackBuffer;
        private bool _canBufferAttack;

        private int _comboCounter;
        private float _attackDelayTimer = Mathf.Infinity;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _gravityCache = _rb.gravityScale;
            _playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if(_attackDelayTimer < attackDelay && !_playerController.GetDashState()) _attackDelayTimer += Time.deltaTime;
        }

        public void AttackInterpreter(bool attackInput, bool stunned)
        {
            if (stunned) ResetAttack(false);

            else if (_attackDelayTimer >= attackDelay && attackInput)
            {
                if (!IsAttacking)
                {
                    _rb.gravityScale = 0;
                    _rb.velocity = Vector2.zero;

                    IsAttacking = true;
                    _playerController.PlayAnimation(_attackAnimations[_comboCounter]);
                    _comboCounter++;
                }
                else if (_canBufferAttack) _attackBuffer = true;
            }
        }

        public void ResetAttack(bool resetAttackDelay)
        {
            _rb.gravityScale = _gravityCache;
            _comboCounter = 0;

            if (resetAttackDelay) _attackDelayTimer = 0;
            else _attackDelayTimer = attackDelay;

            IsAttacking = false;
            _attackBuffer = false;
            _canBufferAttack = false;
        }

        //-----------------------------------------------------------------
        //**********              Animation Calls                **********
        //-----------------------------------------------------------------

        public void ActivateAttackBuffer()
        {
            _canBufferAttack = true;
        }

        public void OnTransitionStart()
        {
            _rb.gravityScale = _gravityCache;
        }

        public void OnTransitionEnd()
        {
            if (_attackBuffer && _comboCounter < _attackAnimations.Length)
            {
                _rb.gravityScale = 0;
                _rb.velocity = Vector2.zero;
                _playerController.PlayAnimation(_attackAnimations[_comboCounter]);
                _comboCounter++;
            }
            else
            {
                _rb.gravityScale = _gravityCache;
                _attackDelayTimer = 0;
                _comboCounter = 0;
                IsAttacking = false;
            }
            _attackBuffer = false;
            _canBufferAttack = false;
        }

        public void DetectHits()
        {
            var objectsList = new List<Transform>();

            foreach (var circle in hitDetectors)
            {
                var objectHits = Physics2D.OverlapCircleAll(circle.position, hitRange, hitLayer);

                foreach (var hit in objectHits)
                {
                    if (objectsList.Contains(hit.transform)) continue;
                    objectsList.Add(hit.transform);
                }
            }

            var totalObjects = 0;

            foreach (var hit in objectsList.Where(hit => hit.GetComponent<HitReceiver>() != null && hit.GetComponent<HitReceiver>().GetCanReceivedHit()))
            {
                hit.GetComponent<HitReceiver>().ReceivedHit(attackDamage, stunForce, transform);
                totalObjects++;
            }

            if(totalObjects > 0) _playerController.ResetInputCounters();
        }

        //----------------------------------------------------------------
        //*************                Gizmos                *************
        //----------------------------------------------------------------

        private void OnDrawGizmosSelected()
        {
            foreach (var circle in hitDetectors)
            {
                Gizmos.DrawWireSphere(circle.position, hitRange);
            }
        }
    }
}
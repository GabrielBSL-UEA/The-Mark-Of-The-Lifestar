using UnityEngine;
using Interactable;

namespace Enemy
{
    public class EnemyBasicMeleeCombat : MonoBehaviour, IEnemyCombat
    {
        EnemyController _enemyController;

        [Header("Type")]
        [SerializeField] private AttackType attackType;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackDamage = 3f;
        [SerializeField] private float stunForce = .3f;

        [Header("Timers")]
        [SerializeField] private float startAttackTime = .5f;
        [SerializeField] private float inBetweenAttacksTime = .5f;

        [Header("Hit")]
        [SerializeField] private Transform[] hitDetectors;
        [SerializeField] private float hitRange = 1.28f;
        [SerializeField] private LayerMask hitLayer;

        private float _startAttackTimer;
        private float _inBetweenAttacksTimer;

        public bool InAttackState { get; private set; } // IEnemyCombat Variable
        private bool _afterAttackLock;

        private void Awake()
        {
            _enemyController = GetComponent<EnemyController>();
        }

        private void Update()
        {
            if (!_afterAttackLock) return;

            AttackDelay();
        }

        public void Attack()
        {
            if (_afterAttackLock) return;

            _startAttackTimer += Time.fixedDeltaTime;

            if (_startAttackTimer < startAttackTime)
            {
                _enemyController.PlayAnimation(EnemyAnimationsList.e_idle);
                return;
            }
            InAttackState = true;
            _enemyController.PlayAnimation(EnemyAnimationsList.e_attack);
        }
        
        //-----------------------------------------------------------------
        //**********              Animation Calls                **********
        //-----------------------------------------------------------------

        public void PerformAttackBehavior(int behavior)
        {
            Transform player = null;

            foreach (var circle in hitDetectors)
            {
                var collision = Physics2D.OverlapCircle(circle.position, hitRange, hitLayer);

                if (collision == null) continue;

                player = collision.transform;
            }

            if (player == null) return;

            player.GetComponent<HitReceiver>().ReceivedHit(attackDamage, stunForce, transform);
        }

        public void OnAttackEnds()
        {
            _afterAttackLock = true;

        } 

        private void AttackDelay()
        {
            _enemyController.PlayAnimation(EnemyAnimationsList.e_idle);

            _inBetweenAttacksTimer += Time.deltaTime;
            if (_inBetweenAttacksTimer < inBetweenAttacksTime) return;

            AttackReset();
        }

        public void AttackReset()
        {
            InAttackState = false;
            _afterAttackLock = false;
            _inBetweenAttacksTimer = 0;
            _startAttackTimer = 0;
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var circle in hitDetectors)
            {
                Gizmos.DrawWireSphere(circle.position, hitRange);
            }
        }

        public void DeactivateComponent()
        {
            enabled = false;
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public AttackType GetAttackType()
        {
            return attackType;
        }

        public float GetAttackRange()
        {
            return attackRange;
        }
    }
}
using UnityEngine;
using Interactable;

namespace Enemy
{
    [RequireComponent(typeof(HitReceiver))]
    [RequireComponent(typeof(EnemyAnimation))]
    [RequireComponent(typeof(EnemySense))]
    [RequireComponent(typeof(EnemyHealth))]
    public class EnemyController : MonoBehaviour
    {
        private EnemyAnimation _enemyAnimation;
        private IEnemyMovement _enemyMovement;
        private IEnemyCombat _enemyCombat;
        private EnemyHealth _enemyHealth;
        private EnemySense _enemySense;

        private void Awake()
        {
            _enemyAnimation = GetComponent<EnemyAnimation>();
            _enemyMovement = GetComponent<IEnemyMovement>();
            _enemyCombat = GetComponent<IEnemyCombat>();
            _enemyHealth = GetComponent<EnemyHealth>();
            _enemySense = GetComponent<EnemySense>();
        }

        private void FixedUpdate()
        {
            if (!_enemyHealth.IsAlive)
            {
                PlayAnimation(EnemyAnimationsList.e_dead);
                DeactivateEnemy();
            }
            else if (_enemyHealth.IsStunned)
            {
                _enemyCombat.AttackReset();
                PlayAnimation(EnemyAnimationsList.e_stun);
            }
            else _enemyMovement.DetectionsInterpreter(
                    _enemySense.PlayerDetected,
                    _enemySense.ObstacleDetected,
                    _enemySense.InAttackRangeDetector,
                    _enemySense.PlayerPosition,
                    _enemyCombat.InAttackState);
        }

        private void DeactivateEnemy()
        {
            _enemyHealth.enabled = false;
            _enemySense.enabled = false;
            _enemyCombat.DeactivateComponent();
            _enemyMovement.DeactivateComponent();
            enabled = false;
        }

        public void FlipEnemy()
        {
            _enemyAnimation.Flip();
        }

        public void AttackPlayer()
        {
            _enemyCombat.Attack();
        }

        public void CheckNearbyPlayer()
        {
            _enemySense.ForceCheckNearPlayer();
        }

        public void PlayAnimation(EnemyAnimationsList enemyAnimationsList)
        {
            _enemyAnimation.Play(enemyAnimationsList);
        }

        public void ApplyBlink(bool value)
        {
            _enemyAnimation.Blink(value);
        }

        public void SetHitReceiver(bool value)
        {
            GetComponent<HitReceiver>().SetCanReceivedHit(value);
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public float GetFacingRightValue()
        {
            return _enemyMovement.FacingRight;
        }

        public AttackType GetAttackType()
        {
            return _enemyCombat.GetAttackType();
        }

        public float GetAttackRange()
        {
            return _enemyCombat.GetAttackRange();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactible;

namespace Enemy
{
    [RequireComponent(typeof(HitReciever))]
    [RequireComponent(typeof(EnemyAnimation))]
    [RequireComponent(typeof(EnemySense))]
    [RequireComponent(typeof(EnemyHealth))]
    public class EnemyController : MonoBehaviour
    {
        private EnemyAnimation enemyAnimation;
        private IEnemyMovement enemyMovement;
        private IEnemyCombat enemyCombat;
        private EnemyHealth enemyHealth;
        private EnemySense enemySense;

        private void Awake()
        {
            enemyAnimation = GetComponent<EnemyAnimation>();
            enemyMovement = GetComponent<IEnemyMovement>();
            enemyCombat = GetComponent<IEnemyCombat>();
            enemyHealth = GetComponent<EnemyHealth>();
            enemySense = GetComponent<EnemySense>();
        }

        private void FixedUpdate()
        {
            if (!enemyHealth.GetIsAlive() || enemyHealth.GetIsStunned()) return;

            enemyMovement.DetectionsInterpreter(
                enemySense.GetPlayerDetected(),
                enemySense.GetObstacleDetected(),
                enemySense.GetinAttackRangeDetector(),
                enemySense.GetPlayerPosition(),
                enemyCombat.GetInAttackState());
        }

        public void FlipEnemy()
        {
            enemyAnimation.Flip();
        }

        public void AttackPlayer()
        {
            enemyCombat.Attack();
        }

        public void PlayAnimation(EnemyAnimationsList animation)
        {
            enemyAnimation.Play(animation);
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public float GetFacingRightValue()
        {
            return enemyMovement.GetFacingRight();
        }

        public attackType GetAttackType()
        {
            return enemyCombat.GetAttackType();
        }

        public float GetAttackRange()
        {
            return enemyCombat.GetAttackRange();
        }
    }
}

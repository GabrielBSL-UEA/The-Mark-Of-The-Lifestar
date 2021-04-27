﻿using System.Collections;
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
            if (!enemyHealth.GetIsAlive())
            {
                PlayAnimation(EnemyAnimationsList.e_dead);
                DeactivateEnemy();
            }
            else if (enemyHealth.GetIsStunned())
            {
                enemyCombat.AttackReset();
                PlayAnimation(EnemyAnimationsList.e_stun);
            }
            else enemyMovement.DetectionsInterpreter(
                    enemySense.GetPlayerDetected(),
                    enemySense.GetObstacleDetected(),
                    enemySense.GetinAttackRangeDetector(),
                    enemySense.GetPlayerPosition(),
                    enemyCombat.GetInAttackState());
        }

        private void DeactivateEnemy()
        {
            enemyHealth.enabled = false;
            enemySense.enabled = false;
            enemyCombat.DeactivateComponent();
            enemyMovement.DeactivateComponent();
            enabled = false;
        }

        public void FlipEnemy()
        {
            enemyAnimation.Flip();
        }

        public void AttackPlayer()
        {
            enemyCombat.Attack();
        }

        public void CheckNearbyPlayer()
        {
            enemySense.ForceCheckNearPlayer();
        }

        public void PlayAnimation(EnemyAnimationsList animation)
        {
            enemyAnimation.Play(animation);
        }

        public void ApplyBlink(bool value)
        {
            enemyAnimation.Blink(value);
        }

        public void SetHitReciever(bool value)
        {
            GetComponent<HitReciever>().SetCanRecieveHit(value);
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

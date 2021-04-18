using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactible;

namespace Enemy
{
    public enum attackType
    {
        melee,
        range
    }

    public class EnemyBasicMeleeCombat : MonoBehaviour, IEnemyCombat
    {
        EnemyController enemyController;
        Rigidbody2D rb;

        [Header("Type")]
        [SerializeField] private attackType attackType;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackDamage = 3f;
        [SerializeField] private float stunForce = .3f;

        [Header("Timers")]
        [SerializeField] private float startAttackTime;
        [SerializeField] private float inBetweenAttacksTime;

        [Header("Hit")]
        [SerializeField] private Transform[] hitDetectors;
        [SerializeField] private float hitRange;
        [SerializeField] private LayerMask hitLayer;

        private float startAttackTimer = 0;
        private float inBetweenAttacksTimer = 0;

        private bool inAttackState = false;
        private bool afterAttackLock = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            enemyController = GetComponent<EnemyController>();
        }

        private void Update()
        {
            if (!afterAttackLock) return;

            AttackDelay();
        }

        public void Attack()
        {
            if (afterAttackLock) return;
            
            startAttackTimer += Time.fixedDeltaTime;

            if (startAttackTimer < startAttackTime)
            {
                enemyController.PlayAnimation(EnemyAnimationsList.e_idle);
                return;
            }
            inAttackState = true;
            enemyController.PlayAnimation(EnemyAnimationsList.e_attack);
        }

        //-----------------------------------------------------------------
        //**********              Animation Calls                **********
        //-----------------------------------------------------------------

        public void PerformAttackBehavior(int behavior)
        {
            Transform player = null;

            for (int i = 0; i < hitDetectors.Length; i++)
            {
                Collider2D collision = Physics2D.OverlapCircle(hitDetectors[i].position, hitRange, hitLayer);

                if (collision == null) continue;

                player = collision.transform;
            }

            if (player == null) return;

            player.GetComponent<HitReciever>().RecieveHit(attackDamage, stunForce, transform);
        }

        public void OnAttackEnds()
        {
            afterAttackLock = true;

        } 

        private void AttackDelay()
        {
            enemyController.PlayAnimation(EnemyAnimationsList.e_idle);

            inBetweenAttacksTimer += Time.deltaTime;
            if (inBetweenAttacksTimer < inBetweenAttacksTime) return;

            inAttackState = false;
            afterAttackLock = false;
            inBetweenAttacksTimer = 0;
            startAttackTimer = 0;
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public attackType GetAttackType()
        {
            return attackType;
        }

        public float GetAttackRange()
        {
            return attackRange;
        }

        public bool GetInAttackState()
        {
            return inAttackState;
        }

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < hitDetectors.Length; i++)
            {
                Gizmos.DrawWireSphere(hitDetectors[i].position, hitRange);
            }
        }
    }
}
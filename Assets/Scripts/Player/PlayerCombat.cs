using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        PlayerController playerController;
        Rigidbody2D rb;

        [Header("Hit")]
        [SerializeField] private Transform[] hitDetectors;
        [SerializeField] private float hitRange;
        [SerializeField] private LayerMask enemyLayer;

        [Header("Attack")]
        [SerializeField] private float attackDelay = .7f;
        

        private readonly AnimationsList[] attackAnimations =
        {
            AnimationsList.p_attack_1,
            AnimationsList.p_attack_2
        };

        private float gravityCache = 0;
        private bool isAttacking = false;
        private bool attackBuffer = false;
        private bool canBufferAttack = false;

        private int comboCounter = 0;
        private float attackDelayTimer = Mathf.Infinity;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            gravityCache = rb.gravityScale;
            playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if(attackDelayTimer < attackDelay) attackDelayTimer += Time.deltaTime;
        }

        public void RecieveAttackInput(bool attackInput, bool dashing)
        {
            if (attackDelayTimer < attackDelay || !attackInput) return;

            if (!isAttacking)
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;

                isAttacking = attackInput;
                playerController.PlayAnimation(attackAnimations[comboCounter]);
                comboCounter++;
            } 
            else if(canBufferAttack) attackBuffer = attackInput;
        }

        public void ActivateAttackBuffer()
        {
            canBufferAttack = true;
        }

        public void OnTransitionEnd()
        {
            if (attackBuffer && comboCounter < attackAnimations.Length)
            {
                playerController.PlayAnimation(attackAnimations[comboCounter]);
                comboCounter++;
            }
            else
            {
                rb.gravityScale = gravityCache;

                attackDelayTimer = 0;
                comboCounter = 0;
                isAttacking = false;
            }
            attackBuffer = false;
            canBufferAttack = false;
        }

        public void DetectHits()
        {
           
            List<Transform> enemiesList = new List<Transform>();

            for (int i = 0; i < hitDetectors.Length; i++)
            {
                Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(hitDetectors[i].position, hitRange, enemyLayer);

                for (int j = 0; j < enemiesHit.Length; j++)
                {
                    if (enemiesList.Contains(enemiesHit[j].transform)) continue;
                    enemiesList.Add(enemiesHit[j].transform);
                }
            }

            if (enemiesList.Count > 0) playerController.ResetInputCounters();

            foreach (Transform enemy in enemiesList)
            {
                enemy.GetComponent<EnemyHitTest>().RegisterHit();
            }
        }

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < hitDetectors.Length; i++)
            {
                Gizmos.DrawWireSphere(hitDetectors[i].position, hitRange);
            }
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public bool GetIsAttacking()
        {
            return isAttacking;
        }
    }
}
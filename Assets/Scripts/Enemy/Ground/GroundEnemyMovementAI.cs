using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class GroundEnemyMovementAI : MonoBehaviour, IEnemyMovement
    {
        private Rigidbody2D rb;
        private EnemyController enemyController;
        public float facingRight { get; private set; } = 1; //isFacingRight == 1 -> Looking Right | isFacingRight == -1 -> Looking Left
        private bool facingObstacle = false;

        [Header("Movement")]
        [SerializeField] private bool moveAround = true;
        [SerializeField] private float velocity = 10f;
        [SerializeField] private float timeToTurnAround = 3f;
        [SerializeField] private float patrolTime = 3f;

        private float turnAroundTimer = 0;
        private float patrolTimer = 0;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            enemyController = GetComponent<EnemyController>();
        }

        public void DetectionsInterpreter(bool playerDetected, bool obstacleDetected, bool inAttackRange, Transform playerPosition, bool inAttackState)
        {
            if (inAttackState) return;

            if (playerDetected) ReachPlayer(playerPosition, obstacleDetected, inAttackRange);
            
            else if(patrolTimer <= 0)
            {
                ObstacleHandler(obstacleDetected);
                Move(obstacleDetected);
            }
            else
            {
                patrolTimer -= Time.fixedDeltaTime;
                rb.velocity = new Vector2(0, rb.velocity.y);
                enemyController.PlayAnimation(EnemyAnimationsList.e_idle);
            }
        }

        private void ReachPlayer(Transform playerPosition, bool obstacleDetected, bool inAttackRange)
        {
            patrolTimer = patrolTime;

            float playerBoxExtends = playerPosition.GetComponent<BoxCollider2D>().bounds.extents.x;

            if (Mathf.Abs(playerPosition.position.x - transform.position.x) - playerBoxExtends < enemyController.GetAttackRange())
            {
                if (inAttackRange) enemyController.AttackPlayer();
                else
                {
                    enemyController.PlayAnimation(EnemyAnimationsList.e_idle);

                    if (playerPosition.position.x > transform.position.x && facingRight == -1) StartFlip();
                    else if (playerPosition.position.x < transform.position.x && facingRight == 1) StartFlip();
                }

                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            else
            {
                float obstacleMultiplier;

                if (obstacleDetected)
                {
                    enemyController.PlayAnimation(EnemyAnimationsList.e_idle);
                    obstacleMultiplier = 0;
                }
                else
                {
                    enemyController.PlayAnimation(EnemyAnimationsList.e_walk);
                    obstacleMultiplier = 1;
                }
                if (playerPosition.position.x > transform.position.x)
                {
                    if (facingRight == -1) StartFlip();

                    rb.velocity = new Vector2(velocity * obstacleMultiplier, rb.velocity.y);
                }
                else if (playerPosition.position.x < transform.position.x)
                {
                    if (facingRight == 1) StartFlip();

                    rb.velocity = new Vector2(-velocity * obstacleMultiplier, rb.velocity.y);
                }
            }
        }

        private void ObstacleHandler(bool obstacleDetected)
        {
            if (!obstacleDetected) return;

            enemyController.PlayAnimation(EnemyAnimationsList.e_idle);
            rb.velocity = new Vector2(0, rb.velocity.y);

            turnAroundTimer -= Time.fixedDeltaTime;

            if (turnAroundTimer <= 0)
            {
                if (facingObstacle)
                {
                    facingObstacle = false;
                    StartFlip();
                }
                else
                {
                    facingObstacle = true;
                    turnAroundTimer = timeToTurnAround;
                }
            }
        }

        private void Move(bool obstacleDetected)
        {
            if (!moveAround || obstacleDetected) return;

            enemyController.PlayAnimation(EnemyAnimationsList.e_walk);
            rb.velocity = new Vector2(velocity * facingRight, rb.velocity.y);
        }

        private void StartFlip()
        {
            facingRight *= -1;
            enemyController.FlipEnemy();
        }

        public void DeactivateComponent()
        {
            enabled = false;
        }
    }
}

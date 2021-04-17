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
        private float facingRight = 1; //isFacingRight == 1 -> Looking Right | isFacingRight == -1 -> Looking Left
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

        public void DetectionsInterpreter(bool playerDetected, bool obstacleDetected, bool inAttackRange, Vector2 playerPosition, bool inAttackState)
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

        private void ReachPlayer(Vector2 playerPosition, bool obstacleDetected, bool inAttackRange)
        {
            patrolTimer = patrolTime;
            
            if (Mathf.Abs(playerPosition.x - transform.position.x) < enemyController.GetAttackRange())
            {
                if (inAttackRange) enemyController.AttackPlayer();
                else
                {
                    enemyController.PlayAnimation(EnemyAnimationsList.e_idle);

                    if (playerPosition.x > transform.position.x && facingRight == -1) StartFlip();
                    else if (playerPosition.x < transform.position.x && facingRight == 1) StartFlip();
                }

                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            else
            {
                if (obstacleDetected) enemyController.PlayAnimation(EnemyAnimationsList.e_idle);
                else enemyController.PlayAnimation(EnemyAnimationsList.e_walk);

                if (playerPosition.x > transform.position.x)
                {
                    if (facingRight == -1) StartFlip();

                    rb.velocity = new Vector2(velocity, rb.velocity.y);
                }
                else if (playerPosition.x < transform.position.x)
                {
                    if (facingRight == 1) StartFlip();

                    rb.velocity = new Vector2(-velocity, rb.velocity.y);
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

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------


        public float GetFacingRight()
        {
            return facingRight;
        }
    }
}

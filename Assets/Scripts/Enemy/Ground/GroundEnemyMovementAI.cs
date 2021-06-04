using UnityEngine;

namespace Enemy
{
    public class GroundEnemyMovementAI : MonoBehaviour, IEnemyMovement
    {
        private Rigidbody2D _rb;
        private EnemyController _enemyController;
        public float FacingRight { get; private set; } = 1; //isFacingRight == 1 -> Looking Right | isFacingRight == -1 -> Looking Left
        private bool _facingObstacle;

        [Header("Movement")]
        [SerializeField] private bool moveAround = true;
        [SerializeField] private float velocity = 10f;
        [SerializeField] private float timeToTurnAround = 3f;
        [SerializeField] private float patrolTime = 3f;

        private float _turnAroundTimer;
        private float _patrolTimer;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _enemyController = GetComponent<EnemyController>();
        }

        public void DetectionsInterpreter(bool playerDetected, bool obstacleDetected, bool inAttackRange, Transform playerPosition, bool inAttackState)
        {
            if (inAttackState) return;

            if (playerDetected) ReachPlayer(playerPosition, obstacleDetected, inAttackRange);
            
            else if(_patrolTimer <= 0)
            {
                ObstacleHandler(obstacleDetected);
                Move(obstacleDetected);
            }
            else
            {
                _patrolTimer -= Time.fixedDeltaTime;
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                _enemyController.PlayAnimation(EnemyAnimationsList.e_idle);
            }
        }

        private void ReachPlayer(Transform playerPosition, bool obstacleDetected, bool inAttackRange)
        {
            _patrolTimer = patrolTime;

            float playerBoxExtends = playerPosition.GetComponent<BoxCollider2D>().bounds.extents.x;

            if (Mathf.Abs(playerPosition.position.x - transform.position.x) - playerBoxExtends < _enemyController.GetAttackRange())
            {
                if (inAttackRange) _enemyController.AttackPlayer();
                else
                {
                    _enemyController.PlayAnimation(EnemyAnimationsList.e_idle);

                    if (playerPosition.position.x > transform.position.x && Mathf.Approximately(FacingRight,-1)) StartFlip();
                    else if (playerPosition.position.x < transform.position.x && Mathf.Approximately(FacingRight,1)) StartFlip();
                }

                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }

            else
            {
                float obstacleMultiplier;

                if (obstacleDetected)
                {
                    _enemyController.PlayAnimation(EnemyAnimationsList.e_idle);
                    obstacleMultiplier = 0;
                }
                else
                {
                    _enemyController.PlayAnimation(EnemyAnimationsList.e_walk);
                    obstacleMultiplier = 1;
                }
                if (playerPosition.position.x > transform.position.x)
                {
                    if (Mathf.Approximately(FacingRight,-1)) StartFlip();

                    _rb.velocity = new Vector2(velocity * obstacleMultiplier, _rb.velocity.y);
                }
                else if (playerPosition.position.x < transform.position.x)
                {
                    if (Mathf.Approximately(FacingRight,1)) StartFlip();

                    _rb.velocity = new Vector2(-velocity * obstacleMultiplier, _rb.velocity.y);
                }
            }
        }

        private void ObstacleHandler(bool obstacleDetected)
        {
            if (!obstacleDetected) return;

            _enemyController.PlayAnimation(EnemyAnimationsList.e_idle);
            _rb.velocity = new Vector2(0, _rb.velocity.y);

            _turnAroundTimer -= Time.fixedDeltaTime;

            if (_turnAroundTimer <= 0)
            {
                if (_facingObstacle)
                {
                    _facingObstacle = false;
                    StartFlip();
                }
                else
                {
                    _facingObstacle = true;
                    _turnAroundTimer = timeToTurnAround;
                }
            }
        }

        private void Move(bool obstacleDetected)
        {
            if (!moveAround || obstacleDetected) return;

            _enemyController.PlayAnimation(EnemyAnimationsList.e_walk);
            _rb.velocity = new Vector2(velocity * FacingRight, _rb.velocity.y);
        }

        private void StartFlip()
        {
            FacingRight *= -1;
            _enemyController.FlipEnemy();
        }

        public void DeactivateComponent()
        {
            enabled = false;
        }
    }
}

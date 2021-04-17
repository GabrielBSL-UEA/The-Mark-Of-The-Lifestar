using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemySense : MonoBehaviour
    {
        private EnemyController enemyController;
        private BoxCollider2D boxCol2D;

        private bool playerDetected = false;
        private bool obstacleDetected = false;
        private bool inAttackRangeDetector = false;

        private attackType attack;
        private float range;
        private Vector2 playerPosition;

        [Header("Player Detection")]
        [SerializeField] private Transform enemyEyesPosition;
        [SerializeField] private float awarenessRayDistance = 20f;
        [SerializeField] private string playerTag = "Player";

        [Header("Melee")]
        [SerializeField] private float frontRayDistance = 20f;
        [SerializeField] private float backRayDistance = 0f;

        [Header("Range")]
        [SerializeField] private float playerDetectionRayDistance = 20f;

        [Header("Wall & Edge Detection")]
        [SerializeField] private float collisionDetectionOffset = .1f;
        [SerializeField] private float groundCheckRayValue = .2f;
        [SerializeField] private LayerMask groundLayerMask;

        [Header("Debug")]
        [SerializeField] private bool activateGizmos = false;

        private void Awake()
        {
            enemyController = GetComponent<EnemyController>();
            boxCol2D = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            attack = enemyController.GetAttackType();
            range = enemyController.GetAttackRange();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            obstacleDetected = CheckWall() || !CheckEdge();

            if (playerDetected) {
                if (!CheckNearPlayer(awarenessRayDistance)) playerDetected = false;
                else inAttackRangeDetector = CheckInAttackRange(range);
            }
            else
            {
                if ((attack == attackType.melee && CheckPlayerGround()) ||
                    (attack == attackType.range && CheckNearPlayer(playerDetectionRayDistance)))
                    playerDetected = true;
            }
        }

        private bool CheckPlayerGround()
        {
            RaycastHit2D[] rayHitFront = Physics2D.RaycastAll(enemyEyesPosition.position, Vector2.right * FacingRight(), frontRayDistance);
            RaycastHit2D[] rayHitBack = Physics2D.RaycastAll(enemyEyesPosition.position, Vector2.right * -FacingRight(), backRayDistance);

            for (int i = 0; i < rayHitFront.Length; i++)
            {
                if (rayHitFront[i].transform.CompareTag(playerTag))
                {
                    playerPosition = rayHitFront[i].transform.position;
                    return true;
                }
            }

            for (int i = 0; i < rayHitBack.Length; i++)
            {
                if (rayHitBack[i].transform.CompareTag(playerTag))
                {
                    playerPosition = rayHitBack[i].transform.position;
                    return true;
                }
            }

            return false;
        }

        private bool CheckNearPlayer(float rayRange)
        {
            RaycastHit2D[] rayHit = Physics2D.CircleCastAll(enemyEyesPosition.position, rayRange, Vector2.zero);

            for (int i = 0; i < rayHit.Length; i++)
            {
                if (rayHit[i].transform.CompareTag(playerTag))
                {
                    playerPosition = rayHit[i].transform.position;
                    return true;
                }
            }
            return false;
        }

        private bool CheckInAttackRange(float rayRange)
        {
            RaycastHit2D[] rayHit;

            if (attack == attackType.melee)
                rayHit = Physics2D.RaycastAll(enemyEyesPosition.position, Vector2.right * FacingRight(), rayRange);

            else
                rayHit = Physics2D.CircleCastAll(enemyEyesPosition.position, rayRange, Vector2.zero);

            for (int i = 0; i < rayHit.Length; i++)
            {
                if (rayHit[i].transform.CompareTag(playerTag)) return true;
            }
            return false;
        }

        private bool CheckWall()
        {
            RaycastHit2D rayHit = Physics2D.BoxCast(boxCol2D.bounds.center, boxCol2D.bounds.size - new Vector3(boxCol2D.bounds.extents.x, collisionDetectionOffset, 0f),
                0f, new Vector2(FacingRight(), 0), boxCol2D.bounds.extents.x + collisionDetectionOffset, groundLayerMask);

            if (rayHit.collider != null) return true;
            
            return false;
        }

        private bool CheckEdge()
        {
            RaycastHit2D[] rayHit = Physics2D.RaycastAll(boxCol2D.bounds.center + new Vector3(boxCol2D.bounds.extents.x * FacingRight(), 0, 0), Vector2.down, boxCol2D.bounds.extents.y + groundCheckRayValue);

            for (int i = 0; i < rayHit.Length; i++)
            {
                if (rayHit[i].transform.gameObject.layer == Mathf.Log(groundLayerMask.value, 2)) return true;
            }

            return false;
        }

        private float FacingRight()
        {
            return enemyController.GetFacingRightValue();
        }

        private void OnDrawGizmosSelected()
        {
            if (!activateGizmos) return;

            if (playerDetected)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(enemyEyesPosition.position, awarenessRayDistance);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(enemyEyesPosition.position, new Vector3((enemyEyesPosition.position.x + range * FacingRight()), enemyEyesPosition.position.y, enemyEyesPosition.position.z));
            }
            else
            {
                Gizmos.DrawLine(enemyEyesPosition.position, new Vector3((enemyEyesPosition.position.x + frontRayDistance * FacingRight()), enemyEyesPosition.position.y, enemyEyesPosition.position.z));
                Gizmos.DrawLine(enemyEyesPosition.position, new Vector3((enemyEyesPosition.position.x - backRayDistance * FacingRight()), enemyEyesPosition.position.y, enemyEyesPosition.position.z));
            }
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public bool GetPlayerDetected()
        {
            return playerDetected;
        }

        public bool GetObstacleDetected()
        {
            return obstacleDetected;
        }

        public bool GetinAttackRangeDetector()
        {
            return inAttackRangeDetector;
        }

        public Vector2 GetPlayerPosition()
        {
            return playerPosition;
        }
    }
}

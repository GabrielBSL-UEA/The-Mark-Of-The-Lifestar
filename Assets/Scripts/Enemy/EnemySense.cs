using System.Linq;
using UnityEngine;
using Interactable;

namespace Enemy
{
    public class EnemySense : MonoBehaviour
    {
        private EnemyController _enemyController;
        private BoxCollider2D _boxCol2D;

        public bool PlayerDetected { get; private set; }
        public bool ObstacleDetected { get; private set; }
        public bool InAttackRangeDetector { get; private set; }

        public Transform PlayerPosition { get; private set; }

        private AttackType _attack;
        private float _range;

        [Header("Player Detection")]
        [SerializeField] private Transform enemyEyesPosition;
        [SerializeField] private Transform enemyFeetPosition;
        [SerializeField] private float awarenessRayDistance = 20f;
        [SerializeField] private string playerTag = "Player";

        [Header("Melee")]
        [SerializeField] private float frontRayDistance = 20f;
        [SerializeField] private float backRayDistance;

        [Header("Range")]
        [SerializeField] private float playerDetectionRayDistance = 20f;

        [Header("Wall & Edge Detection")]
        [SerializeField] private float collisionDetectionOffset = .1f;
        [SerializeField] private float groundCheckRayValue = .2f;
        [SerializeField] private LayerMask groundLayerMask;

        [Header("Debug")]
        [SerializeField] private bool activateGizmos;

        private void Awake()
        {
            _enemyController = GetComponent<EnemyController>();
            _boxCol2D = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            _attack = _enemyController.GetAttackType();
            _range = _enemyController.GetAttackRange();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            ObstacleDetected = CheckWall() || !CheckEdge();

            if (PlayerDetected) {
                if (!CheckNearPlayer(awarenessRayDistance)) PlayerDetected = false;
                else InAttackRangeDetector = CheckInAttackRange(_range);
            }
            else
            {
                if ((_attack == AttackType.Melee && CheckPlayerGround()) ||
                    (_attack == AttackType.Range && CheckNearPlayer(playerDetectionRayDistance)))
                    PlayerDetected = true;
            }
        }

        private bool CheckPlayerGround()
        {
            var position = enemyEyesPosition.position;
            
            var rayHitFront = Physics2D.RaycastAll(position, Vector2.right * FacingRight(), frontRayDistance);
            var rayHitBack = Physics2D.RaycastAll(position, Vector2.right * -FacingRight(), backRayDistance);

            foreach (var hit in rayHitFront)
            {
                if (!hit.transform.CompareTag(playerTag) || !CanReceivedHits(hit.transform)) continue;
                
                PlayerPosition = hit.transform;
                return true;
            }

            foreach (var hit in rayHitBack)
            {
                if (!hit.transform.CompareTag(playerTag) || !CanReceivedHits(hit.transform)) continue;
                
                PlayerPosition = hit.transform;
                return true;
            }

            return false;
        }

        private bool CheckNearPlayer(float rayRange)
        {
            var rayHit = Physics2D.CircleCastAll(enemyEyesPosition.position, rayRange, Vector2.zero);

            foreach (var hit in rayHit)
            {
                if (!hit.transform.CompareTag(playerTag) || !CanReceivedHits(hit.transform)) continue;
                
                PlayerPosition = hit.transform;
                return true;
            }
            return false;
        }

        private bool CheckInAttackRange(float rayRange)
        {
            if (_attack == AttackType.Melee)
            {
                var rayHitEyes = Physics2D.RaycastAll(enemyEyesPosition.position, Vector2.right * FacingRight(), rayRange);
                var rayHitFeet = Physics2D.RaycastAll(enemyFeetPosition.position, Vector2.right * FacingRight(), rayRange);

                foreach (var hit in rayHitEyes)
                {
                    if (!hit.transform.CompareTag(playerTag) || !CanReceivedHits(hit.transform)) continue;
                    
                    PlayerPosition = hit.transform;
                    return true;
                }

                foreach (var hit in rayHitFeet)
                {
                    if (!hit.transform.CompareTag(playerTag) || !CanReceivedHits(hit.transform)) continue;
                    
                    PlayerPosition = hit.transform;
                    return true;
                }
            }
            else
            {
                var rayHit = Physics2D.CircleCastAll(enemyEyesPosition.position, rayRange, Vector2.zero);

                return rayHit.Any(hit => hit.transform.CompareTag(playerTag) && CanReceivedHits(hit.transform));
            }
            return false;
        }

        private bool CheckWall()
        {
            var bounds = _boxCol2D.bounds;
            var rayHit = Physics2D.BoxCast(bounds.center, bounds.size - new Vector3(bounds.extents.x, collisionDetectionOffset, 0f),
                0f, new Vector2(FacingRight(), 0), bounds.extents.x + collisionDetectionOffset, groundLayerMask);

            return rayHit.collider != null;
        }

        private bool CheckEdge()
        {
            var bounds = _boxCol2D.bounds;
            var rayHit = Physics2D.RaycastAll(bounds.center + new Vector3(bounds.extents.x * FacingRight(), 0, 0), Vector2.down, bounds.extents.y + groundCheckRayValue);

            return rayHit.Any(hit => Mathf.Approximately(hit.transform.gameObject.layer, Mathf.Log(groundLayerMask.value, 2)));
        }

        public void ForceCheckNearPlayer()
        {
            if (CheckNearPlayer(playerDetectionRayDistance)) PlayerDetected = true;
        }

        private static bool CanReceivedHits(Component player)
        {
            return player.GetComponent<HitReceiver>().GetCanReceivedHit();
        }

        private float FacingRight()
        {
            return _enemyController.GetFacingRightValue();
        }

        private void OnDrawGizmosSelected()
        {
            if (!activateGizmos) return;

            var eyesPosition = enemyEyesPosition.position;
            
            if (PlayerDetected)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(eyesPosition, awarenessRayDistance);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(eyesPosition, new Vector3((eyesPosition.x + _range * FacingRight()), eyesPosition.y, eyesPosition.z));
                Gizmos.DrawLine(eyesPosition, new Vector3((eyesPosition.x + _range * FacingRight()), eyesPosition.y, eyesPosition.z));
            }
            else
            {
                Gizmos.DrawLine(eyesPosition, new Vector3((eyesPosition.x + frontRayDistance * FacingRight()), eyesPosition.y, eyesPosition.z));
                Gizmos.DrawLine(eyesPosition, new Vector3((eyesPosition.x - backRayDistance * FacingRight()), eyesPosition.y, eyesPosition.z));
            }
        }
    }
}

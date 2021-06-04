using UnityEngine;
using Interactable;
using Camera;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour, IHittable
    {
        EnemyController _enemyController;
        Rigidbody2D _rb;

        [Header("Health")]
        [SerializeField] private float maxHealth = 30;

        [Header("Camera Queue")]
        [SerializeField] private float cameraShakeIntensity = 25f;
        [SerializeField] private float cameraShakeTime = .15f;

        [Header("Critic")]
        [SerializeField] private bool acceptCritic = true;
        [SerializeField] private float criticStunPenaltyMultiplier = 2;

        [Header("Stun")]
        [SerializeField] private float stunTime = 1;
        [SerializeField] private float stunHandleLimit = 10;
        [SerializeField] private float damageStunPenaltyMultiplier = 2;
        [SerializeField] private float stunValueDecreaseBySecond = 1;

        public bool IsAlive { get; private set; } = true; //IHittable variable
        public bool IsStunned { get; private set; }

        private float _stunValue;
        private float _stunTimer;
        private float _currentHealth;

        private void Awake()
        {
            _enemyController = GetComponent<EnemyController>();
            _rb = GetComponent<Rigidbody2D>();
            _currentHealth = maxHealth;
        }

        private void Update()
        {
            if (IsStunned)
            {
                if (_stunTimer < 0) IsStunned = false;
                else _stunTimer -= Time.deltaTime;
            }

            else if (_stunValue > 0) _stunValue -= stunValueDecreaseBySecond * Time.deltaTime;
        }

        public void RegisterHit(float damage, float stun, Transform aggressor)
        {
            if (!IsAlive) return;

            _currentHealth = IsStunned ? _currentHealth - (damage * damageStunPenaltyMultiplier) : _currentHealth - damage;
            var stunPenalty = IsStunned ? .5f : 1f;

            var direction = _enemyController.GetFacingRightValue();
            var criticHit = false;

            if (acceptCritic && !IsStunned
                && (aggressor.position.x < transform.position.x && Mathf.Approximately(direction, 1))
                || (aggressor.position.x > transform.position.x && Mathf.Approximately(direction, -1)))
            {
                CinemachineShake.Instance.StartShake(cameraShakeIntensity, cameraShakeTime);
                _stunValue += stun * criticStunPenaltyMultiplier;
                criticHit = true;
                _enemyController.CheckNearbyPlayer();
            }
            else
            {
                CinemachineShake.Instance.StartShake(cameraShakeIntensity / 2, cameraShakeTime / 2);
                _stunValue += stun * stunPenalty;
            }
            _enemyController.ApplyBlink(criticHit);

            if (_currentHealth <= 0)
            {
                IsAlive = false;
                _enemyController.SetHitReceiver(false);
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }
            else if (_stunValue >= stunHandleLimit)
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                IsStunned = true;
                _stunValue = 0;
                _stunTimer = stunTime;
            }
        }
    }
}

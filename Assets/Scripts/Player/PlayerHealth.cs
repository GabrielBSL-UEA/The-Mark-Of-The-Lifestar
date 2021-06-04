using UnityEngine;
using Interactable;
using Camera;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IHittable
    {
        PlayerController _playerController;
        Rigidbody2D _rb;

        [Header("Health")]
        [SerializeField] private float maxHealth = 50f;

        [Header("Camera Queue")]
        [SerializeField] private float cameraShakeIntensity = 40f;
        [SerializeField] private float cameraShakeTime= .2f;

        [Header("Hit")]
        [SerializeField] private float invulnerabilityTime = 1.5f;
        [SerializeField] private float hitStunTime = .2f;

        public bool IsAlive { get; private set; } = true; //Interface variable
        public bool IsStunned { get; private set; }
        public float AggressorDirection { get; private set; }

        private float _invulnerabilityTimer;
        private float _hitStunTimer;
        private float _currentHealth;

        private bool _isInvulnerable;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            _rb = GetComponent<Rigidbody2D>();
            _currentHealth = maxHealth;
        }

        private void Update()
        {
            if (IsStunned)
            {
                if (_hitStunTimer < 0) IsStunned = false;
                else _hitStunTimer -= Time.deltaTime;
            }

            if (_isInvulnerable)
            {
                if (_invulnerabilityTimer < 0)
                {
                    _isInvulnerable = false;
                    _playerController.MakePlayerBlink(false);
                }
                else _invulnerabilityTimer -= Time.deltaTime;
            }
        }

        public void RegisterHit(float damage, float stun, Transform aggressor)
        {
            if (!IsAlive || _isInvulnerable || _playerController.GetDashState()) return;
            
            CinemachineShake.Instance.StartShake(cameraShakeIntensity, cameraShakeTime);
            AggressorDirection = aggressor.position.x < transform.position.x ? 1 : -1;
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                _playerController.EnablePlayerInputs(false);
                IsAlive = false;
                _playerController.SetHitReceiver(false);
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }
            else
            {
                _invulnerabilityTimer = invulnerabilityTime;
                _playerController.MakePlayerBlink(true);
                _hitStunTimer = hitStunTime;
                _isInvulnerable = true;
                IsStunned = true;

                _playerController.PlayAnimation(PlayerAnimationsList.p_hurt);
            }
        }
    }
}

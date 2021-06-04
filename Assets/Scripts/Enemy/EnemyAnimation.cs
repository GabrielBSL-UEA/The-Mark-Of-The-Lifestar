using UnityEngine;

namespace Enemy
{
    public class EnemyAnimation : MonoBehaviour
    {
        [SerializeField] private float blinkDuration = .2f;

        private float _blinkDurationTimer;
        private bool _criticHit;

        private EnemyAnimationsList _currentAnimation;
        private SpriteRenderer _spriteRenderer;
        private Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (_blinkDurationTimer <= 0) return;

            _blinkDurationTimer -= Time.deltaTime;
            if (_criticHit) _spriteRenderer.color = new Color(1, (blinkDuration - _blinkDurationTimer) / blinkDuration, (blinkDuration - _blinkDurationTimer) / blinkDuration);
            else _spriteRenderer.color = new Color(1, (blinkDuration - _blinkDurationTimer / 2) / blinkDuration, (blinkDuration - _blinkDurationTimer / 2) / blinkDuration);
        }

        public void Flip()
        {
            var enemyTransform = transform;
            
            Vector2 scaler = enemyTransform.localScale;
            scaler.x *= -1;
            enemyTransform.localScale = scaler;
        }

        public void Play(EnemyAnimationsList enemyAnimationsList)
        {
            if (_currentAnimation.Equals(enemyAnimationsList)) return;

            _anim.Play(enemyAnimationsList.ToString());
            _currentAnimation = enemyAnimationsList;
        }

        public void Blink(bool critic)
        {
            _criticHit = critic;
            _blinkDurationTimer = blinkDuration;
        }

        public void DeactivateAnimationScript()
        {
            enabled = false;
        }
    }
}
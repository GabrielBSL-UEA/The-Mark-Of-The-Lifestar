using UnityEngine;

namespace Player
{
    public enum PlayerAnimationsList
    {
        p_idle,
        p_run,
        p_attack_1,
        p_attack_1_transition,
        p_attack_2,
        p_jump,
        p_jump_to_fall,
        p_fall,
        p_wall_slide,
        p_dash_start,
        p_dash,
        p_dash_finish,
        p_dash_attack,
        p_hurt,
        p_death
    }

    public class PlayerAnimations : MonoBehaviour
    {
        [SerializeField] private float blinkFrequency = .05f;

        private float _blinkFrequencyTimer;

        private Animator _anim;
        private SpriteRenderer _spriteRenderer;
        private PlayerController _playerController;

        private bool _facingRight = true;
        private bool _blinking;

        private PlayerAnimationsList _currentAnimation;

        // Start is called before the first frame update
        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            float direction = _playerController.GetMovementInputs().x;

            if (Mathf.Abs(direction) > _playerController.GetDeadZone() && (!_playerController.GetPlayerIsAttacking() || _playerController.GetDashPerfomed())) Flip(direction > 0);

            if (_blinking) MakePlayerBlink();
        }

        private void MakePlayerBlink()
        {
            _blinkFrequencyTimer += Time.deltaTime;

            if (_blinkFrequencyTimer > blinkFrequency)
            {
                _spriteRenderer.enabled = !_spriteRenderer.enabled;
                _blinkFrequencyTimer = 0;
            }
        }

        private void Flip(bool right)
        {
            if (_facingRight == right) return;
            var playerTransform = transform;
            
            Vector2 scaler = playerTransform.localScale;
            scaler.x *= -1;
            playerTransform.localScale = scaler;
            _facingRight = right;
        }

        public void ForceFlip()
        {
            Flip(!_facingRight);
        }

        public void Play(PlayerAnimationsList playerAnimationsList)
        {
            if (_currentAnimation.Equals(playerAnimationsList)) return;

            //Exceptions
            if (playerAnimationsList == PlayerAnimationsList.p_jump_to_fall && _currentAnimation == PlayerAnimationsList.p_fall) return;
            if (playerAnimationsList == PlayerAnimationsList.p_dash_start && _currentAnimation == PlayerAnimationsList.p_dash) return;
            if (playerAnimationsList == PlayerAnimationsList.p_jump_to_fall && _currentAnimation != PlayerAnimationsList.p_jump) playerAnimationsList = PlayerAnimationsList.p_fall;
            //Exceptions

            _anim.Play(playerAnimationsList.ToString());

            _currentAnimation = playerAnimationsList;
        }

        public void StartBlink(bool value)
        {
            _blinking = value;

            if (!value) _spriteRenderer.enabled = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
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

        private float blinkFrequencyTimer = 0;

        private Rigidbody2D rb;
        private Animator anim;
        private SpriteRenderer spriteRenderer;
        private PlayerController playerController;

        private bool facingRight = true;
        private bool blinking = false;

        private PlayerAnimationsList currentAnimation;

        // Start is called before the first frame update
        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            float direction = playerController.GetMovementInputs().x;

            if (Mathf.Abs(direction) > playerController.GetDeadZone() && !playerController.GetPlayerIsAttacking()) Flip(direction > 0);

            if (blinking)
            {
                blinkFrequencyTimer += Time.deltaTime;

                if(blinkFrequencyTimer > blinkFrequency)
                {
                    spriteRenderer.enabled = !spriteRenderer.enabled;
                    blinkFrequencyTimer = 0;
                }
            }
        }

        public void Flip(bool right)
        {
            if (facingRight == right) return;

            Vector2 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
            facingRight = right;
        }

        public void ForceFlip()
        {
            Flip(!facingRight);
        }

        public void Play(PlayerAnimationsList animation)
        {
            if (currentAnimation.Equals(animation)) return;

            //Exceções
            if (animation == PlayerAnimationsList.p_jump_to_fall && currentAnimation == PlayerAnimationsList.p_fall) return;
            if (animation == PlayerAnimationsList.p_dash_start && currentAnimation == PlayerAnimationsList.p_dash) return;
            if (animation == PlayerAnimationsList.p_jump_to_fall && currentAnimation != PlayerAnimationsList.p_jump) animation = PlayerAnimationsList.p_fall;
            //Exceções

            anim.Play(animation.ToString());

            currentAnimation = animation;
        }

        public void StartBlink(bool value)
        {
            blinking = value;

            if (!value) spriteRenderer.enabled = true;
        }
    }
}

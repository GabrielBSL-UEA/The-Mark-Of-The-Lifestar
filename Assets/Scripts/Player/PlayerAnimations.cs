using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public enum AnimationsList
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
        private Rigidbody2D rb;
        private Animator anim;
        private PlayerController playerController;

        private bool facingRight = true;
        private AnimationsList currentAnimation;

        // Start is called before the first frame update
        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            float direction = playerController.GetMovementInputs().x;

            if (Mathf.Abs(direction) > playerController.GetDeadZone() && !playerController.GetPlayerIsAttacking()) Flip(direction > 0);
        }

        private void Flip(bool right)
        {
            if (facingRight == right) return;

            Vector2 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
            facingRight = right;
        }

        public void Play(AnimationsList animation)
        {
            if (currentAnimation.Equals(animation)) return;

            //Exceções
            if (animation == AnimationsList.p_jump_to_fall && currentAnimation == AnimationsList.p_fall) return;
            if (animation == AnimationsList.p_dash_start && currentAnimation == AnimationsList.p_dash) return;

            anim.Play(animation.ToString());

            currentAnimation = animation;
        }
    }
}

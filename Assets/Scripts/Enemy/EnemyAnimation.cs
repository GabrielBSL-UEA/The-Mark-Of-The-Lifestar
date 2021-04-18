using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyAnimation : MonoBehaviour
    {
        [SerializeField] private float blinkDuration;

        private float blinkDurationTimer = 0;
        private bool critHit = false;

        private EnemyAnimationsList currentAnimation;
        private SpriteRenderer spriteRenderer;
        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (blinkDurationTimer <= 0) return;

            blinkDurationTimer -= Time.deltaTime;
            if (critHit) spriteRenderer.color = new Color(1, (blinkDuration - blinkDurationTimer) / blinkDuration, (blinkDuration - blinkDurationTimer) / blinkDuration);
            else spriteRenderer.color = new Color(1, (blinkDuration - blinkDurationTimer / 2) / blinkDuration, (blinkDuration - blinkDurationTimer / 2) / blinkDuration);
        }

        public void Flip()
        {
            Vector2 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
        }

        public void Play(EnemyAnimationsList animation)
        {
            if (currentAnimation.Equals(animation)) return;

            anim.Play(animation.ToString());
            currentAnimation = animation;
        }

        public void Blink(bool crit)
        {
            critHit = crit;
            blinkDurationTimer = blinkDuration;
        }
    }
}
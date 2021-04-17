using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public enum EnemyAnimationsList
    {
        e_idle,
        e_walk,
        e_attack,
        e_stun,
        e_dead
    }

    public class EnemyAnimation : MonoBehaviour
    {
        private EnemyAnimationsList currentAnimation;

        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
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
    }
}
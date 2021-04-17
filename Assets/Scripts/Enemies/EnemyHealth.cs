using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour, IHitable
    {
        EnemyController enemyController;
        Rigidbody2D rb;

        [Header("Health")]
        [SerializeField] private float maxHealth;

        [Header("Stun")]
        [SerializeField] private float stunTime;
        [SerializeField] private float stunHandleLimit;
        [SerializeField] private float stunValue = 0;
        [SerializeField] private float stunValueDecreaseOverSecond;

        private float stunTimer = 0;

        private float currentHealth;
        private bool isAlive = true;
        private bool isStunned = false;

        private void Awake()
        {
            enemyController = GetComponent<EnemyController>();
            rb = GetComponent<Rigidbody2D>();
            currentHealth = maxHealth;
        }

        private void Update()
        {
            if (isStunned)
            {
                if(stunTimer < 0) isStunned = false;
                else stunTimer -= Time.deltaTime;
            }

            else if (stunValue > 0) stunValue -= stunValueDecreaseOverSecond * Time.deltaTime;
        }

        public void RegisterHit(float damage, float stun, float direction)
        {
            if (!isAlive) return;

            currentHealth -= damage;

            if(!isStunned) stunValue = enemyController.GetFacingRightValue() == direction ? stunValue + stun * 2 : stunValue + stun;

            if (currentHealth <= 0)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                enemyController.PlayAnimation(EnemyAnimationsList.e_dead);
                isAlive = false;
            }
            else if (stunValue >= stunHandleLimit)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                enemyController.PlayAnimation(EnemyAnimationsList.e_stun);
                isStunned = true;
                stunValue = 0;
                stunTimer = stunTime;
            }
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public bool GetIsAlive()
        {
            return isAlive;
        }

        public bool GetIsStunned()
        {
            return isStunned;
        }
    }
}

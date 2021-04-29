using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactible;
using Camera;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour, IHitable
    {
        EnemyController enemyController;
        Rigidbody2D rb;

        [Header("Health")]
        [SerializeField] private float maxHealth = 30;

        [Header("Camera Queue")]
        [SerializeField] private float cameraShakeIntensity = 25f;
        [SerializeField] private float cameraShakeTime = .15f;

        [Header("Critic")]
        [SerializeField] private bool acceptCritic = true;
        [SerializeField] private float critStunPenaltyMultiplier = 2;

        [Header("Stun")]
        [SerializeField] private float stunTime = 1;
        [SerializeField] private float stunHandleLimit = 10;
        [SerializeField] private float damageStunPenaltyMultiplier = 2;
        [SerializeField] private float stunValueDecreaseBySecond = 1;

        public bool isAlive { get; private set; } = true; //IHitable variable
        public bool isStunned { get; private set; } = false;

        private float stunValue = 0;
        private float stunTimer = 0;
        private float currentHealth;

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
                if (stunTimer < 0) isStunned = false;
                else stunTimer -= Time.deltaTime;
            }

            else if (stunValue > 0) stunValue -= stunValueDecreaseBySecond * Time.deltaTime;
        }

        public void RegisterHit(float damage, float stun, Transform agressor)
        {
            if (!isAlive) return;

            currentHealth = isStunned ? currentHealth - (damage * damageStunPenaltyMultiplier) : currentHealth - damage;
            float stunPenalty = isStunned ? .5f : 1f;

            float direction = enemyController.GetFacingRightValue();
            bool critHit = false;

            if (acceptCritic && !isStunned
                && (agressor.position.x < transform.position.x && direction == 1)
                || (agressor.position.x > transform.position.x && direction == -1))
            {
                CinemachineShake.Instance.StartShake(cameraShakeIntensity, cameraShakeTime);
                stunValue += stun * critStunPenaltyMultiplier;
                critHit = true;
                enemyController.CheckNearbyPlayer();
            }
            else
            {
                CinemachineShake.Instance.StartShake(cameraShakeIntensity / 2, cameraShakeTime / 2);
                stunValue += stun * stunPenalty;
            }
            enemyController.ApplyBlink(critHit);

            if (currentHealth <= 0)
            {
                isAlive = false;
                enemyController.SetHitReciever(false);
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else if (stunValue >= stunHandleLimit)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                isStunned = true;
                stunValue = 0;
                stunTimer = stunTime;
            }
        }
    }
}

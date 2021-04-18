using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactible;
using Camera;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IHitable
    {
        PlayerController playerController;
        Rigidbody2D rb;

        [Header("Health")]
        [SerializeField] private float maxHealth;

        [Header("Hit")]
        [SerializeField] private float cameraShakeIntensity = 40f;
        [SerializeField] private float cameraShakeTime= .2f;
        [SerializeField] private float invunerabilityTime;
        [SerializeField] private float hitStunTime;
        
        private float invunerabilityTimer = 0;
        private float hitStunTimer = 0;
        private float currentHealth;
        private float agressorDirection;

        private bool isAlive = true;
        private bool isStunned = false;
        private bool isInvunerable = false;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            rb = GetComponent<Rigidbody2D>();
            currentHealth = maxHealth;
        }

        private void Update()
        {
            if (isStunned)
            {
                if (hitStunTimer < 0) isStunned = false;
                else hitStunTimer -= Time.deltaTime;
            }

            if (isInvunerable)
            {
                if (invunerabilityTimer < 0)
                {
                    isInvunerable = false;
                    playerController.MakePlayerBlink(false);
                }
                else invunerabilityTimer -= Time.deltaTime;
            }
        }

        public void RegisterHit(float damage, float stun, Transform agressor)
        {
            if (!isAlive || isInvunerable || playerController.GetDashState()) return;
            
            CinemachineShake.Instance.StartShake(cameraShakeIntensity, cameraShakeTime);
            agressorDirection = agressor.position.x < transform.position.x ? 1 : -1;
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                playerController.EnablePlayerInputs(false);
                isAlive = false;
                playerController.SetHitReciever(false);
                rb.velocity = new Vector2(0, rb.velocity.y);
                playerController.PlayAnimation(PlayerAnimationsList.p_death);
            }
            else
            {
                invunerabilityTimer = invunerabilityTime;
                playerController.MakePlayerBlink(true);
                hitStunTimer = hitStunTime;
                isInvunerable = true;
                isStunned = true;
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

        public float GetAgressorDirection()
        {
            return agressorDirection;
        }
    }
}

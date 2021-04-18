using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Camera;
using Interactible;
using System;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        PlayerController playerController;
        Rigidbody2D rb;

        [Header("Hit")]
        [SerializeField] private Transform[] hitDetectors;
        [SerializeField] private float hitRange;
        [SerializeField] private LayerMask hitLayer;

        [Header("Camera Queue")]
        [SerializeField] private float cameraShakeIntensity = 25f;
        [SerializeField] private float cameraShakeTime = .15f;

        [Header("Attack")]
        [SerializeField] private float attackDamage = 5f;
        [SerializeField] private float attackDelay = .7f;
        [SerializeField] private float stunForce = 1f;


        private readonly PlayerAnimationsList[] attackAnimations =
        {
            PlayerAnimationsList.p_attack_1,
            PlayerAnimationsList.p_attack_2
        };

        private float gravityCache = 0;
        private bool isAttacking = false;
        private bool attackBuffer = false;
        private bool canBufferAttack = false;

        private int comboCounter = 0;
        private float attackDelayTimer = Mathf.Infinity;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            gravityCache = rb.gravityScale;
            playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if(attackDelayTimer < attackDelay) attackDelayTimer += Time.deltaTime;
        }

        public void AttackInterpreter(bool attackInput, bool dashing, bool stunned)
        {
            if (stunned) ResetAttack();

            else if (attackDelayTimer < attackDelay || !attackInput) return;

            else if (!isAttacking)
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;

                isAttacking = attackInput;
                playerController.PlayAnimation(attackAnimations[comboCounter]);
                comboCounter++;
            } 
            else if(canBufferAttack) attackBuffer = attackInput;
        }

        private void ResetAttack()
        {
            rb.gravityScale = gravityCache;
            attackDelayTimer = 0;
            comboCounter = 0;

            isAttacking = false;
            attackBuffer = false;
            canBufferAttack = false;
        }

        //-----------------------------------------------------------------
        //**********              Animation Calls                **********
        //-----------------------------------------------------------------

        public void ActivateAttackBuffer()
        {
            canBufferAttack = true;
        }

        public void OnTransitionStart()
        {
            rb.gravityScale = gravityCache;
        }

        public void OnTransitionEnd()
        {
            if (attackBuffer && comboCounter < attackAnimations.Length)
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
                playerController.PlayAnimation(attackAnimations[comboCounter]);
                comboCounter++;
            }
            else
            {
                rb.gravityScale = gravityCache;
                attackDelayTimer = 0;
                comboCounter = 0;
                isAttacking = false;
            }
            attackBuffer = false;
            canBufferAttack = false;
        }

        public void DetectHits()
        {
            List<Transform> objectsList = new List<Transform>();

            for (int i = 0; i < hitDetectors.Length; i++)
            {
                Collider2D[] objectHits = Physics2D.OverlapCircleAll(hitDetectors[i].position, hitRange, hitLayer);

                for (int j = 0; j < objectHits.Length; j++)
                {
                    if (objectsList.Contains(objectHits[j].transform)) continue;
                    objectsList.Add(objectHits[j].transform);
                }
            }

            int totalObjects = 0;

            foreach (Transform hit in objectsList)
            {
                if (hit.GetComponent<HitReciever>() != null && hit.GetComponent<HitReciever>().GetCanRecieveHit())
                {
                    hit.GetComponent<HitReciever>().RecieveHit(attackDamage, stunForce, transform);
                    totalObjects++;
                }
            }

            if(totalObjects > 0)
            {
                playerController.ResetInputCounters();
                CinemachineShake.Instance.StartShake(cameraShakeIntensity, cameraShakeTime);
            }
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public bool GetIsAttacking()
        {
            return isAttacking;
        }

        //----------------------------------------------------------------
        //*************                Gizmos                *************
        //----------------------------------------------------------------

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < hitDetectors.Length; i++)
            {
                Gizmos.DrawWireSphere(hitDetectors[i].position, hitRange);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAnimations : MonoBehaviour
    {
        private Rigidbody2D rb;

        private bool facingRight = true;


        // Start is called before the first frame update
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (rb.velocity.x > 0 && !facingRight) Flip(true);
            else if (rb.velocity.x < 0 && facingRight) Flip(false);
        }

        private void Flip(bool right)
        {
            Vector2 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
            facingRight = right;
        }
    }
}

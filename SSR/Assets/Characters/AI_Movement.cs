using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Character;

namespace SS.AI
{
    public class AI_Movement : MonoBehaviour
    {

        public float movementSpeed;
        private void SetMoveSpeed(float speed) { movementSpeed = speed; }
        public bool moveLeft;
        public bool grounded;
        private Character_Stats characterStats;
        public float jumpPower;

        private AI_Brain aIBrain;

        //=====Getters and Setters=====
        public void SetMoveLeft(bool keepMovingLeft) { moveLeft = keepMovingLeft; }
        
        //=============================


        private Rigidbody2D rb;

        // Use this for initialization
        void Start()
        {
            SetRefrences();
            SetStats();
        }
        private void SetRefrences()
        {
            rb = this.GetComponent<Rigidbody2D>();
            characterStats = this.GetComponent<Character_Stats>();
            aIBrain = this.GetComponent<AI_Brain>();
        }
        private void SetStats()
        {
            jumpPower = characterStats.GetJumpPower();
        }
        

        // Update is called once per frame
        void Update()
        {
            ProcessHorizontalMovement();           
        }

        private void ProcessHorizontalMovement()
        {
            if (!characterStats.GetKnockedBack())
            {
                movementSpeed = characterStats.GetMovementSpeed();
                if (aIBrain.GetTarget() != null)
                {
                    if (moveLeft) //TODO DETECT THE DIRECTION
                    {
                        rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
                        ChangeLookDirectionToLeft(true);
                    }
                    else
                    {
                        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
                        ChangeLookDirectionToLeft(false);
                    }
                }
            }
        }
        public void ProcessJump()
        {
            grounded = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            if (grounded)
            {
                grounded = false;                
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
        }
        

        private void ChangeLookDirectionToLeft(bool shouldGoLeft)
        {
            if (shouldGoLeft)
            {
                this.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else
            {
                this.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Ground" && this.gameObject.tag == "Enemy" || collision.gameObject.tag == "Ground" && this.gameObject.tag == "Enemy")
            {

                grounded = true;
            }
        }
        private void StantIdle()
        {

        }
        
    }
}
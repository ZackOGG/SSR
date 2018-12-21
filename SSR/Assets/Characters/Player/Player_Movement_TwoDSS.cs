using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.Character
{
    public class Player_Movement_TwoDSS : MonoBehaviour
    {
        [SerializeField] float halfJumpPower;

        private bool cantMove = false;
        private float movementSpeed = 5f;
        private float jumpPower = 15f;
        private Follow_Mouse followMouse;
        private Rigidbody2D rb;
        private Character_Stats characterStats;
        public bool grounded = true;
        private Vector2 lastWalkPos;
        private bool moveInput;
        private bool knockedBack = false;
        private float knockBackTimer;
        private Animator_Controller animCon;        
        private CapsuleCollider2D hitBox;
        public bool isPlatform;
        private GameObject player;
        public bool dead = false;
        public void SetDead(bool newBool) { dead = newBool; }
        public void SetCantMove(bool newBool) { cantMove = newBool; }

        // Use this for initialization
        void Start()
        {
            SetRefrences();
            StartCoroutine("LateStart");
        }
        IEnumerator LateStart()
        {
            yield return new WaitForSeconds(0.1f);
            SetStats();
        }

        private void SetRefrences()
        {
            rb = this.GetComponent<Rigidbody2D>();
            characterStats = this.GetComponent<Character_Stats>();
            animCon = this.GetComponent<Animator_Controller>();            
            hitBox = this.GetComponent<CapsuleCollider2D>();
            //characterStats.KnockingBack += KnockBack;
            characterStats.callingDeath += SetDead;
            player = characterStats.gameObject;
            followMouse = GameObject.FindObjectOfType<Follow_Mouse>();
        }
        //=====Getters and Setters=====
        private void SetStats()
        {
            movementSpeed = characterStats.GetMovementSpeed();
            jumpPower = characterStats.GetJumpPower();
        }
        public void SetGrounded(bool theBool) { grounded = theBool; }
        public void SetPlatform(bool theBool) { isPlatform = theBool; }

        //==============================

        // Update is called once per frame
        void Update()
        {
            if(!dead && !cantMove)
            {
                ProcessHorizontalMovement();
                //ProcessCrouch();            
                ProcessJump();
                ProcessHalfJump();
                ProcessFallThroughPlatform();
                ProcessLookDirection();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
            


        }
        //=====Movement=====
        private void ProcessHorizontalMovement()
        {
            if (!characterStats.GetKnockedBack())
            {
                float h = Input.GetAxis("Horizontal");

                rb.velocity = new Vector2(h * movementSpeed, rb.velocity.y);
                if (Input.GetKey(KeyCode.D)) //TODO DETECT THE DIRECTION
                {
                    //FaceRight();
                    moveInput = true;
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    //FaceLeft();
                    moveInput = true;
                }
                else
                {
                    moveInput = false;
                }
            }
        }

        private void ProcessJump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.LeftControl) && grounded)
            {
                grounded = false;
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }            
        }
        private void ProcessHalfJump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.LeftControl) && grounded)
            {
                grounded = false;
                rb.velocity = new Vector2(rb.velocity.x, (halfJumpPower));
            }
        }
        private void ProcessFallThroughPlatform() // TODO Make this nicer
        {
            if (Input.GetKeyDown(KeyCode.S) && isPlatform)
            {
                player.layer = LayerMask.NameToLayer("Player_NoPlatforms");
            }
        }
       
        private void ProcessLookDirection()
        {
            Vector2 mousePos = followMouse.GetPositionVTwo();
            Vector2 thisPos = this.transform.position;
            Vector2 direction = mousePos - thisPos;

            if(direction.x >0)
            {
                FaceRight();
            }
            if(direction.x < 0)
            {
                FaceLeft();
            }
        }

        //========================

        private void OnTriggerStay2D(Collider2D collision)
        {
            /*if (collision.gameObject.tag == "Ground" && this.gameObject.tag == "Player" || collision.gameObject.tag == "Platform" && this.gameObject.tag == "Player")
            {
                grounded = true;
                
            }
            if(collision.gameObject.tag == "Ground" && this.gameObject.tag == "Player")
            {
                TurnColliderOn();
            }
            if(collision.gameObject.tag == "Platform")
            {
                isPlatform = true;
            }
            else
            {
                isPlatform = false;
            }*/

        }

        /*private void KnockBack(float force, float duration, Vector3 direction)
        {
            StopAllCoroutines();
            knockedBack = true;
            print("Player getting knocked back");
            knockBackTimer = duration;
            print(direction);
            rb.velocity = -direction * force;
            StartCoroutine("KnockBackStop");
        }
        IEnumerator KnockBackStop()
        {
            yield return new WaitForSeconds(knockBackTimer);
            //rb.velocity = Vector3.zero;
            knockedBack = false;
        }*/
        //=====Direction Facing=====
        private void FaceRight()
        {
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        private void FaceLeft()
        {
            this.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        //==========================
        //=====Public functions=====
        public void TurnColliderOff()
        {
            hitBox.enabled = false;
        }
        public void TurnColliderOn()
        {
            hitBox.enabled = true;
        }

        //==========================
        /*/=====DetectMoveing=====
        private void DetectWalking()
        {
            lastWalkPos = this.transform.position;
            StartCoroutine("CheckWalking");
        }
        IEnumerator CheckWalking()
        {
            yield return new WaitForSeconds(0.1f);
            Vector2 currentPos = this.transform.position;
            if (currentPos == lastWalkPos)
            {                
                characterStats.SetWalking(false);
            }
            else
            {
                if (moveInput)
                {                    
                    characterStats.SetWalking(true);
                }
            }
            DetectWalking();
        }

        //=======================*/
    }
}
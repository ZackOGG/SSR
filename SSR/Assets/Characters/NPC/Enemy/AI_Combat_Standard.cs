using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Character;
using System;

/*
 This script is for standard attack pattens.
 What it Does
    1.It will walk towards the target
    2.It will detect which direction the target is and change diraction acordingly
    3.If it is in range of the target it will tell the Combat Controller to attack
What it does not do
    1.Run any kind of damage calcualtions
    2.Deal damage
 */
namespace SS.AI
{
    public class AI_Combat_Standard : MonoBehaviour
    {
        [Header("Combat Stats")]
        [SerializeField] float attackRange = 2f;
        //[SerializeField] float atemptAttackTime = 2f; // This is the ai at when it should attack

        private float attackSpeed;        
        private float attackTimer;
        private float aATr; 
        public AI_Animator_Controller aIAnimCon;        
        public GameObject target;
        public void SetTarget(GameObject theTarget) { target = theTarget; }
        private float agroRange;
        private AI_Brain aIBrain;
        private bool targetIsLeft = false;
        private PolygonCollider2D attackHitBox;
        private float movementSpeed = 5f;
        private Character_Stats characterStats;
        private Rigidbody2D rb;
        private AI_Combat_Controller combatCon;
        private AI_Movement aIMovement;
        private Weapon_Controller wepCon;
        // Use this for initialization
        void Start()
        {
            SetRefences();
            SetStats();
        }

        private void SetRefences()
        {
            aIAnimCon = this.GetComponent<AI_Animator_Controller>();
            aIBrain = this.GetComponent<AI_Brain>();
            attackHitBox = this.GetComponent<PolygonCollider2D>();
            characterStats = this.GetComponent<Character_Stats>();
            rb = this.GetComponent<Rigidbody2D>();
            combatCon = this.GetComponentInChildren<AI_Combat_Controller>();
            aIMovement = this.GetComponent<AI_Movement>();
            wepCon = this.GetComponentInChildren<Weapon_Controller>();
        }
        private void SetStats()
        {
            attackSpeed = characterStats.GetAttackSpeed();
            agroRange = aIBrain.GetAgroRange();            
            attackTimer = attackSpeed;
            movementSpeed = characterStats.GetMovementSpeed();
            //aATr = atemptAttackTime;
        }

        // Update is called once per frame
        void Update()
        {            
            AttackHandler();
            FollowTarget();           
            DetectTargetDirection();            
        }
      
        private void AttackHandler()
        {
            aATr -= Time.deltaTime;

            if (aATr <= 0 && InAttackRange()) // Starting the attack (atemptAttackTime) after the attack has ended
            {
                //combatCon.Standard_Attack();
                aIAnimCon.callingAttack();

                aATr = aIAnimCon.GetAttackClipLength() / attackSpeed;
            }
        }

        private void DetectTargetDirection()
        {
            
            var direction = target.transform.position - this.transform.position;
            if(direction.x > 0)
            {                
                targetIsLeft = false;
            }
            if (direction.x < 0)
            {
                targetIsLeft = true;
            }            
            ChangeDirection();
        }
        private bool InAttackRange()
        {
            bool result;
            var distance = target.transform.position - this.transform.position;
            if (distance.magnitude < attackRange)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private void ChangeDirection()
        {
            if(targetIsLeft)
            {
                this.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else
            {
                this.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }
        
        private void FollowTarget()//TODO MAKE MOVEMENT CONTROLLER FOR NPCS
        {
            if(targetIsLeft)
            {
                aIMovement.SetMoveLeft(true);
            }
            else
            {
                aIMovement.SetMoveLeft(false);
            }
        }
        

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position, attackRange);
        }
    }
}

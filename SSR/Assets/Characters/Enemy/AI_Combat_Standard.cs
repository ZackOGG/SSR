using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Character;
using System;

namespace SS.AI
{
    public class AI_Combat_Standard : MonoBehaviour
    {
        [Header("Combat Stats")]
        [SerializeField] float attackRange = 2f;
        [SerializeField] float atemptAttackTime = 2f; // This is the ai at when it should attack

        private float attackSpeed;        
        private float attackTimer;
        private float aATr; 
        private Animator_Controller animCon;        
        public GameObject target;
        public void SetTarget(GameObject theTarget) { target = theTarget; }
        private float agroRange;
        private AI_Brain aIBrain;
        private bool targetIsLeft = false;
        private PolygonCollider2D attackHitBox;
        private float movementSpeed = 5f;
        private Character_Stats characterStats;
        private Rigidbody2D rb;
        private Combat_Controller combatCon;
        private AI_Movement aIMovement;
        // Use this for initialization
        void Start()
        {
            SetRefences();
            SetStats();
        }

        private void SetRefences()
        {
            animCon = this.GetComponent<Animator_Controller>();
            aIBrain = this.GetComponent<AI_Brain>();
            attackHitBox = this.GetComponent<PolygonCollider2D>();
            characterStats = this.GetComponent<Character_Stats>();
            rb = this.GetComponent<Rigidbody2D>();
            combatCon = this.GetComponentInChildren<Combat_Controller>();
            aIMovement = this.GetComponent<AI_Movement>();
        }
        private void SetStats()
        {
            attackSpeed = characterStats.GetAttackSpeed();
            agroRange = aIBrain.GetAgroRange();            
            attackTimer = attackSpeed;
            movementSpeed = characterStats.GetMovementSpeed();
            aATr = atemptAttackTime;
        }

        // Update is called once per frame
        void Update()
        {
            aATr -= Time.deltaTime;

            if(aATr <= 0 && InAttackRange()) // Starting the attack (atemptAttackTime) after the attack has ended
            {
                combatCon.Standard_Attack();
                aATr = atemptAttackTime + animCon.GetAttackClipLength() / attackSpeed;                
            }

            FollowTarget();           
            DetectTargetDirection();            
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
            print("Test99");
            if(targetIsLeft)
            {
                print("Move left");
                aIMovement.SetMoveLeft(true);
            }
            else
            {
                print("Move right");
                aIMovement.SetMoveLeft(false);
            }
        }
        IEnumerator ActivateHitbox()
        {
            yield return new WaitForSeconds(1f);
            attackHitBox.enabled = true;
            StartCoroutine("DeactivateHitBox");
        }
        IEnumerator DeactivateHitBox()
        {
            yield return new WaitForSeconds(0.5f);
            attackHitBox.enabled = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position, attackRange);
        }
        private void OnEnable()
        {
            print("imon");
        }
    }
}

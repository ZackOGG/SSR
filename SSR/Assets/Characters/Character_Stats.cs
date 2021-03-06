﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Equipment;

namespace SS.Character
{
    public class Character_Stats : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] int maxHealth = 1;
        [SerializeField] int health;
        [Tooltip("Attacks pre second")]
        [SerializeField] float attackSpeed;        
        [SerializeField] float defaultMovementSpeed;
        [SerializeField] int strength = 1;
        [SerializeField] int naturalPhysicalArmour;
        [SerializeField] int naturalMagicalArmour;
        [SerializeField] int weaponDamage = 0;
        [SerializeField] float jumpPower = 10f;
        [SerializeField] bool cantDie = false;
        [SerializeField] bool invulnerable = false;
        [SerializeField] bool isPlayer = false;

        [Header("Non Stats")]
        [Tooltip("The attack point is where the attack connects (in relation to the user of the weapon.) EG Peck area")]
        [SerializeField] Vector2 attackPointOffSet;

        //[SerializeField] NPCFeelings feelingsToPlayer = NPCFeelings.Friendly;
        private float movementSpeed = 5f;
        private bool amIDead = false;

        public delegate void Walking(bool amWalking);
        public Walking amWalking;

        //public delegate void KnockBack(float force, float duration, Vector3 direction);
        //public KnockBack KnockingBack;

        public delegate void CallDeath(bool isDead);
        public CallDeath callingDeath;

        public bool walking;
        public bool GetWalking() { return walking; }
        public void SetWalking(bool isWalking) { walking = isWalking; }
        //public StatusAffects currentStatusAffect = StatusAffects.none;
        private float stunLengthTempTimer = 0f; //This is for ienums for durations ect
        private float slowLengthTempTimer = 0f; //This is for ienums for durations ect
        private float boostLengthTempTimer = 0f; //This is for ienums for durations ect
        //====Getters and Setters=====
        public int GetMaxHealth() { return maxHealth; }
        public int GetTrueMaxHealth() { return maxHealth * 4; }// TODO Make this just one
        public int GetCurrentHealth() { return health; }
        public float GetAttackSpeed() { return attackSpeed; }    
        public void SetInvunerable(bool newBool) { invulnerable = newBool; }
        //public StatusAffects GetCurrentStatusAffect() { return currentStatusAffect; }
        public float GetMovementSpeed() { return movementSpeed; }
        public int GetStrength() { return strength;}
        public float GetJumpPower() { return jumpPower; }
        public bool GetKnockedBack() { return knockedBack; }
        //public NPCFeelings GetFeelings() { return feelingsToPlayer; }
        private Player_Movement_TwoDSS playerMovement;
        private Animator_Controller animCon;
        private SpriteRenderer spriteRen;
        private Vector2 lastWalkPos;
        private bool knockedBack = false;
        private float knockBackTimer;
        private Rigidbody2D rb;
        private int totalPhysicalArmour;
        private int totalMagicalArmour;
        private Equipment_Manager equipmentManager;

        public Vector2 GetFacingDirection()
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;            
            return direction.normalized;
        }
        public Vector2 CharacterPosition2D()
        {
            Vector2 twoDPos = new Vector2(this.transform.position.x, this.transform.position.y);
            return twoDPos;           
        }
        public Vector2 GetDirectionToObject(GameObject obj)
        {
            Vector2 objPosTwoD = new Vector2(obj.transform.position.x, obj.transform.position.y);
            Vector2 thisPosTwoD = new Vector2(this.transform.position.x, this.transform.position.y);

            Vector2 direction = thisPosTwoD - objPosTwoD;
            return direction;
        }


        public bool IsPlayer()
        {
            return isPlayer;
        }

        //============================
        void Start()
        {
            SetRefrences();
            SetStartingStats();
            //DetectWalking();
            StartCoroutine("LateStart");

        }

        IEnumerator LateStart()
        {
            yield return new WaitForSeconds(0.2f);
            DetectWalking();
        }
        private void SetRefrences()
        {
            playerMovement = this.GetComponent<Player_Movement_TwoDSS>();
            animCon = this.GetComponent<Animator_Controller>();

            spriteRen = this.GetComponent<SpriteRenderer>();
            rb = this.GetComponent<Rigidbody2D>();
            if (IsPlayer())
            {
                equipmentManager = this.GetComponent<Equipment_Manager>();
            }
        }
        private void SetStartingStats()
        {
            health = maxHealth * 4;
            movementSpeed = defaultMovementSpeed;

        }
        // Update is called once per frame
        void Update()
        {
            if(health <= 0 && !cantDie && isPlayer)
            {                
                amIDead = true;               
            }
            if(health <= 0 && !cantDie)
            {
                //callingDeath(true);
                Die();
            }

            if (health <= 0 && !cantDie && !isPlayer)
            {
                Die();
            }

        }

        public void TakeDamage(int physicalDamage,int magicalDamage,KnockbackPower knockbackPower)
        {
            if (!invulnerable && !amIDead)
            {
                CalculateTotalResistances(); //TODO Make this get called when gear is changed with a deligate
                int totalPhysicalDamage = physicalDamage - totalPhysicalArmour ;
                int totalMagicalDamage = magicalDamage - totalMagicalArmour;
                if(totalPhysicalDamage > 0)
                {
                    health = health - totalPhysicalDamage;
                }
                if(totalMagicalDamage > 0)
                {
                    health = health - totalMagicalDamage;
                }
                
                GiveDamageFeedBack();
                KnockBack(knockbackPower);
            }
            else
            {
                Debug.Log(this.name + " is invunerable");
            }
            
        }

        public int CalculateDamage()
        {
            return strength + weaponDamage;
        }
        public void CalculateTotalResistances()
        {//TODO CHANGE THIS TO BE CALCULATEDS IN THE EQUIPMENT MANAGER
            if(IsPlayer() && equipmentManager.GetBodyArmour() != null)
            {
                totalPhysicalArmour = naturalPhysicalArmour + equipmentManager.GetBodyArmour().GetPhysicalResistance();
                totalMagicalArmour = naturalPhysicalArmour + equipmentManager.GetBodyArmour().GetMagicalResistance();
                
            }
            else
            {
                totalPhysicalArmour = naturalPhysicalArmour;
                totalMagicalArmour = naturalMagicalArmour;
            }
            
        }

        private void GiveDamageFeedBack()
        {
            spriteRen.color = Color.red;            
            StartCoroutine("ChangeColourBack");            
        }
        
        IEnumerator ChangeColourBack()
        {
            yield return new WaitForSeconds(0.2f);
            spriteRen.color = Color.white;
            
        }

        private void Die()
        {
            Destroy(this.gameObject);
        }
        //=====Take Status Affect=====

        private void KnockBack(KnockbackPower knockbackPower)
        {
            //TODO Create an offset for knock back in relation to the targets center
            //StopAllCoroutines();            
            knockedBack = true;
            knockBackTimer = knockbackPower.duration;
            rb.velocity = -(knockbackPower.direction * knockbackPower.force);
            //rb.velocity = -(new Vector2(Mathf.Ceil(knockbackPower.direction.x), Mathf.Ceil(knockbackPower.direction.y)) * knockbackPower.force);
            //print(-(new Vector2(Mathf.Ceil(knockbackPower.direction.x), Mathf.Ceil(knockbackPower.direction.y))));
            StartCoroutine("KnockBackStop");
        }
        IEnumerator KnockBackStop()
        {
            yield return new WaitForSeconds(knockBackTimer);
            rb.velocity = Vector3.zero;
            knockedBack = false;
        }

        public void KockBack(float force, float duration, Vector3 direction)
        {
            print("Knocking back");
            //KnockingBack(force, duration, direction);
        }
        //============================

        private void DetectWalking()
        {
            lastWalkPos = this.transform.position;
            StartCoroutine("CheckWalking");
        }
        IEnumerator CheckWalking()
        {
            yield return new WaitForSeconds(0.1f); // TODO WORK ON REDUCING FOR CLEANER ANIM STOP
            Vector2 currentPos = this.transform.position;
            if (currentPos == lastWalkPos)
            {
                walking = false;
                amWalking(false);
            }
            else
            {
                walking = true;
                amWalking(true);
            }
            DetectWalking();
        }
        //======Consumeables=====
        public bool Consume(Item theItem)
        {
            if (theItem.GetStatType() == StatType.Health && health < GetTrueMaxHealth())
            {
                health += theItem.GetModifier();
                return true;
            }
            print("health to full");
            return false;
        }

        public Vector2 GetAttackPointOrigin()
        {
            Vector2 thisPos = this.transform.position;
            return (thisPos + attackPointOffSet);
        }

        private void OnDrawGizmos()
        {
            Vector2 thisPos = this.transform.position;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(thisPos + attackPointOffSet, 0.1f);

        }
        
    }
}



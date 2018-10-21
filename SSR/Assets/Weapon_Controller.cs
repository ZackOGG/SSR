using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SS.Equipment;


namespace SS.Character
{
    public class Weapon_Controller : MonoBehaviour
    {
        [SerializeField] AnimationClip defaultAttack;
        [SerializeField] AnimatorOverrideController animOveride;
        
        public Weapons currentWeapon;
        private void SetCurrentWeapon(Weapons newWeapon)
        {
            this.GetComponent<SpriteRenderer>().sprite = null;
            currentWeapon = newWeapon;
            SetHitbox();
            OverrideAttackAnimation();
            SetAnimSpeeds();
            CalculateTotalDamage();
            if (equipmentManager.GetDefaultWeapon() == newWeapon)
            {
                print("Turn off");
                this.GetComponent<SpriteRenderer>().sprite = null;
            }


        }
        private Animator anim;
        private Animator_Controller animCon;
        private Character_Stats characterStats;
        private Equipment_Manager equipmentManager;
        private PolygonCollider2D hitBox;
        private int totalDamage;
        

        // Use this for initialization
        void Start()
        {
            SetRefrences();
            equipmentManager.equipWeapon += SetCurrentWeapon;
            animCon.callingAttack += Attack;
            characterStats.amWalking += Walk;
        }

        private void SetRefrences()
        {
            anim = this.GetComponent<Animator>();
            animCon = this.GetComponentInParent<Animator_Controller>();
            characterStats = this.GetComponentInParent<Character_Stats>();
            equipmentManager = this.GetComponentInParent<Equipment_Manager>();
            hitBox = this.GetComponent<PolygonCollider2D>();
        }
        private void SetAnimSpeeds()
        {
            float attackSpeedCal = currentWeapon.GetAttackSpeed() * characterStats.GetAttackSpeed(); 
            anim.SetFloat("AttackSpeed", attackSpeedCal);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void Attack()
        {   
            if(currentWeapon != null)
            {
                anim.SetTrigger("Attack");
            }
                 
        }
        private void Walk(bool newBool)
        {
            anim.SetBool("Walking", newBool);
        }
        private void OverrideAttackAnimation()
        {
            anim.runtimeAnimatorController = animOveride;
            animOveride["DEFAULT_ATTACK_WEP"] = currentWeapon.GetWepAttackClip();
            animOveride["DEFAULT_WALK_WEP"] = currentWeapon.GetWepWalkingClip();
            animOveride["DEFAULT_IDLE_WEP"] = currentWeapon.GetWepIdleClip();
        }

        private void SetHitbox()
        {
            hitBox.points = currentWeapon.GetHitBox().points;
        }
        private void CalculateTotalDamage()
        {
            
            totalDamage = characterStats.GetStrength() + currentWeapon.GetDamage();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Character_Stats>())
            {
                DealDamage(collision.GetComponent<Character_Stats>());
                //deltDamage = true;
            }
        }

        private void DealDamage(Character_Stats targetStats)
        {

            targetStats.TakeDamage(totalDamage);
        }
    }
}
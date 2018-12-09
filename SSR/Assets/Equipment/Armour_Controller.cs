using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Character;

namespace SS.Equipment
{
    public class Armour_Controller : MonoBehaviour {

        [SerializeField] AnimatorOverrideController animOveride;
        [SerializeField] Armour currentArmour;
        [SerializeField] Weapons currentWeapon;
        [SerializeField] Weapons defaultWeapon;

        private Character_Stats characterStats;
        private Equipment_Manager equipmentManager;
        private Animator anim;
        private Animator_Controller animCon;

        // Use this for initialization
        void Start()
        {
            SetRefrences();
        }

        private void SetRefrences()
        {
            characterStats = this.GetComponentInParent<Character_Stats>();
            anim = this.GetComponent<Animator>();
            equipmentManager = this.GetComponentInParent<Equipment_Manager>();
            equipmentManager.equipBodyArmour += SetCurrentArmour;
            equipmentManager.restartAnimationEvent += RestartAnimation;
            equipmentManager.equipWeapon += SetCurrentWeapon;
            //characterStats.amWalking += SetWalking;
            animCon = this.GetComponentInParent<Animator_Controller>();
            animCon.callingAttack += Attack;
            
        }

        //=====Getters and Setters=====
        public void SetCurrentArmour(Armour newArmour)
        {
            /*
             * this.GetComponent<SpriteRenderer>().sprite = null;
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
             */
            
            this.GetComponent<SpriteRenderer>().sprite = null;
            currentArmour = newArmour;
            anim.enabled = true;
            OverrideAnimations();
            SetAnimSpeeds();
            if (currentArmour == null)
            {
                print("Turn off");
                anim.enabled = false;
                this.GetComponent<SpriteRenderer>().sprite = null;
            }

        }
        public void SetCurrentWeapon(Weapons newWeapon)
        {
            currentWeapon = newWeapon;
            SetAnimSpeeds();
            OverrideAnimations();

        }

        //=============================

        // Update is called once per frame
        void Update()
        {
           
        }

        private void OverrideAnimations()
        {
            if(currentArmour == null)
            {
                return;
            }
            anim.runtimeAnimatorController = animOveride;
            if(currentWeapon == defaultWeapon)
            {
                animOveride["DEFAULT_ATTACK_WEP"] = currentArmour.GetAttackBase();
                animOveride["DEFAULT_WALK_WEP"] = currentArmour.GetWalkingBase();
                animOveride["DEFAULT_IDLE_WEP"] = currentArmour.GetIdleBase();
            }
            if(currentWeapon.GetAttackType() == AttackType.Axe)
            {
                animOveride["DEFAULT_ATTACK_WEP"] = currentArmour.GetAxeAttackOne();
                animOveride["DEFAULT_WALK_WEP"] = currentArmour.GetWalkingAxe();
                animOveride["DEFAULT_IDLE_WEP"] = currentArmour.GetIdleAxe();
            }
            if (currentWeapon.GetAttackType() == AttackType.Sword)
            {
                animOveride["DEFAULT_ATTACK_WEP"] = currentArmour.GetSwordAttackOne();
                animOveride["DEFAULT_WALK_WEP"] = currentArmour.GetWalkingSword();
                animOveride["DEFAULT_IDLE_WEP"] = currentArmour.GetIdleSword();
            }


        }
        private void SetAnimSpeeds()
        {
            float attackSpeedCal = currentWeapon.GetAttackSpeed() * characterStats.GetAttackSpeed();
            anim.SetFloat("AttackSpeed", attackSpeedCal);
            anim.SetFloat("WalkSpeed", characterStats.GetMovementSpeed()); ;
        }
        private void RestartAnimation()
        {
            //anim.Play(anim.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0f);
        }
        private void SetWalking(bool newBool)
        {
            anim.SetBool("Walking", newBool);
            
        }
        private void Attack()
        {
            if (currentArmour != null)
            {
                anim.SetTrigger("Attack");
            }
        }
    }
}
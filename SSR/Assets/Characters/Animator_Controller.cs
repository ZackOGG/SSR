using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SS.Equipment;

namespace SS.Character
{
    //This is the central animatior controllers it controlls other animators eg armor and weapon. It also handles the base layer of the player
    public class Animator_Controller : MonoBehaviour
    {
        [SerializeField] AnimatorOverrideController animOverCon;
        [Header("Animation Clips")]

        [Header("Base Animation Clips")]
        [SerializeField] AnimationClip baseIdleClip;
        [SerializeField] AnimationClip baseWalkingClip;
        [SerializeField] AnimationClip baseAttackingClip;
        public float GetAttackClipLength() { return baseAttackingClip.length; }

        [Header("Axe Animations")]
        [SerializeField] AnimationClip twoHandedAxeIdle;        
        [SerializeField] AnimationClip twoHandedAxeWalk;
        [SerializeField] AnimationClip twoHandedAxeAttack;

        [Header("Sword Animations")]
        [SerializeField] AnimationClip twoHandedSwordIdle;
        [SerializeField] AnimationClip twoHandedSwordAttack;
        [SerializeField] AnimationClip twoHandedSwordWalk;

        public delegate void CallAttacking();
        public CallAttacking callingAttack;
        //[SerializeField] AnimationClip jumpingClip;


        public Weapons currentWeapon;
        private Equipment_Manager equipmentManager;
        private Character_Stats charStats;
        private Animator playerAnim;
        private Animator weaponAnim;
        private Animator armourAnim;
        private Vector2 lastWalkPos;
        public bool walking = false;
        private bool alreadyWalking = false;
        private bool alreadyNotWalking = false;
        public float attackTimer = 0;
        
        IEnumerator syncTest()
        {
            print("Sync test starting in 7");
            yield return new WaitForSeconds(5);
            print("Sync test in 2");
            yield return new WaitForSeconds(2);
            print("Test Started");
            SetAnimsToAttack();
        }

        private void Start()
        {
            SetRefrences();            
            DetectWalking();
            SetOverrides();
            //StartCoroutine("syncTest");



        }
        private void SetRefrences()
        {
            playerAnim = this.GetComponent<Animator>();
            weaponAnim = this.GetComponentInChildren<Weapon_Controller>().GetComponent<Animator>();
            armourAnim = this.GetComponentInChildren<Armour_Controller>().GetComponent<Animator>();
            charStats = this.GetComponent<Character_Stats>();
            equipmentManager = this.GetComponent<Equipment_Manager>();
            equipmentManager.equipWeapon += EquipWeaponForAnim;
            charStats.callingDeath += LiveOrDie;
            callingAttack += BodyAttackAnim;
            equipmentManager.restartAnimationEvent += AnimationSyncer;
        }

        public void SetAnimationSpeed(float moveSpeed, float attackSpeed)
        {
            float totalAttackSpeed = attackSpeed * currentWeapon.GetAttackSpeed(); //TODO MAKE THIS WORK INDEPENTANT OF LEGS
            //anim.SetFloat("IdleSpeed", 1f);
            
            playerAnim.SetFloat("AttackSpeed", totalAttackSpeed);
            playerAnim.SetFloat("MovementSpeed", moveSpeed);
        }
        public void SetOverrides()
        {

            playerAnim.runtimeAnimatorController = animOverCon;
            animOverCon["DEFAULT_IDLE_ANIMATION"] = baseIdleClip;
            animOverCon["DEFAULT_WALK_ANIMATION"] = baseWalkingClip;
            animOverCon["DEFAULT_ATTACK_ANIMATION"] = baseAttackingClip;  
        }
        public void OverrideAttackAnimation(AnimationClip animClip) // This is to change the attack animation for 
        {            
            animOverCon["DEFAULT_ATTACK_ANIMATION"] = animClip;            
        }
        public void OverrideWalkAnimation(AnimationClip animationClip)
        {
            animOverCon["DEFAULT_WALK_ANIMATION"] = animationClip;
        }
        public void OverrideIdleAnimation(AnimationClip animationClip)
        {
            animOverCon["DEFAULT_IDLE_ANIMATION"] = animationClip;
        }
        private void EquipWeaponForAnim(Weapons newWeapon)
        {
            currentWeapon = newWeapon;
            if(newWeapon.GetAttackType() == AttackType.Sword) // TODO NEEDS ATACHING ANIMATIONS TO WEAPONS
            {
                OverrideAttackAnimation(twoHandedSwordAttack);
                OverrideIdleAnimation(twoHandedSwordIdle);
                OverrideWalkAnimation(twoHandedSwordWalk);

            }
            else if(newWeapon.GetAttackType() == AttackType.Axe)
            {
                OverrideAttackAnimation(twoHandedAxeAttack);
                OverrideIdleAnimation(twoHandedAxeIdle);
                OverrideWalkAnimation(twoHandedAxeWalk);
            }
            else if(newWeapon.GetAttackType() == AttackType.Punch)
            {
                OverrideAttackAnimation(baseAttackingClip);
                OverrideIdleAnimation(baseIdleClip);
                OverrideWalkAnimation(baseWalkingClip);
            }
        SetAnimationSpeed(charStats.GetMovementSpeed(), charStats.GetAttackSpeed());
        }

        private void Update()
        {
            attackTimer -= Time.deltaTime;
            if (walking)
            {
                if (!alreadyWalking)
                {

                    SetAnimsToWalk();
                    alreadyNotWalking = false;
                    alreadyWalking = true;
                }
            }
            else
            {
                if(!alreadyNotWalking)
                {
                    alreadyNotWalking = true;
                    alreadyWalking = false;
                    SetAnimsToStopWalk();
                }
                
            }
            if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && attackTimer <= 0 && this.gameObject.tag == "Player")
            {
                attackTimer = 1 / charStats.GetAttackSpeed();
                //callingAttack();
                SetAnimsToAttack();
            }
            
        }

        public void StartWalkAnimation()
        {

            playerAnim.SetBool("Walking", true);            
        }
        public void StopWalkingAnimation()
        {
            playerAnim.SetBool("Walking", false);
        }
        public void AttackAnimation()
        {
            playerAnim.SetTrigger("Attacking");            
        }
        

        private void DetectWalking()
        {
            lastWalkPos = this.transform.position;
            StartCoroutine("CheckWalking");
        }
        IEnumerator CheckWalking()
        {
            yield return new WaitForSeconds(0.1f);
            Vector2 currentPos = this.transform.position;
            if(currentPos == lastWalkPos)
            {
                walking = false;
            }
            else
            {
                walking = true;
            }
            DetectWalking();
        }
        private void LiveOrDie(bool newbool)
        {
            //Run death animation
            if(newbool)
            {
                playerAnim.SetBool("Dead", true);
            }
            else
            {
                playerAnim.SetBool("Dead", false);
            }
            
        }
        private void BodyAttackAnim()
        {
            playerAnim.SetTrigger("Attacking");
        }
        private void SetAnimsToAttack()
        {
            print("Attacking");
            BodyAttackAnim();
            weaponAnim.SetTrigger("Attack");
            armourAnim.SetTrigger("Attack");
            StartCoroutine("EndAttackReset");
        }
        IEnumerator EndAttackReset()
        {
            float timerTime = playerAnim.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(timerTime);
        }
        private void SetAnimsToWalk()
        {
            StartWalkAnimation();
            weaponAnim.SetBool("Walking", true);
            armourAnim.SetBool("Walking", true);
            
        }

        private void SetAnimsToStopWalk()
        {
            StopWalkingAnimation();
            weaponAnim.SetBool("Walking", false);
            armourAnim.SetBool("Walking", false);
            //AnimationSyncer();
        }

        private void RestartAnimation()
        {
            print("Reseting Player");
            playerAnim.Play(playerAnim.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0f);
        }
        private void AnimationSyncer()
        {
            playerAnim.Play(playerAnim.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0f);
            weaponAnim.Play(weaponAnim.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0f);
            armourAnim.Play(armourAnim.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0f);
            
        }
    }
}
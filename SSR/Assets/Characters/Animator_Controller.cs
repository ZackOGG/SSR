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
        private Equipment_Manager equipMentManager;
        private Character_Stats charStats;
        private Animator anim;
        private Vector2 lastWalkPos;
        public bool walking = false;
        private float attackTimer = 0;

        private void Start()
        {
            SetRefrences();            
            DetectWalking();
            SetOverrides();
            
            
        }
        private void SetRefrences()
        {
            anim = this.GetComponent<Animator>();
            charStats = this.GetComponent<Character_Stats>();
            equipMentManager = this.GetComponent<Equipment_Manager>();
            equipMentManager.equipWeapon += EquipWeaponForAnim;
            charStats.callingDeath += LiveOrDie;
            callingAttack += BodyAttackAnim;
        }

        public void SetAnimationSpeed(float moveSpeed, float attackSpeed)
        {
            float totalAttackSpeed = attackSpeed * currentWeapon.GetAttackSpeed(); //TODO MAKE THIS WORK INDEPENTANT OF LEGS
            anim.SetFloat("AttackSpeed", totalAttackSpeed);
            anim.SetFloat("MovementSpeed", moveSpeed);
        }
        public void SetOverrides()
        {
            anim.runtimeAnimatorController = animOverCon;
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
                StartWalkAnimation();
            }
            else
            {
                StopWalkingAnimation();
            }
            if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && attackTimer <= 0)
            {
                attackTimer = 1 / charStats.GetAttackSpeed();
                callingAttack();
            }
        }

        public void StartWalkAnimation()
        {
            anim.SetBool("Walking", true);
        }
        public void StopWalkingAnimation()
        {
            anim.SetBool("Walking", false);
        }
        public void AttackAnimation()
        {
            anim.SetTrigger("Attacking");
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
                anim.SetBool("Dead", true);
            }
            else
            {
                anim.SetBool("Dead", false);
            }
            
        }
        private void BodyAttackAnim()
        {
            anim.SetTrigger("Attacking");
        }
    }
}
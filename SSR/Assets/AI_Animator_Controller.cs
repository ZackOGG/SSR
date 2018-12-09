using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Character;
using SS.Equipment;

namespace SS.AI
{
    public class AI_Animator_Controller : MonoBehaviour {

        [SerializeField] AnimatorOverrideController animOverRide;
        [SerializeField] AnimationClip idleClip;
        [SerializeField] AnimationClip walkClip;
        [SerializeField] AnimationClip attackClip;
        [SerializeField] Weapons attackWep;

        public delegate void CallAttacking();
        public CallAttacking callingAttack;

        private PolygonCollider2D hitBox;
        private Weapon_Controller weaponCon;
        private Animator aIAnim;
        private Character_Stats characterStats;
        // Use this for initialization

        public float GetAttackClipLength()
        {
            return attackClip.length;
        }

        void Start()
        {
            SetRefrences();            
            SetUpHitBox();
            StartCoroutine("LateStart");
        }

        IEnumerator LateStart()
        {
            yield return new WaitForSeconds(0.1f);
            SetUpAnimator();
        }

        private void SetRefrences()
        {
            characterStats = this.GetComponent<Character_Stats>();
            weaponCon = this.GetComponentInChildren<Weapon_Controller>();
            aIAnim = this.GetComponent<Animator>();
            
        }
        private void SetUpAnimator()
        {
            characterStats.amWalking += AmIWalking;
            callingAttack += Attack;
            aIAnim.runtimeAnimatorController = animOverRide;
            animOverRide["DEFAULT_IDLE_ANIMATION"] = idleClip;
            animOverRide["DEFAULT_WALK_ANIMATION"] = walkClip;
            animOverRide["DEFAULT_ATTACK_ANIMATION"] = attackClip;
            SetAnimationSpeed(characterStats.GetMovementSpeed(), characterStats.GetAttackSpeed());
        }
        private void SetUpHitBox()
        {
            hitBox = this.GetComponent<PolygonCollider2D>();
            hitBox.points = attackWep.GetHitBox().points;
        }

        public void SetAnimationSpeed(float moveSpeed, float attackSpeed)
        {
            float totalAttackSpeed = weaponCon.GetWeaponSpeed() * attackSpeed;
            
            aIAnim.SetFloat("AttackSpeed", totalAttackSpeed);
            aIAnim.SetFloat("MovementSpeed", moveSpeed);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.T))
            {
                callingAttack();
            }
        }
        private void AmIWalking(bool newBool)
        {
            aIAnim.SetBool("Walking", newBool);
        }
        private void Attack()
        {
            aIAnim.SetTrigger("Attacking");
        }

        private void RunDeathAnimation()
        {
            aIAnim.SetBool("Dead", true);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.Character
{
    public class Animator_Controller : MonoBehaviour
    {
        [SerializeField] AnimatorOverrideController animOverCon;
        [Header("Animation Clips")]
        [SerializeField] AnimationClip idleClip;
        [SerializeField] AnimationClip walkingClip;
        [SerializeField] AnimationClip attackingClip;
        public float GetAttackClipLength() { return attackingClip.length; }


        //[SerializeField] AnimationClip jumpingClip;



        private Character_Stats charStats;
        private Animator anim;
        private Vector2 lastWalkPos;
        public bool walking = false;

        private void Start()
        {
            SetRefrences();            
            DetectWalking();
            SetOverrides();
            SetAnimationSpeed(charStats.GetMovementSpeed(), charStats.GetAttackSpeed());
        }
        private void SetRefrences()
        {
            anim = this.GetComponent<Animator>();
            charStats = this.GetComponent<Character_Stats>();
            charStats.callingDeath += LiveOrDie;
        }

        public void SetAnimationSpeed(float moveSpeed, float attackSpeed)
        {           
            anim.SetFloat("AttackSpeed", attackSpeed);
            anim.SetFloat("MovementSpeed", moveSpeed);
        }
        public void SetOverrides()
        {
            anim.runtimeAnimatorController = animOverCon;
            animOverCon["DEFAULT_IDLE_ANIMATION"] = idleClip;
            animOverCon["DEFAULT_WALK_ANIMATION"] = walkingClip;
            animOverCon["DEFAULT_ATTACK_ANIMATION"] = attackingClip;
            

        }
        private void Update()
        {
            if (walking)
            {
                StartWalkAnimation();
            }
            else
            {
                StopWalkingAnimation();
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
            print("Live or die");
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
    }
}
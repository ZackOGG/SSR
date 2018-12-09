using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SS.Equipment;
using SS.AI;

namespace SS.Character
{
    public class Weapon_Controller : MonoBehaviour
    {
        //[SerializeField] AnimationClip defaultAttack;
        [SerializeField] Weapons defaultWeapon;
        [SerializeField] AnimatorOverrideController animOveride;
        public AnimatorOverrideController cloneAnimOveride;

        public Weapons currentWeapon;
        private void SetCurrentWeapon(Weapons newWeapon)
        {

            this.GetComponent<SpriteRenderer>().sprite = null;
            currentWeapon = newWeapon;
            if (!characterStats.IsPlayer())
            {
                currentWeapon = defaultWeapon;
            }
            SetHitbox();
            OverrideAnimations();
            SetAnimSpeeds();
            CalculateTotalDamage();
            if (characterStats.IsPlayer())
            {
                if (equipmentManager.GetDefaultWeapon() == newWeapon)
                {
                    print("Turn off");
                    this.GetComponent<SpriteRenderer>().sprite = null;
                }
            }
        }
        private Animator anim;
        private Animator_Controller animCon;
        private AI_Animator_Controller aIAnimCon;
        private Character_Stats characterStats;
        private Equipment_Manager equipmentManager;
        private PolygonCollider2D hitBox;
        private int totalDamage;
        public bool deltDamage = false;

        public float GetWeaponSpeed() { return currentWeapon.GetAttackSpeed(); }

        // Use this for initialization
        void Start()
        {
            SetRefrences();
            if (characterStats.IsPlayer())
            {
                equipmentManager.equipWeapon += SetCurrentWeapon;
                animCon.callingAttack += Attack;
            }
            else
            {
                SetCurrentWeapon(null);
                aIAnimCon.callingAttack += Attack;
            }
            //characterStats.amWalking += Walk;
            if (characterStats.IsPlayer())
            {
                equipmentManager.restartAnimationEvent += RestartAnimation;
            }

        }

        private void SetRefrences()
        {
            anim = this.GetComponent<Animator>();

            animCon = this.GetComponentInParent<Animator_Controller>();
            aIAnimCon = this.GetComponentInParent<AI_Animator_Controller>();

            characterStats = this.GetComponentInParent<Character_Stats>();
            equipmentManager = this.GetComponentInParent<Equipment_Manager>();
            hitBox = this.GetComponent<PolygonCollider2D>();
            //cloneAnimOveride = Instantiate(animOveride);
        }
        private void SetAnimSpeeds()
        {
            float attackSpeedCal = currentWeapon.GetAttackSpeed() * characterStats.GetAttackSpeed();
            anim.SetFloat("AttackSpeed", attackSpeedCal);
            anim.SetFloat("WalkSpeed", characterStats.GetMovementSpeed());
        }

        // Update is called once per frame
        void Update()
        {
            //OverrideAnimations();
            //SetAnimSpeeds();
        }

        private void Attack()
        {
            if (currentWeapon != null || !characterStats.IsPlayer())
            {
                anim.SetTrigger("Attack");
            }


        }
        private void Walk(bool newBool)
        {
            anim.SetBool("Walking", newBool);
        }
        private void OverrideAnimations()
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
            if (collision.GetComponent<Character_Stats>() && deltDamage == false)
            {
                DealDamage(collision.GetComponent<Character_Stats>());
                deltDamage = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            deltDamage = false;
        }

        private void DealDamage(Character_Stats targetStats)
        {
            //targetStats.KnockingBack(20f, 0.25f, -Vector3.up);
            targetStats.TakeDamage(totalDamage, CalculateKnockbackPower(targetStats.transform));
        }

        private KnockbackPower CalculateKnockbackPower(Transform targetTrans)
        {
            KnockbackPower knockBackPower;
            knockBackPower.direction = CalculateDirectionToTarget(targetTrans);
            knockBackPower.duration = 0.3f;
            knockBackPower.force = -currentWeapon.GetKnockbackPower();
            return knockBackPower;
        }
        
        private Vector2 CalculateDirectionToTarget(Transform targetTrans)
        {
            Vector2 thisPos = this.transform.position;
            Vector2 targetPos = targetTrans.position;
            Vector2 directionToTarget = (targetPos - thisPos).normalized;
            return directionToTarget;
        }
        
        private void RestartAnimation()
        {
            //anim.Play(anim.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0f);
        }
    }
}
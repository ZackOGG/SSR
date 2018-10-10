using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SS.Character
{
    public class Combat_Controller : MonoBehaviour {

        [SerializeField] float animationHitBoxTimer;

        private Animator_Controller animCon;
        private Character_Stats characterStats;
        private PolygonCollider2D polyCon;
        private float attackSpeed;
        private int damage;
        private float attackSpeedTimer;
        public bool deltDamage = false;
        

        // Use this for initialization
        void Start()
        {            
            SetRefrences();
            SetStats();
        }

        private void SetRefrences()
        {
            animCon = this.GetComponentInParent<Animator_Controller>();
            polyCon = this.GetComponent<PolygonCollider2D>();
            characterStats = this.GetComponentInParent<Character_Stats>();
            
        }

        private void SetStats()
        {
            attackSpeed = characterStats.GetAttackSpeed();
            damage = characterStats.CalculateDamage();
            
            attackSpeedTimer = 0f;
            
            
            
        }

        private void Update()
        {
            
            Standard_Attack();
        }

        // Update is called once per frame
        public void Standard_Attack()
        {
            attackSpeedTimer -= Time.deltaTime;
            //Attack animation
            if (attackSpeedTimer <= 0 && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                animCon.AttackAnimation();
                StartCoroutine("HitBoxDelay");
                attackSpeedTimer = animCon.GetAttackClipLength() / attackSpeed;
            }            
        }
        IEnumerator HitBoxDelay()
        {
            yield return new WaitForSeconds(animationHitBoxTimer / attackSpeed);
            polyCon.enabled = true;
            yield return new WaitForSeconds(0.1f);
            polyCon.enabled = false;
        }

        private void OnTriggerEnter2D (Collider2D collision)
        {
            if (collision.GetComponent<Character_Stats>())
            {
                DealDamage(collision.GetComponent<Character_Stats>());
                //deltDamage = true;
            }
        }        

        private void DealDamage(Character_Stats targetStats)
        {

            targetStats.TakeDamage(damage);
        }

        private void OnEnable()
        {
            //deltDamage = false;
        }
    }
}
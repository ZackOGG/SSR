using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SS.Character;
/*
 What it needs to do
 1.On hit box detection of an enemy to deal damage.
 2.Has an attack function which makes the animator do the attack
 */


namespace SS.AI
{
    public class AI_Combat_Controller : MonoBehaviour
    {        
        public Animator animCon;

        private AI_Animator_Controller aIAnimCon;
        private Character_Stats characterStats;
        private PolygonCollider2D polyCon;
        private float attackSpeed;
        private int damage;
        private float attackSpeedTimer;
        private bool deltDamage = false;


        // Use this for initialization
        void Start()
        {
            SetRefrences();
            SetStats();
        }

        private void SetRefrences()
        {
            animCon = this.GetComponent<Animator>();
            polyCon = this.GetComponent<PolygonCollider2D>();
            characterStats = this.GetComponentInParent<Character_Stats>();
            aIAnimCon = this.GetComponentInParent<AI_Animator_Controller>();
            aIAnimCon.callingAttack += Standard_Attack;
        }

        private void SetStats()
        {
            attackSpeed = characterStats.GetAttackSpeed();
            damage = characterStats.CalculateDamage();

            attackSpeedTimer = 0f;
        }

        private void Update()
        {

        }
        // Update is called once per frame
        public void Standard_Attack()
        {

            animCon.SetTrigger("Attack");
        }

        /*private void OnTriggerEnter2D(Collider2D collision)
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
        }*/

        private void OnEnable()
        {
            //deltDamage = false;
        }        
    }
}
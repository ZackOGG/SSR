using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.Character
{
    public class BodyDamage : MonoBehaviour
    {

        public Character_Stats characterStats;
        private int damage;

        // Use this for initialization
        void Start()
        {
            SetUpRefrences();
            SetUpStats();
        }
        private void SetUpRefrences()
        {
            characterStats = this.GetComponent<Character_Stats>();

        }
        private void SetUpStats()
        {
            damage = characterStats.GetStrength();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            print("Test1: " + collision.gameObject.name);
            DealDamage(collision.gameObject.GetComponent<Character_Stats>());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            
        }

        private void DealDamage(Character_Stats targetsStats)
        {
            print("Test2");
            if (targetsStats)
            {
                print("Test3");
                targetsStats.TakeDamage(characterStats.GetStrength(), 0,CalculateKnockbackPower(targetsStats.GetComponent<Transform>()));
            }
            
        }       

        private KnockbackPower CalculateKnockbackPower(Transform targetTrans)
        {
            print("Test4");
            KnockbackPower knockBackPower;
            knockBackPower.direction = CalculateDirectionToTarget(targetTrans);
            knockBackPower.duration = 0.1f;
            knockBackPower.force = -10;
            return knockBackPower;
        }

        private Vector2 CalculateDirectionToTarget(Transform targetTrans)
        {
            print("Test5");
            Vector2 thisPos = characterStats.GetAttackPointOrigin();
            Vector2 targetPos = targetTrans.position;
            Vector2 directionToTarget = (targetPos - thisPos);
            return directionToTarget.normalized;
        }
    }
}
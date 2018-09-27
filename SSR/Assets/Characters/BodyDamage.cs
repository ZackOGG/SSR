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

        private void OnTriggerEnter2D (Collider2D collision)
        {
            DealDamage(collision.gameObject.GetComponent<Character_Stats>());
        }

        private void DealDamage(Character_Stats targetsStats)
        {
            if(targetsStats)
            {
                targetsStats.KnockingBack(20f, 0.25f, characterStats.GetDirectionToObject(targetsStats.gameObject)); //TODO CHANGE HARD CODE OF KNOCK BACK
                targetsStats.TakeDamage(damage);
            }
            
        }
    }
}
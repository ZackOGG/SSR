using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.Equipment
{
    [CreateAssetMenu(menuName = "SSR/Equipment/Weapons")]
    public class Weapons : ScriptableObject
    {
        [SerializeField] PolygonCollider2D hitBox;
        public PolygonCollider2D GetHitBox() { return hitBox; }
        [SerializeField] Item thisItem;

        [Header("Stats")]
        [SerializeField] int physicalDamage = 1;
        [SerializeField] int magicalDamage = 0;
        [SerializeField] float attackSpeed = 1;
        [SerializeField] float knockBackPower = 10;
        //[SerializeField] int throwDamageMultiplier = 2;

        [Header("Animations")]
        [SerializeField] AnimationClip idle;
        [SerializeField] AnimationClip walking;
        [SerializeField] AnimationClip attackOne;    
        [SerializeField] AttackType attackType;
        

             
        //=====Getters and Setters=====
        public int GetPhysicalDamage()
        {
            return physicalDamage;
        }
        public int GetMagicalDamage()
        {
            return magicalDamage;
        }
        public AnimationClip GetWepIdleClip()
        {
            return idle;
        }
        public AnimationClip GetWepWalkingClip()
        {
            return walking;
        }
        public AnimationClip GetWepAttackClip()
        {
            return attackOne;
        }
        
        public AttackType GetAttackType()
        {
            return attackType;
        }
        public float GetAttackSpeed()
        {
            return attackSpeed;
        }
        /*public int GetThrownDamage()
        {
            return damage * throwDamageMultiplier;
        }*/

        /*public Abilitys GetAbilityOne()
        {
            return abilityOne;
        }*/
        public Item GetItem()
        {
            return thisItem;
        }

        public float GetKnockbackPower()
        {
            return knockBackPower;
        }
        //=============================

        public void Attack()
        {
            Debug.Log("Attacking");
        }
     
    }

}

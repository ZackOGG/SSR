using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.Equipment
{
    [CreateAssetMenu(menuName = "RPG/Equipment/Weapons")]
    public class Weapons : ScriptableObject
    {        
        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackClip;
        [SerializeField] int damage = 1;
        [SerializeField] int throwDamageMultiplier = 2; 
        
        
     
        //=====Getters and Setters=====
        
        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;          
        }

        public AnimationClip GetAnimationClip()
        {
            return attackClip;
        }

        public int GetDamage()
        {
            return damage;
        }
        public int GetThrownDamage()
        {
            return damage * throwDamageMultiplier;
        }

        /*public Abilitys GetAbilityOne()
        {
            return abilityOne;
        }*/
        //=============================

        public void Attack()
        {
            Debug.Log("Attacking");
        }
     
    }

}

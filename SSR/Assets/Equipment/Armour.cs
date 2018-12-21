using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.Equipment
{
    [CreateAssetMenu(menuName = "SSR/Equipment/Armour")]
    public class Armour : ScriptableObject
    {
        [SerializeField] Item thisItem;
        [SerializeField] Weapons currenWeapon;

        [Header("Stats")]
        [SerializeField] int physicalResistance;
        [SerializeField] int magicalResistance;

        [Header("Animations")]
        [Header("Base")]
        [SerializeField] AnimationClip idleBase;
        [SerializeField] AnimationClip walkingBase;
        [SerializeField] AnimationClip attackBase;

        [Header("2HSword")]
        [SerializeField] AnimationClip idleTwoHandedSword;
        [SerializeField] AnimationClip walkingTwoHandedSword;
        [SerializeField] AnimationClip swordAttackOne;

        [Header("2HAxe")]
        [SerializeField] AnimationClip idleTwoHandedAxe;
        [SerializeField] AnimationClip walkingTwoHandedAxe;           
        [SerializeField] AnimationClip axeAttackOne;

        //=====Getters and Setters=====
        public Item GetItem() { return thisItem; }
        public int GetPhysicalResistance() { return physicalResistance; }
        public int GetMagicalResistance() { return magicalResistance; }
        //=====Animation Base=====
        public AnimationClip GetWalkingBase() { return walkingBase; }
        public AnimationClip GetIdleBase() { return idleBase; }
        public AnimationClip GetAttackBase() { return attackBase; }
        //=====Animation Sword=====
        public AnimationClip GetWalkingSword() { return walkingTwoHandedSword; }
        public AnimationClip GetIdleSword() { return idleTwoHandedSword; }
        public AnimationClip GetSwordAttackOne() { return swordAttackOne; }
        //=====Animation Axe=====
        public AnimationClip GetWalkingAxe() { return walkingTwoHandedAxe; }
        public AnimationClip GetIdleAxe() { return idleTwoHandedAxe; }
        public AnimationClip GetAxeAttackOne() { return axeAttackOne; }
        //========================
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
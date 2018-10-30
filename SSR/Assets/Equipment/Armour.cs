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
        [SerializeField] int defense;

        [Header("Animations")]
        [SerializeField] AnimationClip idleBase;
        [SerializeField] AnimationClip idleTwoHandedSword;
        [SerializeField] AnimationClip idleTwoHandedAxe;
        [SerializeField] AnimationClip walkingBase;
        [SerializeField] AnimationClip walkingTwoHandedSword;
        [SerializeField] AnimationClip walkingTwoHandedAxe;
        [SerializeField] AnimationClip swordAttackOne;    
        [SerializeField] AnimationClip axeAttackOne;

        //=====Getters and Setters=====
        //public Item GetItem() { return thisItem; }

        //=============================

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SS.Character;

namespace SS.Equipment
{
    [CreateAssetMenu(menuName = ("SSR/Equipment/Item"))]
    public class Item : ScriptableObject
    {
        [SerializeField] string itemName;
        [SerializeField] Sprite icon;
        [SerializeField] ItemType itemType;
        [SerializeField] Weapons weapon;
        //[SerializeField] EquipmentSlotType equipmentSlotType;
        //[SerializeField] Armour bodyArmour;
        //[SerializeField] Armour armor;
        [SerializeField] Money value;
        [Header("Stats")]
        [SerializeField] StatType statType;
        [SerializeField] int modifier;

        //=====Getters and Setters=====
        public string GetItemName() { return itemName; }
        public Sprite GetItemIcon() { return icon; }
        public ItemType GetItemType() { return itemType; }
        public Weapons GetItemWeapon() { return weapon; }
        public StatType GetStatType() { return statType; }
        public int GetModifier() { return modifier; }
        public Money GetValue() { return value; }
        //=============================
        public void Consunme(Character_Stats characterStats)
        {
            if(itemType == ItemType.Consumable)
            {
                characterStats.Consume(this);
            }           
        }
    }
}
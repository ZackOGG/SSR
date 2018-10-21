using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SS.Equipment
{
    [CreateAssetMenu(menuName = ("SSR/Equipment/Item"))]
    public class Item : ScriptableObject
    {
        [SerializeField] string itemName;
        [SerializeField] Sprite icon;
        [SerializeField] ItemType itemType;
        [SerializeField] Weapons weapon;
        [SerializeField] Armour armor;

        //=====Getters and Setters=====
        public string GetItemName() { return itemName; }
        public Sprite GetItemIcon() { return icon; }
        public Weapons GetItemWeapon() { return weapon; }

        //=============================

    }
}
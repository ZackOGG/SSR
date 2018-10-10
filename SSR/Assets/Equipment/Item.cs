using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SS.Equipment
{
    [CreateAssetMenu(menuName = ("SSR/Equipment/Item"))]
    public class Item : ScriptableObject
    {
        public string itemName;
        public Sprite icon;

    }
}
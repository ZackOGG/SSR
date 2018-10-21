using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.Equipment
{
    [CreateAssetMenu(menuName = ("SSR/Equipment/Armour"))]
    public class Armour : ScriptableObject
    {
        [Header("Stats")]
        [SerializeField] int defense;

        [Header("Animations")]
        [SerializeField] AnimationClip idle;
        [SerializeField] AnimationClip walking;
        [SerializeField] AnimationClip attackOne;
    }
}
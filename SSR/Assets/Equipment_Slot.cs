using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment_Slot : MonoBehaviour {

    [SerializeField] EquipmentSlotType slotType;
    public EquipmentSlotType GetEquipmentSlotType()
    {
        return slotType;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

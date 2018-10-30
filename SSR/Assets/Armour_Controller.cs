using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armour_Controller : MonoBehaviour {

    [SerializeField] AnimatorOverrideController animOveride; 
    private Animator anim;

	// Use this for initialization
	void Start ()
    {
        SetRefrences();	
	}

    private void SetRefrences()
    {
        anim = this.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    private void EquipArmor()
    {
        print(" ");
    }
}

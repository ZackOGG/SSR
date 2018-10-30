using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SS.Equipment;

public class Money_UI_Manager : MonoBehaviour {

    [SerializeField] Text goldText;
    [SerializeField] Text silverText;
    [SerializeField] Text copperText;
    [SerializeField] Inventory theInventory;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateMoney();

    }
    private void UpdateMoney()
    {
        goldText.text = ("" + theInventory.GetMoney().Gold);
        silverText.text = ("" + theInventory.GetMoney().Silver);
        copperText.text = ("" + theInventory.GetMoney().Copper);
    }
}

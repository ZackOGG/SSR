using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SS.UI;
using SS.Equipment;

public class UI_Inventory_Manager : MonoBehaviour {

    [SerializeField] float holdTime;
    [SerializeField] GameObject canvas;

    private GraphicRaycaster gRaycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    private GameObject selectedIcon;
    private GameObject selectedIconOrginalHome;
    private bool isHoldIcon = false;
    private Inventory inventory;

	// Use this for initialization
	void Start ()
    {
        CheckingAssertions();
        SetRefrences();
	}
    private void CheckingAssertions()
    {
        if (canvas == null)
        {
            Debug.LogError("NO CANVAS ATTACHED TO :" + this.name);
        }
    }
    private void SetRefrences()
    {
        inventory = FindObjectOfType<Inventory>(); // TODO NOT 2PlAYER FREINDLY
        eventSystem = this.GetComponent<EventSystem>();
        gRaycaster = canvas.GetComponent<GraphicRaycaster>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        if (Input.GetMouseButtonDown(0) && eventSystem.IsPointerOverGameObject())
        {
            StartCoroutine("StartHoldIcon");
        }
        if(isHoldIcon)
        {
            HoldIcon();
        }
        if(isHoldIcon && Input.GetMouseButtonUp(0))
        {
            DropIcon();
        }
    }

    private void ProcessSelectUIObject()
    {
            selectedIconOrginalHome = OverAnotherInventorySlot().gameObject;
            selectedIcon = OverAnotherInventorySlot().GetComponentInChildren<InventorySlotIcon>().gameObject;

    }
    //=====Inventory=====
    
    private Inventory_Slot OverAnotherInventorySlot()
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();

        gRaycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult theResult in results)
        {
            if (theResult.gameObject.GetComponent<Inventory_Slot>())
            {
                return theResult.gameObject.GetComponent<Inventory_Slot>();
            }
        }
        return null;
    }
    IEnumerator StartHoldIcon()
    {
        ProcessSelectUIObject();
        GameObject tempIconObject = selectedIcon;
        yield return new WaitForSeconds(holdTime);
        if (Input.GetMouseButton(0) && selectedIcon == tempIconObject && OverAnotherInventorySlot() == selectedIconOrginalHome.GetComponent<Inventory_Slot>())
        {
            
            isHoldIcon = true;
        }
    }
    private void HoldIcon()
    {
        selectedIcon.transform.SetParent(canvas.transform);
        selectedIcon.transform.position = Input.mousePosition;
    }
    private void DropIcon()
    {
        if (OverAnotherInventorySlot() == null || OverAnotherInventorySlot() == selectedIconOrginalHome.GetComponent<Inventory_Slot>())
        {
            isHoldIcon = false;
            selectedIcon.transform.SetParent(selectedIconOrginalHome.transform);
            selectedIcon.transform.position = selectedIconOrginalHome.transform.position;
            //print(selectedIcon.transform.position);
        }
        else
        {
            print("Drop Icon in this slot");
            isHoldIcon = false;
            selectedIcon.transform.SetParent(selectedIconOrginalHome.transform);
            selectedIcon.transform.position = selectedIconOrginalHome.transform.position;
            inventory.SwitchBagSlots(selectedIcon.GetComponent<InventorySlotIcon>(), OverAnotherInventorySlot().GetComponentInChildren<InventorySlotIcon>());
        }

    }
    //==================
}

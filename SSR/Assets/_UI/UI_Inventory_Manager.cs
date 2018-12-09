using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SS.UI;
using SS.Equipment;
using SS.Character;

public class UI_Inventory_Manager : MonoBehaviour {

    [SerializeField] float holdTime;
    [SerializeField] GameObject canvas;

    private GraphicRaycaster gRaycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    private GameObject selectedIcon;
    private GameObject selectedIconOrginalHome;
    private bool isHoldIcon = false;
    private Inventory inventoryManager;
    private Character_Stats characterStats;
    private Equipment_Manager equipmentManager;
    public Shop_Manager shopManager;
    public void SetShopManager(Shop_Manager newShopManager) { shopManager = newShopManager; }
    public Inventory playerInventory;
    public void SetPlayerInventory(Inventory newInventory) { playerInventory = newInventory; }
    public bool ShopOpen = false;
    public void SetIsShopOpen(bool theBool) { ShopOpen = theBool; }
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
        inventoryManager = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        eventSystem = this.GetComponent<EventSystem>();
        gRaycaster = canvas.GetComponent<GraphicRaycaster>();
        equipmentManager = GameObject.FindGameObjectWithTag("Player").GetComponent<Equipment_Manager>();
        characterStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Character_Stats>();
    }
	
	// Update is called once per frame
	void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && eventSystem.IsPointerOverGameObject())
        {
            StartCoroutine("StartHoldIcon");
        }
        if(Input.GetMouseButtonUp(1))
        {
            
            ProcessRightClick();
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
        if(OverInventoryOrEquipment().inventorySlot != null)
        {
            selectedIcon = OverAnotherInventorySlot().GetComponentInChildren<InventorySlotIcon>().gameObject;
            selectedIconOrginalHome = OverAnotherInventorySlot().GetComponent<Inventory_Slot>().gameObject;
        }
        if(OverInventoryOrEquipment().equipmentSlot != null)
        {
            selectedIcon = OverAnotherEquipmentSlot().GetComponentInChildren<InventorySlotIcon>().gameObject;
            selectedIconOrginalHome = OverAnotherEquipmentSlot().GetComponent<Equipment_Slot>().gameObject;
        }
        
        
    }
    private void ProcessRightClick()
    {
        if(OverAnotherInventorySlot() && !OverAnotherShopSlot() && !ShopOpen)
        {
            inventoryManager.UseItemAbility(OverAnotherInventorySlot().GetComponent<Inventory_Slot>());
        }
        if(OverAnotherShopSlot())
        {
            shopManager.BuyItem(OverAnotherInventorySlot());
        }
        if(OverAnotherInventorySlot() && ShopOpen && !OverAnotherShopSlot())
        {
            shopManager.SellItem(OverAnotherInventorySlot());
        }
    }
    //=====Inventory=====
    
    private InventoyrOrEquipmentSlot OverInventoryOrEquipment()
    {
        InventoyrOrEquipmentSlot inventoryOrEquipmentSlot;
        if (OverAnotherEquipmentSlot() != null && OverAnotherInventorySlot() != null)
        {            
            Debug.LogError("SLOT IS BOTH EQUIPMENT AND INVENTORY");
            inventoryOrEquipmentSlot.inventorySlot = null;
            inventoryOrEquipmentSlot.equipmentSlot = null;
            return inventoryOrEquipmentSlot;
        }
        if (OverAnotherInventorySlot() != null)
        {
            inventoryOrEquipmentSlot.inventorySlot = OverAnotherInventorySlot();
            inventoryOrEquipmentSlot.equipmentSlot = null;
            return inventoryOrEquipmentSlot;
        }
        if(OverAnotherEquipmentSlot() != null)
        {            
            inventoryOrEquipmentSlot.inventorySlot = null;
            inventoryOrEquipmentSlot.equipmentSlot = OverAnotherEquipmentSlot();
            return inventoryOrEquipmentSlot;
        }
        inventoryOrEquipmentSlot.inventorySlot = null;
        inventoryOrEquipmentSlot.equipmentSlot = null;
        return inventoryOrEquipmentSlot;

    }

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
    private Equipment_Slot OverAnotherEquipmentSlot()
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();

        gRaycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult theResult in results)
        {
            if (theResult.gameObject.GetComponent<Equipment_Slot>())
            {
                return theResult.gameObject.GetComponent<Equipment_Slot>();
            }
        }
        return null;
    }
    private Shop_Slot OverAnotherShopSlot()
    {

        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();

        gRaycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult theResult in results)
        {
            if (theResult.gameObject.GetComponent<Shop_Slot>())
            {
                return theResult.gameObject.GetComponent<Shop_Slot>();
            }
        }
        return null;
    }

    IEnumerator StartHoldIcon()
    {
        ProcessSelectUIObject();
        GameObject tempIconObject = selectedIcon;
        yield return new WaitForSeconds(holdTime);
        if(OverInventoryOrEquipment().inventorySlot != null && OverInventoryOrEquipment().equipmentSlot == null && !OverAnotherShopSlot())
        {
            if (Input.GetMouseButton(0) && selectedIcon == tempIconObject && OverAnotherInventorySlot() == selectedIconOrginalHome.GetComponent<Inventory_Slot>())
            { 
                isHoldIcon = true;
            }
        }
        if(OverInventoryOrEquipment().equipmentSlot != null && OverInventoryOrEquipment().inventorySlot == null)
        {
            if (Input.GetMouseButton(0) && OverAnotherEquipmentSlot() == selectedIconOrginalHome.GetComponent<Equipment_Slot>())
            {
                isHoldIcon = true;
            }
        }
        if(OverInventoryOrEquipment().equipmentSlot !=null && OverInventoryOrEquipment().inventorySlot != null)
        { Debug.LogError("OVER BOTH INVENTORY AND EQUIPMENT SLOT AT THE SAME TIME!!!!"); }
        
    }
    private void HoldIcon()
    {
        selectedIcon.transform.SetParent(canvas.transform);
        selectedIcon.transform.position = Input.mousePosition;
    }
    private void DropIcon()
    {
        if (OverAnotherEquipmentSlot()) // Droping onto an equipment slot
        {
            isHoldIcon = false;
            
            selectedIcon.transform.SetParent(selectedIconOrginalHome.transform);
            selectedIcon.transform.position = selectedIconOrginalHome.transform.position;
            Inventory_Slot tempInventorySlot = selectedIcon.GetComponentInParent<Inventory_Slot>();
            print(tempInventorySlot);
            SwapEquipmentAndInventory(OverAnotherEquipmentSlot(), tempInventorySlot);
            return;
        }
        if (OverAnotherInventorySlot() == null || OverAnotherInventorySlot() == selectedIconOrginalHome.GetComponent<Inventory_Slot>())
        {
            isHoldIcon = false;
            selectedIcon.transform.SetParent(selectedIconOrginalHome.transform);
            selectedIcon.transform.position = selectedIconOrginalHome.transform.position;
            //print(selectedIcon.transform.position);
        }
        else
        {
            isHoldIcon = false;
            selectedIcon.transform.SetParent(selectedIconOrginalHome.transform);
            selectedIcon.transform.position = selectedIconOrginalHome.transform.position;
            if(selectedIconOrginalHome.GetComponent<Inventory_Slot>() != null)
            {
                print("Test1");
                inventoryManager.SwitchBagSlots(selectedIcon.GetComponent<InventorySlotIcon>(), OverAnotherInventorySlot().GetComponentInChildren<InventorySlotIcon>());
            }
            if(selectedIconOrginalHome.GetComponent<Equipment_Slot>() != null) // Droping from and equipment slot onto and inventory
            {
                Equipment_Slot tempEquipmentSlot = selectedIcon.GetComponentInParent<Equipment_Slot>();
                SwapEquipmentAndInventory(tempEquipmentSlot, OverAnotherInventorySlot());
                return;
            }
            
        }
        

    }

    //=====Equipment Management with Inventory=====
    public void SwapEquipmentAndInventory(Equipment_Slot equipmentSlot, Inventory_Slot inventorySlot)
    {
        Item tempEquipSlotItem = equipmentManager.WhichItemInSlot(equipmentSlot);
        Item tempInventoryItem = inventoryManager.FindItemInSlot(inventorySlot);
        
        if(equipmentManager.DoesEquipmentMatchSlot(tempInventoryItem, equipmentSlot) == true)
        {
            Debug.Log("Slot is correct");
            inventoryManager.RemoveItemFromInventory(null, inventoryManager.WhichInventorySlot(inventorySlot.GetComponentInChildren<InventorySlotIcon>()));
            inventoryManager.AddItemToInventory(tempEquipSlotItem, inventoryManager.WhichInventorySlot(inventorySlot.GetComponentInChildren<InventorySlotIcon>()));

            //equipmentManager.UnEquipSlot(equipmentSlot); Was stopping flow of execution, not sure why.
            equipmentManager.EquipEquipmentInSlot(tempInventoryItem, equipmentSlot); // TODO, Semi cheaty as it does not remove before adding new ones it just overides
        }
        else if(tempInventoryItem == null && tempEquipSlotItem != null)
        {            
            inventoryManager.AddItemToInventory(tempEquipSlotItem, inventoryManager.WhichInventorySlot(inventorySlot.GetComponentInChildren<InventorySlotIcon>()));
            equipmentManager.EquipEquipmentInSlot(tempInventoryItem, equipmentSlot);

        }
        else
        {
            Debug.Log("Slot not correct");
        }
        
       
    }
    //=====Character Profile=====


    //===========================
}

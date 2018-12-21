using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SS.Quests;
using SS.Character;
using SS.UI;    

namespace SS.Equipment
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] bool nPC;
        [SerializeField] Money money;
        public Money GetMoney() { return money; }
        [SerializeField] Item[] inventoryItems;
        public Item[] GetInventoryItems() { return inventoryItems; }
        [SerializeField] GameObject uIInventoryWindow;
        [SerializeField] Transform uIItventorySlotParent;
        [SerializeField] Item startingItem;
        [SerializeField] Item startingItemTwo;
        [SerializeField] Item startingItemThree;
        [SerializeField] Item startingItemFour;
        public Image[] uIInventorySlotsIcons;
        public Image[] uiShopSlotIcons;

        private Transform shopIconParent;
        private Quest_Jornal questJornal;
        private Character_Stats characterStats;
        private Equipment_Manager equipmentManager;
        private UI_Inventory_Manager uiInventoryManager;
        //=====Money Manager=====
        public void AddMoney(Money amount)
        {
            money.Copper += amount.Copper;
            money.Silver += amount.Silver;
            money.Gold += amount.Gold;

            if (money.Copper > 99)
            {
                money.Copper = money.Copper - 100;
                money.Silver = money.Silver + 1;
            }
            if (money.Silver > 99)
            {
                money.Silver = money.Silver - 100;
                money.Gold = money.Gold + 1;
            }
        }
        public void RemoveMoney(Money amount)
        {
            print("Test2");
            if (amount.Copper > money.Copper)
            {                
                if(money.Silver < 1)
                {
                    money.Gold -= 1;
                    money.Silver += 99;
                }
                else
                {
                    money.Silver = money.Silver - 1;

                }
                money.Copper = money.Copper + 100;
                
            }
            money.Copper -= amount.Copper;
            if (amount.Silver > money.Silver)
            {
                money.Gold -= 1;
                money.Silver = money.Silver + 100;
                money.Silver = money.Silver - amount.Silver;
            }
            else
            {
                money.Silver = money.Silver - amount.Silver;
            }
            money.Gold = money.Gold - amount.Gold;
        }
        public bool EnoughMoney(Money amount)
        {
            
            int amountNeededInCopper = amount.Copper + ((amount.Silver * 100) + (amount.Gold * 10000));
            int amountHaveInCopper = money.Copper + ((money.Silver * 100) + (money.Gold * 10000));
            if(amountHaveInCopper >= amountNeededInCopper)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //=======================

        private void Start()
        {
            SetRefrences();
            SetStartingBagSize();
            if(!nPC)
            {
                FindIcons();
                UpdateInventoryUI();
            }
            if(nPC)
            {
                GetShopUISlotsIcons();
            }
            AddStartingItems();
            
        }
        private void AddStartingItems()
        {
            AddItemToInventory(startingItem, -1);
            AddItemToInventory(startingItemTwo, -1);
            AddItemToInventory(startingItemThree, -1);
            AddItemToInventory(startingItemFour, -1);
        }
        private void SetRefrences()
        {
            questJornal = this.GetComponent<Quest_Jornal>();
            characterStats = this.GetComponent<Character_Stats>();
            if(nPC)
            {
                shopIconParent = GameObject.FindObjectsOfType<Shop_Icon_Parent>()[0].GetComponent<Transform>();
                shopIconParent.GetComponentInParent<Shop_UI_Manager>().gameObject.SetActive(false);
                if(Object.FindObjectsOfType<Shop_Icon_Parent>().Length > 1)
                {                    
                    Debug.LogError("TWO OR MORE SHOP_ICON_PARENTS DETECTED");
                }
            }
            equipmentManager = this.GetComponent<Equipment_Manager>();
            uiInventoryManager = GameObject.FindObjectOfType<UI_Inventory_Manager>();
        }
        private void SetStartingBagSize()
        {
            inventoryItems = new Item[16];
            if (!nPC)
            {                
                uIInventorySlotsIcons = new Image[inventoryItems.Length];
            }
            if(nPC)
            {
                uiShopSlotIcons = new Image[inventoryItems.Length];
            }
        }
        //=====Inventory UI=====
        private void FindIcons()
        {
            int slotNum = 0;
            foreach (Transform obj in uIItventorySlotParent )
            {
                GameObject tempIcon = obj.GetComponentInChildren<InventorySlotIcon>().gameObject;
                uIInventorySlotsIcons[slotNum] = tempIcon.GetComponent<Image>();
                slotNum++;
            }
        }
        private void GetShopUISlotsIcons()
        {
            int slotNum = 0;
            foreach (Transform obj in shopIconParent)
            {
                GameObject tempIcon = obj.GetComponentInChildren<InventorySlotIcon>().gameObject;
                uiShopSlotIcons[slotNum] = tempIcon.GetComponent<Image>();
                slotNum++;
            }
        }

        private void Update()
        {            
            if (Input.GetKeyDown(KeyCode.B) && !nPC)
            {
                ToggleInventoryWindow();
            }
            if(Input.GetKeyDown(KeyCode.Escape) && !nPC)
            {
                CloseInventoryWindow();
            }

        }

        public void ToggleInventoryWindow()
        {
            uIInventoryWindow.SetActive(!uIInventoryWindow.activeInHierarchy);
        }
        public void OpenInventoryWindow()
        {
            uIInventoryWindow.SetActive(true);
        }
        public void CloseInventoryWindow()
        {
            uIInventoryWindow.SetActive(false);            
        }

        public void UpdateInventoryUI()
        {
            int slotNum = 0;
            foreach (Item item in inventoryItems)
            {
                if (item != null)
                {
                    uIInventorySlotsIcons[slotNum].sprite = item.GetItemIcon();
                    uIInventorySlotsIcons[slotNum].enabled = true;
                    slotNum++;
                }
                else
                {
                    uIInventorySlotsIcons[slotNum].sprite = null;
                    uIInventorySlotsIcons[slotNum].enabled = false;
                    slotNum++;
                }
            }
        }
        
        //========================
        //=====Bag Management=====
        public void AddItemToInventory(Item newItem, int slotNum)
        {
            if (FindAmountOfFreeSlots() > 0 && slotNum < 0)
            {
                inventoryItems[FindFirstFreeSlot()] = newItem;
                if(!nPC)
                {
                    UpdateInventoryUI();
                    questJornal.UpdateItemQuests();
                }
              
                
                return;
            }
            if (slotNum >= 0)
            {
                if (FindSpesificFreeSlot(slotNum))
                {
                    inventoryItems[slotNum] = newItem;
                    if (!nPC)
                    {
                        UpdateInventoryUI();
                        questJornal.UpdateItemQuests();
                    }
                }
                else { Debug.LogError("Slot not free when trying to move spesifc into it"); }

            }
            if (!nPC)
            {
                questJornal.UpdateItemQuests();
            }
        }
        public void RemoveItemFromInventory(Item theItem, int slotNum)
        {
            if (slotNum >= 0)
            {
                inventoryItems[slotNum] = null;
                if(!nPC)
                {
                    UpdateInventoryUI();
                    questJornal.UpdateItemQuests();
                }                
            }
            if (theItem != null)
            {
                RemoveItemFromInventory(null,FindItemInBagSlot(theItem));
            }
            if(theItem != null && slotNum >= 0)
            {
                Debug.LogError("REMOVE ITEM HAS TWO USEABLE VARABLES!!! SHOULD ONLY BE ONE CHECK RemoveItemFromInventory CALL");
            }
        }
        private int FindFirstFreeSlot()
        {
            int freeSlot = 0;
            foreach (Item item in inventoryItems)
            {
                if (item == null)
                {
                    return freeSlot;
                }
                freeSlot++;
            }
            print("No free slots");
            return -1;
        }
        private int FindAmountOfFreeSlots()
        {
            int amountOfFreeSlots = 0;
            foreach (Item item in inventoryItems)
            {
                if (item == null)
                {
                    amountOfFreeSlots++;
                }
            }
            return amountOfFreeSlots;
        }
        private bool FindSpesificFreeSlot(int slotNum) // Is the spesific slot free
        {
            if (inventoryItems[slotNum] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int WhichInventorySlot(InventorySlotIcon theSlot)
        {
            if (!nPC)
            {
                int slotNum = 0;
                foreach (Image slot in uIInventorySlotsIcons)
                {
                    if (slot.GetComponent<InventorySlotIcon>() == theSlot)
                    {                        
                        return slotNum;
                    }
                    slotNum++;
                }

                return -1;
            }
            if(nPC)
            {
                int slotNum = 0;
                foreach (Image slot in uiShopSlotIcons)
                {
                    if (slot.GetComponent<InventorySlotIcon>() == theSlot)
                    {
                        return slotNum;
                    }
                    slotNum++;
                }

                return -1;
            }
            return -1;
        }
        public void SwitchBagSlots(InventorySlotIcon firstToSwitch, InventorySlotIcon secondToSwitch)
        {
            if (FindSpesificFreeSlot(WhichInventorySlot(firstToSwitch)) == false && FindSpesificFreeSlot(WhichInventorySlot(secondToSwitch)) == false)  // Find out if these two slots have anything in them if both dont then do
            {
                Item tempItemOne = inventoryItems[WhichInventorySlot(firstToSwitch)];
                Item tempItemTwo = inventoryItems[WhichInventorySlot(secondToSwitch)];
                RemoveItemFromInventory(null,WhichInventorySlot(firstToSwitch));
                print(WhichInventorySlot(firstToSwitch));
                RemoveItemFromInventory(null,WhichInventorySlot(secondToSwitch));
                AddItemToInventory(tempItemOne, WhichInventorySlot(secondToSwitch));
                AddItemToInventory(tempItemTwo, WhichInventorySlot(firstToSwitch));
            }
            if (FindSpesificFreeSlot(WhichInventorySlot(firstToSwitch)) == false && FindSpesificFreeSlot(WhichInventorySlot(secondToSwitch)) == true) // This is if you are moving an item into an empty spot
            {
                Item tempItemOne = inventoryItems[WhichInventorySlot(firstToSwitch)];
                RemoveItemFromInventory(null,WhichInventorySlot(firstToSwitch));
                AddItemToInventory(tempItemOne, WhichInventorySlot(secondToSwitch));
            }
            if (FindSpesificFreeSlot(WhichInventorySlot(firstToSwitch)) == true && FindSpesificFreeSlot(WhichInventorySlot(secondToSwitch)) == true)
            {
                print("Fist has nothing to switch");
            }
        }        

        public int CheckAmountOfItemInBag(Item theItem)
        {
            int amountOfItem = 0;
            foreach (Item item in inventoryItems)
            {
                if (item == theItem)
                {
                    amountOfItem++;
                }
            }
            return (amountOfItem);
        }
        public int FindItemInBagSlot(Item theItem)//Finds a spesific item and returns the slot that it is in
        {
            int slotNum = 0;
            foreach(Item item in inventoryItems)
            {
                if(item == theItem)
                {
                    return slotNum;
                }
                slotNum++;
            }
            Debug.LogError("ITEM TO BE REMOVED NOT IN BAG");
            return -1;
        }
        public Item FindItemInSlot(Inventory_Slot slot) // Finds what item is in a spesific slot
        {
            return inventoryItems[WhichInventorySlot(slot.GetComponentInChildren<InventorySlotIcon>())];
        }
        public void UseItemAbility(Inventory_Slot slot)
        {
            Item itemToUse = FindItemInSlot(slot);
            if(itemToUse.GetItemType() == ItemType.Consumable)
            {
                if(characterStats.Consume(itemToUse))
                {
                    RemoveItemFromInventory(null, WhichInventorySlot(slot.GetComponentInChildren<InventorySlotIcon>()));
                }
            }
            if(itemToUse.GetEquipmentSlotType() == EquipmentSlotType.RHWeapon) // DO WHEN ADDING NEW SLOTS
            {
                Equipment_Slot tempEquipmentSlot = equipmentManager.GetUIRightHand().GetComponent<Equipment_Slot>();
                Inventory_Slot tempInventorySlot = uIInventorySlotsIcons[FindItemInBagSlot(itemToUse)].GetComponentInParent<Inventory_Slot>();
                uiInventoryManager.SwapEquipmentAndInventory(tempEquipmentSlot, tempInventorySlot);
            }
            if(itemToUse.GetEquipmentSlotType() == EquipmentSlotType.BodyArmour)
            {
                Equipment_Slot tempEquipmentSlot = equipmentManager.GetUIBodyArmour().GetComponent<Equipment_Slot>();
                Inventory_Slot tempInventorySlot = uIInventorySlotsIcons[FindItemInBagSlot(itemToUse)].GetComponentInParent<Inventory_Slot>();
                uiInventoryManager.SwapEquipmentAndInventory(tempEquipmentSlot, tempInventorySlot);
            }
        }
    }
}
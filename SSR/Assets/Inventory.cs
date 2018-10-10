using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SS.Quests;

namespace SS.Equipment
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] Item[] inventoryItems;
        public Item[] GetInventoryItems() { return inventoryItems; }
        [SerializeField] GameObject uIInventoryWindow;
        [SerializeField] Transform uIItventorySlotParent;
        [SerializeField] Item testItem;
        [SerializeField] Item testItemTwo;
        public Image[] uIInventorySlotsIcons;

        private Quest_Jornal questJornal;

        private void Start()
        {
            SetRefrences();
            SetStartingBagSize();
            FindIcons();
            UpdateInventoryUI();
            //AddItemToInventory(testItem, -1);
            //AddItemToInventory(testItemTwo, -1);

        }
        private void SetRefrences()
        {
            questJornal = this.GetComponent<Quest_Jornal>();
        }
        private void SetStartingBagSize()
        {
            inventoryItems = new Item[16];
            uIInventorySlotsIcons = new Image[inventoryItems.Length];
        }
        //=====Inventory UI=====
        private void FindIcons()
        {
            int slotNum = 0;
            foreach (Transform obj in uIItventorySlotParent)
            {
                GameObject tempIcon = obj.GetComponentInChildren<InventorySlotIcon>().gameObject;
                uIInventorySlotsIcons[slotNum] = tempIcon.GetComponent<Image>();
                slotNum++;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                RemoveItemFromInventory(testItem, -1);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                ToggleInventoryWindow();
            }

        }

        public void ToggleInventoryWindow()
        {
            uIInventoryWindow.SetActive(!uIInventoryWindow.activeInHierarchy);
        }

        public void UpdateInventoryUI()
        {
            int slotNum = 0;
            foreach (Item item in inventoryItems)
            {
                if (item != null)
                {
                    uIInventorySlotsIcons[slotNum].sprite = item.icon;
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
                UpdateInventoryUI();
                questJornal.UpdateItemQuests();
                return;
            }
            if (slotNum >= 0)
            {
                if (FindSpesificFreeSlot(slotNum))
                {
                    inventoryItems[slotNum] = newItem;
                    UpdateInventoryUI();
                    questJornal.UpdateItemQuests();
                }
                else { Debug.LogError("Slot not free when trying to move spesifc into it"); }

            }
            questJornal.UpdateItemQuests();
        }
        public void RemoveItemFromInventory(Item theItem, int slotNum)
        {
            if (slotNum >= 0)
            {
                inventoryItems[slotNum] = null;
                UpdateInventoryUI();
                questJornal.UpdateItemQuests();
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
        private int WhichInventorySlot(InventorySlotIcon theSlot)
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
        public void SwitchBagSlots(InventorySlotIcon firstToSwitch, InventorySlotIcon secondToSwitch)
        {
            print(FindSpesificFreeSlot(WhichInventorySlot(firstToSwitch)));
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
        public int FindItemInBagSlot(Item theItem)
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
    }
}
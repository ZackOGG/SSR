using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SS.Equipment;

namespace SS.UI
{
    public class Shop_UI_Manager : MonoBehaviour
    {

        [SerializeField] Inventory nPCInventory;
        [SerializeField] Item[] inventoryItems;
        public void SetInventoryItems(Item[] items) { inventoryItems = items; }
        [SerializeField] Transform itemsIconParent;
        [SerializeField] Image[] uIShopSlotsIcon;
        public void SetNPCInventory(Inventory newInventory)
        {
            nPCInventory = newInventory;
            if (newInventory != null)
            {
                inventoryItems = newInventory.GetInventoryItems();
            }            
            UpdateUI();
        }
        // Use this for initialization
        void Start()
        {
            
        }
        public void SetShopSlotIcons()
        {
            uIShopSlotsIcon = new Image[16]; //TODO CHANGE FOR DIFFENENT INVENTORY SIZES
            int slotNum = 0;
            foreach(Transform slot in itemsIconParent)
            {
                uIShopSlotsIcon[slotNum] = slot.GetComponentInChildren<InventorySlotIcon>().GetComponent<Image>();
                slotNum++;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OpenShop(Inventory shopInventory)
        {            
            SetNPCInventory(shopInventory);
        }

        public void UpdateUI() // TODO MAKE THIS
        {
            SetShopSlotIcons();
            int slotNum = 0;
            foreach (Item item in inventoryItems)
            {
                if (item != null)
                {
                    uIShopSlotsIcon[slotNum].sprite = item.GetItemIcon();
                    uIShopSlotsIcon[slotNum].enabled = true;
                    slotNum++;
                }
                else
                {
                    uIShopSlotsIcon[slotNum].sprite = null;
                    uIShopSlotsIcon[slotNum].enabled = false;
                    slotNum++;
                }
            }
        }

    }
}
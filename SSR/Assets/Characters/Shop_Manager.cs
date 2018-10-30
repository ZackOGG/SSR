using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.UI;
using SS.Equipment;

namespace SS.Character
{
    public class Shop_Manager : MonoBehaviour
    {
        //TODO BUG WHERE IT THINKS THAT YOU DONT HAVE ENOUGH MONEY 
        public UI_Inventory_Manager uIInventoryManager;
        public Shop_UI_Manager shopUIManager;

        [SerializeField] Inventory tradersInventory; // Make this for multiple players
        private Inventory thisInventory;
        [Tooltip("How much the shop reduces the price when it buys from someone else.")]
        [SerializeField] int shopsBuyRate;
        [Tooltip("How much the shop sets their prices to of the standard value.")]
        [SerializeField] int shopsSellRate;

        public Item testItem;

        //=====Getters and Setters
        public void SetTraderInventory(Inventory newInventory) { tradersInventory = newInventory; }

        // Use this for initialization
        void Start()
        {
            SetRefrences();
            //CalculateAmountToPay(testItem.GetValue(), shopsSellRate);
        }

        private void SetRefrences()
        {
            thisInventory = this.GetComponent<Inventory>();

            if (Resources.FindObjectsOfTypeAll<UI_Inventory_Manager>().Length > 1)
            {
                Debug.LogError("Multiple UI Inventory Manager detected");
            }

            uIInventoryManager = GameObject.FindObjectsOfType<UI_Inventory_Manager>()[0]; // This is trying to find an inactive game object

            if (GameObject.FindObjectsOfType<UI_Inventory_Manager>().Length > 1)
            {
                Debug.LogError("Multiple Shop UI Manager detected");
            }

            shopUIManager = Resources.FindObjectsOfTypeAll<Shop_UI_Manager>()[0];
            tradersInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>(); // TODO MAKE THIS FOR MULTIPLAYER
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CloseShop();
            }
        }

        public void OpenShop()
        {
            print("Shop is open for buissness");
            
            shopUIManager.GetComponent<Transform>().gameObject.SetActive(true);
            shopUIManager.SetNPCInventory(thisInventory);
            uIInventoryManager.SetShopManager(this);
            uIInventoryManager.SetIsShopOpen(true);
        }
        public void CloseShop()
        {
            shopUIManager.SetNPCInventory(null);
            uIInventoryManager.SetShopManager(null);
            shopUIManager.GetComponent<Transform>().gameObject.SetActive(false);
            uIInventoryManager.SetIsShopOpen(false);
        }

        public void BuyItem(Inventory_Slot slot)
        {            
            Item itemToBuy = thisInventory.FindItemInSlot(slot);
            Money convertedAmount = CalculateAmountToPay(itemToBuy.GetValue(), shopsSellRate);


            if (tradersInventory.EnoughMoney(convertedAmount))
            {
                tradersInventory.RemoveMoney(convertedAmount);
                thisInventory.AddMoney(convertedAmount);
                thisInventory.RemoveItemFromInventory(null, thisInventory.WhichInventorySlot(slot.GetComponentInChildren<InventorySlotIcon>()));
                shopUIManager.UpdateUI();
                tradersInventory.AddItemToInventory(itemToBuy, -1);
            }
            else
            {                
                print("You do not have enough money");

            }
            
        }
        public void SellItem(Inventory_Slot slot)
        {
            Item itemToSell = tradersInventory.FindItemInSlot(slot);
            //print("Selling Item");
            thisInventory.AddItemToInventory(tradersInventory.FindItemInSlot(slot),-1);
            tradersInventory.RemoveItemFromInventory(null, tradersInventory.WhichInventorySlot(slot.GetComponentInChildren<InventorySlotIcon>()));
            thisInventory.RemoveMoney(itemToSell.GetValue());
            tradersInventory.AddMoney(itemToSell.GetValue());
            shopUIManager.UpdateUI();
        }
        private Money CalculateAmountToPay(Money amount, int addedAmount)
        {
            
            Money finishedAmount;
            finishedAmount.Gold = 0;
            finishedAmount.Silver = 0;
            finishedAmount.Copper = 0;
            float goldToCopper = amount.Gold * 10000;
            float silverToCopper = amount.Silver * 100;
            float copper = amount.Copper;
            
            float totalCopper = (goldToCopper + silverToCopper + copper);
            float totalConvertedCopper = (totalCopper * (100 + addedAmount)) / 100;

            while(totalConvertedCopper >= 10000)
            {
                finishedAmount.Gold++;
                totalConvertedCopper = totalConvertedCopper - 10000;
            }

            while(totalConvertedCopper >= 100)
            {
                finishedAmount.Silver++;
                totalConvertedCopper = totalConvertedCopper - 100;
            }

            finishedAmount.Copper = (int)totalConvertedCopper;
            
            return finishedAmount;
        }

    }
}

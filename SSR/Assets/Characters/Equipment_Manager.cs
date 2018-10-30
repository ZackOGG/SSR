using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SS.Equipment
{
    public class Equipment_Manager : MonoBehaviour
    {
        [Header ("UI Elements")]
        [SerializeField] GameObject uICharacterWindow;
        [SerializeField] GameObject uiRightHand;

        [Header("Slots")]
        [SerializeField] Item rightHand;
        [SerializeField] Sprite defaultWeaponIcon;
        [SerializeField] Item bodyArmor;
        [SerializeField] Item helmet;
        [SerializeField] Item defaultWeapon;
        public Item GetDefaultWeapon() { return defaultWeapon; }
        public Weapons testWeapon;
        public Armour testArmour;

        public delegate void EquipWeapon(Weapons newWeapon);
        public EquipWeapon equipWeapon;
        public delegate void EquipBodyArmourEvent(Armour newArmour);
        public EquipBodyArmourEvent equipBodyArmour;

        // Use this for initialization
        void Start()
        {
            Invoke("LateStart", 0.1f);
        }
        private void LateStart()
        {            
            EquipRightHand(defaultWeapon);           
            rightHand = null;
            
        }

        // Update is called once per frame
        void Update()
        {
            UpdateEquipmentUI();
            if (Input.GetKeyDown(KeyCode.C))
            {
                ToggleCharacterProfile();
            }

            if(Input.GetKeyDown(KeyCode.T))
            {
                //EquipRightHand(testWeapon.GetItem());
                //EquipBodyArmour(testArmour.GetItem());
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CloseCharacterProfile();
            }
        }

        private void ToggleCharacterProfile()
        {
            uICharacterWindow.SetActive(!uICharacterWindow.activeSelf);
        }
        private void CloseCharacterProfile()
        {
            uICharacterWindow.SetActive(false);
        }
        //=====UI Equipment Management=====
        private void UpdateEquipmentUI()
        {
            if (rightHand != null)
            {
                uiRightHand.GetComponentInChildren<InventorySlotIcon>().GetComponent<Image>().enabled = true;
                uiRightHand.GetComponentInChildren<InventorySlotIcon>().GetComponent<Image>().sprite = rightHand.GetItemIcon();
            }
            else
            {
                uiRightHand.GetComponentInChildren<InventorySlotIcon>().GetComponent<Image>().sprite = defaultWeaponIcon;
            }
        }

        //=====EquipmentSlotManagement=====
        //EquipRightHand
        //UnEquipRightHand

        //EquipmentSlotFree
        
        private void EquipRightHand(Item  theItem)
        {
            rightHand = theItem;
            if(theItem != null)
            {
                equipWeapon(theItem.GetItemWeapon());
                UpdateEquipmentUI();
            }
            else
            {
                equipWeapon(defaultWeapon.GetItemWeapon());
                UpdateEquipmentUI();
            }
            
            UpdateEquipmentUI();
        }
        public void UnEquipRightHand()
        {
            rightHand = null;
            equipWeapon(null);
            UpdateEquipmentUI();
        }
        public void EquipBodyArmour(Item theItem)
        {
            bodyArmor = theItem;
            if(theItem != null)
            {
                //NEED TO UPDATE EQUIPMENT UI
            }
        }
        public void UnEquipSlot(Equipment_Slot slot)
        {
            if(slot.GetEquipmentSlotType() == EquipmentSlotType.RHWeapon)
            {
                UnEquipRightHand();
            }
            else
            {
                Debug.LogError("Slot type not supported");
            }
        }
        public void EquipEquipmentInSlot(Item theItem, Equipment_Slot slot) // Equips in the correct slot
        {
            if(slot.GetEquipmentSlotType() == EquipmentSlotType.RHWeapon)
            {
                EquipRightHand(theItem);
            }
        }
        public bool DoesEquipmentMatchSlot(Item theItem, Equipment_Slot slot) // Checks to see if the equipment matches the slot
        {
            if (theItem != null)
            {
                if (theItem.GetItemWeapon() != null)
                {
                    print("Ready to put weapon in slot");
                    if (slot.GetEquipmentSlotType() == EquipmentSlotType.RHWeapon)
                    {
                        print("Equip right hand weapon");
                        return true;
                    }
                    Debug.LogError("Wrong slot type");
                    return false;
                }
                Debug.LogWarning("No weapon");
                return false;
            }
            print("no item");
            return false;
        }
        
        private bool EquipmentSlotFree(Equipment_Slot equipmentSlot)
        {
            if(equipmentSlot.GetEquipmentSlotType() == EquipmentSlotType.RHWeapon)
            {
                if(rightHand == false) // FIND IF THE RIGHT HAND SLOT IS EMPY THEN RIGHT ALL THE OTHER SLOTS IF THEY ARE EMPTY
                {
                    return true;
                }
                return false;
            }
            //add others equipment slots here
            Debug.LogError("EQUIPMENTSLOT FREE CHECK OUT OF BOUNDS");
            return false;
        }
        private void EquipEquipment(Equipment_Slot slot, Item theItem)
        {
            if(slot.GetEquipmentSlotType() == EquipmentSlotType.RHWeapon)
            {
                EquipRightHand(theItem);
            }
            if(slot.GetEquipmentSlotType() == EquipmentSlotType.BodyArmour)
            {
                print("Equiping some arrmor... Come on program it!!!");
            }
        }
        private EquipmentSlotType WhichEquipmentSlot(Equipment_Slot theSlot)
        {
            return theSlot.GetEquipmentSlotType();
        }
        public Item WhichItemInSlot(Equipment_Slot slot)
        {
            if (slot.GetEquipmentSlotType() == EquipmentSlotType.RHWeapon)
            {
                return rightHand;
            }
            if (slot.GetEquipmentSlotType() == EquipmentSlotType.BodyArmour)
            {
                print("Equiping some arrmor... Come on program it!!!");
                return null;
            }
            return null;
        }

        //=====Animator Updaters=====


        //===========================
    }
}
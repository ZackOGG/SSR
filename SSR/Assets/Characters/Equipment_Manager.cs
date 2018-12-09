using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SS.Character;

namespace SS.Equipment
{
    public class Equipment_Manager : MonoBehaviour
    {
        [Header ("UI Elements")]
        [SerializeField] GameObject uICharacterWindow;
        [SerializeField] GameObject uiRightHand;
        [SerializeField] GameObject uiBodyArmour;

        [Header("Slots")]
        [SerializeField] Item rightHand;
        [SerializeField] Sprite defaultWeaponIcon;
        [SerializeField] Sprite defaultArmourIcon;
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
        public delegate void RestartAnimations();
        public RestartAnimations restartAnimationEvent;

        //=====Getters and Setters=====
        public GameObject GetUIRightHand() { return uiRightHand; }
        public GameObject GetUIBodyArmour() { return uiBodyArmour; }

        private bool isPlayer;

        // Use this for initialization
        void Start()
        {
            isPlayer = this.GetComponent<Character_Stats>().IsPlayer();
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
            if(isPlayer)
            {
                UpdateEquipmentUI();
                if (Input.GetKeyDown(KeyCode.C))
                {
                    ToggleCharacterProfile();
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CloseCharacterProfile();
                }
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
        private void UpdateEquipmentUI() // DO FOR NEW EQUIPMENT SLOT
        {
            if (isPlayer)
            {
                if (rightHand != null)
                {
                    //uiRightHand.GetComponentInChildren<InventorySlotIcon>().GetComponent<Image>().enabled = true;
                    uiRightHand.GetComponentInChildren<InventorySlotIcon>().GetComponent<Image>().sprite = rightHand.GetItemIcon();
                }
                else
                {
                    uiRightHand.GetComponentInChildren<InventorySlotIcon>().GetComponent<Image>().sprite = defaultWeaponIcon;
                }
                if (bodyArmor != null)
                {
                    //uiBodyArmour.GetComponentInChildren<InventorySlotIcon>().GetComponent<Image>().enabled = true;
                    uiBodyArmour.GetComponentInChildren<InventorySlotIcon>().GetComponent<Image>().sprite = bodyArmor.GetItemIcon();
                }
                else
                {
                    uiBodyArmour.GetComponentInChildren<InventorySlotIcon>().GetComponent<Image>().sprite = defaultArmourIcon;
                }
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
                restartAnimationEvent();
                UpdateEquipmentUI();
            }
            else
            {
                equipWeapon(defaultWeapon.GetItemWeapon());
                restartAnimationEvent();
                UpdateEquipmentUI();
            }
            
            UpdateEquipmentUI();
            restartAnimationEvent();
        }
        public void UnEquipRightHand()
        {
            rightHand = null;
            equipWeapon(null);
            UpdateEquipmentUI();
            restartAnimationEvent();
        }
        public void EquipBodyArmour(Item theItem)
        {
            print("Equiping body armour");
            bodyArmor = theItem;
            if(theItem != null)
            {
                equipBodyArmour(theItem.GetItemBodyArmour());
                restartAnimationEvent();
            }
            else
            {
                UnEquipBodyArmour();
            }
            UpdateEquipmentUI();
            restartAnimationEvent();
            
        }
        public void UnEquipBodyArmour()
        {
            print("Test2");
            bodyArmor = null;
            equipBodyArmour(null);
            UpdateEquipmentUI();
            restartAnimationEvent();
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
            restartAnimationEvent();
        }
        public void EquipEquipmentInSlot(Item theItem, Equipment_Slot slot) // Equips in the correct slot // FOR ADDING NEW EQUIPMENTSLOTS
        {
            if(slot.GetEquipmentSlotType() == EquipmentSlotType.RHWeapon)
            {
                EquipRightHand(theItem);
                
            }
            if (slot.GetEquipmentSlotType() == EquipmentSlotType.BodyArmour)
            {
                //equipBodyArmour(theItem.GetItemBodyArmour());
                EquipBodyArmour(theItem);
            }
            restartAnimationEvent();
        }
        public bool DoesEquipmentMatchSlot(Item theItem, Equipment_Slot slot) // Checks to see if the equipment matches the slot
        {
            if (theItem != null)
            {
                if (theItem.GetItemWeapon() != null || theItem.GetItemBodyArmour() != null) // FOR ADDING NEW EQUIPMENTSLOTS
                {
                    print("Ready to put weapon in slot");
                    if (slot.GetEquipmentSlotType() == theItem.GetEquipmentSlotType()) // TODO PROGRAM THIS SO IT CAN LOOK AT THE SLOT AND THE WEAPON
                    {
                        print("Equip equipment");
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
                return bodyArmor;
            }
            return null;
        }

        //=====Animator Updaters=====


        //===========================
    }
}
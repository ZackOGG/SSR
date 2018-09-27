using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using SS.Character;

namespace SS.Camera_UI
{

    public class UI_Controller : MonoBehaviour
    {
        private int healthIntivals = 4;// This is to change how many hit points you need to lose before loseing a part of the heart.

        [Header("Heart Sprites")]
        [SerializeField] Sprite fullHeart;
        [SerializeField] Sprite emptyHeart;
        [SerializeField] Sprite threeQHeart;
        [SerializeField] Sprite halfHeart;
        [SerializeField] Sprite oneQHeart;

        [Header("UI Heart Renderers")]
        [SerializeField] Image[] heartImages;

        [Header("Heart Sprites")]
        [SerializeField] Sprite fullManaCrystal;
        [SerializeField] Sprite emptyManaCrystal;
        [SerializeField] Sprite threeQManaCrystal;
        [SerializeField] Sprite halfManaCrystal;
        [SerializeField] Sprite oneQManaCrystal;

        [Header("UI Mana Renderers")]
        [SerializeField] Image[] manaImages;

        [Header("UI Text")]
        [SerializeField] Text playerPromptText;
        [SerializeField] float promptFadeTime;

        [Header("Inventory Elements")]
        [SerializeField] GameObject inventoryWindow;

        [Header("Player Profile Elements")]
        [SerializeField] GameObject playerProfileWindow;


        private Character_Stats playerStatCon;
        private int playerMaxHealth;
        private int playerCurrentHealth;
        private int playerMaxMana;
        private int playerCurrentMana;        
        private Button testBTN;

        // Use this for initialization
        void Start()
        {
            playerStatCon = GameObject.FindGameObjectWithTag("Player").GetComponent<Character_Stats>();

            //smallTextBoxText = smallTextBox.GetComponentInChildren<Text>();
            playerMaxHealth = playerStatCon.GetMaxHealth();
            playerCurrentHealth = playerStatCon.GetCurrentHealth();

            CheckAmountOfHearts();
            //CheckAmountOfManaCrystals();
            FillHearts();


        }

        // Update is called once per frame
        void Update()
        {
            //TODO TAKE THESE OUT OF UPDATE AND MAKE IT DONE ONLY WHEN NEEDED
            playerMaxHealth = playerStatCon.GetMaxHealth();
            playerCurrentHealth = playerStatCon.GetCurrentHealth();
            //playerMaxMana = playerStatCon.GetMaxMana();
            //playerCurrentMana = playerStatCon.GetCurrentMana();
            FillHearts();
            FillManaCrystals();

           /* if (Input.GetButtonDown("Inventory"))
            {
                OpenCloseInventory();
            }

            if(Input.GetButtonDown("Player Profile"))
            {
                OpenClosePlayerProfile();
            }*/

        }

        private void CheckAmountOfHearts()
        {

            int heartsToMake = playerMaxHealth - 1;

            while (heartsToMake >= 0)
            {
                //print("Need to make: " + heartsToMake + " more hearts");
                //TODO MAKE HEARTS APEAR
                heartImages[heartsToMake].gameObject.SetActive(true);
                heartsToMake--;
            }
        }
        //=====Health Checking =====
        private void FillHearts()
        {
            //SetMaxHealthHearts();
            int heartsToFill = (playerCurrentHealth / 4) - 1;
            int currentHeartImage = 0;

            foreach (Image png in heartImages)
            {

                if (heartsToFill >= currentHeartImage)
                {

                    png.sprite = fullHeart;

                }
                else if (playerCurrentHealth % 4 == 1 && playerCurrentHealth % 4 != 2 && playerCurrentHealth % 4 != 3 && ((currentHeartImage + 1) * 4) - 3 == playerCurrentHealth)
                {
                    png.sprite = oneQHeart;
                }
                else if (playerCurrentHealth % 4 == 2 && playerCurrentHealth % 4 != 1 && playerCurrentHealth % 4 != 3 && ((currentHeartImage + 1) * 4) - 2 == playerCurrentHealth)
                {
                    png.sprite = halfHeart;
                }
                else if (playerCurrentHealth % 4 == 3 && playerCurrentHealth % 4 != 2 && playerCurrentHealth % 4 != 1 && ((currentHeartImage + 1) * 4) - 1 == playerCurrentHealth)
                {
                    png.sprite = threeQHeart;
                }

                else
                {
                    png.sprite = emptyHeart;
                }
                currentHeartImage++;
            }

        }

        private void SetMaxHealthHearts()
        {
            print("Test1");
            int heartsToFill = (playerMaxHealth / 4) - 1;
            int currentHeartImage = 0;
            foreach(Image png in heartImages)
            {
                if (heartsToFill >= currentHeartImage)
                {
                    print("Test2");
                    png.enabled = true;
                    

                }
                else
                {
                    print("Test3");
                    png.enabled = false;
                }
            }
        }

        //=====Mana Checking=====
        private void CheckAmountOfManaCrystals()
        {

            int manaCrystalsToMake = playerMaxHealth - 1;

            while (manaCrystalsToMake >= 0)
            {
                //print("Need to make: " + heartsToMake + " more hearts");
                //TODO MAKE HEARTS APEAR
                manaImages[manaCrystalsToMake].gameObject.SetActive(true);
                manaCrystalsToMake--;
            }
        }
        private void FillManaCrystals()
        {
            int crystalsToFill = (playerCurrentMana / 4) - 1;
            int currentManaImage = 0;

            foreach (Image png in manaImages)
            {

                if (crystalsToFill >= currentManaImage)
                {

                    png.sprite = fullManaCrystal;

                }
                else if (playerCurrentMana % 4 == 1 && playerCurrentMana % 4 != 2 && playerCurrentMana % 4 != 3 && ((currentManaImage + 1) * 4) - 3 == playerCurrentMana)
                {
                    png.sprite = oneQManaCrystal;
                }
                else if (playerCurrentMana % 4 == 2 && playerCurrentMana % 4 != 1 && playerCurrentMana % 4 != 3 && ((currentManaImage + 1) * 4) - 2 == playerCurrentMana)
                {
                    png.sprite = halfManaCrystal;
                }
                else if (playerCurrentMana % 4 == 3 && playerCurrentMana % 4 != 2 && playerCurrentMana % 4 != 1 && ((currentManaImage + 1) * 4) - 1 == playerCurrentMana)
                {
                    png.sprite = threeQManaCrystal;
                }

                else
                {
                    png.sprite = emptyManaCrystal;
                }
                currentManaImage++;
            }

        }

        public void addSpeachUIFN()
        {
            //SpeachPanel.SetActive(true);
        }

        public void removeSpeachUIFN()
        {
            //SpeachPanel.SetActive(false);
        }

        public void addSmallTextBox(string text)
        {
            //smallTextBox.SetActive(true);
            //smallTextBoxText.text = text;

        }

        public void removerSmallTextBox()
        {
            //smallTextBox.SetActive(false);
        }

        public IEnumerator removeSmallTextBoxTimeFN(float time)
        {
            print("Co Started");
            yield return new WaitForSeconds(time);
            print("Co Stoped");
        }
        //=====UI_Text_Functions=====

        public void StartDisplayPlayerPrompt(string msg)
        {
            StopAllCoroutines();
            StartCoroutine(DisplayPlayerPrompt(msg));
        }

        IEnumerator DisplayPlayerPrompt(string msg)
        {
            playerPromptText.gameObject.SetActive(true);
            playerPromptText.text = msg;

            yield return new WaitForSeconds(promptFadeTime);

            StopDisplayPlayerPrompt();
        }
        private void StopDisplayPlayerPrompt()
        {
            StopAllCoroutines();
            playerPromptText.gameObject.SetActive(false);
        }

        //===========================

        //=====Inventory=====

        /* public void SetBagUI(Sprite bagImage)
         {
             inventoryWindow.GetComponent<Image>().sprite = bagImage;

         }*/

        private void OpenCloseInventory()
        {
            
            inventoryWindow.SetActive(!inventoryWindow.activeSelf);
            
        }

        private void OpenClosePlayerProfile()
        {
            playerProfileWindow.SetActive(!playerProfileWindow.activeSelf);
        }
        /*private void GetIconPoints()
       {

           foreach(Transform child in inventoryWindow.transform)
           {

               iconPoints.Add(child);
           }
       }*/

        /* public void PlaceIconOnUI(int slot, Item item)
         {

             iconPoints[slot].GetComponent<Image>().sprite = item.GetItem().itemIcon;
             Color tempColor = iconPoints[slot].GetComponent<Image>().color;
             //Image tempIconImage = iconPoints[slot].GetComponent<Image>().color.a = 1;
             //tempIconImage.color.a = 0;
             tempColor.a = 255;

             iconPoints[slot].GetComponent<Image>().color = tempColor;
             /*GameObject newIcon = Instantiate(defaultIcon, iconPoints[(slot)]);
             newIcon.GetComponentInChildren<Image>().sprite = item.GetItem().itemIcon;
         }*/




        //===================
    }
    //TODO MAKE THE UI BEABLE TO REMOVE ICONS
}
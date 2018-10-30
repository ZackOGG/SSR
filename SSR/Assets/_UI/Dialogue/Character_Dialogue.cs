using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Quests;
using SS.Equipment;
using SS.Character;

namespace SS.UI
{

    public class Character_Dialogue : MonoBehaviour {

        private float talkRadius = 1.2f;
        [SerializeField] string nPCName;
        [SerializeField] Dialouge[] sentances;
        private int currentSelfSwitch;
        private GameObject player;
        private Dialogue_Manager dialogueManger;
        private bool talkingToThisNPC = false;
        private int currentDialougeState = 0;
        private Inventory playersInventory;

        private void Start()
        {            
            SetRefrences();
        }
        private void SetRefrences()
        {
            dialogueManger = GameObject.Find("Dialogue Manager").GetComponent<Dialogue_Manager>();
            player = GameObject.FindGameObjectWithTag("Player");
            playersInventory = player.GetComponent<Inventory>();
        }

        private void Update()
        {
            float distance = (player.transform.position - this.transform.position).magnitude;
            if (distance < talkRadius)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {                    
                    StartDialogue();
                }
            }
            else if(talkingToThisNPC)
            {                
                StopDialogue();
                SetNPCName();
                if (this.GetComponent<Shop_Manager>())
                {
                    this.GetComponent<Shop_Manager>().CloseShop();
                }

            }
            if (Input.GetKeyDown(KeyCode.Escape) && talkingToThisNPC)
            {
                StopDialogue();
                SetNPCName();
                if (this.GetComponent<Shop_Manager>())
                {
                    this.GetComponent<Shop_Manager>().CloseShop();
                }
            }

        }

        private void StartDialogue()
        {
            talkingToThisNPC = true;
            dialogueManger.OpenDialogueBox(this);
            SetNPCName();
            SetNPCWordsAndAnswers(0);
        }
        private void StopDialogue()
        {
            talkingToThisNPC = false;
            currentDialougeState = 0;
            dialogueManger.CloseDialogueBox();
            
        }

        private void SetNPCName()
        {
            dialogueManger.SetNPCName(nPCName);
        }
        private void SetNPCWordsAndAnswers(int theDialogueID)
        {
            if(theDialogueID == -1)
            {
                StopDialogue();
                return;
            }
            currentDialougeState = theDialogueID;
            dialogueManger.SetNPCWords(sentances[currentSelfSwitch].dialogue[theDialogueID].sentence);
            int theResponses = 0;
            foreach(Response response in sentances[currentSelfSwitch].dialogue[theDialogueID].responces)
            {
                dialogueManger.SetAnswerBTNTText(theResponses, response.response);
                theResponses++;
            }
            dialogueManger.EnableDisableBoxes(sentances[currentSelfSwitch].dialogue[theDialogueID].responces.Length);
            if (sentances[currentSelfSwitch].dialogue[theDialogueID].nPCAction == NPCAction.Item) // Give item to player
            {
                playersInventory.AddItemToInventory(sentances[currentSelfSwitch].dialogue[theDialogueID].theItemToGiveOrTake, -1);

                //TODO ADD IF BAG IS FULL SOMTHING EG DROP
            }
            if(sentances[currentSelfSwitch].dialogue[theDialogueID].nPCAction == NPCAction.TakeItem) //Take Item from player
            {
                playersInventory.RemoveItemFromInventory(sentances[currentSelfSwitch].dialogue[theDialogueID].theItemToGiveOrTake, -1);
            }
            if (sentances[currentSelfSwitch].dialogue[theDialogueID].nPCAction == NPCAction.Quest) // Gives player a new quest
            {
                player.GetComponent<Quest_Jornal>().AddNewQuest(sentances[currentSelfSwitch].dialogue[theDialogueID].theQuestToGiveOrTake);
            }
            if (sentances[currentSelfSwitch].dialogue[theDialogueID].nPCAction == NPCAction.takeQuest) // Checks to see if the quest is complete
            {
                Quest_Jornal playerJournal = player.GetComponent<Quest_Jornal>();
                if (playerJournal.IsQuestComplete(sentances[currentSelfSwitch].dialogue[theDialogueID].theQuestToGiveOrTake) == true)
                {
                    SetNPCWordsAndAnswers(sentances[currentDialougeState].dialogue[theDialogueID].questSuccessLocalID);
                    playerJournal.ResetQuestData(sentances[currentSelfSwitch].dialogue[theDialogueID].theQuestToGiveOrTake);
                    playerJournal.RemoveQuest(sentances[currentSelfSwitch].dialogue[theDialogueID].theQuestToGiveOrTake);
                    
                }
                else
                {
                    SetNPCWordsAndAnswers(sentances[currentDialougeState].dialogue[theDialogueID].questFailedLocalID);
                }
            }
            
            if(sentances[currentSelfSwitch].dialogue[theDialogueID].nPCAction == NPCAction.OpenShop)
            {
                dialogueManger.CloseDialogueBox();
                this.GetComponent<Shop_Manager>().OpenShop();
            }
            
            if (sentances[currentSelfSwitch].dialogue[theDialogueID].selfSwitchToSwitchTo > 0) // Checks to selfswitch
            {
                currentSelfSwitch = sentances[currentSelfSwitch].dialogue[theDialogueID].selfSwitchToSwitchTo;
                StopDialogue();
            }           
            
            
        }

        //=====Answers=====
        public void AnswerOnePressed()
        {
            SetNPCWordsAndAnswers(sentances[currentSelfSwitch].dialogue[currentDialougeState].responces[0].responceNextID);
        }
        public void AnswerTwoPressed()
        {
            SetNPCWordsAndAnswers(sentances[currentSelfSwitch].dialogue[currentDialougeState].responces[1].responceNextID);
        }
        public void AnswerThreePressed()
        {
            SetNPCWordsAndAnswers(sentances[currentSelfSwitch].dialogue[currentDialougeState].responces[2].responceNextID);
        }
        public void AnswerFourPressed()
        {
            SetNPCWordsAndAnswers(sentances[currentSelfSwitch].dialogue[currentDialougeState].responces[3].responceNextID);
        }
        //=================


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, talkRadius);
        }


    }
}
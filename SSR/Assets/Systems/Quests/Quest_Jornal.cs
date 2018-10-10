using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Equipment;

namespace SS.Quests
{
    public class Quest_Jornal : MonoBehaviour {

        [SerializeField] List<Quest> questList;
        [SerializeField] List<QuestContence> questProgress;
        //[SerializeField] QuestContence testQuestTwo;

        private Inventory playersInventory;

        private void Start()
        {
            SetRefrences();
        }
        private void SetRefrences()
        {
            playersInventory = this.GetComponent<Inventory>();
        }

        public void AddNewQuest(Quest newQuest)
        {            
            questList.Add(newQuest);
            CreateQuestProgressData(newQuest);
            UpdateItemQuests();
            UpdateHuntQuests(null, -1, newQuest);
        }

        public void RemoveQuest(Quest finishedQuest)
        {
            questProgress.Remove(questProgress[FindQuestLocation(finishedQuest)]);
            questList.Remove(finishedQuest);
            
        }
        private void CreateQuestProgressData(Quest newQuest)
        {
            Quest questClone = Instantiate(newQuest);
            questProgress.Add(questClone.GetQuestContence());
        }
        public void UpdateHuntQuests(QuestCounter questCounter, int amount, Quest newQuest) //(HUNT QUESTS) Check to see if the objectives are met and if they all are the set quest complete 
        {
            if (questCounter != null)
            {
                foreach (Quest quest in questList)
                {
                    if (quest.GetQuestContence().questID == questCounter.GetQuestID())
                    {
                        foreach (QuestObjectives qO in quest.GetQuestContence().questObjectives)
                        {
                            if (qO.objectiveID == questCounter.GetQuestObjectiveID())
                            {
                                int questObjectiveID = questCounter.GetQuestObjectiveID();
                                questProgress[FindQuestLocation(quest)].questObjectives[questObjectiveID - 1].amount += questCounter.GetAmount();

                                IsQuestQbjectiveComplete(quest, questObjectiveID);
                            }

                        }
                    }

                    IsQuestComplete(quest);
                }
            }
            else
            {
                questProgress[FindQuestLocation(newQuest)].CheckQuestCompleted();
            }
        }
        private int FindQuestLocation(Quest questionedQuest) //Returns a number for the array of the quest progress that matches the order of the questlist
        {
            int questNum = 0;
            foreach(Quest theQuest in questList)
            {
                if(questionedQuest.GetQuestContence().questID == questProgress[questNum].questID)
                {
                    return questNum;
                }
                else
                {
                    questNum++;
                }
                
            }
            
            return -1;
        }
        public bool IsQuestComplete(Quest questionedQuest)
        {
            int theQuestNumber = FindQuestLocation(questionedQuest);
            QuestContence theQuestProgress = questProgress[theQuestNumber];
            foreach(QuestObjectives questObj in questProgress[theQuestNumber].questObjectives)
            {
                if(questObj.Completed == false)
                {
                    questProgress[theQuestNumber].questCompleted = false;
                    return false;
                }                

            }
            questProgress[theQuestNumber].questCompleted = true;
            return true;
        }
        private bool IsQuestQbjectiveComplete(Quest questioned, int qObjectiveID)
        {            
            int theQuestProgress = questProgress[FindQuestLocation(questioned)].questObjectives[qObjectiveID -1].amount;
            foreach (QuestObjectives questObj in questioned.GetQuestContence().questObjectives)
            {
                if (theQuestProgress >= questioned.GetQuestContence().questObjectives[qObjectiveID - 1].amountToComplete)
                {
                    questProgress[FindQuestLocation(questioned)].questObjectives[qObjectiveID - 1].Completed = true;
                    return true;
                }
                else
                {
                    questProgress[FindQuestLocation(questioned)].questObjectives[qObjectiveID - 1].Completed = false;
                    return false;
                }
            }
            Debug.LogError("Unexpected fallout of loop");
            return false;
        }
        public void UpdateItemQuests()
        {
            foreach (Quest theQuestCon in questList)
            {
                
                //Find which quest is theQuestCon then get that array and then check the questProgress[theArray]
                foreach(QuestObjectives theQuestObj in questProgress[FindQuestLocation(theQuestCon)].questObjectives)
                {
                    if(theQuestObj.questType == QuestType.Aquire)
                    {
                        theQuestObj.amount = playersInventory.CheckAmountOfItemInBag(theQuestObj.itemToHandIn);
                        IsQuestQbjectiveComplete(theQuestCon, theQuestObj.objectiveID);
                        IsQuestComplete(theQuestCon);
                    }               
                }
                
            }
            
        }
        public void ResetQuestData(Quest quest)
        {            
            foreach(Quest theQuest in questList)
            {
                if(theQuest == quest)
                {
                    print("RestingData");
                    foreach (QuestObjectives qOb in theQuest.GetQuestObjectives())
                    {
                        qOb.amount = 0;
                        qOb.Completed = false;
                    }
                    theQuest.GetQuestContence().SetQuestCompleted(false);
                }
            }
        }
    }
}
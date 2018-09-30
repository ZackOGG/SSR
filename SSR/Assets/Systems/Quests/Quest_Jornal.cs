using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.Quests
{
    public class Quest_Jornal : MonoBehaviour {

        [SerializeField] List<Quest> questList;
        [SerializeField] List<QuestContence> questProgress;
        //[SerializeField] QuestContence testQuestTwo;


        public void AddNewQuest(Quest newQuest)
        {
            questList.Add(newQuest);
            CreateQuestProgressData(newQuest);
        }

        public void RemoveQuest(Quest finishedQuest)
        {
            questList.Remove(finishedQuest);
        }
        private void CreateQuestProgressData(Quest newQuest)
        {
            Quest questClone = Instantiate(newQuest);
            questProgress.Add(questClone.GetQuestContence());
        }
        public void UpdateQuest(QuestCounter questCounter, int amount)
        {
            foreach(Quest quest in questList)
            {
                if(quest.GetQuestContence().questID == questCounter.GetQuestID())
                {
                    foreach(QuestObjectives qO in quest.GetQuestContence().questObjectives)
                    {
                        if (qO.objectiveID == questCounter.GetQuestObjectiveID())
                        {
                            int questObjectiveID = questCounter.GetQuestObjectiveID();
                            print(questProgress[0].questObjectives[questObjectiveID-1].amount);
                            questProgress[FindQuestLocation(quest)].questObjectives[questObjectiveID - 1].amount += questCounter.GetAmount();
                            print(questProgress[0].questObjectives[questObjectiveID - 1].amount);

                            IsQuestQbjectiveComplete(quest, questObjectiveID);
                            IsQuestComplete(quest);
                            
                        }
                        
                    }
                }
            }
        }
        private int FindQuestLocation(Quest questionedQuest)
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
                    return false;
                }
            }
            Debug.LogError("Unexpected fallout of loop");
            return false;
        }
    }
}
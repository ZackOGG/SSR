using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Equipment;


namespace SS.Quests
{
    [CreateAssetMenu(menuName = ("SSR/Systems/Quest"))]
    public class Quest : ScriptableObject
    {

        [SerializeField] QuestContence quest;
        public QuestContence GetQuestContence() { return quest; }
        public QuestObjectives[] GetQuestObjectives() { return quest.questObjectives; }
        public Item GetQuestObjectiveItem(int arrayNum) { return quest.questObjectives[arrayNum].itemToHandIn; }
        public void CheckQuestCompletionForInventoryItems()
        {
            Debug.Log("Checking quest completion for items");
            Inventory inventory = FindObjectOfType<Inventory>(); //TODO Not two player freindly

            foreach(QuestObjectives qObjective in GetQuestObjectives())
            {
                if(qObjective.questType == QuestType.Aquire)
                {
                    int itemAmount = 0;
                    foreach(Item theItem in inventory.GetInventoryItems())
                    {
                        if(qObjective.itemToHandIn == theItem)
                        {
                            itemAmount++;
                        }
                    }
                    if(itemAmount >= qObjective.amountToComplete)
                    {
                        qObjective.Completed = true;
                    }
                    else
                    {
                        qObjective.Completed = false;
                    }

                }
            }
        }

    }
}
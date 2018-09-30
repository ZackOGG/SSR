using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.Quests
{
    public class QuestCounter : MonoBehaviour
    { 
        [SerializeField] int questID;
        [Tooltip("The id of the individual objective in the quest")]
        [SerializeField] int objectiveID;
        [SerializeField] int amount;

        public int GetQuestID()
        {
            return questID;
        }
        public int GetQuestObjectiveID()
        {
            return objectiveID;
        }
        public int GetAmount()
        {
            return amount;
        }

        private void OnDestroy()
        {
            UpLoadQuestData();
        }

        private void UpLoadQuestData()
        {
            GameObject.Find("Player").GetComponent<Quest_Jornal>().UpdateQuest(this, amount);
        }

    }
}
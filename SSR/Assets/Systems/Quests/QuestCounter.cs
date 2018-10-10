using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Character;

namespace SS.Quests
{
    public class QuestCounter : MonoBehaviour
    { 
        [SerializeField] int questID;
        [Tooltip("The id of the individual objective in the quest")]
        [SerializeField] int objectiveID;
        [SerializeField] int amount;
        [SerializeField] Quest linkedQuest;

        private Character_Stats characterStats;

        private void Start()
        {
            characterStats = this.GetComponent<Character_Stats>();
            characterStats.callingDeath += UpLoadQuestData;
        }

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
            //UpLoadQuestData();
        }

        private void UpLoadQuestData(bool ignoreBool)
        {
            print("Uploading data");
            linkedQuest.GetQuestContence().questObjectives[(objectiveID -1)].amount += amount;
            GameObject.Find("Player").GetComponent<Quest_Jornal>().UpdateHuntQuests(this, amount, null);
        }

    }
}
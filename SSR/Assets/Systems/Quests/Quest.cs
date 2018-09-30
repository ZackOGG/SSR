using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SS.Quests
{
    [CreateAssetMenu(menuName = ("SSR/Systems/Quest"))]
    public class Quest : ScriptableObject {

        [SerializeField] QuestContence quest;
        public QuestContence GetQuestContence() { return quest; }


    }
}
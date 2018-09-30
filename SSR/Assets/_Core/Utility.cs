using UnityEngine;
using SS.Quests;

public enum Direction {Up, Down, Left, Right}
public enum AlignmentToPlayer { Friendly, Neutral, Hostile}
public enum QuestType { Fetch, Hunt, Gather}
public enum NPCGiveOrTakeType {None, Quest, Item, takeQuest, TakeItem}

namespace SS.Quests
{
    [System.Serializable]
    public class QuestContence
    {
        public int questID;
        public string questName;
        public QuestObjectives[] questObjectives;
        public bool questCompleted;
        public void SetQuestCompleted(bool newBool)
        {
            questCompleted = newBool;
        }
    }
    [System.Serializable]
    public struct QuestObjectives
    {
        public int objectiveID;
        public QuestType questType;
        public int amount;
        public int amountToComplete;
        public bool Completed;
    }


}

[System.Serializable]
public class Dialouge
{
    public Sentence[] dialogue;
}

[System.Serializable]
public struct Sentence
{
    public int localID;
    public string sentence;
    [Tooltip("0 and no self switch will be activated")]
    public int selfSwitchToSwitchTo;
    public NPCGiveOrTakeType giveOrTakeType;
    public Quest theQuestToGiveOrTake;
    public int questSuccessLocalID;
    public int questFailedLocalID;    
    public Response[] responces;
}

[System.Serializable]
public struct Response
{
    public string response;
    public int responceNextID;
}

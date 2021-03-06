﻿using UnityEngine;
using SS.Quests;
using SS.Equipment;

public enum Direction {Up, Down, Left, Right}
public enum AlignmentToPlayer { Friendly, Neutral, Hostile}
public enum QuestType {Hunt, Aquire}
public enum NPCAction {None, Quest, Item, takeQuest, TakeItem, OpenShop}
public enum ItemType {None, Consumable, Weapon, Armour}
public enum AttackType { Sword, Axe, Punch}
public enum EquipmentSlotType { None, RHWeapon, BodyArmour, Helmet}
public enum StatType { Strength, Health }


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
        public void CheckQuestCompleted()
        {
            foreach (QuestObjectives qO in questObjectives)
            {
                if (!(qO.amount >= qO.amountToComplete))
                {
                    Debug.Log("SETTING FALSE");
                    SetQuestCompleted(false);
                    return;
                }
                else
                {
                    qO.Completed = true;
                }
            }
            Debug.Log("SETTING TURE");
            SetQuestCompleted(true);
        }
    }
    [System.Serializable]
    public class QuestObjectives
    {
        public int objectiveID;
        public QuestType questType;
        public int amount;
        public int amountToComplete;
        public Item itemToHandIn;
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
    public NPCAction nPCAction;
    public Quest theQuestToGiveOrTake;
    public Item theItemToGiveOrTake;
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

public struct InventoyrOrEquipmentSlot
{
    public Inventory_Slot inventorySlot;
    public Equipment_Slot equipmentSlot;
}

[System.Serializable]
public struct Money
{
    public int Gold;
    public int Silver;
    public int Copper;
}

public struct KnockbackPower
{
    public float force;
    public float duration;
    public Vector2 direction;
}

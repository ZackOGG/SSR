using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SS.UI
{
    public class Dialogue_Manager : MonoBehaviour
    {   
        [Tooltip("The current NPC dialouge script that is being talked to.")]
        [SerializeField] Character_Dialogue currentDialogue;
        public void SetCurrentDialogue(Character_Dialogue newDialogue){ currentDialogue = newDialogue;}
        public Character_Dialogue GetDialogue(){ return currentDialogue;}

        [Header("UI Elements")]
        [SerializeField] GameObject DialogueBox;
        [SerializeField] Text nPCName;
        [SerializeField] Text nPCWords;
        [Tooltip("This is the text box of the btn")]
        [SerializeField] Text[] answerBTNText;
        [Tooltip("This is the number and the button ready enabling and disabling")]
        [SerializeField] GameObject[] answerFields; 

        public void OpenDialogueBox(Character_Dialogue newDialogue)
        {
            DialogueBox.SetActive(true);
            SetCurrentDialogue(newDialogue);
        }
        public void CloseDialogueBox()
        {   
            DialogueBox.SetActive(false);
            currentDialogue = null;
        }

        public void SetNPCName(string newString)
        {
            nPCName.text = newString;
        }
        public void SetNPCWords(string newString)
        {
            nPCWords.text = newString;
        }
        public void SetAnswerBTNTText(int theBoxNum, string newString)
        {
            answerBTNText[theBoxNum].text = newString; 
        }
        public void EnableDisableBoxes(int amountOfBoxes)
        {
            int boxNum = 0;
            foreach(GameObject fields in answerFields)
            {
                if(boxNum >= amountOfBoxes)
                {
                    fields.SetActive(false);
                }
                else
                {
                    fields.SetActive(true);
                }
                boxNum++;
            }
        }

        public void AnswerOnePressed()
        {
            currentDialogue.AnswerOnePressed();
        }

        public void AnswerTwoPressed()
        {
            currentDialogue.AnswerTwoPressed();
        }

        public void AnswerThreePressed()
        {
            currentDialogue.AnswerThreePressed();
        }

        public void AnswerFourPressed()
        {
            currentDialogue.AnswerFourPressed();
        }

        public void AnswerFivePressed()
        {
            currentDialogue.AnswerFivePressed();
        }

    }
}
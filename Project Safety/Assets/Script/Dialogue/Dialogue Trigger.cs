using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;

// CONTAINS DATA ABOUT A SINGLE DIALOGUE
// SENDS MESSAGE TO THE DIALOGUEMANAGER CLASS

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] List<DialogueProperties> dialogueProperties = new List<DialogueProperties>();
    public bool isSpeaking = false;
    public bool speechTrigger = false;

    #region - START DIALOGUE | EVENT TRIGGERED - 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !speechTrigger)
        {
            dialogueManager.DialogueStart(dialogueProperties);
            speechTrigger = true;
        }
    }

    #endregion

    #region - START DIALOGUE | INPUT PRESSED - 

    public void StartDialogue()
    {
        if(!isSpeaking)
        {
            Debug.Log("Start Dialogue - Dialogue Trigger Script!");
            dialogueManager.DialogueStart(dialogueProperties);
            isSpeaking = true;
        }
    }

    #endregion

    // public void SpeechTriggerStatus(bool _status)
    // {
    //     speechTrigger = _status;
    // }

}

[System.Serializable]
public class DialogueProperties
{
    public string npcName;
    [TextArea(3, 10)]
    public string dialogue;
    public bool isEnd;

    [Header("Branch")]
    public bool isQuestion;
    public string answerOption1;
    public string answerOption2;
    public int option1IndexJump;
    public int option2IndexJump;
    [Space(10)]
    public bool is3Question;
    public string answerOption3;
    public int option3IndexJump;
    [Space(10)]
    public bool otherEvent;


    [Header("Trigger Event")]
    public UnityEvent startDialogueEvent;
    public UnityEvent endDialogueEvent;
}
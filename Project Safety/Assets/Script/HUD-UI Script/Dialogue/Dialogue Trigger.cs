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

}

[System.Serializable]
public class DialogueProperties
{
    public string npcName;
    [TextArea(3, 10)]
    public string dialogue;
    public float delayNextDialogue;
    public bool isEnd;

    [Header("Choices")]
    public bool isDialogueAQuestion; // DEFAULT CHOICES TO CHOOSE IS 2
    public bool isDialogueA3ChoicesQuestion;

    [Space(20)]
    public string choiceAnswer1;
    public int choice1JumpTo;
    
    [Space(10)]
    public string choiceAnswer2;
    public int choice2JumpTo;

    [Space(10)]
    public string choiceAnswer3;
    public int choice3JumpTo;

    [Space(20)]
    public AudioClip dialogouAudio;
    public bool isOtherEvent; // DEFAULT IS AUTOMATATIC ENABLING PLAYER SCRIPTS


    [Header("Trigger Event")]
    public UnityEvent startDialogueEvent;
    public UnityEvent endDialogueEvent;
}
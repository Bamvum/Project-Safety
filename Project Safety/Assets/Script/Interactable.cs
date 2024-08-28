using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    [Header("Script")]
    [SerializeField] PrologueSceneManager prologueSceneManager;
    [SerializeField] DialogueTrigger dialogueTrigger;
    
    [Header("Flags")]
    public bool isLightSwitch;
    public bool isDoor;
    public bool isPC;
    public bool isMonitor;

    [Header("Light Switch")]
    [SerializeField] GameObject lightSource;
    [SerializeField] GameObject switchOn;
    [SerializeField] GameObject switchOff;
    [Space(10)]
    bool isInteracted;
    
    [Header("Door")]
    [SerializeField] GameObject doorParent;
    
    [SerializeField] Animator doorAnimator;

    void Update()
    {

    }

    public void LightSwitchTrigger()
    {
        Debug.Log("Light Switch Trigger");
        if(lightSource.activeSelf)
        {
            lightSource.SetActive(false);
            switchOn.SetActive(false);
            switchOff.SetActive(true);
        }
        else
        {
            lightSource.SetActive(true);
            switchOn.SetActive(true);
            switchOff.SetActive(false);

            
            if(!isInteracted)
            {
                MissionManager.instance.HideMission();
                isInteracted = true;

                if(SceneManager.GetActiveScene().name == "Prologue")
                {
                    this.enabled = false;
                    this.gameObject.layer = 0;
                }
            }
        }
    }

    public void DoorTrigger()
    {
        // TODO FLAG TO NOT TRIGGER DIALOGUE AGAIN (ELSE PLAY SFX - LOCKED DOOR)
        if(SceneManager.GetActiveScene().name == "Prologue")
        {
            if(!isInteracted)
            {
                dialogueTrigger.StartDialogue();
                isInteracted = true;
            }
        }
        
        if(isInteracted)
        {
            DoorClose();
            isInteracted = false;
        }
        else
        {
            DoorOpen();
            isInteracted = true;
        }
    }

    void DoorOpen()
    {
        Debug.Log("Door Open");
        doorAnimator.SetBool("Door Open", true);
        doorAnimator.SetBool("Door Close", false);

        // PLAY DOOR SFX
    }

    void DoorClose()
    {
        Debug.Log("Door Close");
        doorAnimator.SetBool("Door Close", true);
        doorAnimator.SetBool("Door Open", false);

        // PLAY DOOR SFX
    }

    

    public void PC()
    {
        prologueSceneManager.PC.layer = 0;
        prologueSceneManager.monitorScreen[0].SetActive(true);
        Invoke("DelayStartPC", 20);
    }

    void DelayStartPC()
    {
        prologueSceneManager.monitor.layer = 8;
        prologueSceneManager.monitorScreen[0].SetActive(false);
        prologueSceneManager.monitorScreen[1].SetActive(true);
        prologueSceneManager.monitorSFX.Play();
    }

    public void AccessMonitor()
    {
        Debug.Log("Player Accessed the Monitor!");
        // prologueSceneManager.TransitionToHomeworkQuiz();
        dialogueTrigger.StartDialogue();
    }
}

using System.Collections;
using System.Collections.Generic;
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
                prologueSceneManager.HideMission();
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
        // TODO ANIMATION OF OPENING THE DOORS

        

        // TODO FLAG TO NOT TRIGGER DIALOGUE AGAIN (ELSE PLAY SFX - LOCKED DOOR)
        if(SceneManager.GetActiveScene().name == "Prologue")
        {
            if(!isInteracted)
            {
                dialogueTrigger.StartDialogue();
                isInteracted = true;
            }
        }
    }


    public void PC()
    {
        // prologueSceneManager.PC.layer = 0;
        // prologueSceneManager.monitorScreen[0].SetActive(true);
        Invoke("DelayStartPC", 20);
    }

    void DelayStartPC()
    {
        // prologueSceneManager.monitor.layer = 8;
        // prologueSceneManager.monitorScreen[0].SetActive(false);
        // prologueSceneManager.monitorScreen[1].SetActive(true);
        // prologueSceneManager.monitorSFX.Play();
    }

    public void AccessMonitor()
    {
        Debug.Log("Player Accessed the Monitor!");
        // prologueSceneManager.TransitionToHomeworkQuiz();
        dialogueTrigger.StartDialogue();
    }
}

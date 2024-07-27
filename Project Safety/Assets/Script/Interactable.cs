using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    [Header("Script")]
    [SerializeField] PrologueSceneManager prologueSceneManager;

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
    [SerializeField] bool isInteracted;
    

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
                }
            }
        }
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
        prologueSceneManager.TransitionToHomeworkQuiz();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Script")]
    [SerializeField] PrologueSceneManager prologueSceneManager;

    [Header("Flags")]
    public bool isLightSwitch;
    [SerializeField] bool isDoor;

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
            }
        }
    }
    
    
}

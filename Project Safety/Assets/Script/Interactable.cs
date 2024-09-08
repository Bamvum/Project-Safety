using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Interactable : MonoBehaviour
{
    [Header("Script")]
    [SerializeField] DialogueTrigger dialogueTrigger;
    
    [Header("Flags")]
    public bool isAlarm;
    public bool isLightSwitch;
    public bool isDoor;
    public bool isPC;
    public bool isMonitor;
    public bool isSocketPlug;
    public bool isWardrobe;
    public bool isOutsideDoor;
    public bool isBus;

    [Header("Alarm")]
    [SerializeField] GameObject phoneLight;

    [Header("Light Switch")]
    [SerializeField] GameObject lightSource;
    [SerializeField] GameObject switchOn;
    [SerializeField] GameObject switchOff;
    [Space(10)]
    bool isInteracted;
    
    [Header("Door")]
    [SerializeField] GameObject doorParent;
    
    [SerializeField] Animator doorAnimator;

    [Header("Plug")]
    [SerializeField] GameObject plug;
    [SerializeField] GameObject unplug;

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

            if(SceneManager.GetActiveScene().name == "Prologue")
            {
                if (!isInteracted)
                {
                    MissionManager.instance.HideMission();
                    isInteracted = true;


                    this.enabled = false;
                    this.gameObject.layer = 0;
                    PrologueSceneManager.instance.PC.layer = 8;
                    Debug.Log(PrologueSceneManager.instance.PC.layer);
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
        else
        {
            if (isInteracted)
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
        PrologueSceneManager.instance.PC.layer = 0;
        PrologueSceneManager.instance.monitorScreen[0].SetActive(true);
        Invoke("DelayStartPC", 20);
    }

    void DelayStartPC()
    {
        PrologueSceneManager.instance.monitor.layer = 8;
        PrologueSceneManager.instance.monitorScreen[0].SetActive(false);
        PrologueSceneManager.instance.monitorScreen[1].SetActive(true);
        PrologueSceneManager.instance.monitorSFX.Play();
    }

    public void AccessMonitor()
    {
        Debug.Log("Player Accessed the Monitor!");
        // prologueSceneManager.TransitionToHomeworkQuiz();
        dialogueTrigger.StartDialogue();
    }

    public void Unplug()
    {
        if(SceneManager.GetActiveScene().name == "Act 1 Scene 1")
        {
            gameObject.layer = 0;

            PlayerScript.instance.playerMovement.enabled = false;
            PlayerScript.instance.interact.enabled = false;
            PlayerScript.instance.cinemachineInputProvider.enabled = false;
            // PlayerScript.instance.examine.enabled = false;

            StartCoroutine(UnplugPlug());
        }

    }

    IEnumerator UnplugPlug()
    {
        HUDManager.instance.FadeInForDialogue();

        yield return new WaitForSeconds(1);
        
        plug.SetActive(false);
        unplug.SetActive(true);
        
        Act1StudentSceneManager.instance.plugInteracted++;
        HUDManager.instance.FadeOutForDialogue();

        yield return new WaitForSeconds(1);
        
        if(Act1StudentSceneManager.instance.plugInteracted <= 4)
        {
            PlayerScript.instance.playerMovement.enabled = true;
            PlayerScript.instance.interact.enabled = true;
            PlayerScript.instance.cinemachineInputProvider.enabled = true;
        }
        // PlayerScript.instance.examine.enabled = true;
    }


    public void ChangeClothes()
    {
        if(SceneManager.GetActiveScene().name == "Act 1 Scene 1")
        {
            gameObject.layer = 0;

            PlayerScript.instance.playerMovement.enabled = false;
            PlayerScript.instance.interact.enabled = false;
            PlayerScript.instance.cinemachineInputProvider.enabled = false;
            
            StartCoroutine(ChangingCloth());
        }
    }

    IEnumerator ChangingCloth()
    {
        HUDManager.instance.FadeInForDialogue();

        yield return new WaitForSeconds(1);

        HUDManager.instance.FadeOutForDialogue();

        yield return new WaitForSeconds(1);

        PlayerScript.instance.playerMovement.enabled = true;
        PlayerScript.instance.interact.enabled = true;
        PlayerScript.instance.cinemachineInputProvider.enabled = true;

        Act1StudentSceneManager.instance.DoorEndingScene.layer = 8;
        MissionManager.instance.HideMission();
    }

    public void GoOutside()
    {
        if(SceneManager.GetActiveScene().name == "Act 1 Scene 1")
        {
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

            LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => 
            {
                PlayerScript.instance.DisablePlayerScripts();

                LoadingSceneManager.instance.loadingScreen.SetActive(true);
                LoadingSceneManager.instance.enabled = true;
                LoadingSceneManager.instance.sceneName = "Act 1 Scene 2";
            });
        }
    }
    public void BussEnter()
    {
        if(SceneManager.GetActiveScene().name == "Act 1 SCene 2")
        {
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

            LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => 
            {
                PlayerScript.instance.DisablePlayerScripts();

                LoadingSceneManager.instance.loadingScreen.SetActive(true);
                LoadingSceneManager.instance.enabled = true;
                LoadingSceneManager.instance.sceneName = "Act 1 Scene 3";
            });
        }
    }

    public void Alarm()
    {
        StartCoroutine(DecreaseVolume());

        PrologueSceneManager.instance.lightSwitch.layer = 8;
        this.gameObject.layer = 0;
        MissionManager.instance.HideMission();
    }

    IEnumerator DecreaseVolume()
    {
        float startVolume = PrologueSceneManager.instance.alarmSFX.volume;

        PrologueSceneManager.instance.onAndOffGameObject.isToggling = false;
        phoneLight.SetActive(false);

        for (float t = 0; t < .5f; t += Time.deltaTime)
        {
            PrologueSceneManager.instance.alarmSFX.volume = Mathf.Lerp(startVolume, 0, t / LoadingSceneManager.instance.fadeDuration);
            yield return null;
        }
        
        PrologueSceneManager.instance.alarmSFX.volume = 0;
    }
}

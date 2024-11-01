using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;

using TMPro;
using Cinemachine;

public class Act1Scene3SceneManager : MonoBehaviour
{
    public static Act1Scene3SceneManager instance { get; private set;} 
    // Update is called once per frame
    
    void Awake()
    {
        instance = this;
        sequence =  DOTween.Sequence();

        PlayerPrefs.SetInt("Fire Station Scene", 1);
    }
    
    [Header("HUD")]
    [SerializeField] CanvasGroup sceneNameText;
    [SerializeField] Sequence sequence;
    
    [Header("Language Preference")]
    [SerializeField] InstructionSO englishInstructionSO;
    [SerializeField] InstructionSO tagalogInstructionSO;

    [Space(5)]
    [SerializeField] GameObject englishLanguage;
    [SerializeField] GameObject tagalogLanguage;

    [Space(5)]
    public int languageIndex;
    
    [Header("Dialogue Trigger")]
    [SerializeField] DialogueTrigger englishStartDialogue;
    [SerializeField] DialogueTrigger tagalogStartDialogue;
    
    [Space(5)]
    [SerializeField] DialogueTrigger englishEndDialogue;
    [SerializeField] DialogueTrigger tagalogEndDialogue;
    
    [Space(5)]
    [SerializeField] DialogueTrigger englishMonologue;
    [SerializeField] DialogueTrigger tagalogMonologue;
    


    [Header("Game Objects")]
    [SerializeField] GameObject player;
    // [SerializeField] CinemachineInputProvider;

    [Space(10)]
    [SerializeField] GameObject fireFighter;
    [SerializeField] GameObject fireTruckPeople;
    [SerializeField] GameObject fireTruck;

    [Header("Audio")]
    [SerializeField] AudioSource englishCarAmbiance;
    [SerializeField] AudioSource tagalogCarAmbiance;
    [SerializeField] AudioSource streetAmbiance;
    [SerializeField] AudioSource sceneNameRevealSFX;

    [Header("Flags")]
    [SerializeField] GameObject fireToBeExtinguish1;
    [SerializeField] GameObject fireToBeExtinguish2;
    [SerializeField] GameObject fireToBeExtinguish3;
    
    [Space(10)]
    [SerializeField] bool stopLoop;

    [Space(10)]
    [SerializeField] float fireTruckSpeed;
    [SerializeField] bool moveTowardFireTruck;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        // FADE IMAGE ALPHA SET 1
        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                                LoadingSceneManager.instance.fadeImage.color.g,
                                                                LoadingSceneManager.instance.fadeImage.color.b,
                                                                1);

        // CAR AMBIANCE - OFF
        if(SettingMenu.instance.languageDropdown.value == 0)
        {
            englishLanguage.SetActive(true);
            tagalogLanguage.SetActive(false);
            InstructionManager.instance.instructionsSO = englishInstructionSO;
            languageIndex = 0;
        }
        else
        {
            englishLanguage.SetActive(true);
            tagalogLanguage.SetActive(false);
            InstructionManager.instance.instructionsSO = tagalogInstructionSO;
            languageIndex = 1;
        }


        StartCoroutine(MonologueStart());

    }

    IEnumerator MonologueStart()
    {
        yield return new WaitForSeconds(1);

        if(languageIndex == 0)
        {
            englishMonologue.StartDialogue();
        }
        else
        {
            tagalogMonologue.StartDialogue();
        }
    }

    public void FadeOutEffect()
    {
        StartCoroutine(PreFadeOutEffect());
    }

    IEnumerator PreFadeOutEffect()
    {
        yield return new WaitForSeconds(1);
        
        sceneNameRevealSFX.Play();
        sceneNameText.DOFade(1, 1)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                sceneNameText.DOFade(0, 1)
                    .SetDelay(3)
                    .SetUpdate(true);
            });
        
        yield return new WaitForSeconds(5);

        LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
            // TRIGGER DIALOGUE
            EnableAmbiance();

            moveTowardFireTruck = false;   
        });
    }

    public void EnableAmbiance()
    {
        if (languageIndex == 0)
        {
            englishCarAmbiance.Play();
        }
        else
        {
            tagalogCarAmbiance.Play();
        }

        streetAmbiance.Play();
    }

    void Update()
    {
        if(!moveTowardFireTruck)
        {
            fireTruck.transform.position = Vector3.MoveTowards(fireTruck.transform.position, new Vector3 (fireTruck.transform.position.x, fireTruck.transform.position.y, -21), Time.deltaTime * fireTruckSpeed);

            if(fireTruck.transform.position == new Vector3(fireTruck.transform.position.x, fireTruck.transform.position.y, -21))
            {
                // FADE IN 
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
                LoadingSceneManager.instance.fadeImage.DOFade(1, 2).OnComplete(() =>
                {
                    player.SetActive(true);
                    fireFighter.SetActive(true);
                    fireTruckPeople.SetActive(false);
                    LoadingSceneManager.instance.fadeImage.DOFade(0, 2).OnComplete(() =>
                    {
                        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
                        
                        if(languageIndex == 0)
                        {
                            englishStartDialogue.StartDialogue();
                        }
                        else
                        {
                            tagalogStartDialogue.StartDialogue();
                        }
                        streetAmbiance.Play();
                    });
                });

                moveTowardFireTruck = true;                
            }
        }

        if(!stopLoop)
        {
            if(!fireToBeExtinguish1.activeSelf && !fireToBeExtinguish2.activeSelf && !fireToBeExtinguish3.activeSelf)
            {
                if (languageIndex == 0)
                {
                    englishEndDialogue.StartDialogue();
                }
                else
                {
                    tagalogEndDialogue.StartDialogue();
                }
                stopLoop = false;
            }
        }
    }

    public void EndOfScene()
    {
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                PlayerScript.instance.DisablePlayerScripts();

                LoadingSceneManager.instance.loadingScreen.SetActive(true);
                LoadingSceneManager.instance.enabled = true;
                LoadingSceneManager.instance.sceneName = "Act 1 Scene 4";
            });
    }
}

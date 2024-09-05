using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;



public class PrologueSceneManager : MonoBehaviour
{
    public static PrologueSceneManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }
 
    [Header("Script")]
    [SerializeField] HomeworkManager homeworkManager;

    [Header("Player")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerModel;
    
    [Space(10)]
    [SerializeField] AudioSource suspenceSFX;

    [Header("Prologue Game Object")]
    public GameObject PC;
    public GameObject monitor;
    public GameObject[] monitorScreen;

    [Header("Dialogue Triggers")]
    [SerializeField] DialogueTrigger startDialogueTrigger;
    
    [Space(15)]
    public AudioSource monitorSFX;
    [SerializeField] AudioSource alarmAndWakeSFX;

    [Header("Flag")]
    public bool toGetUp;
    int missionIndex;
    bool audioRepeat = false;
    bool isGamepad;
    bool isSuspenceSFXPlaying;
    bool isLastPageReached;
    

    void Start()
    {
        // TODO -   IF PAUSE UI IS ACTIVE
        //      -   AND IF STATEMENT DEVICEMANAGER

        // Cursor.lockState = CursorLockMode.Locked;

        // FADE IMAGE ALPHA SET 1
        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                                LoadingSceneManager.instance.fadeImage.color.g,
                                                                LoadingSceneManager.instance.fadeImage.color.b,
                                                                1);
        
        // PLAY AUDIO CLIP IN PLAYERAUDIO
        // alarmAndWakeSFX.Play();

        ChangeInstructionPageButtons(false, true, false); 
        
        StartCoroutine(FadeOutFadeImage());     
    }
    
    void Update()
    {
        
        // TODO - ALARM SOUND
        //      - DISPLAY TUTORIAL
        //      - 

        // if (HUDManager.instance.instructionHUD.activeSelf)
        // {
        //     DeviceChecker();
        // }

    
        // if(!isLastPageReached)
        // {
        //     LastPageChecker();
        // }

        // // TO NEXT SCENE (ACT 1 - STUDENT)
        // if(isSuspenceSFXPlaying)
        // {
        //     // ELAPSED TIME == suspenseSFX.Clip.length
        //     if(suspenceSFX.time >= suspenceSFX.clip.length)
        //     {
        //         LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

        //         LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
        //             .SetEase(Ease.Linear)
        //             .OnComplete(() =>
        //         {
        //             PlayerScript.instance.DisablePlayerScripts();

        //             LoadingSceneManager.instance.loadingScreen.SetActive(true);
        //             LoadingSceneManager.instance.enabled = true;
        //             // NEXT SCENE NAME
        //             LoadingSceneManager.instance.sceneName = "Act 1 SCene 1";
        //         });

        //         isSuspenceSFXPlaying = false;
        //     }    
        // }
    }

    IEnumerator FadeOutFadeImage()
    {
        yield return new WaitForSeconds(5);

        Debug.Log("Wait for 5 Seconds");
        LoadingSceneManager.instance.fadeImage
                .DOFade(0, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
        {
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
        });
        
        yield return new WaitForSeconds(5);
        toGetUp = true;
    }

    void DeviceChecker()
    {
        if (DeviceManager.instance.keyboardDevice)
        {
            ChangeImageStatus(true, false, HUDManager.instance.keyboardSprite[0],
                                            HUDManager.instance.keyboardSprite[1],
                                            HUDManager.instance.keyboardSprite[2]);

            EventSystem.current.SetSelectedGameObject(null);

            isGamepad = false;
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            ChangeImageStatus(false, true, HUDManager.instance.gamepadSprite[0],
                                            HUDManager.instance.gamepadSprite[1],
                                            HUDManager.instance.gamepadSprite[2]);

            if (!isGamepad)
            {
                if(HUDManager.instance.instructionButton[1].activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(HUDManager.instance.instructionButton[1]);
                }   
                else
                {
                    EventSystem.current.SetSelectedGameObject(HUDManager.instance.instructionButton[2]);
                }

                isGamepad = true;
            }
        }
    }

    void LastPageChecker()
    {
        if (HUDManager.instance.instructionPage[2].activeSelf)
        {
            isLastPageReached = true;
        }
    }

    public void TransitionToHomeworkQuiz()
    {
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

        // FADEIN EFFECTS
        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .OnComplete(() =>
        {       
            HUDManager.instance.homeworkHUD.SetActive(true);
            // FADEOUT EFFECTS
            LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
                .OnComplete(() =>
            {
                homeworkManager.enabled = true;
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
                
            });
        });
    }

    // #region - INSTRUCTION -

    [ContextMenu("Display")]
    public void DisplayInstruction()
    {
        Time.timeScale = 0;
        HUDManager.instance.instructionHUD.SetActive(true);
            
        Debug.Log("Display Instruction");
        HUDManager.instance.instructionBGRectTransform
            .DOSizeDelta(new Vector2(1920, HUDManager.instance.instructionBGRectTransform.sizeDelta.y), .5f)
            .SetEase(Ease.InQuad)
            .SetUpdate(true)
            .OnComplete(() =>
        {
            HUDManager.instance.instructionContent.SetActive(true);
            HUDManager.instance.instructionContentCG
                .DOFade(1, .75f)
                .SetUpdate(true);
        });
    }
    [ContextMenu("Display1")]
    public void HideInstruction()
    {
        Time.timeScale = 1;
        HUDManager.instance.instructionContentCG
            .DOFade(1, .75f).OnComplete(() =>
        {
            HUDManager.instance.instructionContent.SetActive(false);
            HUDManager.instance.instructionBGRectTransform
                .DOSizeDelta(new Vector2(0, HUDManager.instance.instructionBGRectTransform.sizeDelta.y), .5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
            {
                HUDManager.instance.instructionHUD.SetActive(false);

                //ENABLE SCRIPT
                PlayerScript.instance.playerMovement.enabled = true;
                // PlayerScript.instance.playerMovement.playerAnim.enabled = true;
                PlayerScript.instance.cinemachineInputProvider.enabled = true;
                PlayerScript.instance.interact.enabled = true;
                // PlayerScript.instance.examine.enabled = true;

                // Cursor.lockState = CursorLockMode.Locked;

                HUDManager.instance.playerHUD.SetActive(true);
                MissionManager.instance.DisplayMission();
            });
        });
    }

    public void instructionNextPage()
    {
        if(HUDManager.instance.instructionPage[0].activeSelf)
        {
            if(isLastPageReached)
            {
                ChangeInstructionPageButtons(true, true, true);
            }
            else
            {
                ChangeInstructionPageButtons(true, true, false);
            }

            HUDManager.instance.instructionPage[0].SetActive(false);
            HUDManager.instance.instructionPage[1].SetActive(true);

            EventSystem.current.SetSelectedGameObject(HUDManager.instance.instructionButton[1]);
        }
        else if (HUDManager.instance.instructionPage[1].activeSelf)
        {
            // TODO -   DOUBLE CHECK. IF NEED TO DO A LASTPAGEREACHED IF STATEMENT 

            ChangeInstructionPageButtons(true,  false, true);
        
            HUDManager.instance.instructionPage[1].SetActive(false);
            HUDManager.instance.instructionPage[2].SetActive(true);

            EventSystem.current.SetSelectedGameObject(HUDManager.instance.instructionButton[2]);    
        }
        else if (HUDManager.instance.instructionPage[2].activeSelf)
        {

        }
    }

    public void instructionPreviousPage()
    {
        if (HUDManager.instance.instructionPage[0].activeSelf)
        {
            
        }
        else if (HUDManager.instance.instructionPage[1].activeSelf)
        {
            if(isLastPageReached)
            {
                ChangeInstructionPageButtons(false, true, true);
            }
            else
            {
                ChangeInstructionPageButtons(false, true, false);
            }

            HUDManager.instance.instructionPage[0].SetActive(true);
            HUDManager.instance.instructionPage[1].SetActive(false);

            EventSystem.current.SetSelectedGameObject(HUDManager.instance.instructionButton[1]);
        }
        else if (HUDManager.instance.instructionPage[3].activeSelf)
        {

            ChangeInstructionPageButtons(true, true, true);

            HUDManager.instance.instructionPage[1].SetActive(true);
            HUDManager.instance.instructionPage[2].SetActive(false);

            EventSystem.current.SetSelectedGameObject(HUDManager.instance.instructionButton[0]);
        }
    }

    void ChangeInstructionPageButtons(bool leftButton, bool rightButton, bool doneButton)
    {
        HUDManager.instance.instructionButton[0].SetActive(leftButton);
        HUDManager.instance.instructionButton[1].SetActive(rightButton);
        HUDManager.instance.instructionButton[2].SetActive(doneButton);
    }

    void ChangeImageStatus(bool keyboardActive, bool gamepadActive, Sprite crouchSprite,
                        Sprite interactSprite, Sprite examineSprite)
    {
        HUDManager.instance.keyboardInstruction.SetActive(keyboardActive);
        HUDManager.instance.gamepadInstruction.SetActive(gamepadActive);

        HUDManager.instance.imageHUD[0].sprite = crouchSprite;
        HUDManager.instance.imageHUD[1].sprite = interactSprite;
        HUDManager.instance.imageHUD[2].sprite = examineSprite;
    }
    
    #region - SCENE MANAGEMENT -

    public void RotatePlayer()
    {
        player.transform.rotation = Quaternion.Euler(0, 120,0);
    }

    public void MovePlayer()
    {
        player.transform.position = new Vector3(-6.5f, player.transform.position.y, -11);
        playerModel.transform.position = new Vector3(0,0,0);
    }

    public void StartSuspenceSequence()
    {
        //TODO  -   CHANGE ALL LAYER OF EXAMINABLE GAMEOBJECT TO DEFAULT (LAYER 0)
        //      -   START SUSPENCE SOUND
        //      -   START VIGNETTE

        suspenceSFX.Play();
        isSuspenceSFXPlaying = true; 
    }

    #endregion
}

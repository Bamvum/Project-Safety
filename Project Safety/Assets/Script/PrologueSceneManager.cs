using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class PrologueSceneManager : MonoBehaviour
{
    [SerializeField] MissionSO missionSO;

    [Header("Player")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerModel;

    [Header("Prologue Game Object")]
    public GameObject PC;
    public GameObject monitor;
    public GameObject[] monitorScreen;

    [Header("Dialogue Triggers")]
    [SerializeField] DialogueTrigger startDialogueTrigger;
    
    [Space(15)]
    [SerializeField] AudioSource playerAudio;
    [SerializeField] AudioClip alarmAndWakeSFX;

    [Header("Flag")]
    int missionIndex;
    bool audioRepeat = false;
    bool isGamepad;
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
        playerAudio.clip = alarmAndWakeSFX;
        playerAudio.Play();

        ChangeInstructionPageButtons(false, true, false);        
    }
    
    void Update()
    {
        
        if(!audioRepeat)
        {
            CheckPlayerAudioPlaying();
        }

        if (HUDManager.instance.instructionHUD.activeSelf)
        {
            DeviceChecker();
        }
        // else
        // {
        //     // Cursor.lockState = CursorLockMode.Locked;
        // }
    
        if(!isLastPageReached)
        {
            LastPageChecker();
        }
    }

    void CheckPlayerAudioPlaying()
    {
        if (!playerAudio.isPlaying)
        {
            Debug.Log("Player Audio is Done playing");

            // FADEOUT EFFECTS
            LoadingSceneManager.instance.fadeImage
                .DOFade(0, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
            {
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);

                audioRepeat = true;
                startDialogueTrigger.StartDialogue();
            });
        }
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
        // ScriptManager.instance.transitionManager.transitionImage.DOFade(1, 1f).OnComplete(() =>
        // {
        //     ScriptManager.instance.transitionManager.transitionImage.DOFade(1, 1f).OnComplete(() =>
        //     {
        //         // DISPLAY HOMEWORK HUD
        //         HUDManager.instance.homeworkHUD.SetActive(true);
        //         ScriptManager.instance.transitionManager.transitionImage.DOFade(0, 1f);
                
        //     });
        // });

        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

        // FADEIN EFFECTS
        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .OnComplete(() =>
        {
            HUDManager.instance.homeworkHUD.SetActive(true);
            
            // FADEOUT EFFECTS
            LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration).OnComplete(() =>
            {
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
            });
        });
    }

    #region - MISSION -

    public void DisplayMission()
    {
        HUDManager.instance.missionText.text = missionSO.missions[missionIndex];
        PlayerScript.instance.missionSFX.Play();

        HUDManager.instance.missionCG.DOFade(1, 1);
        HUDManager.instance.missionRectTransform
            .DOAnchorPos(new Vector2(225.5f, HUDManager.instance.missionRectTransform.anchoredPosition.y), 1);
        
        if(missionIndex == 1)
        {
            // PC.layer = 8;
        }
        else
        {
            // PC.layer = 0;
        }

        // missionText.text = ScriptManager.instance.mission.missionSO.missions[missionIndex];
        // missionSFX.Play();

        // missionCG.DOFade(1, 1f); 
        // missionRT.DOAnchorPos(new Vector2(225.5f, missionRT.anchoredPosition.y), 1);

        // if(missionIndex == 1)
        // {
        //     PC.layer = 8;
            
        // }
        // else
        // {
        //     PC.layer = 0;
        // }
    }

    public void HideMission()
    {
        HUDManager.instance.missionRectTransform
            .DOAnchorPos(new Vector2(-325, HUDManager.instance.missionRectTransform.anchoredPosition.y), 1)
            .OnComplete(() =>
        {
            HUDManager.instance.missionRectTransform
                .DOAnchorPos(new Vector2(-325, HUDManager.instance.missionRectTransform.anchoredPosition.y), .5f)
                .OnComplete(() =>
            {
                if(missionIndex < missionSO.missions.Length - 1)
                {
                    missionIndex++;
                }
                DisplayMission();
            });
        });
    }

    #endregion

    #region - INSTRUCTION -

     public void DisplayInstruction()
    {
        Time.timeScale = 0;
        HUDManager.instance.instructionHUD.SetActive(true);

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
                DisplayMission();
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
        else if (HUDManager.instance.instructionPage[2].activeSelf)
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

    #endregion

    void ChangeImageStatus(bool keyboardActive, bool gamepadActive, Sprite crouchSprite,
                        Sprite interactSprite, Sprite examineSprite)
    {
        HUDManager.instance.keyboardInstruction.SetActive(keyboardActive);
        HUDManager.instance.gamepadInstruction.SetActive(gamepadActive);

        HUDManager.instance.imageHUD[0].sprite = crouchSprite;
        HUDManager.instance.imageHUD[1].sprite = interactSprite;
        HUDManager.instance.imageHUD[2].sprite = examineSprite;
    }
    

    public void RotatePlayer()
    {
        // player.transform.rotation = Quaternion.Euler(0, 120,0);
    }

    public void MovePlayer()
    {
        // player.transform.position = new Vector3(-6.5f, player.transform.position.y, -11);
        // playerModel.transform.position = new Vector3(0,0,0);
    }

    public void StartSuspenceSequence()
    {
        //TODO  -   CHANGE ALL LAYER OF EXAMINABLE GAMEOBJECT TO DEFAULT (LAYER 0)
        //      -   START SUSPENCE SOUND
        //      -   START SUSPENCE SOUND

    }
}

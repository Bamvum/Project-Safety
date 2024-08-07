using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Cinemachine;
using Synty.AnimationBaseLocomotion.Samples;
using UnityEngine.iOS;


public class PrologueSceneManager : MonoBehaviour
{
    [Header("Script")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] TransitionManager transitionManager;
    [SerializeField] Mission mission;

    [Header("Prologue Game Object")]
    public GameObject PC;
    public GameObject monitor;
    public GameObject[] monitorScreen;

    [Header("Instruction HUD")]
    [SerializeField] GameObject instructionHUD;
    [SerializeField] GameObject instructionHUDContent;
    CanvasGroup instructionHUDContentCG;
    [SerializeField] Image instructionBG;
    RectTransform instructionBGRT;

    [Space(10)]
    [SerializeField] GameObject keyboardInstruction;
    [SerializeField] GameObject gamepadInstruction;
    [Space(10)]
    [SerializeField] GameObject[] instructionHUDPage;
    [Space(10)]
    [SerializeField] GameObject instructionButtonLeft;
    [SerializeField] GameObject instructionButtonRight;
    [SerializeField] GameObject instructionButtonDone;
    [Space(10)]
    [SerializeField] Image[] imageHUD;
    [SerializeField] Sprite[] keyboardSprite;
    [SerializeField] Sprite[] gamepadSprite;
    
    [Header("Homework HUD")]
    [SerializeField] GameObject homeworkHUD;


    [Header("Mission HUD")]
    [SerializeField] TMP_Text missionText;
    [SerializeField] RectTransform missionRT;
    [SerializeField] CanvasGroup missionCG;
    int missionIndex;

    [Header("SFX")]
    [SerializeField] AudioSource missionSFX;
    public AudioSource monitorSFX;

    [Header("Flags")]
    public bool interactedLightSwitch;
    bool isGamepad;
    bool isLastPageReached;

    void Start()
    {
        instructionBGRT = instructionBG.GetComponent<RectTransform>();
        instructionHUDContentCG = instructionHUDContent.GetComponent<CanvasGroup>();

        instructionBGRT.sizeDelta = new Vector2(0, instructionBGRT.sizeDelta.y); 
        instructionHUDContentCG.alpha = 0;
        
        missionCG.alpha = 0f;
        missionRT.anchoredPosition = new Vector2(-325, missionRT.anchoredPosition.y);
        
        ChangeInstructionPageButtons(false, true, false);
    }

    void Update()
    {
       if(instructionHUD.activeSelf)
       {
            if (DeviceManager.instance.keyboardDevice)
            {
                ChangeImageStatus(true, false, keyboardSprite[0], keyboardSprite[1], keyboardSprite[2]);
                EventSystem.current.SetSelectedGameObject(null);
                isGamepad = false;
            }
            else if (DeviceManager.instance.gamepadDevice)
            {   
                ChangeImageStatus(false, true, gamepadSprite[0], gamepadSprite[1], gamepadSprite[2]);
                
                if(!isGamepad)
                {
                    if (instructionButtonRight.activeSelf)
                    {
                        EventSystem.current.SetSelectedGameObject(instructionButtonRight);
                    }
                    else
                    {
                        EventSystem.current.SetSelectedGameObject(instructionButtonDone);
                    }

                    isGamepad = true;
                }
            }
       }

       if(!isLastPageReached)
       {
            if(instructionHUDPage[2].activeSelf)
            {
                isLastPageReached = true;
            }
       }
    }
    


    public void TransitionToHomeworkQuiz()
    {
        transitionManager.transitionImage.DOFade(1, 1f).OnComplete(() =>
        {
            transitionManager.transitionImage.DOFade(1, 1f).OnComplete(() =>
            {
                // DISPLAY HOMEWORK HUD
                homeworkHUD.SetActive(true);
                transitionManager.transitionImage.DOFade(0, 1f);
                
            });
        });
    }

    #region - MISSION -

    public void DisplayMission()
    {
        missionText.text = mission.missionSO.missions[missionIndex];
        missionSFX.Play();

        missionCG.DOFade(1, 1f); 
        missionRT.DOAnchorPos(new Vector2(325, missionRT.anchoredPosition.y), 1);

        if(missionIndex == 1)
        {
            PC.layer = 8;
            
        }
        else
        {
            PC.layer = 0;
        }
    }

    public void HideMission()
    {
        missionRT.DOAnchorPos(new Vector2(-325, missionRT.anchoredPosition.y), 1f).OnComplete(() =>
        {
            if (missionIndex < mission.missionSO.missions.Length - 1)
            {
                missionIndex++;
            }
            DisplayMission();
        });
    }

    #endregion

    #region - INSTRUCTION -

     public void DisplayInstruction()
    {
        instructionHUD.SetActive(true);
        instructionBGRT.DOSizeDelta(new Vector2(1920, instructionBGRT.sizeDelta.y), .5f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            instructionHUDContent.SetActive(true);
            instructionHUDContentCG.DOFade(1, .75f);
            // Change to CanvasGroup
        });
    } 

    public void HideInstruction()
    {
        instructionHUDContentCG.DOFade(1, .75f).OnComplete(() =>
        {
            instructionHUDContent.SetActive(false);
            instructionBGRT.DOSizeDelta(new Vector2(0, instructionBGRT.sizeDelta.y), .5f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                instructionHUD.SetActive(false);

                PlayerManager.instance.playerMovement.enabled = true;
                PlayerManager.instance.playerMovement.playerAnim.enabled = true;
                PlayerManager.instance.interact.enabled = true;
                PlayerManager.instance.cinemachineInputProvider.enabled = true;

                // playerMovement.playerHUD.SetActive(true);
                HUDManager.instance.playerHUD.SetActive(true);

                DisplayMission();
            });
        });
    }

    public void instructionNextPage()
    {
        if(instructionHUDPage[0].activeSelf)
        {
            if(isLastPageReached)
            {
                ChangeInstructionPageButtons(true, true, true);
            }
            else
            {
                ChangeInstructionPageButtons(true, true, false);
            }
            
            instructionHUDPage[0].SetActive(false);
            instructionHUDPage[1].SetActive(true);

            EventSystem.current.SetSelectedGameObject(instructionButtonRight);
        }
        else if(instructionHUDPage[1].activeSelf)
        {
            ChangeInstructionPageButtons(true, false, true);

            instructionHUDPage[1].SetActive(false);
            instructionHUDPage[2].SetActive(true);
            EventSystem.current.SetSelectedGameObject(instructionButtonDone);
        }
        else if(instructionHUDPage[2].activeSelf)
        {
           
        }
    }

    public void instructionPreviousPage()
    {
        if(instructionHUDPage[0].activeSelf)
        {
            
        }
        else if(instructionHUDPage[1].activeSelf)
        {
            if(isLastPageReached)
            {
                ChangeInstructionPageButtons(false, true, true);
            }
            else
            {
                ChangeInstructionPageButtons(false, true, false);
            }
            instructionHUDPage[0].SetActive(true);
            instructionHUDPage[1].SetActive(false);

            EventSystem.current.SetSelectedGameObject(instructionButtonRight);
        }
        else if(instructionHUDPage[2].activeSelf)
        {
            if (isLastPageReached)
            {
                ChangeInstructionPageButtons(true, false, true);
            }
            else
            {
                ChangeInstructionPageButtons(true, false, false);
            }
            instructionHUDPage[1].SetActive(true);
            instructionHUDPage[2].SetActive(false);

            EventSystem.current.SetSelectedGameObject(instructionButtonLeft);
        }
    }

    void ChangeInstructionPageButtons(bool leftButton, bool rightButton, bool doneButton)
    {
        instructionButtonLeft.SetActive(leftButton);
        instructionButtonRight.SetActive(rightButton);
        instructionButtonDone.SetActive(doneButton);
    }

    #endregion

    void ChangeImageStatus(bool keyboardActive, bool gamepadActive, Sprite crouchSprite,
                        Sprite interactSprite, Sprite examineSprite)
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            Cursor.lockState = CursorLockMode.None; 
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            Cursor.lockState = CursorLockMode.Locked; 
        }
        
        keyboardInstruction.SetActive(keyboardActive);
        gamepadInstruction.SetActive(gamepadActive);
        imageHUD[0].sprite = crouchSprite;
        imageHUD[1].sprite = interactSprite;
        imageHUD[2].sprite = examineSprite;
    }
    
}

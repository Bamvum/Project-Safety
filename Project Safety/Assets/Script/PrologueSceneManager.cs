using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class PrologueSceneManager : MonoBehaviour
{
    [Header("Script")]
    [SerializeField] TransitionManager transitionManager;
    [SerializeField] Mission mission;

    [Header("Instruction HUD")]
    [SerializeField] GameObject instructionHUD;
    [Space(10)]
    [SerializeField] GameObject keyboardInstruction;
    [SerializeField] GameObject gamepadInstruction;
    [Space(10)]
    [SerializeField] GameObject[] instructionHUDPage;
    [SerializeField] Button[] instructionButton;
    [Space(10)]
    [SerializeField] Image[] imageHUD;
    [SerializeField] Sprite[] keyboardSprite;
    [SerializeField] Sprite[] gamepadSprite;

    [Header("Mission HUD")]
    [SerializeField] TMP_Text missionText;
    [SerializeField] RectTransform missionRT;
    [SerializeField] CanvasGroup missionCG;
    int missionIndex;

    [Header("SFX")]
    [SerializeField] AudioSource missionSFX;

    [Header("Flags")]
    public bool interactedLightSwitch;



    void Start()
    {
        missionCG.alpha = 0f;
        missionRT.anchoredPosition = new Vector2(-325, missionRT.anchoredPosition.y);
    }

    void Update()
    {
        if(instructionHUD.activeSelf)
        {
            if (DeviceManager.instance.keyboardDevice)
            {
                ChangeImageStatus(true, false, keyboardSprite[0], keyboardSprite[1], keyboardSprite[2]);
                EventSystem.current.SetSelectedGameObject(null);
            }
            else if (DeviceManager.instance.gamepadDevice)
            {
                ChangeImageStatus(false, true, gamepadSprite[0], gamepadSprite[1], gamepadSprite[2]);

                if (instructionHUDPage[0].activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(instructionButton[0].gameObject);
                }
                else if (instructionHUDPage[1].activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(instructionButton[1].gameObject);
                }
                else if (instructionHUDPage[2].activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(instructionButton[2].gameObject);
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }
    }

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
    
    public void DisplayMission()
    {
        missionText.text = mission.missionSO.missions[missionIndex];
        missionSFX.Play();

        missionCG.DOFade(1, 1f);
        missionRT.DOAnchorPos(new Vector2(325, missionRT.anchoredPosition.y), 1);
    }

    public void HideMission()
    {
        // missionCG.DOFade(0, 1f).OnComplete(() =>
        // {
        //     if(missionIndex < mission.missionSO.missions.Length - 1)
        //     {
        //         missionIndex++;
        //     }
        //     DisplayMission();
        // });

        missionRT.DOAnchorPos(new Vector2(-325, missionRT.anchoredPosition.y), 1f).OnComplete(() =>
        {
            if (missionIndex < mission.missionSO.missions.Length - 1)
            {
                missionIndex++;
            }
            DisplayMission();
        });
    }
}

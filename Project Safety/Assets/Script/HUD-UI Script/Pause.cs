using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public static Pause instance {get; private set;}

    void Awake()
    {
        playerControls = new PlayerControls();
        instance = this;
    }

    PlayerControls playerControls;

    [Header("HUD")]
    
    [Space(5)]
    public RectTransform pauseHUDRectTransform;
    [SerializeField] CanvasGroup pauseButtonCG;
    [SerializeField] TMP_Text settingNavGuide;

    [Space(5)]
    [SerializeField] TMP_Text currentMissionText;
    

    [Space(5)]
    [SerializeField] RectTransform settingRectTransform;
    [SerializeField] CanvasGroup settingButtonCG;

    [Space(5)]
    [SerializeField] RectTransform audioSettingRectTransform;
    [SerializeField] RectTransform graphicsSettingRectTransform;
    [SerializeField] RectTransform controlsSettingRectTransform;
    [SerializeField] RectTransform languageSettingRectTransform;

    [Header("Selected Button")]
    [SerializeField] GameObject lastSelectedButton; // FOR GAMEPAD
    
    [Space(5)]
    [SerializeField] GameObject pauseSelectedButton; 
    [SerializeField] GameObject settingSelectedButton;

    [Space(5)]
    [SerializeField] GameObject audioSettingSelectedButton;
    [SerializeField] GameObject graphicsSettingSelectedButton;
    [SerializeField] GameObject controlsSettingSelectedButton;
    [SerializeField] GameObject languageSettingSelectedButton;
    
    [Header("Audio")]
    [SerializeField] AudioSource bgm;

    [Header("Flag")]
    [SerializeField] bool canInput;
    public bool isPause;
    public bool isGamepad;

    void Start()
    {
        pauseHUDRectTransform.gameObject.SetActive(false);
        pauseHUDRectTransform.localScale = Vector3.zero;
        pauseButtonCG.interactable = false;

        settingRectTransform.gameObject.SetActive(false);
        settingButtonCG.interactable = false;
    }

    void OnEnable()
    {
        playerControls.Pause.Action.performed += ToPause;

        playerControls.Pause.NextCategory.performed += SettingNextCategoty;
        playerControls.Pause.PreviousCategory.performed += SettingPreviousCategoty;

        playerControls.Pause.Back.performed += ToBack;

        playerControls.Pause.Enable();
    }

    #region - SETTING NEXT CATEGORY -
    private void SettingNextCategoty(InputAction.CallbackContext context)
    {
        if(settingRectTransform.gameObject.activeSelf)
        {
            if (audioSettingRectTransform.gameObject.activeSelf)
            {
                audioSettingRectTransform.gameObject.SetActive(false);
                graphicsSettingRectTransform.gameObject.SetActive(true);
            }
            else if (graphicsSettingRectTransform.gameObject.activeSelf)
            {
                graphicsSettingRectTransform.gameObject.SetActive(false);
                controlsSettingRectTransform.gameObject.SetActive(true);
            }
            else if (controlsSettingRectTransform.gameObject.activeSelf)
            {
                controlsSettingRectTransform.gameObject.SetActive(false);
                languageSettingRectTransform.gameObject.SetActive(true);
            }
            else if (languageSettingRectTransform.gameObject.activeSelf)
            {
                Debug.Log("Can't Next Category");
            }
        
            isGamepad = false;
        }
    }

    #endregion

    #region - SETTING PREVIOUS CATEGORY - 

    private void SettingPreviousCategoty(InputAction.CallbackContext context)
    {
        if (settingRectTransform.gameObject.activeSelf)
        {
            if (audioSettingRectTransform.gameObject.activeSelf)
            {
                Debug.Log("Can't previous Category");
            }
            else if (graphicsSettingRectTransform.gameObject.activeSelf)
            {
                audioSettingRectTransform.gameObject.SetActive(true);
                graphicsSettingRectTransform.gameObject.SetActive(false);
            }
            else if (controlsSettingRectTransform.gameObject.activeSelf)
            {
                graphicsSettingRectTransform.gameObject.SetActive(true);
                controlsSettingRectTransform.gameObject.SetActive(false);
            }
            else if (languageSettingRectTransform.gameObject.activeSelf)
            {
                controlsSettingRectTransform.gameObject.SetActive(true);
                languageSettingRectTransform.gameObject.SetActive(false);
            }
            
            isGamepad = false;
        }
    }

    #endregion

    #region - TO BACK - 

    private void ToBack(InputAction.CallbackContext context)
    {
        if(settingRectTransform.gameObject.activeSelf)
        {
            SettingBack();
        }
    }

    #endregion

    #region - TO PAUSE - 

    private void ToPause(InputAction.CallbackContext context)
    {
        if(!isPause)
        {
            if(canInput)
            {
                if (SceneManager.GetActiveScene().name == "Act 2 Scene 2")
                {
                    if (!LoadingSceneManager.instance.fadeImage.gameObject.activeSelf && !HUDManager.instance.dialogueHUD.activeSelf  && !HUDManager.instance.gameOverHUD.activeSelf)
                    {
                        ShowPause();
                    }
                }
                else if (SceneManager.GetActiveScene().name == "Prologue")
                {
                    if (!LoadingSceneManager.instance.fadeImage.gameObject.activeSelf && !HUDManager.instance.dialogueHUD.activeSelf && !HUDManager.instance.dialogueHUD.activeSelf && !HUDManager.instance.examineHUD.activeSelf
                        && !HomeworkManager.instance.homeworkHUD.activeSelf)
                    {
                        ShowPause();
                    }
                }
                else
                {
                    if (!LoadingSceneManager.instance.fadeImage.gameObject.activeSelf && !HUDManager.instance.dialogueHUD.activeSelf && !HUDManager.instance.dialogueHUD.activeSelf && !HUDManager.instance.examineHUD.activeSelf)
                    {
                        ShowPause();
                    }
                }
            }

        }
        else 
        {
            if(!settingRectTransform.gameObject.activeSelf)
            {
                HidePause();
            }
        }
    }

    void ShowPause()
    {
        Time.timeScale = 0;

        if (MissionManager.instance.missionSO != null)
        {
            currentMissionText.text = MissionManager.instance.missionSO.missions[MissionManager.instance.missionIndex];
        }
        
        pauseHUDRectTransform.gameObject.SetActive(true);
        pauseButtonCG.interactable = false;

        if(bgm != null)
        {
            bgm.DOFade(.25f, 1);
        }
        
        pauseHUDRectTransform.DOScale(Vector3.one, .5f).SetEase(Ease.OutBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                pauseButtonCG.interactable = true;
                isGamepad = false;
                isPause = true;
            });
    }
    
    public void HidePause()
    {
        if (bgm != null)
        {
            bgm.DOFade(1, 1);
        }

        pauseButtonCG.interactable = false;
        pauseHUDRectTransform.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                pauseHUDRectTransform.gameObject.SetActive(false);
                Time.timeScale = 1;
                isGamepad = false;
                isPause = false;
                Cursor.lockState = CursorLockMode.Locked;
            });
    }

    #endregion

    void OnDisable()
    {
        playerControls.Pause.Disable();
    }

    void Update()
    {
        // CursorChecker();
        DeviceInputCheckerUI();
        DeviceInputCheckerNavGuide();

        // GAMEPAD VIBRATION ON NAVIGATION 
        if (Gamepad.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            if (Gamepad.current.leftStick.ReadValue() != Vector2.zero || Gamepad.current.dpad.ReadValue() != Vector2.zero)
            {
                GameObject currentSelectedButton = EventSystem.current.currentSelectedGameObject;

                Debug.Log("Inputed Leftstick and Dpad");
                // Check if the selected UI element has changed (button navigation)
                if (currentSelectedButton != lastSelectedButton)
                {
                    // Trigger vibration when navigating to a new button
                    VibrateGamepad();
                    lastSelectedButton = currentSelectedButton; // Update the last selected button
                }
            }
        }
    }

    private void VibrateGamepad()
    {
        // Set a short vibration
        Gamepad.current.SetMotorSpeeds(0.3f, 0.3f); // Adjust the intensity here
        Invoke("StopVibration", 0.1f); // Stops vibration after 0.1 seconds
        StartCoroutine(StopVibration(.1f));
    }


    IEnumerator StopVibration(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Gamepad.current.SetMotorSpeeds(0, 0);
    }


    #region - DEVICE INPUT CHECKER [HUD/UI]

    void DeviceInputCheckerUI()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            if(pauseHUDRectTransform.gameObject.activeSelf || settingRectTransform.gameObject.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                EventSystem.current.SetSelectedGameObject(null);
                isGamepad = false;
            }

        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            Cursor.lockState = CursorLockMode.Locked;

            if(!isGamepad)
            {
                if(pauseHUDRectTransform.gameObject.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(pauseSelectedButton);
                    isGamepad = true;
                }

                if (settingRectTransform.gameObject.activeSelf)
                {
                    if(audioSettingRectTransform.gameObject.activeSelf)
                    {
                        EventSystem.current.SetSelectedGameObject(audioSettingSelectedButton);
                        isGamepad = true;
                    }

                    if(graphicsSettingRectTransform.gameObject.activeSelf)
                    {
                        EventSystem.current.SetSelectedGameObject(graphicsSettingSelectedButton);
                        isGamepad = true;
                    }

                    if (controlsSettingRectTransform.gameObject.activeSelf)
                    {
                        EventSystem.current.SetSelectedGameObject(controlsSettingSelectedButton);
                        isGamepad = true;
                    }

                    if (languageSettingRectTransform.gameObject.activeSelf)
                    {
                        EventSystem.current.SetSelectedGameObject(languageSettingSelectedButton);
                        isGamepad = true;
                    }
                }
                
            }
        }
    }

    #endregion

    #region - NAVIGATION GUIDE -

    void DeviceInputCheckerNavGuide()
    {
        if(DeviceManager.instance.keyboardDevice)
        {   
            if(settingRectTransform.gameObject.activeSelf)
            {
                settingNavGuide.text = "<sprite name=\"Q\"> <sprite name=\"E\">  Switch Category   <sprite name=\"Escape\"> Back   ";
            }
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            if(settingRectTransform.gameObject.activeSelf)
            {
                settingNavGuide.text = "<sprite name=\"Left Shoulder\"> <sprite name=\"Right Shoulder\">  Switch Category   <sprite name=\"Circle\"> Back   ";
            }
        }
    }

    #endregion


    public void DisplaySetting()
    {
        settingRectTransform.sizeDelta = new Vector2(0, 1080);

        pauseButtonCG.interactable = false;
        isGamepad = true;

        settingRectTransform.gameObject.SetActive(true);
        settingRectTransform.DOSizeDelta(new Vector2(1920, settingRectTransform.sizeDelta.y), .25f)
            .SetEase(Ease.InFlash)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                settingButtonCG.gameObject.SetActive(true);
                settingButtonCG.DOFade(1, .25f).SetUpdate(true).OnComplete(() =>
                {
                    settingButtonCG.interactable = true;
                    isGamepad = false;
                });
            });
    }

    public void SettingBack()
    {
        settingButtonCG.DOFade(0, .25f).SetUpdate(true).OnComplete(() =>
        {
            settingButtonCG.gameObject.SetActive(false);
            settingButtonCG.interactable = false;
            isGamepad = true;

            settingRectTransform.DOSizeDelta(new Vector2(settingRectTransform.sizeDelta.x, 0), .25f)
                .SetEase(Ease.OutFlash)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    settingRectTransform.gameObject.SetActive(false);
                    pauseButtonCG.interactable = true;
                    isGamepad = false;
                });
        });
    }
    
    #region - MAIN MENU -

    public void GoToMainMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseButtonCG.interactable = false;
        
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                PlayerScript.instance.DisablePlayerScripts();

                LoadingSceneManager.instance.loadingScreen.SetActive(true);
                LoadingSceneManager.instance.enabled = true;

                LoadingSceneManager.instance.sceneName = "Main Menu";
                Time.timeScale = 1;
            });
    }

    #endregion

    #region - CAN INPUT -

    public void PauseCanInput(bool status)
    {
        canInput = status;
    }

    #endregion
}

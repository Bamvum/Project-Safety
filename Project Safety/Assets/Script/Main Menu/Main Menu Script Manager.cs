using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;
using System;
using Unity.VisualScripting;


public class MainMenuScriptManager : MonoBehaviour
{   
    PlayerControls playerControls;

    [Header("Title Screen")]
    [SerializeField] TMP_Text pingPongText;
    [SerializeField] float pingPongSpeed;

    [Header("HUD")]
    [SerializeField] RectTransform mainMenuHUDRectTransform;
    [SerializeField] CanvasGroup mainMenuButtonCG;
    
    [Space(10)]
    [SerializeField] RectTransform selectSceneRectTransform;
    [SerializeField] CanvasGroup selectSceneButtonCG;
    [SerializeField] TMP_Text selectSceneNavGuide;

    [Space(10)]
    [SerializeField] RectTransform settingRectTransform;
    [SerializeField] CanvasGroup settingButtonCG;
    [SerializeField] TMP_Text settingNavGuide;

    [Space(5)]
    [SerializeField] RectTransform audioSettingRectTransform;
    [SerializeField] RectTransform graphicsSettingRectTransform;
    [SerializeField] RectTransform controlsSettingRectTransform;
    [SerializeField] RectTransform languageSettingRectTransform;

    [Space(10)]
    [SerializeField] RectTransform achievementRectTransform;
    [SerializeField] CanvasGroup achievementButtonCG;
    [SerializeField] TMP_Text achievementNavGuide;

    [Space(5)]
    [SerializeField] RectTransform[] achievementRectTransformPage;


    [Header("Set Selected Game Object")]
    [SerializeField] GameObject lastSelectedButton; // FOR GAMEPAD
    
    [Space(5)]
    [SerializeField] GameObject mainMenuSelectedButton; 
    [SerializeField] GameObject selectSceneSelectedButton; 
    [SerializeField] GameObject settingSelectedButton;

    [Space(5)]
    [SerializeField] GameObject audioSettingSelectedButton;
    [SerializeField] GameObject graphicsSettingSelectedButton;
    [SerializeField] GameObject controlsSettingSelectedButton;
    [SerializeField] GameObject languageSettingSelectedButton;

    [Space(5)]
    [SerializeField] GameObject achievementSelectedButton;

    [Header("Audio")]
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource sfxButtom;
    
    bool isGamepad;

    bool actionInput;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.MainMenu.Action.performed += ctx => actionInput = true; 
        playerControls.MainMenu.Action.canceled += ctx => actionInput = false; 

        playerControls.MainMenu.PreviousCategory.performed += SettingPreviousCategory;
        playerControls.MainMenu.NextCategory.performed += SettingNextCategory;

        playerControls.MainMenu.Back.performed += ToBack;

        playerControls.MainMenu.Enable();
    }

    #region - SETTING CATEGORY NAVIGATION -

    #region - SETTING PREVIOUS CATEGORY -

    private void SettingPreviousCategory(InputAction.CallbackContext context)
    {
        if(settingRectTransform.gameObject.activeSelf)
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

    #region - SETTING NEXT CATEGORY -

    private void SettingNextCategory(InputAction.CallbackContext context)
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
    #endregion

    #region - RETURN OR BACK -

    private void ToBack(InputAction.CallbackContext context)
    {
        if(selectSceneRectTransform.gameObject.activeSelf)
        {
            selectSceneBack();
        }
        else if(settingRectTransform.gameObject.activeSelf)
        {
            SettingBack();
        }
        else if (achievementRectTransform.gameObject.activeSelf)
        {
            AchievementBack();
        }

    }
    
    #endregion

    void OnDisable()
    {
        playerControls.MainMenu.Disable();
    }


    void Start()
    {
        // FADE IMAGE ALPHA
        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                                LoadingSceneManager.instance.fadeImage.color.g,
                                                                LoadingSceneManager.instance.fadeImage.color.b,
                                                                1);

        // INITIALIZATION
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
        pingPongText.gameObject.SetActive(false);

        // INTIALIZATION MAIN MENU
        // mainMenuHUDRectTransform.gameObject.SetActive(false);
        // mainMenuHUDRectTransform.sizeDelta = new Vector2(600, 0);
        // mainMenuButtonCG.alpha = 0;
        // mainMenuButtonCG.interactable = false;

        mainMenuHUDRectTransform.anchoredPosition = new Vector2(325, 0);


        // INTIALIZATION SELECT SCENE
        // selectSceneRectTransform.gameObject.SetActive(false);
        // selectSceneRectTransform.sizeDelta = new Vector2(0, 1080);
        // selectSceneButtonCG.alpha = 0;
        // selectSceneButtonCG.interactable = false;

        // INTIALIZATION SELECT SCENE
        // achievementRectTransform.gameObject.SetActive(false);
        // achievementRectTransform.sizeDelta = new Vector2(0, 1080);
        // selectSceneButtonCG.alpha = 0;
        // selectSceneButtonCG.interactable = false;

        // INTIALIZATION SETTING
        settingRectTransform.gameObject.SetActive(false);
        audioSettingRectTransform.gameObject.SetActive(true);
        graphicsSettingRectTransform.gameObject.SetActive(false);
        controlsSettingRectTransform.gameObject.SetActive(false);
        languageSettingRectTransform.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;


        // FADEOUT EFFECT
        LoadingSceneManager.instance.fadeImage.DOFade(0, .25f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            LoadingSceneManager.instance.fadeImage.DOFade(0,2).OnComplete(() =>
            {
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
                pingPongText.gameObject.SetActive(true);
            });
        });
    }

    void Update()
    {
        PingPongProperties();

        DeviceInputChecker();
    }

    void PingPongProperties()
    {
        if (pingPongText.gameObject.activeSelf)
        {
            float alpha = Mathf.PingPong(Time.time * pingPongSpeed, 1f);

            Color currentTextColor = pingPongText.color;
            currentTextColor.a = alpha;
            pingPongText.color = currentTextColor;

            if (actionInput)
            {
                pingPongText.gameObject.SetActive(false);
                ShowMainMenu();
            }
        }
    }

    void DeviceInputChecker()
    {
        DeviceInputCheckerPingPongText();

        DeviceInputCheckerUI();

        DeviceInputCheckerNavGuide();

        // GAMEPAD VIBRATION ON NAVIGATION 
        if (Gamepad.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            if (Gamepad.current.leftStick.ReadValue() != Vector2.zero || Gamepad.current.dpad.ReadValue() != Vector2.zero)
            {
                GameObject currentSelectedButton = EventSystem.current.currentSelectedGameObject;

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
    }


    private void StopVibration()
    {
        Gamepad.current.SetMotorSpeeds(0, 0);
    }

    #region - DEVICE CHECKER [PING PONG TEXT] -

    void DeviceInputCheckerPingPongText()
    {
        if (DeviceManager.instance.keyboardDevice)
        {
            // PINGPONG ACTIVE
            if (pingPongText.gameObject.activeSelf)
            {
                pingPongText.text = "PRESS <sprite name=\"Space\"> TO CONTINUE";
            }
        } 
        else if(DeviceManager.instance.gamepadDevice)
        {
            if (pingPongText.gameObject.activeSelf)
            {
                pingPongText.text = "PRESS <sprite name=\"Cross\"> TO CONTINUE";
            }
        }

    }

    #endregion

    #region - DEVICE CHECKER [HUD/UI]

    void DeviceInputCheckerUI()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            if (mainMenuHUDRectTransform.gameObject.activeSelf || selectSceneRectTransform.gameObject.activeSelf || 
                settingRectTransform.gameObject.activeSelf || achievementRectTransform.gameObject.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                EventSystem.current.SetSelectedGameObject(null);
                isGamepad = false;
            }
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            if (!isGamepad)
            {
                if (mainMenuHUDRectTransform.gameObject.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(mainMenuSelectedButton);
                    isGamepad = true;
                }
                
                if (selectSceneRectTransform.gameObject.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(selectSceneSelectedButton);
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

                if(achievementRectTransform.gameObject.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(achievementSelectedButton);
                    isGamepad = true;
                }
            }
        }
    }

    #endregion

    #region  - NAVIGATION GUIDE -

    void DeviceInputCheckerNavGuide()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            if(selectSceneRectTransform.gameObject.activeSelf)
            {
                selectSceneNavGuide.text = "<sprite name=\"Escape\"> Back   ";
            }
            
            if(settingRectTransform.gameObject.activeSelf)
            {
                settingNavGuide.text = "<sprite name=\"Q\"> <sprite name=\"E\">  Switch Category   <sprite name=\"Escape\"> Back   ";
            }

            if(achievementRectTransform.gameObject.activeSelf)
            {
                achievementNavGuide.text = "<sprite name=\"Left Mouse Button\">  Show Description   <sprite name=\"Escape\"> Back   ";
            }
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            if(selectSceneRectTransform.gameObject.activeSelf)
            {
                selectSceneNavGuide.text = "<sprite name=\"Circle\"> Back   ";
            }
            
            if(settingRectTransform.gameObject.activeSelf)
            {
                settingNavGuide.text = "<sprite name=\"Left Shoulder\"> <sprite name=\"Right Shoulder\">  Switch Category   <sprite name=\"Circle\"> Back   ";
            }

            if(achievementRectTransform.gameObject.activeSelf)
            {
                achievementNavGuide.text = "<sprite name=\"Cross\">  Show Description   <sprite name=\"Escape\"> Back   ";
            }
        }
    }

    #endregion

    #region  - TITLE SCREEN -

    void ShowMainMenu()
    {
        mainMenuButtonCG.interactable = false;
        mainMenuHUDRectTransform.gameObject.SetActive(true);
        mainMenuHUDRectTransform.DOAnchorPos(new Vector2(-325,0), .5f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                mainMenuButtonCG.interactable = true;
                isGamepad = false;
            });
    }

    public void Play()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        mainMenuButtonCG.interactable = false;
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            bgm.DOFade(0, 1);
            mainMenuHUDRectTransform.gameObject.SetActive(false);

            LoadingSceneManager.instance.loadingScreen.SetActive(true);
            LoadingSceneManager.instance.enabled = true;
            LoadingSceneManager.instance.sceneName = "Prologue";
        });

        isGamepad = false;
        lastSelectedButton = null;    
    }

    public void ChapterSelect()
    {
        mainMenuButtonCG.interactable = false;
        isGamepad = true;

        mainMenuHUDRectTransform.DOAnchorPos(new Vector2(325, 0), .5f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                mainMenuHUDRectTransform.gameObject.SetActive(false);
                selectSceneRectTransform.gameObject.SetActive(true);

                selectSceneRectTransform.DOAnchorPos(new Vector2(-960, 0), .5f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() =>
                    {
                        selectSceneButtonCG.interactable = true;
                        isGamepad = false;
                    });
            });
    }

    public void Settings()
    {
        mainMenuButtonCG.interactable = false;
        isGamepad = true;

        mainMenuHUDRectTransform.DOAnchorPos(new Vector2(325, 0), .5f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                mainMenuHUDRectTransform.gameObject.SetActive(false);
                settingRectTransform.gameObject.SetActive(true);

                settingRectTransform.DOAnchorPos(new Vector2(-960, 0), .5f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() =>
                    {
                        settingButtonCG.interactable = true;
                        isGamepad = false;
                    });
            });
    }

    public void Achievements()
    {
        mainMenuButtonCG.interactable = false;
        isGamepad = true;

        mainMenuHUDRectTransform.DOAnchorPos(new Vector2(325, 0), .5f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                mainMenuHUDRectTransform.gameObject.SetActive(false);
                achievementRectTransform.gameObject.SetActive(true);

                achievementRectTransform.DOAnchorPos(new Vector2(-960, 0), .5f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() =>
                    {
                        achievementButtonCG.interactable = true;
                        isGamepad = false;
                    });
            });
    }    

    public void Credits()
    {
        Debug.Log("Access Credits!");
    }

    public void Quit()
    {
        mainMenuButtonCG.interactable = false;
        mainMenuButtonCG.DOFade(0, .25f).OnComplete(() =>
        {
            mainMenuButtonCG.gameObject.SetActive(false);
            mainMenuHUDRectTransform.DOSizeDelta(new Vector2(0, mainMenuHUDRectTransform.sizeDelta.y), .25f)
                .SetEase(Ease.OutFlash)
                .OnComplete(() =>
            {
                mainMenuHUDRectTransform.gameObject.SetActive(false);
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
                LoadingSceneManager.instance.fadeImage.DOFade(1, 1)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        bgm.DOFade(0, 1).OnComplete(() =>
                        {
                            Application.Quit();
#if UNITY_EDITOR
                            UnityEditor.EditorApplication.isPlaying = false;
#endif
                        });
                    });                
            });
        });
    }

    #endregion
    
    #region - SELECT SCENE SCREEN -

    public void selectSceneBack()
    {
        selectSceneButtonCG.interactable = false;
        selectSceneRectTransform.DOAnchorPos(new Vector2(960, 0), 1)
            .SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                selectSceneRectTransform.gameObject.SetActive(false);
                mainMenuHUDRectTransform.gameObject.SetActive(true);
                isGamepad = false;
                ShowMainMenu();
            });

    }

    public void SelectScene(string sceneName)
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        bgm.DOFade(0, LoadingSceneManager.instance.fadeDuration).SetEase(Ease.Linear);
        selectSceneButtonCG.interactable = false;    
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            mainMenuHUDRectTransform.gameObject.SetActive(false);

            LoadingSceneManager.instance.loadingScreen.SetActive(true);
            LoadingSceneManager.instance.enabled = true;
            LoadingSceneManager.instance.sceneName = sceneName;
        });
    } 

    #endregion

    #region  - SETTING SCREEN -

    public void SettingBack()
    {
        settingButtonCG.interactable = false;
        settingRectTransform.DOAnchorPos(new Vector2(960, 0), 1)
            .SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                settingRectTransform.gameObject.SetActive(false);
                mainMenuHUDRectTransform.gameObject.SetActive(true);
                isGamepad = false;
                ShowMainMenu();
            });
    }

    public void AccessAudioSetting()
    {
        audioSettingRectTransform.gameObject.SetActive(true);
        graphicsSettingRectTransform.gameObject.SetActive(false);
        controlsSettingRectTransform.gameObject.SetActive(false);
        languageSettingRectTransform.gameObject.SetActive(false);
    }

    public void AccessGraphicsSetting()
    {
        audioSettingRectTransform.gameObject.SetActive(false);
        graphicsSettingRectTransform.gameObject.SetActive(true);
        controlsSettingRectTransform.gameObject.SetActive(false);
        languageSettingRectTransform.gameObject.SetActive(false);
    }

    public void AccessControlsSetting()
    {
        audioSettingRectTransform.gameObject.SetActive(false);
        graphicsSettingRectTransform.gameObject.SetActive(false);
        controlsSettingRectTransform.gameObject.SetActive(true);
        languageSettingRectTransform.gameObject.SetActive(false);
    }

    public void AccessLanguageSetting()
    {
        audioSettingRectTransform.gameObject.SetActive(false);
        graphicsSettingRectTransform.gameObject.SetActive(false);
        controlsSettingRectTransform.gameObject.SetActive(false);
        languageSettingRectTransform.gameObject.SetActive(true);
    }

    #endregion

    #region  = ACHIEVEMENT SCREEN -

    public void AchievementBack()
    {
        achievementButtonCG.interactable = false;
        achievementRectTransform.DOAnchorPos(new Vector2(960, 0), 1)
            .SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                achievementRectTransform.gameObject.SetActive(false);
                mainMenuHUDRectTransform.gameObject.SetActive(true);
                isGamepad = false;
                ShowMainMenu();
            });
    }

    #endregion
}

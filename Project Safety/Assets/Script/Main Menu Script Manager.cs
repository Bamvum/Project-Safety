using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;


public class MainMenuScriptManager : MonoBehaviour
{   
    PlayerControls playerControls;

    [Header("Title Screen")]
    [SerializeField] TMP_Text pingPongText;
    [SerializeField] float pingPongSpeed;

    [Header("HUD")]
    [SerializeField] RectTransform mainMenuHUDRectTransform;
    [SerializeField] CanvasGroup mainMenuButtonCG;
    
    [Space(5)]
    [SerializeField] RectTransform selectSceneRectTransform;
    [SerializeField] CanvasGroup selectSceneButtonCG;

    [Space(5)]
    [SerializeField] RectTransform settingRectTransform;
    [SerializeField] CanvasGroup settingButtonCG;

    [Space(15)]
    [SerializeField] RectTransform audioSettingRectTransform;
    [SerializeField] RectTransform graphicsSettingRectTransform;
    [SerializeField] RectTransform controlsSettingRectTransform;
    [SerializeField] RectTransform languageSettingRectTransform;


    [Header("Set Selected Game Object")]
    [SerializeField] GameObject lastSelectedButton; // FOR GAMEPAD
    
    [Space(5)]
    [SerializeField] GameObject mainMenuSelectedButton; 
    [SerializeField] GameObject selectSceneSelectedButton; 
    [SerializeField] GameObject settingSelectedButton;


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

        playerControls.MainMenu.Enable();
    }

    
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
        mainMenuHUDRectTransform.gameObject.SetActive(false);
        mainMenuHUDRectTransform.sizeDelta = new Vector2(600, 0);
        mainMenuButtonCG.alpha = 0;
        mainMenuButtonCG.interactable = false;

        // INTIALIZATION SELECT SCENE
        selectSceneRectTransform.gameObject.SetActive(false);
        selectSceneRectTransform.sizeDelta = new Vector2(0, 1080);
        selectSceneButtonCG.alpha = 0;
        selectSceneButtonCG.interactable = false;

        // INTIALIZATION SETTING
        settingRectTransform.gameObject.SetActive(false);
        audioSettingRectTransform.gameObject.SetActive(true);
        graphicsSettingRectTransform.gameObject.SetActive(false);
        controlsSettingRectTransform.gameObject.SetActive(false);
        languageSettingRectTransform.gameObject.SetActive(false);
        settingRectTransform.sizeDelta = new Vector2(0, 1080);
        settingButtonCG.alpha = 0;
        settingButtonCG.interactable = false;
        



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
                Cursor.lockState = CursorLockMode.Locked;
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
                settingRectTransform.gameObject.activeSelf)
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
                    EventSystem.current.SetSelectedGameObject(settingSelectedButton);
                    isGamepad = true;
                }
            }
        }
    }

    #endregion

    #region  - TITLE SCREEN -

    void ShowMainMenu()
    {
        mainMenuHUDRectTransform.gameObject.SetActive(true);
        mainMenuHUDRectTransform.DOSizeDelta(new Vector2(mainMenuHUDRectTransform.sizeDelta.x, 500), .25f)
            .SetEase(Ease.InFlash)
            .OnComplete(() =>
        {
            mainMenuButtonCG.gameObject.SetActive(true);
            mainMenuButtonCG.DOFade(1, .25f).OnComplete(() =>
            {
                mainMenuButtonCG.interactable = true;
            });
        });
    }

    public void Play()
    {
        Debug.Log("Access Play!");

        // canNavigateUI = false;
        
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
        Debug.Log("Access Chapter Select!");
        
        selectSceneRectTransform.sizeDelta = new Vector2(0, 1080);
        
        mainMenuButtonCG.interactable = false;
        isGamepad = true;
        mainMenuButtonCG.DOFade(0, .25f).OnComplete(() =>
        {
            mainMenuButtonCG.gameObject.SetActive(false);
            mainMenuHUDRectTransform.DOSizeDelta(new Vector2(0, mainMenuHUDRectTransform.sizeDelta.y), .25f)
                .SetEase(Ease.OutFlash)
                .OnComplete(() =>
            {
                mainMenuHUDRectTransform.gameObject.SetActive(false);
                selectSceneRectTransform.gameObject.SetActive(true);
                selectSceneRectTransform.DOSizeDelta(new Vector2(1920, selectSceneRectTransform.sizeDelta.y), .25f)
                    .SetEase(Ease.InFlash)
                    .OnComplete(() =>
                    {
                        selectSceneButtonCG.gameObject.SetActive(true);
                        selectSceneButtonCG.DOFade(1, .25f).OnComplete(() =>
                        {
                            selectSceneButtonCG.interactable = true;
                            isGamepad = false;
                        });
                    });
            });
        });        
    }

    public void Settings()
    {
        Debug.Log("Access Settings!");

        settingRectTransform.sizeDelta = new Vector2(0, 1080);
        
        mainMenuButtonCG.interactable = false;
        isGamepad = true;
        mainMenuButtonCG.DOFade(0, .25f).OnComplete(() =>
        {
            mainMenuButtonCG.gameObject.SetActive(false);
            mainMenuHUDRectTransform.DOSizeDelta(new Vector2(0, mainMenuHUDRectTransform.sizeDelta.y), .25f)
                .SetEase(Ease.OutFlash)
                .OnComplete(() =>
            {
                mainMenuHUDRectTransform.gameObject.SetActive(false);
                settingRectTransform.gameObject.SetActive(true);
                settingRectTransform.DOSizeDelta(new Vector2(1920, settingRectTransform.sizeDelta.y), .25f)
                    .SetEase(Ease.InFlash)
                    .OnComplete(() =>
                    {
                        settingButtonCG.gameObject.SetActive(true);
                        settingButtonCG.DOFade(1, .25f).OnComplete(() =>
                        {
                            settingButtonCG.interactable = true;
                            isGamepad = false;
                        });
                    });
            });
        });
    }


    public void Credits()
    {
        Debug.Log("Access Credits!");
    }

    public void Quit()
    {
        Debug.Log("Access Quit!");

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
                        });
                    });                
            });
        });
    }

    #endregion
    
    #region - SELECT SCENE SCREEN -

    public void selectSceneBack()
    {
        Debug.Log("Go back to Main Menu");

        selectSceneButtonCG.DOFade(0, .25f).OnComplete(() =>
        {
            selectSceneButtonCG.gameObject.SetActive(false);
            selectSceneButtonCG.interactable = false;
            isGamepad = true;
            selectSceneRectTransform.DOSizeDelta(new Vector2(selectSceneRectTransform.sizeDelta.x, 0), .25f)
                .SetEase(Ease.OutFlash)
                .OnComplete(() =>
            {
                selectSceneRectTransform.gameObject.SetActive(false);
                mainMenuHUDRectTransform.gameObject.SetActive(true);
                mainMenuHUDRectTransform.DOSizeDelta(new Vector2(600, mainMenuHUDRectTransform.sizeDelta.y), .25f)
                    .SetEase(Ease.InFlash)
                    .OnComplete(() =>
                    {
                        mainMenuButtonCG.gameObject.SetActive(true);
                        mainMenuButtonCG.DOFade(1, .25f).OnComplete(() =>
                        {
                            mainMenuButtonCG.interactable = true;
                            isGamepad = false;
                        });
                    });
            });
        });
    }

    public void SelectScene(string sceneName)
    {
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
        Debug.Log("Go back to Main Menu");

        settingButtonCG.DOFade(0, .25f).OnComplete(() =>
        {
            settingButtonCG.gameObject.SetActive(false);
            settingButtonCG.interactable = false;
            isGamepad = true;
            settingRectTransform.DOSizeDelta(new Vector2(settingRectTransform.sizeDelta.x, 0), .25f)
                .SetEase(Ease.OutFlash)
                .OnComplete(() =>
            {
                settingRectTransform.gameObject.SetActive(false);
                mainMenuHUDRectTransform.gameObject.SetActive(true);
                mainMenuHUDRectTransform.DOSizeDelta(new Vector2(600, mainMenuHUDRectTransform.sizeDelta.y), .25f)
                    .SetEase(Ease.InFlash)
                    .OnComplete(() =>
                    {
                        mainMenuButtonCG.gameObject.SetActive(true);
                        mainMenuButtonCG.DOFade(1, .25f).OnComplete(() =>
                        {
                            AccessAudioSetting();
                            mainMenuButtonCG.interactable = true;
                            isGamepad = false;
                        });
                    });
            });
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
}

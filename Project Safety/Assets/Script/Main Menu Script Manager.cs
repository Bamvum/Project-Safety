using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;
using UnityEngine.iOS;


public class MainMenuScriptManager : MonoBehaviour
{   
    PlayerControls playerControls;

    [Header("Title Screen")]
    [SerializeField] TMP_Text pingPongText;
    [SerializeField] float pingPongSpeed;

    // [Header("Virtual Camera")]
    // [SerializeField] CinemachineBrain cinemachineBrain;

    // [Space(5)]
    // [SerializeField] CinemachineVirtualCamera titleVC;
    // [SerializeField] CinemachineVirtualCamera mainMenuVC;

    [Header("HUD")]
    [SerializeField] RectTransform mainMenuHUDRectTransform;
    [SerializeField] CanvasGroup mainMenuButtonCG;
    
    [Space(10)]
    [SerializeField] RectTransform selectSceneRectTransform;
    [SerializeField] CanvasGroup selectSceneButtonCG;

    [Header("Set Selected Game Object")]
    [SerializeField] GameObject lastSelectedButton; // FOR GAMEPAD
    
    [Space(5)]
    [SerializeField] GameObject mainMenuScreenSelectedButton; 
    [SerializeField] GameObject selectSceneScreenSelectedButton; 

    [Header("Flag")]
    [SerializeField] bool canNavigateUI; 




    [Header("Screens")]
    [SerializeField] RectTransform mainMenuRectTransform; 
    [SerializeField] GameObject selectSceneScreen; 
    

    [Header("Screens")]
    [SerializeField] GameObject landingScreen; 
    // [SerializeField] GameObject mainMenu; 
    [SerializeField] GameObject titleScreen; 
    [SerializeField] GameObject settingScreen; 
    [SerializeField] GameObject[] settingContent;

    
    [Header("Event Select")]
    [SerializeField] GameObject titleSelectObject;
    [SerializeField] GameObject sceneSelectObject;
    [SerializeField] GameObject settingSelectObject;
    
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
        mainMenuHUDRectTransform.gameObject.SetActive(false);
        selectSceneRectTransform.gameObject.SetActive(false);
        // cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();

        // FADEOUT EFFECT
        LoadingSceneManager.instance.fadeImage.DOFade(0,.5f)
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
                // StartCoroutine(ShowMainMenu());

            }
        }
    }

    // IEnumerator ShowMainMenu()
    // {
    //     titleVC.Priority = 0;
    //     mainMenuVC.Priority = 10;

    //     yield return new WaitUntil(() => cinemachineBrain.IsBlending);
    // }

    void ShowMainMenu()
    {
        mainMenuHUDRectTransform.gameObject.SetActive(true);
        mainMenuHUDRectTransform.DOSizeDelta(new Vector2(mainMenuHUDRectTransform.sizeDelta.x, 430), .25f)
            .SetEase(Ease.InFlash)
            .OnComplete(() =>
        {
            mainMenuButtonCG.gameObject.SetActive(true);
            mainMenuButtonCG.DOFade(1, 1);
        });
    }

    void ShowSelectScene()
    {
        mainMenuButtonCG.DOFade(0, 1).OnComplete(() =>
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
                        selectSceneButtonCG.DOFade(1, 1);
                    });
            });
        });
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
            if (mainMenuHUDRectTransform.gameObject.activeSelf || selectSceneScreen.activeSelf)
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
                    EventSystem.current.SetSelectedGameObject(mainMenuScreenSelectedButton);
                    isGamepad = true;
                }
                else if (selectSceneScreen.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(selectSceneScreenSelectedButton);
                    isGamepad = true;
                }
            }
        }
    }

    #endregion

    #region  - TITLE SCREEN -
    public void Play()
    {
        Debug.Log("Access Play!");

        // canNavigateUI = false;
        
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            mainMenuHUDRectTransform.gameObject.SetActive(false);

            LoadingSceneManager.instance.loadingScreen.SetActive(true);
            LoadingSceneManager.instance.enabled = true;
            LoadingSceneManager.instance.sceneName = "Prologue";
        });

        isGamepad = false;       
    }

    public void ChapterSelect()
    {
        Debug.Log("Access Chapter Select!");
        
        ShowSelectScene();

        isGamepad = false;
    }

    public void Settings()
    {
        Debug.Log("Access Settings!");

        titleScreen.SetActive(false);

        settingScreen.SetActive(true);
    }


    public void Credits()
    {
        Debug.Log("Access Credits!");
    }

    public void Quit()
    {
        Debug.Log("Access Quit!");
        Application.Quit();
    }

    #endregion
    
    #region - SELECT SCENE SCREEN -

    public void selectSceneBack()
    {
        Debug.Log("Go back to Main Menu");
        selectSceneScreen.SetActive(false);
        titleScreen.SetActive(true);
    }

    public void PrologueSceneSelect()
    {
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            mainMenuHUDRectTransform.gameObject.SetActive(false);

            LoadingSceneManager.instance.loadingScreen.SetActive(true);
            LoadingSceneManager.instance.enabled = true;
            LoadingSceneManager.instance.sceneName = "Prologue";
        });
    }

    public void Act1Scene1SceneSelect()
    {
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            mainMenuHUDRectTransform.gameObject.SetActive(false);

            LoadingSceneManager.instance.loadingScreen.SetActive(true);
            LoadingSceneManager.instance.enabled = true;
            LoadingSceneManager.instance.sceneName = "Act 1 Scene 1";
        });        
    }

    public void Act1Scene2SceneSelect()
    {
        // FADEIN EFFECT
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            mainMenuHUDRectTransform.gameObject.SetActive(false);

            LoadingSceneManager.instance.loadingScreen.SetActive(true);
            LoadingSceneManager.instance.enabled = true;
            LoadingSceneManager.instance.sceneName = "Act 1 Scene 2";
        });
    }

    public void Act1Scene3SceneSelect()
    {
        // FADEIN EFFECT
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            mainMenuHUDRectTransform.gameObject.SetActive(false);

            LoadingSceneManager.instance.loadingScreen.SetActive(true);
            LoadingSceneManager.instance.enabled = true;
            LoadingSceneManager.instance.sceneName = "Act 1 Scene 3";
        });
    }

    public void Act1Scene4SceneSelect()
    {
        // FADEIN EFFECT
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            mainMenuHUDRectTransform.gameObject.SetActive(false);

            LoadingSceneManager.instance.loadingScreen.SetActive(true);
            LoadingSceneManager.instance.enabled = true;
            LoadingSceneManager.instance.sceneName = "Act 1 Scene 4";
        });        
    }

    public void Act2SceneSelect()
    {
        
    }

    public void Act3SceneSelect()
    {
       
    }    

    #endregion

    #region - SETTING SCREEN -

    public void GameSetting()
    {
        Debug.Log("Access Game Setting!");
        // ENABLE CONTENT
        settingContent[0].SetActive(true);

        // DISABLE
        settingContent[1].SetActive(false);
        settingContent[2].SetActive(false);
        settingContent[3].SetActive(false);
        settingContent[4].SetActive(false);
    }

    public void VideoSetting()
    {
        Debug.Log("Access Video Setting!");

        // ENABLE CONTENT
        settingContent[1].SetActive(true);

        // DISABLE
        settingContent[0].SetActive(false);
        settingContent[2].SetActive(false);
        settingContent[3].SetActive(false);
        settingContent[4].SetActive(false);
    }

    public void GraphicSetting()
    {
        Debug.Log("Access Graphics Setting!");

        // ENABLE CONTENT
        settingContent[2].SetActive(true);

        // DISABLE
        settingContent[0].SetActive(false);
        settingContent[1].SetActive(false);
        settingContent[3].SetActive(false);
        settingContent[4].SetActive(false);
    }

    public void SoundSetting()
    {
        Debug.Log("Access Sound Setting!");

        // ENABLE CONTENT
        settingContent[3].SetActive(true);

        // DISABLE
        settingContent[0].SetActive(false);
        settingContent[1].SetActive(false);
        settingContent[2].SetActive(false);
        settingContent[4].SetActive(false);
    }
    
    public void ControlsSetting()
    {
        Debug.Log("Access Controls Setting!");

        // ENABLE CONTENT
        settingContent[4].SetActive(true);

        // DISABLE
        settingContent[0].SetActive(false);
        settingContent[1].SetActive(false);
        settingContent[2].SetActive(false);
        settingContent[3].SetActive(false);
    }
    
    public void settingBack()
    {
        Debug.Log("Access Setting Back!");

        settingScreen.SetActive(false);

        titleScreen.SetActive(true);
        
        GameSetting();
    }

    #endregion
}

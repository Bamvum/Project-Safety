using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class MainMenuScriptManager : MonoBehaviour
{   
    PlayerControls playerControls;

    [Header("Title Screen")]
    [SerializeField] TMP_Text pingPongText;
    [SerializeField] float pingPongSpeed;

    [Header("Cinemachine")]
    [SerializeField] CinemachineBrain cinemachineBrain;
    [SerializeField] CinemachineVirtualCamera titleVC;
    [SerializeField] CinemachineVirtualCamera mainMenuVC;

    [Header("Screens")]
    [SerializeField] GameObject landingScreen; 
    [SerializeField] GameObject mainMenu; 
    [SerializeField] GameObject titleScreen; 
    [SerializeField] GameObject settingScreen; 
    [SerializeField] GameObject selectSceneScreen; 
    [SerializeField] GameObject[] settingContent;

    
    [Header("Event Select")]
    [SerializeField] GameObject titleSelectObject;
    [SerializeField] GameObject sceneSelectObject;
    [SerializeField] GameObject settingSelectObject;
    
    bool isGamepad;

    bool actionInput;
    bool isFading;

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

        // FADEOUT EFFECT
        LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
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

                titleVC.Priority = 0;
                mainMenuVC.Priority = 10;

                StartCoroutine(DelayDisplayMainMenu());
            }
        }
    }

    IEnumerator DelayDisplayMainMenu()
    {
        yield return new WaitUntil(() => cinemachineBrain.IsBlending);

        Debug.Log("DoTween main menu from out of screen to left");
    }
    
    void DeviceInputChecker()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            if(pingPongText.gameObject.activeSelf)
            {
                pingPongText.text = "PRESS <sprite name=\"Space\"> TO CONTINUE"; 
            }
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            if(pingPongText.gameObject.activeSelf)
            {
                pingPongText.text = "PRESS <sprite name=\"Cross\"> TO CONTINUE";
            }
        }
    }
    
    IEnumerator FadeText()
    {
        isFading = true;
        while (true)
        {
            // PingPong the alpha value between 0 and 1
            float alpha = Mathf.PingPong(Time.time * 1, 1f);
            Color currentColor = pingPongText.color;
            currentColor.a = alpha;
            pingPongText.color = currentColor;

            // Yield to wait for the next frame
            yield return null;
        }
    }

    // void Update()
    // {
    //     // TODO - DEVICE CHECKER

    //     if(DeviceManager.instance.keyboardDevice)
    //     {
    //         if(mainMenu.activeSelf)
    //         {
    //             Cursor.lockState = CursorLockMode.None;
    //             isGamepad = false;
    //         }
    //     }
    //     else if(DeviceManager.instance.gamepadDevice)
    //     {
    //         if(!isGamepad)
    //         {
    //             Cursor.lockState = CursorLockMode.Locked;

    //             // if (titleScreen.activeSelf && !selectSceneScreen.activeSelf && !settingScreen.activeSelf)
    //             // {  
    //             //     Debug.Log("Title Scelect Object");
    //             //     EventSystem.current.SetSelectedGameObject(titleSelectObject);
    //             // }
    //             // else if (!titleScreen.activeSelf && selectSceneScreen.activeSelf && !settingScreen.activeSelf)
    //             // {
    //             //     Debug.Log(" Scelect Object");
    //             //     EventSystem.current.SetSelectedGameObject(sceneSelectObject);
    //             // }
    //             // else
    //             // {
    //             //     isGamepad = false;
    //             // }

    //             if(titleScreen.activeInHierarchy)
    //             {
    //                 Debug.Log("Title Scelect Object");
    //                 EventSystem.current.SetSelectedGameObject(titleSelectObject);
    //             }

    //             // if()
    //             // {

    //             // }

    //             // if (selectSceneScreen.activeSelf)
    //             // {
    //             //     EventSystem.current.SetSelectedGameObject(sceneSelectObject);
    //             // }
    //             // else
    //             // {
    //             //     isGamepad = false;
    //             // }
                
    //             // if (settingScreen.activeSelf)
    //             // {
    //             //     EventSystem.current.SetSelectedGameObject(settingSelectObject);
    //             // }
    //             // else
    //             // {
    //             //     isGamepad = false;
    //             // }

    //             isGamepad = true;
    //         }
    //     }
    // }
    
    #region  - TITLE SCREEN -

    public void PlayTest()
    {   
        // FADEIN EFFECT
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

        mainMenu.SetActive(false);

        LoadingSceneManager.instance.loadingScreen.SetActive(true);
        LoadingSceneManager.instance.enabled = true;
        LoadingSceneManager.instance.sceneName = "Prologue";

        // LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
        //     .SetEase(Ease.Linear)
        //     .OnComplete(() =>
        // {
        //     mainMenu.SetActive(false);

        //     LoadingSceneManager.instance.loadingScreen.SetActive(true);
        //     LoadingSceneManager.instance.enabled = true;
        //     LoadingSceneManager.instance.sceneName = "Prologue";
        // });
    }

    public void Play()
    {
        Debug.Log("Access Play!");
    }

    public void ChapterSelect()
    {
        Debug.Log("Access Chapter Select!");
        
        titleScreen.SetActive(false);
        selectSceneScreen.SetActive(true);
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
            mainMenu.SetActive(false);

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
            mainMenu.SetActive(false);

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
            mainMenu.SetActive(false);

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
            mainMenu.SetActive(false);

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
            mainMenu.SetActive(false);

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

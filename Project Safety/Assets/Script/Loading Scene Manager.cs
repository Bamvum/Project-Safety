using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.ProBuilder.MeshOperations;


public class LoadingSceneManager : MonoBehaviour
{
    // ATTACHED TO CANVAS GAME OBJECT

    public static LoadingSceneManager instance {get; private set;}

    PlayerControls playerControls;

    [Header("Scene to Load")]
    public string sceneName;

    [Header("Loading Screen")]
    public GameObject loadingScreen; 
    
    [Space(10)]
    public Image fadeImage;
    [SerializeField] Image progressBar;
    [SerializeField] Image previewImg;
    [SerializeField] Image prompt;
    [SerializeField] Sprite[] sprite;
    bool actionPressed;

    [Space(10)]
    [SerializeField] TMP_Text dykText;
    [SerializeField] LoadingSO loadingSO;

    [Header("Tweening")]
    public float fadeDuration = 1;
    
    void Awake()
    {
        instance = this;

        playerControls = new PlayerControls();
    }
    
    void OnEnable()
    {
        ActivatePlayerControls();

        playerControls.LoadingUI.Enable();
    }

    void ActivatePlayerControls()
    {
        playerControls.LoadingUI.Action.performed += ctx => actionPressed = true;
        playerControls.LoadingUI.Action.canceled += ctx => actionPressed = false;
    }

    void OnDisable()
    {
        playerControls.LoadingUI.Disable();
    }
    
    void Start()
    {
        // RANDOM DID YOU KNOW, FACTS, TRIVIA
        string RandomDYK = PickRandomDYK(loadingSO);
        dykText.text = RandomDYK;
        
        PreviewChecer(sceneName);
        
        fadeImage.DOFade(0, fadeDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            Debug.Log("Scene to be Load: " + sceneName);
            StartCoroutine(LoadSceneCoroutine(sceneName));
        });
    }

    void Update()
    {
        DeviceChecker();
    }

    void DeviceChecker()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            prompt.sprite = sprite[0];
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            prompt.sprite = sprite[1];
        }
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            if (progressBar != null)
            {
                progressBar.fillAmount = progress;
            }
            
            if (asyncOperation.progress >= 0.9f)
            {
                progressBar.gameObject.SetActive(false);
                prompt.gameObject.SetActive(true);

                // NEW INPUT SYSTEM
                if(actionPressed)
                {
                    // Fade Image Problem!!
                    fadeImage.DOFade(1, fadeDuration).OnComplete(() =>
                    {
                        asyncOperation.allowSceneActivation = true;
                        sceneName = string.Empty;
                        loadingScreen.SetActive(false);
                        this.enabled = false;
                    });
                }
            }
            
            yield return null;
        }
    }

    string PickRandomDYK(LoadingSO DYKData)
    {
        int randomIndex = Random.Range(0, DYKData.loadingText.Length);
        
        return DYKData.loadingText[randomIndex];
    }

    void PreviewChecer(string sceneToBeLoad)
    {
        if(sceneToBeLoad == "Prologue")
        {
            previewImg.sprite = loadingSO.previewScene[0];
        }
        else if (sceneToBeLoad == "Act 1 Scene 1")
        {
            previewImg.sprite = loadingSO.previewScene[1];
        }
        else if (sceneToBeLoad == "Act 1 Scene 2")
        {
            previewImg.sprite = loadingSO.previewScene[2];
        }
        else if (sceneToBeLoad == "Act 1 Scene 3")
        {
            previewImg.sprite = loadingSO.previewScene[3];
        }
        else if(sceneToBeLoad == "Act 1 Scene 4")
        {
            previewImg.sprite = loadingSO.previewScene[4];
        }
        else if(sceneToBeLoad == "Act 2 Scene 1")
        {
            previewImg.sprite = loadingSO.previewScene[5];
        }
        else if(sceneToBeLoad == "Act 2 Scene 2")
        {
            previewImg.sprite = loadingSO.previewScene[6];
        }
        else if(sceneToBeLoad == "Act 3")
        {
            previewImg.sprite = loadingSO.previewScene[7];
        }

        // IF MAIN MENU RANDOMIZE loadingSO.previewScene 
    }
}

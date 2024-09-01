using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class LoadingSceneManager : MonoBehaviour
{
    // TODO - SCRIPTABLE OBJECT FOR DID YOU KNOW/TRIVIA
    //      - RANDOMIZE DYK/TRIVIA 

    public static LoadingSceneManager instance {get; private set;}

    [Header("Scene to Load")]
    public string sceneName;

    [Header("Loading Screen")]
    public GameObject loadingScreen; 
    public Image fadeImage;
    [SerializeField] Image progressBar;
    [SerializeField] GameObject prompt;
    
    [Space(10)]
    [SerializeField] TMP_Text dykText;
    [SerializeField] LoadingSO loadingSO;

    [Header("Tweening")]
    public float fadeDuration = 1;
    
    void Awake()
    {
        instance = this;

        // if (instance == null)
        // {
        //     instance = this;
        //     DontDestroyOnLoad(gameObject);

        // }
        // else
        // {
        //     Destroy(gameObject);
        // }
    }
    
    void Start()
    {
        string RandomDYK = PickRandomDYK(loadingSO);
        dykText.text = RandomDYK;

        fadeImage.DOFade(0, fadeDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            Debug.Log("Scene to be Load: " + sceneName);
            StartCoroutine(LoadSceneCoroutine(sceneName));
        });
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
                prompt.SetActive(true);
                
                Debug.Log("PRESS SPACE TO CONTINUE!!");

                if(Input.GetKeyDown(KeyCode.Space))
                {
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
}

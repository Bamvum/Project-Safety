using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEditor.Search;


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
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        fadeImage.DOFade(0,2).OnComplete(() =>
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
                Debug.Log("PRESS SPACE TO CONTINUE!!");

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    fadeImage.DOFade(1, 2).OnComplete(() =>
                    {
                        asyncOperation.allowSceneActivation = true;

                        sceneName = string.Empty;
                        // HUDManager.instance.loadingUI.SetActive(false);
                        // loadingScreen.SetActive(false);
                        this.enabled = false;
                    });
                }
            }
            yield return null;
        }
    }
}

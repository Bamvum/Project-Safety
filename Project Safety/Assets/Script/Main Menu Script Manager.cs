    using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScriptManager : MonoBehaviour
{   
    [SerializeField] GameObject mainMenu; 
    [SerializeField] GameObject titleScreen; 
    [SerializeField] GameObject settingScreen; 
    [SerializeField] GameObject selectSceneScreen; 
    [SerializeField] GameObject[] settingContent;

    #region  - TITLE SCREEN -

    void Start()
    {
        mainMenu.SetActive(true);
        titleScreen.SetActive(true);
        settingScreen.SetActive(false);
        selectSceneScreen.SetActive(false);

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
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
        });
    }

    public void PlayTest()
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
            LoadingSceneManager.instance.sceneName = "Prologue";
        });
        
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
        selectSceneScreen.SetActive(false);

        titleScreen.SetActive(true);
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

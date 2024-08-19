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
    [SerializeField] GameObject[] settingContent;

    #region  - TITLE SCREEN -
    public void PlayTest()
    {
        Debug.Log("SceneManager");

        // TODO - LOAD ASYNC
        //      - LOADING SCREEN (SCENE DEDICATED ONLY FOR LOADING)
        // SceneManager.LoadScene("Loading Scene");

        LoadingSceneManager.instance.fadeImage.DOFade(1,2).SetEase(Ease.Linear).OnComplete(() =>
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

    public void Settings()
    {
        Debug.Log("Access Settings!");

        titleScreen.SetActive(false);

        settingScreen.SetActive(true);
    }

    public void ChapterSelect()
    {
        Debug.Log("Access Chapter Select!");

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

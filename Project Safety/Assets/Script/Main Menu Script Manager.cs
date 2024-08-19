using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScriptManager : MonoBehaviour
{
    public void PlayTest()
    {
        Debug.Log("SceneManager");

        // TODO - LOAD ASYNC
        //      - LOADING SCREEN (SCENE DEDICATED ONLY FOR LOADING)
        // SceneManager.LoadScene("Loading Scene");

        LoadingSceneManager.instance.fadeImage.DOFade(1,2).OnComplete(() =>
        {
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
}

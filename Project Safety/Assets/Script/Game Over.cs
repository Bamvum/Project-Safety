using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static GameOver instance { get; private set; }

    void Awake()
    {
        instance = this;
    }

    public void GameIsOver()
    {
        if(SceneManager.GetActiveScene().name == "Act 2 Scene 1")
        {
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
            
            LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    PlayerScript.instance.DisablePlayerScripts();

                    LoadingSceneManager.instance.loadingScreen.SetActive(true);
                    LoadingSceneManager.instance.enabled = true;

                    LoadingSceneManager.instance.sceneName = "Act 2 Scene 1";
                });
        }

        if(SceneManager.GetActiveScene().name == "Act 2 Scene 2")
        {
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
            
            LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    PlayerScript.instance.DisablePlayerScripts();

                    LoadingSceneManager.instance.loadingScreen.SetActive(true);
                    LoadingSceneManager.instance.enabled = true;

                    LoadingSceneManager.instance.sceneName = "Act 2 Scene 2";
                });
        }
    }

}

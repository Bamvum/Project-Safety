using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static GameOver instance { get; private set; }

    void Awake()
    {
        instance = this;
    }

    [Header("HUD")] 
    [SerializeField] RectTransform gameOverHUDRectTransform;
    [SerializeField] CanvasGroup gameOverCG;

    [Header("Flag")]
    [SerializeField] bool isGamepad;

    void Update()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            
        }
        else if(DeviceManager.instance.gamepadDevice)
        {

        }
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

    public void ShowGameOver()
    {
        Time.timeScale = 0;

        gameOverHUDRectTransform.sizeDelta = new Vector2(0, 700);
        gameOverCG.interactable = false;
        
        gameOverHUDRectTransform.gameObject.SetActive(true);
        gameOverHUDRectTransform.DOSizeDelta(new Vector2(1250, gameOverHUDRectTransform.sizeDelta.y), .25f)
            .SetEase(Ease.InFlash)
            .SetUpdate(true)
            .OnComplete(() =>
            {
               gameOverCG.gameObject.SetActive(true); 
               gameOverCG.DOFade(1, .25f)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    gameOverCG.interactable = true;
                }); 
            });
    }

    public void GoToMainMenu()
    {
        gameOverCG.interactable = false;

        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                PlayerScript.instance.DisablePlayerScripts();

                LoadingSceneManager.instance.loadingScreen.SetActive(true);
                LoadingSceneManager.instance.enabled = true;

                LoadingSceneManager.instance.sceneName = "Main Menu";
            });
    }
}

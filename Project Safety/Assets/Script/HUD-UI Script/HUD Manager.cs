using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance { get; private set;}
    
    void Awake()
    {
        instance = this;
    }

    // TODO -   CURSOR STATE
    //      -   IF STATEMENT (DIALOGUE HUD, PLAYER HUD, HOMEWORK HUD  IS ACTIVE) 
    
    public Image fadeImage;
    public Image fadeImageForDialogue;

    [Header("Player Related HUD")]
    public GameObject playerHUD;
    public GameObject examineHUD;
    public GameObject dialogueHUD;    
    public GameObject missionHUD;   

    [Space(10)] 
    public RectTransform gameOverHUDRectTransform;
    public CanvasGroup gameOverCG;

    public void ShowGameOver()
    {
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

    public void HideGameOver()
    {
        gameOverCG.DOFade(0, .25f).OnComplete(() =>
        {
            gameOverCG.gameObject.SetActive(false);
            gameOverHUDRectTransform.DOSizeDelta(new Vector2(gameOverHUDRectTransform.sizeDelta.x, 0), .25f)
                .SetEase(Ease.OutFlash)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    // TRIGGER LOADING SCREEN 
                });
        });

    }

    public void FadeInForDialogue()
    {
        fadeImageForDialogue.gameObject.SetActive(true);
        fadeImageForDialogue.DOFade(1, LoadingSceneManager.instance.fadeDuration).SetEase(Ease.Linear);
    }

    public void FadeOutForDialogue()
    {
        fadeImageForDialogue.DOFade(0, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            fadeImageForDialogue.gameObject.SetActive(false);
        });
    }

    public void FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration).SetEase(Ease.Linear);
    }

    public void FadeOut()
    {
        fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => 
        {
            fadeImage.gameObject.gameObject.SetActive(false);
        });
    }

}

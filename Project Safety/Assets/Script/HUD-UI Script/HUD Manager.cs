using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

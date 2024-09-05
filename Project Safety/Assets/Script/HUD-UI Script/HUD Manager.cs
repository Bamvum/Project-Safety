using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
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
    
    public Image fadeImageForDialogue;

    [Header("Player Related HUD")]
    public GameObject playerHUD;
    public GameObject examineHUD;
    public GameObject dialogueHUD;
    
    [Header("Interact HUD")]
    public Image[] interactImage;
    public Sprite[] sprite;
    
    [Header("Homework HUD")]
    public GameObject homeworkHUD;
    public GameObject homeworkQnA;
    public GameObject homeworkScore;
    public TMP_Text homeworkScoreText;
    public TMP_Text questionText;
    public GameObject[] homeworkChoices;

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
}

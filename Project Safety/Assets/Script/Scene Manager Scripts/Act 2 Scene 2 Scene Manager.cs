using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using UnityEditor.PackageManager;

public class Act2Scene2SceneManager : MonoBehaviour
{
    public static Act2Scene2SceneManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }

    [Header("Trigger Dialogue")]
    [SerializeField] DialogueTrigger startDialogue;

    [Header("HUD")]
    [SerializeField] CanvasGroup sceneNameText;

    [Space(10)]
    [SerializeField] TMP_Text timerText;
    [SerializeField] float remainingTime;

    // [Space(10)]
    // [SerializeField] GameObject ;

    [Header("Audio")]
    [SerializeField] AudioSource bgm;
    
    [Header("Flag")]
    [SerializeField] bool isStopTimer;

    void Start()
    {
        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                         LoadingSceneManager.instance.fadeImage.color.g,
                                                         LoadingSceneManager.instance.fadeImage.color.b,
                                                         1);
        TimerStatus(true);
        EscapeTimer();

        StartCoroutine(FadeOutEffect());
    }

    IEnumerator FadeOutEffect()
    {
        yield return new WaitForSeconds(1);
        sceneNameText.gameObject.SetActive(true);
        sceneNameText.DOFade(1, 1).OnComplete(() =>
        {
            sceneNameText.DOFade(1,2).OnComplete(() =>
            {
                sceneNameText.DOFade(0, 1).OnComplete(() =>
                {
                    sceneNameText.gameObject.SetActive(false);
                });
            });
        });


        yield return new WaitForSeconds(5);
        
        LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            bgm.Play();
            bgm.DOFade(1, 1).OnComplete(() =>
            {
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
                // TRIGGER DIALOGUE
                Debug.Log("Trigger Dialogue");
                startDialogue.StartDialogue();
            });
        });
    }

    void Update()
    {
        if(!isStopTimer)
        {
            EscapeTimer();
        }
    }

    #region - TIMER -

    void EscapeTimer()
    {
        remainingTime -= Time.deltaTime;

        // Calculate minutes, seconds, and centiseconds
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        int centiseconds = Mathf.FloorToInt((remainingTime * 100) % 100);

        // Update the timer text with minutes, seconds, and centiseconds
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, centiseconds);

    }

    public void TimerStatus(bool stopTimer)
    {
        isStopTimer = stopTimer;
    }

    public void DecreaseTimer(float decreaseValue)
    {
        remainingTime -= decreaseValue;
    }

    #endregion

    #region - ELEVATOR SPECIAL EVENT -

    public void Elevator()
    {
        StartCoroutine(ElevatorGameOver());
    }

    IEnumerator ElevatorGameOver()
    {
        yield return new WaitForSeconds(1);

        // FADE IN
        // TELEPORT PLAYER INSIDE ELEVATOR
        // FADE OUT
        // DOOR MALFUNCTION 
        // SMOKE FRON BELOW FLOOR RAISE AND ENTER INSIDE THE ELEVATOR
        // PLAYER SUFFOCATE INSIDE
        
        //

    }

    #endregion
}

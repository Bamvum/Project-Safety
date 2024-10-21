using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Act2Scene2SceneManager : MonoBehaviour
{
    public static Act2Scene2SceneManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }

    [Header("Player")]
    public float playerHealth;
    [SerializeField] float playerMaxHealth = 100;

    [Header("Trigger Dialogue")]
    [SerializeField] DialogueTrigger startDialogue;

    [Header("Global Volume")]
    [SerializeField] Volume globalVolume;
    [SerializeField] Vignette vignette;

    [Header("HUD")]
    [SerializeField] CanvasGroup sceneNameText;

    [Space(10)]
    [SerializeField] TMP_Text timerText;
    [SerializeField] float remainingTime;
    [SerializeField] TMP_Text timerDecrease;

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

        GetGlobalVolumeVignette();

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
                // Debug.Log("Trigger Dialogue");
                // startDialogue.StartDialogue();

                // ENABLE MOVEMENT
                PlayerScript.instance.playerMovement.enabled = true;
                PlayerScript.instance.cinemachineInputProvider.enabled = true;
                PlayerScript.instance.interact.enabled = true;
                PlayerScript.instance.stamina.enabled = true;

                isStopTimer = false;
            });
        });
    }

    #region - GLOBAL VOLUME -

    void GetGlobalVolumeVignette()
    {
        // Try to get the Vignette effect from the volume profile
        if (globalVolume.profile.TryGet<Vignette>(out vignette))
        {
            // Vignette successfully retrieved
        }
        
    }

    #endregion

    void Update()
    {
        if(!isStopTimer)
        {
            EscapeTimer();
            PlayerHealthInhilationChecker();
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

    public void PlayerToElevator(Transform locationPosition)
    {
        PlayerScript.instance.playerMovement.gameObject.transform.position = locationPosition.position;
        PlayerScript.instance.playerMovement.gameObject.transform.rotation = Quaternion.Euler(0,0,0);
    }

    #endregion

    #region - PLAYER SMOKE INHILATION
 
    void PlayerHealthInhilationChecker()
    {
        if(playerHealth <= 0)
        {
            GameOver.instance.ShowGameOver();
        }
        else
        {
            // GET GLOBAL VOLUME VIGNETTE INTENSITY AND SMOOTHNESS

            // Adjust the vignette based on player's health
            vignette.intensity.value = Mathf.Lerp(0.25f, 1f, 1 - (playerHealth / playerMaxHealth));
            vignette.smoothness.value = Mathf.Lerp(0.4f, 0.8f, 1 - (playerHealth / playerMaxHealth));
        }
    }

    public void PlayerHealthDamage(float damage)
    {
        playerHealth -= damage;
    }

    #endregion
}

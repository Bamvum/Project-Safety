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

    [Space(20)]
    [SerializeField] GameObject firstFloor;
    [SerializeField] GameObject flashLight;
    [SerializeField] GameObject invWallGroundToBasement;

    void Start()
    {
        Time.timeScale = 1;
        
        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                         LoadingSceneManager.instance.fadeImage.color.g,
                                                         LoadingSceneManager.instance.fadeImage.color.b,
                                                         1);
        TimerStatus(true);
        EscapeTimer();

        GetGlobalVolumeVignette();

        StartCoroutine(FadeOutEffect());

        Cursor.lockState = CursorLockMode.Locked;
    }

    IEnumerator FadeOutEffect()
    {
        yield return new WaitForSeconds(1);
        sceneNameText.gameObject.SetActive(true);
        sceneNameText.DOFade(1, 1)
            .SetUpdate(true)
            .OnComplete(() =>
        {
            sceneNameText.DOFade(1,2)
                .SetUpdate(true)
                .OnComplete(() =>
            {
                sceneNameText.DOFade(0, 1)
                    .SetUpdate(true)
                    .OnComplete(() =>
                {
                    sceneNameText.gameObject.SetActive(false);
                });
            });
        });


        yield return new WaitForSeconds(5);
        
        LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
            .SetUpdate(true)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            bgm.Play();
            bgm.DOFade(1, 1)
                .SetUpdate(true)
                .OnComplete(() =>
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

                Pause.instance.canInput = true;

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

        if(firstFloor.activeSelf)
        {
            if(!flashLight.activeSelf)
            {
                invWallGroundToBasement.SetActive(false);
            }
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

    #region - PLAYER HEALTH [SMOKE INHILATION/INJURY]-
 
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

    #region - HELP ME TRIGGER -

    public void HelpMeTrigger()
    {
        StartCoroutine(HelpMe());
    }

    IEnumerator HelpMe()
    {
        yield return new WaitForSeconds(1);

        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
    }

    #endregion

    #region - TELEPORT TO 1ST FLOOR -

    public void TeleportToFirstFloor()
    {
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                PlayerScript.instance.playerMovement.gameObject.transform.position = new Vector3(80, 0, 48);
                PlayerScript.instance.playerMovement.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

                LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                        .SetUpdate(true)
                        .OnComplete(() =>
                        {
                            LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
                                .SetUpdate(true)
                                .SetEase(Ease.Linear)
                                .OnComplete(() =>
                                {
                                    LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
                                });
                        });
            });
        
    }

    #endregion 

    #region - INVISIBLE WALL GROUND TO BASEMENT -

    void RemoveInvisiableWall()
    {
        if(flashLight)
        {

        }
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;

public class Act2Scene2SceneManager : MonoBehaviour
{
    public static Act2Scene2SceneManager instance {get; private set;}
    private FirebaseAuth auth;

    void Awake()
    {
        instance = this;
    }

    [SerializeField] AchievementTrigger achievementTrigger;
    [SerializeField] AchievementSO tooHotToHandleAchievement;
    [SerializeField] AchievementSO anyPercentAchievement;
    [SerializeField] AchievementSO chainSmokerAchievement;

    public int doorChecked;
    bool stopTooHotToHandleLoop;
    bool stopChainSmokerAchievementLoop;

    [Header("Player")]
    public float playerHealth;
    [SerializeField] float playerMaxHealth = 100;

    [SerializeField] Image lungsVisualHealth;

    [Header("Language Preference")]
    [SerializeField] InstructionSO englishInstructionSO;
    [SerializeField] InstructionSO tagalogInstructionSO;

    [Space(5)]
    [SerializeField] GameObject englishLanguage;
    [SerializeField] GameObject tagalogLanguage;

    [Space(10)]
    public int languageIndex;

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
    [SerializeField] AudioSource sceneNameRevealSFX;
    
    [Header("Flag")]
    [SerializeField] bool isStopTimer;
    [SerializeField] bool stopZeroTimerLoop;
    [SerializeField] bool stopSmokeHealthLoop;

    [Space(20)]
    [SerializeField] GameObject firstFloor;
    [SerializeField] GameObject flashLight;
    [SerializeField] GameObject invWallGroundToBasement;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        PlayerPrefs.SetInt("School: Escape", 1);
        FirebaseManager.Instance.SaveChapterUnlockToFirebase("School: Escape", true);
        Time.timeScale = 1;
        
        // FADE IMAGE ALPHA SET 1
        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                         LoadingSceneManager.instance.fadeImage.color.g,
                                                         LoadingSceneManager.instance.fadeImage.color.b,
                                                         1);
        TimerStatus(true);
        EscapeTimer();

        StartCoroutine(FadeOutEffect());

        Cursor.lockState = CursorLockMode.Locked;
        
        if(SettingMenu.instance.languageDropdown.value == 0) // English
        {
            Debug.Log("English Preference");
            InstructionManager.instance.instructionsSO = englishInstructionSO;
            englishLanguage.SetActive(true);
            tagalogLanguage.SetActive(false);
            languageIndex = 0;
        }
        else
        {
            Debug.Log("Tagalog Preference");
            InstructionManager.instance.instructionsSO = tagalogInstructionSO;
            englishLanguage.gameObject.SetActive(false);
            tagalogLanguage.gameObject.SetActive(true);
            languageIndex = 1;
        }
    }

    IEnumerator FadeOutEffect()
    {
        yield return new WaitForSeconds(1);
        sceneNameRevealSFX.Play();
        sceneNameText.DOFade(1, 1)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                sceneNameText.DOFade(0, 1)
                    .SetDelay(3)
                    .SetUpdate(true);
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

                InstructionManager.instance.enabled = true;
                InstructionManager.instance.ShowInstruction();
            });
        });
    }

    void Update()
    {
        if(!isStopTimer)
        {
            EscapeTimer();
            PlayerHealthInhilationChecker();
        }

        if(remainingTime <= 0)
        {
            if (!stopZeroTimerLoop)
            {
                GameOver.instance.ShowGameOver();
                stopZeroTimerLoop = true;
            }
        }

        if (!stopTooHotToHandleLoop)
        {
            if(doorChecked == 2)
            {
                achievementTrigger.ShowAchievement(tooHotToHandleAchievement);
                stopTooHotToHandleLoop = true;
            }
        }
    }

    public void RecordGatherBelongingsChoice(bool gatherBelongings)
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .Child(auth.CurrentUser.UserId)
            .Child("choices")
            .Child("Act2Scene2_GatherBelongingsChoice")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result.Exists)
                    {
                        Debug.LogWarning("Gather belongings choice has already been saved in Firebase and cannot be changed.");
                        return; // Exit without saving or uploading
                    }
                    else
                    {
                        int choiceValue = gatherBelongings ? 1 : 2; // 1 for gathering belongings, 2 for leaving immediately
                        PlayerPrefs.SetInt("Act2Scene2_GatherBelongingsChoice", choiceValue);
                        PlayerPrefs.Save();
                        FirebaseManager.Instance.SaveChoiceToFirebase("Act2Scene2_GatherBelongingsChoice", choiceValue);
                        Debug.Log("Gather Belongings Choice Recorded: " + (gatherBelongings ? "Gathered Belongings" : "Left Immediately"));
                    }
                }
                else
                {
                    Debug.LogError("Error checking Firebase for existing choice: " + task.Exception);
                }
            });
    }


    public void OnGatherBelongingsChoice()
    {
        RecordGatherBelongingsChoice(true); 
    }

    public void OnLeaveImmediatelyChoice()
    {
        RecordGatherBelongingsChoice(false); 
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

    // Method to save the elevator choice
    public void RecordElevatorChoice(bool useElevator)
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .Child(auth.CurrentUser.UserId)
            .Child("choices")
            .Child("Act2Scene2_ElevatorChoice")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result.Exists)
                    {
                        Debug.LogWarning("Elevator choice has already been saved in Firebase and cannot be changed.");
                        return; // Exit without saving or uploading
                    }
                    else
                    {
                        int choiceValue = useElevator ? 1 : 2;
                        PlayerPrefs.SetInt("Act2Scene2_ElevatorChoice", choiceValue);
                        PlayerPrefs.Save();
                        FirebaseManager.Instance.SaveChoiceToFirebase("Act2Scene2_ElevatorChoice", choiceValue);
                        Debug.Log("Elevator Choice Recorded: " + (useElevator ? "Used Elevator" : "Did Not Use Elevator"));
                    }
                }
                else
                {
                    Debug.LogError("Error checking Firebase for existing choice: " + task.Exception);
                }
            });
    }

    public void OnUseElevatorChoice()
    {
        RecordElevatorChoice(true);
    }

    public void OnDontUseElevatorChoice()
    {
        RecordElevatorChoice(false); 
    }

#endregion

#region - PLAYER HEALTH [SMOKE INHILATION/INJURY]-

void PlayerHealthInhilationChecker()
    {
        if(playerHealth <= 0)
        {
            if (!stopSmokeHealthLoop)
            {
                GameOver.instance.ShowGameOver();
                stopSmokeHealthLoop = true;
            }
        }
        else if (playerHealth < 20)
        {
            if (!stopChainSmokerAchievementLoop)
            {
                achievementTrigger.ShowAchievement(chainSmokerAchievement);
                stopChainSmokerAchievementLoop = true;
            }
        }
        else
        {
            lungsVisualHealth.fillAmount = Mathf.Lerp(1f, 0f, playerHealth / playerMaxHealth);
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

    #region - ENABLE MOVEMENT -
    
    public void enableMovement()
    {
        // ENABLE MOVEMENT
        PlayerScript.instance.playerMovement.enabled = true;
        PlayerScript.instance.cinemachineInputProvider.enabled = true;
        PlayerScript.instance.interact.enabled = true;
        PlayerScript.instance.stamina.enabled = true;

        isStopTimer = false;      
    }

    
    #endregion

    #region - NEXT SCENE -

    public void NextScene()
    {
        bgm.DOFade(0, LoadingSceneManager.instance.fadeDuration).SetUpdate(true);

        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            LoadingSceneManager.instance.loadingScreen.SetActive(true);
            LoadingSceneManager.instance.enabled = true;
            LoadingSceneManager.instance.sceneName = "Statistic";
        });
    }

    #endregion

    #region - ANY%? -

    public void anyPercentTrigger()
    {
        if (remainingTime >= 240) // 240 seconds is equivalent to 4 minutes
        {
            achievementTrigger.ShowAchievement(anyPercentAchievement);
        }
    }

    #endregion
}
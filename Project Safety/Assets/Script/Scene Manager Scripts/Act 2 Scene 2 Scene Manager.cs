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
    
    [Header("Flag")]
    [SerializeField] bool isStopTimer;

    [Space(20)]
    [SerializeField] GameObject firstFloor;
    [SerializeField] GameObject flashLight;
    [SerializeField] GameObject invWallGroundToBasement;

    void Start()
    {
        PlayerPrefs.SetInt("School: Escape", 1);
        Time.timeScale = 1;
        
        // FADE IMAGE ALPHA SET 1
        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                         LoadingSceneManager.instance.fadeImage.color.g,
                                                         LoadingSceneManager.instance.fadeImage.color.b,
                                                         1);
        TimerStatus(true);
        EscapeTimer();

        GetGlobalVolumeVignette();

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

                InstructionManager.instance.enabled = true;
                InstructionManager.instance.ShowInstruction();
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

    public void RecordGatherBelongingsChoice(bool gatherBelongings)
    {
        // Save choice as integer in PlayerPrefs: 1 for gathering belongings, 2 for leaving immediately
        int choiceValue = gatherBelongings ? 1 : 2;
        PlayerPrefs.SetInt("Act2Scene2_GatherBelongingsChoice", choiceValue);
        PlayerPrefs.Save();

        // Log the choice for debugging
        Debug.Log("Gather Belongings Choice Recorded: " + (gatherBelongings ? "Gathered Belongings" : "Left Immediately"));

        // Upload the choice to Firebase
        FirebaseManager.Instance.SaveChoiceToFirebase("Act2Scene2_GatherBelongingsChoice", choiceValue);
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
        // Save choice as integer in PlayerPrefs: 1 for using the elevator, 2 for not using it
        int choiceValue = useElevator ? 1 : 2;
        PlayerPrefs.SetInt("Act2Scene2_ElevatorChoice", choiceValue);
        PlayerPrefs.Save();

        // Log the choice for debugging
        Debug.Log("Elevator Choice Recorded: " + (useElevator ? "Used Elevator" : "Did Not Use Elevator"));

        // Upload the choice to Firebase
        FirebaseManager.Instance.SaveChoiceToFirebase("Act2Scene2_ElevatorChoice", choiceValue);
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
            LoadingSceneManager.instance.sceneName = "Post Assessment";
        });
    }

    #endregion
}

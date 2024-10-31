using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using TMPro;

public class PrologueSceneManager : MonoBehaviour
{
    public static PrologueSceneManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }
    
    [SerializeField] CanvasGroup sceneNameText;
 
    [Header("Script")]
    public OnAndOffGameObject onAndOffGameObject;
    [SerializeField] AchievementTrigger achievementTrigger;
    [SerializeField] AchievementSO achievementSO;

    [Header("Audio")]
    [SerializeField] AudioSource sceneNameReveal;

    [Header("Player")]
    [SerializeField] GameObject playerModel;
    
    [Space(10)]
    [SerializeField] AudioSource suspenceSFX;
    public AudioSource alarmSFX;

    [Header("Prologue Game Object")]
    public GameObject PC;
    public GameObject monitor;
    public GameObject[] monitorScreen;
    public GameObject lightSwitch;

    [Header("Language Preference")]
    [SerializeField] GameObject englishLanguage;
    [SerializeField] GameObject tagalogLanguage;

    [Space(10)]
    [SerializeField] InstructionSO englishInstructionsSO;
    [SerializeField] InstructionSO tagalogInstructionsSO;

    [Header("Dialogue Triggers")]
    [SerializeField] DialogueTrigger startDialogueTrigger;
    
    [Space(15)]
    public AudioSource monitorSFX;

    [Header("Flag")]
    public bool toGetUp;
    bool isSuspenceSFXPlaying;

    [SerializeField] Volume globalVolume; 
    Vignette vignette;

    void Start()
    {
        if(SettingMenu.instance.languageDropdown.value == 0) // English
        {
            Debug.LogWarning("English Preference");
            // englishLanguage.SetActive(true);
            // tagalogLanguage.SetActive(false);
            InstructionManager.instance.instructionsSO = englishInstructionsSO;
        }
        else
        {
            Debug.LogWarning("Tagalog Preference");
            // englishLanguage.SetActive(false);
            // tagalogLanguage.SetActive(true);
            InstructionManager.instance.instructionsSO = tagalogInstructionsSO;
        }

        // FADE IMAGE ALPHA SET 1
        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                                LoadingSceneManager.instance.fadeImage.color.g,
                                                                LoadingSceneManager.instance.fadeImage.color.b,
                                                                1);

        StartCoroutine(FadeOutFadeImage());

        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        // TO NEXT SCENE (ACT 1 - STUDENT)
        if(isSuspenceSFXPlaying)
        {
            Debug.Log("Suspence SFX is Playing!");

            // ELAPSED TIME == suspenseSFX.Clip.length
            if(suspenceSFX.time >= suspenceSFX.clip.length)
            {
                PlayerScript.instance.DisablePlayerScripts();

                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
                LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                    .SetEase(Ease.Linear)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        sceneAchievementTrigger();
                        DisplaySceneName();

                        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                        .SetEase(Ease.Linear)
                        .SetUpdate(true)
                        .SetDelay(6)
                        .OnComplete(() =>
                        {
                            PlayerScript.instance.DisablePlayerScripts();

                            LoadingSceneManager.instance.loadingScreen.SetActive(true);
                            LoadingSceneManager.instance.enabled = true;
                            // NEXT SCENE NAME
                            LoadingSceneManager.instance.sceneName = "Act 1 Scene 1";
                        });
                    });

                isSuspenceSFXPlaying = false;
            }    
        }
    }

    IEnumerator FadeOutFadeImage()
    {
        yield return new WaitForSeconds(2);
        alarmSFX.Play();

        yield return new WaitForSeconds(5);
        Debug.Log("Wait for 5 Seconds");
        LoadingSceneManager.instance.fadeImage
                .DOFade(0, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
        {
           
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
        });

        yield return new WaitForSeconds(3);
        toGetUp = true;
    }

    void DisplaySceneName()
    {
        sceneNameReveal.Play();
        sceneNameText.gameObject.SetActive(true);
        sceneNameText.DOFade(1,1)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                sceneNameText.DOFade(0, 1)
                    .SetUpdate(true)
                    .SetDelay(3)
                    .OnComplete(() =>
                {
                    sceneNameText.gameObject.SetActive(false);
                    
                });
            });

    }
    
    public void TransitionToHomeworkQuiz()
    {
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            HomeworkManager.instance.enabled = true;
            HomeworkManager.instance.homeworkHUD.SetActive(true);
            LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => 
            {
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
                HomeworkManager.instance.homeworkHUD.SetActive(true);
                PlayerScript.instance.playerMovement.enabled = false;
                PlayerScript.instance.cinemachineInputProvider.enabled = false;
                PlayerScript.instance.stamina.enabled = false;
            });
        });
    }

    #region - SCENE MANAGEMENT -

    public void RotatePlayer()
    {
        // CAMAERA FIX (MAKE IT FACE A CERTAIN AXIS)
        playerModel.transform.rotation = Quaternion.Euler(0, 120,0);
    }

    public void MovePlayer()
    {
        playerModel.transform.position = new Vector3(-6.5f, playerModel.transform.position.y, -11);
        // playerModel.transform.position = new Vector3(0,0,0);
    }

    public void StartSuspenceSequence()
    {
        //TODO  -   CHANGE ALL LAYER OF EXAMINABLE GAMEOBJECT TO DEFAULT (LAYER 0)
        //      -   START SUSPENCE SOUND
        //      -   START VIGNETTE

        suspenceSFX.Play();
        isSuspenceSFXPlaying = true; 
    }

    #endregion

    #region - ACHIEVEMENT TRIGGER -

    void sceneAchievementTrigger()
    {
        // Does this mean that the playerprefs is false?
        if (PlayerPrefs.GetInt(achievementSO.achievementPlayerPrefsKey) == 0)
        {
            achievementTrigger.ShowAchievement(achievementSO);
        }
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;

public class Act1Scene4SceneManager : MonoBehaviour
{
    public static Act1Scene4SceneManager instance {get; private set;}
    private FirebaseAuth auth;

    void Awake()
    {
        instance = this;
    }

    
    [Header("HUD")]
    [SerializeField] CanvasGroup sceneNameText;
    

    [Header("Player")]
    [SerializeField] GameObject player;

    [Header("Dialogue Triggers")]
    [SerializeField] DialogueTrigger startMonologueEnglish;
    [SerializeField] DialogueTrigger startMonologueTagalog;

    [Space(5)]
    [SerializeField] DialogueTrigger startDialogueEnglish;
    [SerializeField] DialogueTrigger startDialogueTagalog;


    [Header("Language Preference")]
    [SerializeField] GameObject englishLanguage;
    [SerializeField] GameObject tagalogLanguage;

    [Space(5)]
    [SerializeField] int languageIndex;

    [Header("Cinemachine")]
    [SerializeField] CinemachineVirtualCamera cctvVC;
    [SerializeField] CinemachineVirtualCamera blueFlagVC;
    [SerializeField] CinemachineVirtualCamera smokeAreaVC;
    [SerializeField] CinemachineVirtualCamera dummyVC;
    

    [Header("Audio")]
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource sceneNameRevealSFX;
    [SerializeField] AudioSource ambianceSFX;

    void Start()
    {
        PlayerPrefs.SetInt("Training Grounds Scene", 1);
        auth = FirebaseAuth.DefaultInstance;

        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                         LoadingSceneManager.instance.fadeImage.color.g,
                                                         LoadingSceneManager.instance.fadeImage.color.b,
                                                         1);

        Cursor.lockState = CursorLockMode.Locked;
        
        if(SettingMenu.instance.languageDropdown.value == 0)
        {
            Debug.LogWarning("English Preference");
            englishLanguage.SetActive(true);
            tagalogLanguage.SetActive(false);
            languageIndex = 0;
        }
        else
        {
            Debug.LogWarning("Tagalog Preference");
            englishLanguage.SetActive(false);
            tagalogLanguage.SetActive(true);
            languageIndex = 1;
        }   

        StartCoroutine(StartMonologue());
    }

    IEnumerator StartMonologue()
    {
        yield return new WaitForSeconds(2);

        if(languageIndex == 0)
        {
            startMonologueEnglish.StartDialogue();
        }
        else
        {
            startMonologueTagalog.StartDialogue();
        }
    }

    public void FadeOutEffect()
    { 
        StartCoroutine(PreFadeOutEffect());
    }

    IEnumerator PreFadeOutEffect()
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

        ambianceSFX.DOFade(1, 1).SetUpdate(true);

        LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnComplete(() =>
        {
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);

            if(languageIndex == 0)
            {
                startDialogueEnglish.StartDialogue();
            }
            else
            {
                startDialogueTagalog.StartDialogue();
            }
        });
        
    }

    public void BGMSoundStart()
    {
        bgm.Play();
        bgm.DOFade(1, 1).SetUpdate(true);
    }

    public void CCTVDialogueOn()
    {
        PlayerScript.instance.playerVC.Priority = 0;
        cctvVC.Priority = 10;
    }

    public void CCTVDialogueOff()
    {
        PlayerScript.instance.playerVC.Priority = 10;
        cctvVC.Priority = 0;
    }

    public void BlueFlagDialogueOn()
    {
        Debug.Log("Blue Flag On!");
        PlayerScript.instance.playerVC.Priority = 0;
        blueFlagVC.Priority = 10;
    }

    public void BlueFlagDialogueOff()
    {
        Debug.Log("Blue Flag Off!");
        PlayerScript.instance.playerVC.Priority = 10;
        blueFlagVC.Priority = 0;
    }

    public void SmokeAreaDialogueOn()
    {
        blueFlagVC.Priority = 0;
        smokeAreaVC.Priority = 10;
        
    }

    public void SmokeAreaDialogueOff()
    {
        PlayerScript.instance.playerVC.Priority = 10;
        smokeAreaVC.Priority = 0;
    }

    public void DummyDialogueOn()
    {
        PlayerScript.instance.playerVC.Priority = 0;
        dummyVC.Priority = 10;
    }

    public void DummyDialogueOff()
    {
        PlayerScript.instance.playerVC.Priority = 10;
        dummyVC.Priority = 0;
    }

    public void EndOfScene()
    {
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                PlayerScript.instance.DisablePlayerScripts();

                LoadingSceneManager.instance.loadingScreen.SetActive(true);
                LoadingSceneManager.instance.enabled = true;
                LoadingSceneManager.instance.sceneName = "Act 2 Scene 1";
            });
    }

    public void RecordDummyChoice(bool saveDummy)
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .Child(auth.CurrentUser.UserId)
            .Child("choices")
            .Child("Act1Scene4_DummyChoice")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result.Exists)
                    {
                        Debug.LogWarning("Dummy choice has already been saved in Firebase and cannot be changed.");
                        return; // Exit without saving or uploading
                    }
                    else
                    {
                        int choiceValue = saveDummy ? 1 : 2; // 1 for saving the Dummy, 2 for not saving
                        PlayerPrefs.SetInt("Act1Scene4_DummyChoice", choiceValue);
                        PlayerPrefs.Save();
                        FirebaseManager.Instance.SaveChoiceToFirebase("Act1Scene4_DummyChoice", choiceValue);
                        Debug.Log("Dummy Choice Recorded: " + (saveDummy ? "Saved Dummy" : "Did Not Save Dummy"));
                    }
                }
                else
                {
                    Debug.LogError("Error checking Firebase for existing choice: " + task.Exception);
                }
            });
    }


    public void OnSaveDummyChoice()
    {
        RecordDummyChoice(true);
    }

    public void OnDontSaveDummyChoice()
    {
        RecordDummyChoice(false); 
    }


}

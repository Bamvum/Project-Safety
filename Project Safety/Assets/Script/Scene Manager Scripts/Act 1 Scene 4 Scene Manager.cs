using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Act1Scene4SceneManager : MonoBehaviour
{
    public static Act1Scene4SceneManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }

    // VARIABLE
    [Header("Player")]
    [SerializeField] GameObject player;

    [Header("Dialogue Triggers")]
    [SerializeField] DialogueTrigger startDialogueEnglish;
    [SerializeField] DialogueTrigger startDialogueTagalog;

    [SerializeField] TMP_Text tmpText;


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
    


    void Start()
    {
        PlayerPrefs.SetInt("Training Grounds Scene", 1);

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

        StartCoroutine(FadeOutEffect());
    }

    IEnumerator FadeOutEffect()
    {
        yield return new WaitForSeconds(5);

        LoadingSceneManager.instance.fadeImage
            .DOFade(0, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
            // TRIGGER DIALOGUE
            Debug.Log("Trigger Dialogue");

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
        // Save choice as integer in PlayerPrefs: 1 for saving the Dummy, 2 for not saving
        int choiceValue = saveDummy ? 1 : 2;
        PlayerPrefs.SetInt("Act1Scene4_DummyChoice", choiceValue);
        PlayerPrefs.Save();

        // Log the choice for debugging
        Debug.Log("Dummy Choice Recorded: " + (saveDummy ? "Saved Dummy" : "Did Not Save Dummy"));

        // Upload the choice to Firebase
        FirebaseManager.Instance.SaveChoiceToFirebase("Act1Scene4_DummyChoice", choiceValue);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;

public class Act2Scene1Manager : MonoBehaviour
{
    public static Act2Scene1Manager instance {get; private set;}
    private FirebaseAuth auth;

    void Awake()
    {
        DOTween.SetTweensCapacity(1000, 200);  // 1000 tweeners and 200 sequences

        instance = this;
    }

    [Header("Trigger Dialogue")]
    [SerializeField] DialogueTrigger startDialogue;
    [SerializeField] DialogueTrigger PlaceHolderCalmAndCall;

    [Header("Cinemachine")]
    [SerializeField] CinemachineInputProvider chairInputProvider;

    [Header("Flag")]
    [SerializeField] GameObject personRunningGO;
    [SerializeField] float personRunningSpeed = 5;

    [Space(10)]
    [SerializeField] bool personRunning;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        PlayerPrefs.SetInt("School: Start", 1);

        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                         LoadingSceneManager.instance.fadeImage.color.g,
                                                         LoadingSceneManager.instance.fadeImage.color.b,
                                                         1);

        StartCoroutine(FadeOutEffect());
    }

    IEnumerator FadeOutEffect()
    {
        yield return new WaitForSeconds(3);
                LoadingSceneManager.instance.fadeImage
            .DOFade(0, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
            // TRIGGER DIALOGUE
            Debug.Log("Trigger Dialogue");
            startDialogue.StartDialogue();
        });
    }

    void Update()
    {
        if(personRunning)
        {
            personRunningGO.transform.position = Vector3.MoveTowards(personRunningGO.transform.position, new Vector3(60, personRunningGO.transform.position.y, personRunningGO.transform.position.z), Time.deltaTime * personRunningSpeed);

            if(personRunningGO.transform.position == new Vector3(60, personRunningGO.transform.position.y, personRunningGO.transform.position.z))
            {
                personRunning = false;
                Destroy(personRunningGO);
            }
        }
    }

    // FOR TESTING
    public void PlaceHolderForCall()
    {
        StartCoroutine(DelayFunction());
    }

    IEnumerator DelayFunction()
    {
        yield return new WaitForSeconds(.5f);
        Debug.Log("PlaceHolderForCall");
        PlaceHolderCalmAndCall.StartDialogue();
    }

    public void enablePersonRunningMoveToward(bool enable)
    {
        personRunning = enable;
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
            LoadingSceneManager.instance.sceneName = "Act 2 Scene 2";
        });
    }

    // Call choice functions
    public void SaveCallChoice(string choice)
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .Child(auth.CurrentUser.UserId)
            .Child("choices")
            .Child("Act2Scene1_CallChoice")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result.Exists)
                    {
                        Debug.LogWarning("Call choice has already been saved in Firebase and cannot be changed.");
                        return; // Exit without saving or uploading
                    }
                    else
                    {
                        // Proceed with saving if choice hasn't been made
                        int choiceValue = choice == "Firestation" ? 1 : choice == "EmergencyHotline" ? 2 : choice == "Mom" ? 3 : 0;
                        if (choiceValue == 0) return;

                        PlayerPrefs.SetInt("Act2Scene1_CallChoice", choiceValue);
                        PlayerPrefs.Save();
                        FirebaseManager.Instance.SaveChoiceToFirebase("Act2Scene1_CallChoice", choiceValue);
                        Debug.Log("Call Choice Recorded: " + choice);
                    }
                }
                else
                {
                    Debug.LogError("Error checking Firebase for existing choice: " + task.Exception);
                }
            });
    }


    public void OnCallFirestationChoice()
    {
        SaveCallChoice("Firestation");
    }

    public void OnCallEmergencyHotlineChoice()
    {
        SaveCallChoice("EmergencyHotline");
    }

    public void OnCallMomChoice()
    {
        SaveCallChoice("Mom");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class Act2Scene1Manager : MonoBehaviour
{
    public static Act2Scene1Manager instance {get; private set;}

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
        PlayerPrefs.SetInt("School: Start", 1);
        FirebaseManager.Instance.SaveChapterUnlockToFirebase("School: Start", true);

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
        int choiceValue = 0;

        switch (choice)
        {
            case "Firestation":
                choiceValue = 1; // 1 for local fire station
                break;
            case "EmergencyHotline":
                choiceValue = 2; // 2 for national emergency hotline
                break;
            case "Mom":
                choiceValue = 3; // 3 for mom
                break;
            default:
                Debug.LogError("Invalid choice");
                return; // Exit if invalid choice
        }

        // Save choice in PlayerPrefs
        PlayerPrefs.SetInt("Act2Scene1_CallChoice", choiceValue);
        PlayerPrefs.Save();

        // Log choice for debugging
        Debug.Log("Call Choice Recorded: " + choice);

        // Upload choice to Firebase
        FirebaseManager.Instance.SaveChoiceToFirebase("Act2Scene1_CallChoice", choiceValue);
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

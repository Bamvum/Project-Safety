using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Cinemachine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;

public class Act1StudentSceneManager : MonoBehaviour
{
    public static Act1StudentSceneManager instance {get; private set;}
    private FirebaseAuth auth;

    void Awake()
    {
        instance = this;
    }

    [Header("Dialogue Triggers")]
    [SerializeField] DialogueTrigger startDialogueTrigger;
    [SerializeField] DialogueTrigger momDialogueTrigger;

    [Space(15)]
    public GameObject DoorEndingScene;
    
    [Space(15)]
    [SerializeField] CinemachineVirtualCamera televisionVC;

    [Space(15)]
    [SerializeField] AudioSource bedPlayerAudio;
    [SerializeField] AudioClip heavyBreathingSFX;

    [Header("Flag")]
    bool audioRepeat;
    public int plugInteracted;

    void Start()
    {
        // FADE IMAGE ALPHA SET 1
        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                                LoadingSceneManager.instance.fadeImage.color.g,
                                                                LoadingSceneManager.instance.fadeImage.color.b,
                                                                1);

        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(HeavyBreathingSFX());

        auth = FirebaseAuth.DefaultInstance;
        PlayerPrefs.SetInt("House Scene", 1);
    }

    IEnumerator HeavyBreathingSFX()
    {
        yield return new WaitForSeconds(2);
            
        bedPlayerAudio.clip = heavyBreathingSFX;
        bedPlayerAudio.Play();

        yield return new WaitForSeconds(10);
        LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
            
            startDialogueTrigger.StartDialogue();
        });
    }

    void Update()
    {
        // if (!audioRepeat)
        // {
        //     CheckPlayerAudioPlaying();
        // }

        if (plugInteracted == 4)
        {
            momDialogueTrigger.StartDialogue();
            PlayerScript.instance.DisablePlayerScripts();
            plugInteracted++;
        }
    }

    void CheckPlayerAudioPlaying()
    {
        if(!bedPlayerAudio.isPlaying)
        {
            // FADEOUT EFFECTS
            LoadingSceneManager.instance.fadeImage
                .DOFade(0, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
            {
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);

                audioRepeat = true;
                startDialogueTrigger.StartDialogue();
            });
        }
    }

    public void RecordPlugChoice(bool savePlug)
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .Child(auth.CurrentUser.UserId)
            .Child("choices")
            .Child("Act1Scene1_PlugChoice")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result.Exists)
                    {
                        Debug.LogWarning("Plug choice has already been saved in Firebase and cannot be changed.");
                        return; // Exit without saving or uploading
                    }
                    else
                    {
                        int choiceValue = savePlug ? 1 : 2; // 1 for Unplug, 2 for Not Unplug
                        PlayerPrefs.SetInt("Act1Scene1_PlugChoice", choiceValue);
                        PlayerPrefs.Save();
                        FirebaseManager.Instance.SaveChoiceToFirebase("Act1Scene1_PlugChoice", choiceValue);
                        Debug.Log("Plug Choice Recorded: " + (savePlug ? "Unplug" : "Did Not Unplug"));
                    }
                }
                else
                {
                    Debug.LogError("Error checking Firebase for existing choice: " + task.Exception);
                }
            });
    }



    public void OnUnPlugChoice()
    {
        RecordPlugChoice(true); 
    }

    public void OnDontUnPlugChoice()
    {
        RecordPlugChoice(false);
    }

    public void PlayerToDiningArea(Transform LocationPosition)
    {
        PlayerScript.instance.playerMovement.gameObject.transform.position = LocationPosition.position;
    }

}
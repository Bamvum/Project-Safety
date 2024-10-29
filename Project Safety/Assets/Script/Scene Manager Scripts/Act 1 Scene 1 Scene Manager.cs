using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Cinemachine;

public class Act1StudentSceneManager : MonoBehaviour
{
    public static Act1StudentSceneManager instance {get; private set;}

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

        PlayerPrefs.SetInt("House Scene", 1);
        FirebaseManager.Instance.SaveChapterUnlockToFirebase("House Scene", true);
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
        // Save choice as integer in PlayerPrefs: 1 for Unplug, 2 for not Unlpug
        int choiceValue = savePlug ? 1 : 2;
        PlayerPrefs.SetInt("Act1Scene1_PlugChoice", choiceValue);
        PlayerPrefs.Save();

        // Log the choice for debugging
        Debug.Log("Plug Choice Recorded: " + (savePlug ? "UnPlug" : "Did Not UnPlug"));

        // Upload the choice to Firebase
        FirebaseManager.Instance.SaveChoiceToFirebase("Act1Scene1_PlugChoice", choiceValue);
    }

    
    public void OnUnPlugChoice()
    {
        RecordPlugChoice(true); 
    }

    public void OnDontUnPlugChoice()
    {
        RecordPlugChoice(false);
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Act1Scene2SceneManager : MonoBehaviour
{
    public static Act1Scene2SceneManager instance {get; private set;}
    
    [Header("Dialogue Triggers")]
    [SerializeField] DialogueTrigger startDialogueTriggerEnglish;
    [SerializeField] DialogueTrigger startDialogueTriggerTagalog;
    
    [Header("Language Preference")]
    [SerializeField] GameObject englishLanguage;
    [SerializeField] GameObject tagalogLanguage;

    [Space(5)]
    [SerializeField] InstructionSO englishInstructionSO;
    [SerializeField] InstructionSO tagalogInstructionSO;

    [Space(5)]
    public int languageIndex;

    [Space(10)]
    [SerializeField] AudioSource fireTruckSirenSFX;
    [SerializeField] GameObject bus;
    [SerializeField] GameObject firetruck;
    
    [Space(10)]
    [SerializeField] AudioSource doorPlayerAudio;
    [SerializeField] AudioClip doorSFX;

    [Header("Flag")]
    bool audioRepeat;
    [SerializeField] bool lerpFireTruck;
    [SerializeField] bool lerpBus;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Start()
    {
        PlayerPrefs.SetInt("Neighborhood Scene", 1);

        // FADE IMAGE ALPHA SET 1
        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                                LoadingSceneManager.instance.fadeImage.color.g,
                                                                LoadingSceneManager.instance.fadeImage.color.b,
                                                                1);

        Cursor.lockState = CursorLockMode.Locked;


        Debug.LogWarning("");
        if (SettingMenu.instance.languageDropdown.value == 0)
        {
            Debug.LogWarning("English Preference");
            InstructionManager.instance.instructionsSO = englishInstructionSO;
            englishLanguage.SetActive(true);
            tagalogLanguage.SetActive(false);
            languageIndex = 0;
        }
        else
        {
            Debug.LogWarning("Tagalog Preference");
            InstructionManager.instance.instructionsSO = tagalogInstructionSO;
            englishLanguage.gameObject.SetActive(false);
            tagalogLanguage.gameObject.SetActive(true);
            languageIndex = 1;
        }

        StartCoroutine(FadeOutEffect());
    }

    IEnumerator FadeOutEffect()
    {
        yield return new WaitForSeconds(1);

        // FADEOUT EFFECTS
            LoadingSceneManager.instance.fadeImage
                .DOFade(0, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
            {
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);

                audioRepeat = true;

                if (languageIndex == 0)
                {
                    startDialogueTriggerEnglish.StartDialogue();
                }
                else
                {
                    startDialogueTriggerTagalog.StartDialogue();
                }
            });


    }

    void Update()
    {

       VehicleLerp();
    }

    void VehicleLerp()
    {
        if (lerpFireTruck)
        {
            if (!fireTruckSirenSFX.isPlaying)
            {
                fireTruckSirenSFX.Play();
            }

            firetruck.transform.position = Vector3.MoveTowards(firetruck.transform.position, new Vector3(107, firetruck.transform.position.y, 34), Time.deltaTime * 25f);

            if (firetruck.transform.position == new Vector3(107, firetruck.transform.position.y, 34))
            {
                lerpFireTruck = false;
                Destroy(firetruck);
            }
        }

        if (lerpBus)
        {
            bus.transform.position = Vector3.MoveTowards(bus.transform.position, new Vector3(107, bus.transform.position.y, 34), Time.deltaTime * 25f);
            if (bus.transform.position == new Vector3(107, bus.transform.position.y, 34))
            {
                lerpBus = false;
            }
        }
    }

    public void enableFireTruckLerp(bool enable)
    {
        lerpFireTruck = enable;
    }

    public void enableBusLerp(bool enable)
    {
        bus.SetActive(true);
        lerpBus = enable;
    }

}

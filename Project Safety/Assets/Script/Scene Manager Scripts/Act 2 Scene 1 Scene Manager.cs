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
        instance = this;
    }

    [Header("Trigger Dialogue")]
    [SerializeField] DialogueTrigger startDialogueEnglish;
    [SerializeField] DialogueTrigger startDialogueTagalog;
    [SerializeField] DialogueTrigger PlaceHolderCalmAndCallEnglish;
    [SerializeField] DialogueTrigger PlaceHolderCalmAndCallTagalog;

    [Space(5)]
    public int languageIndex;

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

        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                         LoadingSceneManager.instance.fadeImage.color.g,
                                                         LoadingSceneManager.instance.fadeImage.color.b,
                                                         1);

        StartCoroutine(FadeOutEffect());

        if(SettingMenu.instance.languageDropdown.value == 0) // English
        {
            languageIndex = 0;
        }
        else
        {
            languageIndex = 1;
        }
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

            if(languageIndex == 0)
            {
                startDialogueTagalog.StartDialogue();
            }
            else
            {
                startDialogueTagalog.StartDialogue();
            }
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

        if(languageIndex == 0)
        {
            PlaceHolderCalmAndCallEnglish.StartDialogue();

        }
        else
        {
            PlaceHolderCalmAndCallTagalog.StartDialogue();
        }
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
}

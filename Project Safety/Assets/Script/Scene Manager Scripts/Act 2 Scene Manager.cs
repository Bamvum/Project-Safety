using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class Act2SceneManager : MonoBehaviour
{
    public static Act2SceneManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }

    [Header("Trigger Dialogue")]
    [SerializeField] DialogueTrigger startDialogue;
    [SerializeField] DialogueTrigger PlaceHolderCalmAndCall;

    [Header("Cinemachine")]
    [SerializeField] CinemachineInputProvider chairInputProvider;

    void Start()
    {
        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                         LoadingSceneManager.instance.fadeImage.color.g,
                                                         LoadingSceneManager.instance.fadeImage.color.b,
                                                         1);

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
            chairInputProvider.enabled = true;
            startDialogue.StartDialogue();
        });
    }

    // FOR TESTING
    public void PlaceHolderForCall()
    {
        StartCoroutine(DelayFunction());
    }

    IEnumerator DelayFunction()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("PlaceHolderForCall");
        PlaceHolderCalmAndCall.StartDialogue();
    }


    // public void EndOfScene()
    // {
    //     LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

    //     LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
    //         .SetEase(Ease.Linear)
    //         .OnComplete(() =>
    //     {
    //         PlayerScript.instance.DisablePlayerScripts();

    //         LoadingSceneManager.instance.loadingScreen.SetActive(true);
    //         LoadingSceneManager.instance.enabled = true;
    //         LoadingSceneManager.instance.sceneName = "Act 2";
    //     });
    // }
}

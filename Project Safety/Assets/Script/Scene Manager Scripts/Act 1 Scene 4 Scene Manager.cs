using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Act1Scene4SceneManager : MonoBehaviour
{
    public static Act1Scene4SceneManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }

    // VARIABLE
    [Header("Dialogue Triggers")]
    [SerializeField] DialogueTrigger startDialogueTrigger;

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
            startDialogueTrigger.StartDialogue();
        });
    }
}

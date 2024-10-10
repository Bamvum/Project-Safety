using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Act2Scene2SceneManager : MonoBehaviour
{
    public static Act2Scene2SceneManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }

    [Header("Trigger Dialogue")]
    [SerializeField] DialogueTrigger startDialogue;

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
        
    }
}

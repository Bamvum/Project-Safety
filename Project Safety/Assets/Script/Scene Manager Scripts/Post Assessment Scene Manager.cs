using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PostAssessmentSceneManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] CanvasGroup sceneNameText;

    [Header("Audio")]
    [SerializeField] AudioSource sceneBGM;

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
        yield return new WaitForSeconds(1);
        // sceneNameText.DOFade(1, 1).OnComplete(() =>
        // {
        //     sceneNameText.DOFade(1,1).OnComplete(() =>
        //     {
        //         sceneNameText.DOFade(0, 1);
        //     });
        // });

        sceneNameText.DOFade(1, 1);
        
        yield return new WaitForSeconds(1);

        sceneNameText.DOFade(0, 1);

        yield return new WaitForSeconds(5);

        LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
                HomeworkManager.instance.enabled = true;
                sceneBGM.Play();
            });
    }
}

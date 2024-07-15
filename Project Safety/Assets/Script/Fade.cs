using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Fade : MonoBehaviour
{
    [SerializeField] Image blackImage;

    // [Header("Tweening")]
    // [SerializeField] float fadeInDuration = 1f;
    // [SerializeField] float fadeOutDuration = 1f;
    // [SerializeField] float delayBeforeFadeOut = 1f;

    void Start()
    {

    }

    public void FadeIn(float fadeInDuration)
    {
        blackImage.DOFade(1, fadeInDuration);
    }

    public void FadeOut(float fadeOutDuration)
    {
        blackImage.DOFade(0, fadeOutDuration);
    }
}

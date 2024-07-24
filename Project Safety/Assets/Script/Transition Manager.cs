using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TransitionManager : MonoBehaviour
{
    [Header("Transition")]
    [SerializeField] Image transitionImage;

    [Header("How to Play (Instruction)")]
    public CanvasGroup instructionHUD;


    public bool isDoneTransition;

    void Start()
    {
        transitionImage.DOFade(0, 2).OnComplete(() =>
        {
            DisplayInstruction();
        });
    }

    void DisplayInstruction()
    {
        instructionHUD.DOFade(1, 1.5f);
    }

    public void TransitionFadeIn()
    {
        transitionImage.DOFade(1, 2).SetEase(Ease.InBack);
    }

    public void TransitionFadeOut()
    {
        transitionImage.DOFade(0, 2);
    }
}

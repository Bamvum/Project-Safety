using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using DG.Tweening;


public class Head : MonoBehaviour
{
    public RectTransform dialogueBox;
    public float punchDuration = 0.3f;
    public float punchScale = 0.2f;

    void Start()
    {
        TestingDoTween();
    }

    void TestingDoTween()
    {
        // Set initial state
        dialogueBox.localScale = Vector3.zero;
        dialogueBox.gameObject.SetActive(true);

        // Punch scale effect
        dialogueBox.DOScale(Vector3.one, 0.1f).OnComplete(() => 
        {
            dialogueBox.DOPunchScale(Vector3.one * punchScale, punchDuration, 10, 1);
        });
    }
}

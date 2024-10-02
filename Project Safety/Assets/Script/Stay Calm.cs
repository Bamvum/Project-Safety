using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StayCalm : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("HUD")]
    [SerializeField] CanvasGroup stayCalmHUD;
    [SerializeField] RectTransform stayCalmRectTransform;
    [SerializeField] CanvasGroup stayCalmCG;

    [Space(10)]
    [SerializeField] int inputsPerformed;
    [SerializeField] int inputNeedToFinish;

    [Header("Flag")]
    bool canInput;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        playerControls.StayCalm.Enable();

        StayCalmTrigger();

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void StayCalmTrigger()
    {
        HUDManager.instance.playerHUD.SetActive(false);

        Invoke("DisplayInstruciton", 5);
    }

    void DisplayInstruction()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(stayCalmRectTransform.DOAnchorPos(new Vector2(0f, 425f), .75f));
        sequence.Append(stayCalmRectTransform.DOScale(new Vector2(1f, 1f), 1f));

        sequence.SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            sequence.Join(stayCalmCG.DOFade(1, 1));
            Debug.Log("Sequence Completed");

            canInput = true;
        });
    }
}

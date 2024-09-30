using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SqueezeandSweepFireExtinguisher : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Script")]
    [SerializeField] TPASS tpass;
    [SerializeField] AimFireExtinguisher aimFE;

    [Header("HUD")]
    [SerializeField] CanvasGroup squeezeAndSweepHUD;
    [SerializeField] RectTransform squeezeAndSweepRectTransform;
    [SerializeField] CanvasGroup squeezeAndSweepCG;

    [Header("Flag")]
    [SerializeField] bool canInput;


    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.SqueezeSweepFE.Enable();

        // INTANTIATE
        SqueezeandSweepFireExtinguisherInstance();

        SqueezeandSweepFireExtinguisherTrigger();

    }

    void OnDisable()
    {
        playerControls.SqueezeSweepFE.Disable();
    }

    void SqueezeandSweepFireExtinguisherInstance()
    {
        squeezeAndSweepRectTransform.anchoredPosition = Vector3.zero;
        squeezeAndSweepRectTransform.localScale = new Vector3(2, 2, 2);
        squeezeAndSweepCG.alpha = 0;
        tpass.tpassBackgroundCG.alpha = 1;
        aimFE.aimHUD.gameObject.SetActive(false);
    }

    void SqueezeandSweepFireExtinguisherTrigger()
    {
        // HUDS
        HUDManager.instance.playerHUD.SetActive(false);

        // PLAYER SCRIPTS
        PlayerScript.instance.playerMovement.enabled = false;
        PlayerScript.instance.cinemachineInputProvider.enabled = false;
        PlayerScript.instance.interact.enabled = false;
        PlayerScript.instance.stamina.enabled = false;

        // CINEMACHINE PRIORITY
        // PlayerScript.instance.playerVC.Priority = 0;
        // tpass.SqueezeAndSweepVC.Priority = 10;

        squeezeAndSweepHUD.gameObject.SetActive(true);
        squeezeAndSweepHUD.DOFade(1, 1).SetEase(Ease.Linear);

        Invoke("DisplayInstruction", 5);

    }

    void DisplayInstruction()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(squeezeAndSweepRectTransform.DOAnchorPos(new Vector2(0f, 425f), .75f));
        sequence.Join(squeezeAndSweepRectTransform.DOScale(new Vector3(1f, 1f, 1f), 1f));

        sequence.SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            sequence.Join(squeezeAndSweepCG.DOFade(1f, 1f));
            Debug.Log("Sequence Completed!");

            canInput = true;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

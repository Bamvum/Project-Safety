using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;

public class Pause : MonoBehaviour
{
    public static Pause instance {get; private set;}

    void Awake()
    {
        playerControls = new PlayerControls();
        instance = this;
    }

    PlayerControls playerControls;

    [Header("HUD")]
    [SerializeField] RectTransform pauseHUDRectTransform;
    [SerializeField] CanvasGroup pauseButtonCG;

    [Space(5)]
    [SerializeField] TMP_Text currentMissionText;
    
    [Header("HUD")]
    [SerializeField] GameObject lastSelectedButton; // FOR GAMEPAD
    
    [Space(5)]
    [SerializeField] GameObject pauseSelectedButton; 

    [Header("Flag")]
    public bool isPause;

    void OnEnable()
    {
        playerControls.Pause.Action.performed += ToPause;
        playerControls.Pause.Enable();
    }

    private void ToPause(InputAction.CallbackContext context)
    {
        if(!isPause)
        {
            ShowPause();
        }
        else 
        {
            HidePause();
        }
    }

    void OnDisable()
    {
        playerControls.Pause.Disable();
    }

    [ContextMenu("Display PauseHUD")]
    void ShowPause()
    {
        Time.timeScale = 0;
        pauseHUDRectTransform.gameObject.SetActive(true);
        pauseHUDRectTransform.DOAnchorPos(new Vector2(585, pauseHUDRectTransform.anchoredPosition.y), 1.5f)
            .SetEase(Ease.OutElastic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                pauseButtonCG.interactable = true;
                isPause = true;
            });
    }

    [ContextMenu("Hide PauseHUD")]
    void HidePause()
    {
        pauseButtonCG.interactable = false;
        pauseHUDRectTransform.DOAnchorPos(new Vector2(1335, pauseHUDRectTransform.anchoredPosition.y), 1.5f)
            .SetEase(Ease.InElastic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                pauseHUDRectTransform.gameObject.SetActive(false);
                Time.timeScale = 1;
                isPause = false;
                // this.enabled = false;
            });

    }

    
}

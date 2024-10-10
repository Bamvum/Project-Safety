using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] TMP_Text currentMissionText;
    
    [Space(5)]
    [SerializeField] RectTransform pauseHUDRectTransform;
    [SerializeField] CanvasGroup pauseButtonCG;

    [Space(5)]
    [SerializeField] RectTransform settingRectTransform;
    [SerializeField] CanvasGroup settinButtonCG;
    
    [Header("HUD")]
    [SerializeField] GameObject lastSelectedButton; // FOR GAMEPAD
    
    [Space(5)]
    [SerializeField] GameObject pauseSelectedButton; 
    [SerializeField] GameObject settingSelectedButton; 

    [Header("Flag")]
    public bool isPause;
    public bool isGamepad;

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

    void Update()
    {
                // GAMEPAD VIBRATION ON NAVIGATION 
        if (Gamepad.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            if (Gamepad.current.leftStick.ReadValue() != Vector2.zero || Gamepad.current.dpad.ReadValue() != Vector2.zero)
            {
                GameObject currentSelectedButton = EventSystem.current.currentSelectedGameObject;

                // Check if the selected UI element has changed (button navigation)
                if (currentSelectedButton != lastSelectedButton)
                {
                    // Trigger vibration when navigating to a new button
                    VibrateGamepad();
                    lastSelectedButton = currentSelectedButton; // Update the last selected button
                }
            }
        }
    }

    private void VibrateGamepad()
    {
        // Set a short vibration
        Gamepad.current.SetMotorSpeeds(0.3f, 0.3f); // Adjust the intensity here
        Invoke("StopVibration", 0.1f); // Stops vibration after 0.1 seconds
    }


    private void StopVibration()
    {
        Gamepad.current.SetMotorSpeeds(0, 0);
    }


    void DeviceInputCheckerUI()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            if(pauseHUDRectTransform.gameObject.activeSelf || settingRectTransform.gameObject.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                EventSystem.current.SetSelectedGameObject(null);
                isGamepad = false;
            }

        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            Cursor.lockState = CursorLockMode.Locked;

            if(!isGamepad)
            {
                if(pauseHUDRectTransform.gameObject.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(pauseSelectedButton);
                    isGamepad = true;
                }

                if(settingRectTransform.gameObject.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(settingSelectedButton);
                    isGamepad = true;
                }
                
            }
        }
    }

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

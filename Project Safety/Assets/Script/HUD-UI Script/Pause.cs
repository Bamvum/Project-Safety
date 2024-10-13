using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] CanvasGroup settingButtonCG;
    
    [Header("Selected Button")]
    [SerializeField] GameObject lastSelectedButton; // FOR GAMEPAD
    
    [Space(5)]
    [SerializeField] GameObject pauseSelectedButton; 
    [SerializeField] GameObject settingSelectedButton; 

    [Header("Audio")]
    [SerializeField] AudioSource bgm;

    [Header("Flag")]
    public bool isPause;
    public bool isGamepad;

    void Start()
    {
        pauseHUDRectTransform.gameObject.SetActive(false);
        pauseHUDRectTransform.localScale = Vector3.zero;
    }

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
        DeviceInputCheckerUI();

        // GAMEPAD VIBRATION ON NAVIGATION 
        if (Gamepad.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            if (Gamepad.current.leftStick.ReadValue() != Vector2.zero || Gamepad.current.dpad.ReadValue() != Vector2.zero)
            {
                GameObject currentSelectedButton = EventSystem.current.currentSelectedGameObject;

                Debug.Log("Inputed Leftstick and Dpad");
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
        // Time.timeScale = 0;
        // CAN DISABLE SCRIPTS

        pauseHUDRectTransform.gameObject.SetActive(true);
        pauseButtonCG.interactable = false;

        bgm.DOFade(.25f, 1);
        pauseHUDRectTransform.DOScale(Vector3.one, .5f).SetEase(Ease.OutBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                pauseButtonCG.interactable = true;
                isPause = true;
            });
    }

    void HidePause()
    {
        bgm.DOFade(1, 1);
        pauseButtonCG.interactable = false;
        pauseHUDRectTransform.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                pauseHUDRectTransform.gameObject.SetActive(false);
                // Time.timeScale = 1;
                isPause = false;
            });

    }


    public void DisplaySetting()
    {
        settingRectTransform.sizeDelta = new Vector2(0, 1080);

        pauseButtonCG.interactable = false;
        isGamepad = true;

        settingRectTransform.gameObject.SetActive(true);
        settingRectTransform.DOSizeDelta(new Vector2(1920, settingRectTransform.sizeDelta.y), .25f)
            .SetEase(Ease.InFlash)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                settingButtonCG.gameObject.SetActive(true);
                settingButtonCG.DOFade(1, .25f).SetUpdate(true).OnComplete(() =>
                {
                    settingButtonCG.interactable = true;
                    isGamepad = false;
                });
            });
    }

    public void SettingBack()
    {
        settingButtonCG.DOFade(0, .25f).SetUpdate(true).OnComplete(() =>
        {
            settingButtonCG.gameObject.SetActive(false);
            settingButtonCG.interactable = false;
            isGamepad = true;

            settingRectTransform.DOSizeDelta(new Vector2(settingRectTransform.sizeDelta.x, 0), .25f)
                .SetEase(Ease.OutFlash)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    settingRectTransform.gameObject.SetActive(false);
                    // PAUSE PROPERRTIES
                });
        });
    }
    
}

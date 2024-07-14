using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.iOS;
using UnityEngine.UI;

public class TwistFireExtinguisher : MonoBehaviour
{
    PlayerControls playerControls;
    [Header("Scripts")]
    TPASS tpass;

    [Header("HUD")]
    [SerializeField] GameObject twistFEHUD;
    [Space(10)]
    [SerializeField] Image[] imageHUD;
    [SerializeField] Sprite[] keyboardSprite;
    [SerializeField] Sprite[] gamepadSprite;


    [Space(10)]
    [SerializeField] int inputNeedToFinish;
    [SerializeField] int inputsPerformed;
    bool isButtonPressed1;
    bool isButtonPressed2;



    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.TwistFE.Button1.performed += Button1Pressed;
        playerControls.TwistFE.Button2.performed += Button2Pressed;
        
        playerControls.TwistFE.Enable();
    }

    private void Button1Pressed(InputAction.CallbackContext context)
    {
        if(isButtonPressed2)
        {
            Debug.Log("Button 1 is Pressed!");
            isButtonPressed1 = true;
            isButtonPressed2 = false;

            inputsPerformed++;
        }
    }

    private void Button2Pressed(InputAction.CallbackContext context)
    {
        if(isButtonPressed1)
        {
            Debug.Log("Button 2 is Pressed!");
            isButtonPressed1 = false;
            isButtonPressed2 = true;

            inputsPerformed++;
        }
    }

    void OnDisable()
    {
        playerControls.TwistFE.Disable();
    }

    void Update()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            ChangeImageStatus(keyboardSprite[0], keyboardSprite[1]);
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            ChangeImageStatus(gamepadSprite[0], gamepadSprite[1]);
        }

    }

    void ChangeImageStatus(Sprite button1Sprite, Sprite button2Sprite)
    {
        imageHUD[0].sprite = button1Sprite;
        imageHUD[1].sprite = button2Sprite;
    }

}

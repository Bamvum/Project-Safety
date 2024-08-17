using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EmergencyHotline : MonoBehaviour
{
    [Header("Cinemachine")]
    [SerializeField] CinemachineVirtualCamera playerVC;
    [SerializeField] CinemachineVirtualCamera phoneVC;

    [Header("Animation")]
    [SerializeField] AnimationClip phoneAnim;
    [SerializeField] float phoneAnimLength;

    [Header("HUD")]
    [SerializeField] GameObject contactHUD;
    
    [Space(10)]
    [SerializeField] Image[] choiceImage;
    [SerializeField] Sprite[] keyboardSprite;
    [SerializeField] Sprite[] gamepadSprite;
    
    [Space(15)]
    [SerializeField] GameObject phoneGO;
    bool contactMode;


    void Awake()
    {
        phoneAnimLength = phoneAnim.length;
        
        ScriptManager.instance.playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        ScriptManager.instance.playerControls.Contact.Contact1.performed += AccessContact1;
        ScriptManager.instance.playerControls.Contact.Contact2.performed += AccessContact2;
        ScriptManager.instance.playerControls.Contact.Contact3.performed += AccessContact3;

        ScriptManager.instance.playerControls.Contact.Enable();
    }

    private void AccessContact1(InputAction.CallbackContext context)
    {
        if(contactMode)
        {
            Debug.Log("Accessing Contact 1");
            contactMode = false;

            // TODO - WORK IN PROGRESS. INVOKE IS FOR TESTING PURPORSE ONLY
            Invoke("PreEndOfPhoneCall", 2);
        }
    }

    private void AccessContact2(InputAction.CallbackContext context)
    {
        if(contactMode)
        {
            Debug.Log("Accessing Contact 2");
            contactMode = false;
        }
    }

    private void AccessContact3(InputAction.CallbackContext context)
    {
        if(contactMode)
        {
            Debug.Log("Accessing Contact 3");
            contactMode = false;
        }
    }

    void OnDisable()
    {
        ScriptManager.instance.playerControls.Contact.Disable();
    }

    public void PhoneTrigger()
    {
        // GAMEOBJECT;
        phoneGO.SetActive(true);

        // ANIMATION
        ScriptManager.instance.playerMovement.playerAnim.SetBool("Idle", true);
        ScriptManager.instance.playerMovement.playerAnim.SetBool("Phone", true);

        // CINEMACHINE
        playerVC.Priority = 0;
        phoneVC.Priority = 10;

        // HUD
        HUDManager.instance.playerHUD.SetActive(false);

        // DISABLE SCRIPTS
        ScriptManager.instance.playerMovement.enabled = false;
        ScriptManager.instance.interact.enabled = false;
        ScriptManager.instance.stamina.enabled = false;
        ScriptManager.instance.cinemachineInputProvider.enabled = false;
        
        // DISABLE SCRIPTS
        this.enabled = true;

        // DELAY (CALLING METHOD)
        Invoke("StartPhoneCall", phoneAnimLength);
    }

    void Update()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            // ChangeImageStatus(oneSprite, twoSprite, threeSprite);
            ChangeImageStatus(keyboardSprite[0], keyboardSprite[1], keyboardSprite[2]);
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            // ChangeImageStatus(squareSprite, triangleSprite, circleSprite);
            ChangeImageStatus(gamepadSprite[0], gamepadSprite[1], gamepadSprite[2]);
        }
    }

    void StartPhoneCall()
    {
        //HUD
        contactHUD.SetActive(true);
        contactMode = true;
    }

    void PreEndOfPhoneCall()
    {
        //ANIMATION
        ScriptManager.instance.playerMovement.playerAnim.SetBool("Phone", false);

        // HUD
        contactHUD.SetActive(false);

        Invoke("EndOfPhoneCall", phoneAnimLength);
    }

    void EndOfPhoneCall()
    {
        // GAMEOBJECT;
        phoneGO.SetActive(false);
        
        //ANIMATION
        ScriptManager.instance.playerMovement.playerAnim.SetBool("Idle", false);
        
        // CINEMACHINE
        playerVC.Priority = 10;
        phoneVC.Priority = 0;

        // HUD
        HUDManager.instance.playerHUD.SetActive(true);

        // ENABLE SCRIPTS
        ScriptManager.instance.playerMovement.enabled = true;
        ScriptManager.instance.interact.enabled = true;
        ScriptManager.instance.stamina.enabled = true;
        ScriptManager.instance.cinemachineInputProvider.enabled = true;

        // DISABLE SCRIPTS
        this.enabled = false;
    }

    void ChangeImageStatus(Sprite choice1Sprite, Sprite choice2Sprite, Sprite choice3Sprite)
    {
        choiceImage[0].sprite = choice1Sprite;
        choiceImage[1].sprite = choice2Sprite;
        choiceImage[2].sprite = choice3Sprite;
    }
}


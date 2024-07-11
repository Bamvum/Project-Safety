using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class EmergencyHotline : MonoBehaviour
{
    PlayerControls playerControls;
    [Header("Scripts")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Interact interact;
    [SerializeField] Stamina stamina;
    [SerializeField] CinemachineInputProvider cinemachineInputProvider;

    [Header("Cinemachine")]
    [SerializeField] CinemachineVirtualCamera playerVC;
    [SerializeField] CinemachineVirtualCamera phoneVC;

    [Header("Animation")]
    [SerializeField] AnimationClip phoneAnim;
    [SerializeField] float phoneAnimLength;

    [Header("HUD")]
    [SerializeField] GameObject playerHUD;
    [SerializeField] GameObject contactHUD;
    
    [Space(10)]
    [SerializeField] Image choice1ImageHUD;
    [SerializeField] Image choice2ImageHUD;
    [SerializeField] Image choice3ImageHUD;
    
    [Space(10)]
    [SerializeField] Sprite oneSprite;
    [SerializeField] Sprite twoSprite;
    [SerializeField] Sprite threeSprite;
    
    [Space(10)]
    [SerializeField] Sprite squareSprite;
    [SerializeField] Sprite triangleSprite;
    [SerializeField] Sprite circleSprite;
    
    [Space(15)]
    [SerializeField] GameObject phoneGO;
    bool contactMode;


    void Awake()
    {
        phoneAnimLength = phoneAnim.length;
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Contact.Contact1.performed += AccessContact1;
        playerControls.Contact.Contact2.performed += AccessContact2;
        playerControls.Contact.Contact3.performed += AccessContact3;

        playerControls.Contact.Enable();
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
        playerControls.Contact.Disable();
    }

    public void PhoneTrigger()
    {
        // GAMEOBJECT;
        phoneGO.SetActive(true);

        // ANIMATION
        playerMovement.playerAnim.SetBool("Idle", true);
        playerMovement.playerAnim.SetBool("Phone", true);

        // CINEMACHINE
        playerVC.Priority = 0;
        phoneVC.Priority = 10;

        // HUD
        playerHUD.SetActive(false);

        // DISABLE SCRIPTS
        playerMovement.enabled = false;
        interact.enabled = false;
        stamina.enabled = false;
        cinemachineInputProvider.enabled = false;
        
        // DISABLE SCRIPTS
        this.enabled = true;

        // DELAY (CALLING METHOD)
        Invoke("StartPhoneCall", phoneAnimLength);
    }

    void Update()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            ChangeImageStatus(oneSprite, twoSprite, threeSprite);
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            ChangeImageStatus(squareSprite, triangleSprite, circleSprite);
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
        playerMovement.playerAnim.SetBool("Phone", false);

        // HUD
        contactHUD.SetActive(false);

        Invoke("EndOfPhoneCall", phoneAnimLength);
    }

    void EndOfPhoneCall()
    {
        // GAMEOBJECT;
        phoneGO.SetActive(false);
        
        //ANIMATION
        playerMovement.playerAnim.SetBool("Idle", false);
        
        // CINEMACHINE
        playerVC.Priority = 10;
        phoneVC.Priority = 0;

        // HUD
        playerHUD.SetActive(true);

        // ENABLE SCRIPTS
        playerMovement.enabled = true;
        interact.enabled = true;
        stamina.enabled = true;
        cinemachineInputProvider.enabled = true;

        // DISABLE SCRIPTS
        this.enabled = false;
    }

    void ChangeImageStatus(Sprite choice1Sprite, Sprite choice2Sprite, Sprite choice3Sprite)
    {
        choice1ImageHUD.sprite = choice1Sprite;
        choice2ImageHUD.sprite = choice2Sprite;
        choice3ImageHUD.sprite = choice3Sprite;
    }
}


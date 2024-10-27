using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EmergencyHotline : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Dialogue Trigger")]
    [SerializeField] DialogueTrigger contact1English;
    [SerializeField] DialogueTrigger contact1Tagalog;

    [SerializeField] DialogueTrigger contact2English;
    [SerializeField] DialogueTrigger contact2Tagalog;
   
    [SerializeField] DialogueTrigger contact3English;
    [SerializeField] DialogueTrigger contact3Tagalog;
    
    [Header("Cinemachine")]
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

            if(Act2Scene1Manager.instance.languageIndex == 0)
            {
                contact1English.StartDialogue();
            }
            else
            {
                contact1Tagalog.StartDialogue();
            }
            
            contactHUD.SetActive(false);

            // TODO - WORK IN PROGRESS. INVOKE IS FOR TESTING PURPORSE ONLY
            // Invoke("PreEndOfPhoneCall", 2);
        }
    }

    private void AccessContact2(InputAction.CallbackContext context)
    {
        if(contactMode)
        {
            Debug.Log("Accessing Contact 2");
            contactMode = false;

            if(Act2Scene1Manager.instance.languageIndex == 0)
            {
                contact2English.StartDialogue();
            }
            else
            {
                contact2Tagalog.StartDialogue();
            }

            contactHUD.SetActive(false);
        }
    }

    private void AccessContact3(InputAction.CallbackContext context)
    {
        if(contactMode)
        {
            Debug.Log("Accessing Contact 3");
            contactMode = false;

            if (Act2Scene1Manager.instance.languageIndex == 0)
            {
                contact3English.StartDialogue();
            }
            else
            {
                contact3Tagalog.StartDialogue();
            }

            contactHUD.SetActive(false);
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
        PlayerScript.instance.playerMovement.playerAnim.SetBool("Idle", true);
        PlayerScript.instance.playerMovement.playerAnim.SetBool("Phone", true);

        // CINEMACHINE
        PlayerScript.instance.playerVC.Priority = 0;
        phoneVC.Priority = 10;

        // HUD
        HUDManager.instance.playerHUD.SetActive(false);

        // DISABLE SCRIPTS
        PlayerScript.instance.playerMovement.enabled = false;
        PlayerScript.instance.interact.enabled = false;
        PlayerScript.instance.stamina.enabled = false;
        PlayerScript.instance.cinemachineInputProvider.enabled = false;
        
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

    public void PreEndOfPhoneCall()
    {
        //ANIMATION
        PlayerScript.instance.playerMovement.playerAnim.SetBool("Phone", false);

        // HUD
        contactHUD.SetActive(false);

        Invoke("EndOfPhoneCall", phoneAnimLength);
    }

    void EndOfPhoneCall()
    {
        // GAMEOBJECT;
        phoneGO.SetActive(false);
        
        //ANIMATION
        PlayerScript.instance.playerMovement.playerAnim.SetBool("Idle", false);
        
        // CINEMACHINE
        PlayerScript.instance.playerVC.Priority = 10;
        phoneVC.Priority = 0;

        // HUD
        HUDManager.instance.playerHUD.SetActive(true);

        // // ENABLE SCRIPTS
        // PlayerScript.instance.playerMovement.enabled = true;
        // PlayerScript.instance.interact.enabled = true;
        // PlayerScript.instance.stamina.enabled = true;
        // PlayerScript.instance.cinemachineInputProvider.enabled = true;

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


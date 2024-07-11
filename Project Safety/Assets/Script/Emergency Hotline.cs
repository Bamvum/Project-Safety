using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.iOS;

public class EmergencyHotline : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Scripts")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Interact interact;
    [SerializeField] Stamina stamina;
    [SerializeField] CinemachineInputProvider cinemachineInputProvider;
    
    [Header("Animation")]
    [SerializeField] AnimationClip phoneOnAnim;
    float phoneOnAnimLength;
    [SerializeField] AnimationClip phoneOffAnim;
    float phoneOffAnimLength;

    [Header("Cinemachine")]
    [SerializeField] CinemachineVirtualCamera playerVC;
    [SerializeField] CinemachineVirtualCamera phoneVC;
    
    [Header("HUD")]
    [SerializeField] GameObject playerHUD;
    [SerializeField] GameObject contactHUD;
    [Space(10)]
    [SerializeField] Image choice1Image;
    [SerializeField] Image choice2Image;
    [SerializeField] Image choice3Image;
    [Space(10)]
    [SerializeField] Sprite oneSprite;
    [SerializeField] Sprite twoSprite;
    [SerializeField] Sprite threeSprite;
    [Space(10)]
    [SerializeField] Sprite leftDpadSprite;
    [SerializeField] Sprite upDpadSprite;
    [SerializeField] Sprite rightDpadSprite;
    [Space(10)]
    [SerializeField] GameObject phone;
    bool contactMode;
    
    // [Space(10)]

    void Awake()
    {
        phoneOnAnimLength = phoneOnAnim.length;
        phoneOffAnimLength = phoneOffAnim.length;

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
            Debug.Log("Access Contact 1");
            // TODO
            
            contactMode = false;
            Invoke("PreEndOfPhoneCall", 2);
        }
    }

    private void AccessContact2(InputAction.CallbackContext context)
    {
        if(contactMode)
        {
            Debug.Log("Access Contact 2");
            // TODO

            contactMode = false;
        }
    }

    private void AccessContact3(InputAction.CallbackContext context)
    {
        if(contactMode)
        {
            Debug.Log("Access Contact 3");
            // TODO
            
            contactMode = false;
        }
    }

    void OnDisable()
    {
        playerControls.Contact.Disable();
    }

    void Update()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            ChangeImageStatus(oneSprite, twoSprite, threeSprite);
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            ChangeImageStatus(leftDpadSprite, upDpadSprite, rightDpadSprite);
        }
    }

    public void PhoneTrigger()
    {
        // HUD
        playerHUD.SetActive(false);

        // ANIMATION
        playerMovement.playerAnim.SetBool("Idle", true);
        playerMovement.playerAnim.SetBool("Phone", true);

        //Cinemachine
        playerVC.Priority = 0;
        phoneVC.Priority = 10;

        phone.SetActive(true);

        Invoke("StartPhoneCall", phoneOnAnimLength);
    }

    void StartPhoneCall()
    {
        contactHUD.SetActive(true);
        contactMode = true;
    }

    void PreEndOfPhoneCall()
    {
        // ANIMATION
        playerMovement.playerAnim.SetBool("Idle", false);
        playerMovement.playerAnim.SetBool("Phone", false);

        // CINEMACHINE
        playerVC.Priority = 10;
        phoneVC.Priority = 0;

        Invoke("EndOfPhoneCall", phoneOffAnimLength);
    }

    void EndOfPhoneCall()
    {
        // HUD
        playerHUD.SetActive(true);
        contactHUD.SetActive(false);

        phone.SetActive(false);

        // ENABLE SCRIPTS
        playerMovement.enabled = true;
        interact.enabled = true;
        stamina.enabled = true;
        cinemachineInputProvider.enabled = true;

        // DISABLE SCRIPT
        this.enabled = false;
    }

    void ChangeImageStatus(Sprite choice1Sprite, Sprite choice2Sprite, Sprite choice3Sprite)
    {
        choice1Image.sprite = choice1Sprite;
        choice2Image.sprite = choice2Sprite;
        choice3Image.sprite = choice3Sprite;
    }
}

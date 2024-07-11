using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Synty.AnimationBaseLocomotion.Samples;
using UnityEngine;
using UnityEngine.InputSystem;

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
    
    [Header("HUD")]
    [SerializeField] GameObject playerHUD;
    [SerializeField] GameObject contactHUD;
    [Space(10)]
    [SerializeField] GameObject phone;
    bool contactMode;
    
    // [Space(10)]

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Contact.Contact1.performed += AccessContact1;
        playerControls.Contact.Enable();
    }

    private void AccessContact1(InputAction.CallbackContext context)
    {
        if(contactMode)
        {

        }
    }

    void OnDisable()
    {
        playerControls.Contact.Disable();
    }

    public void PhoneTrigger()
    {
        // HUD
        playerHUD.SetActive(false);
        // contactHUD.SetActive(true);

        // ANIMATION
        playerMovement.playerAnim.SetBool("Idle", true);
        playerMovement.playerAnim.SetBool("Phone", true);

        //Cinemachine
        playerVC.Priority = 0;
        phoneVC.Priority = 10;

        phone.SetActive(true);


    }

    void EndOfPhoneCall()
    {
        playerMovement.playerAnim.SetBool("Idle", false);
        playerMovement.playerAnim.SetBool("Phone", false);

        playerMovement.enabled = true;
        interact.enabled = true;
        stamina.enabled = true;
        
    }

    
}

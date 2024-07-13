using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.iOS;

public class TPASS : MonoBehaviour
{
    PlayerControls playerControls;
    [Header("Scripts")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Interact interact;
    [SerializeField] Stamina stamina;
    [SerializeField] CinemachineInputProvider cinemachineInputProvider;

    [Header("Animation")]
    [SerializeField] AnimationClip inspectExtinguisherAnim;
    float inspectExtinguisherAnimLength;

    [Header("Cinemachine")]
    [SerializeField] CinemachineVirtualCamera playerVC;
    [SerializeField] CinemachineVirtualCamera extinguisherVC;

    [Header("HUD")]
    [SerializeField] GameObject playerHUD;
    [SerializeField] GameObject extinguisherHUD;
    
    [Space(10)]
    [SerializeField] Image[] TPASSHUD;

    [Space(10)]
    [SerializeField] Sprite[] keyboardSprite;
    [SerializeField] Sprite[] gamepadSprite;
    [SerializeField] GameObject fireExitinguisher;
    bool inspectExtinguisherMode;


    void Awake()
    {
        inspectExtinguisherAnimLength = inspectExtinguisherAnim.length;
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Extinguisher.Twist.performed += ToTwist;
        playerControls.Extinguisher.Pull.performed += ToPull;
        playerControls.Extinguisher.Aim.performed += ToAim;
        playerControls.Extinguisher.Squeeze.performed += ToSqueeze;
        playerControls.Extinguisher.Sweep.performed += ToSweep;

        playerControls.Extinguisher.Enable();
    }

    private void ToTwist(InputAction.CallbackContext context)
    {
        if(inspectExtinguisherMode)
        {
            Debug.Log("To Twist Method!");
            inspectExtinguisherMode = false;
        }
    }
    private void ToPull(InputAction.CallbackContext context)
    {
        if(inspectExtinguisherMode)
        {
            Debug.Log("To Pull Method!");
            inspectExtinguisherMode = false;
        }
    }

    private void ToAim(InputAction.CallbackContext context)
    {
        if(inspectExtinguisherMode)
        {
            Debug.Log("To Aim Method!");
            inspectExtinguisherMode = false;
        }
    }

    private void ToSqueeze(InputAction.CallbackContext context)
    {
        if(inspectExtinguisherMode)
        {
            Debug.Log("To Squeeze Method!");
            inspectExtinguisherMode = false;
        }
    }

    private void ToSweep(InputAction.CallbackContext context)
    {
        if(inspectExtinguisherMode)
        {
            Debug.Log("To Sweep Method!");
            inspectExtinguisherMode = false;
        }
    }
    void OnDisable()
    {
        playerControls.Extinguisher.Disable();
    }

    public void ExtinguisherTrigger()
    {
        // GAME OBJECT
        fireExitinguisher.SetActive(true);

        // ANIMATION
        playerMovement.playerAnim.SetBool("Idle", true);
        playerMovement.playerAnim.SetBool("Extinguisher", true);

        // HUD
        playerHUD.SetActive(false);


        // CINEMACHINE
        playerVC.Priority = 0;
        extinguisherVC.Priority = 10;


        // DISABLE SCRIPT
        playerMovement.enabled = false;
        interact.enabled = false;
        stamina.enabled = false;
        cinemachineInputProvider.enabled = false;

        // ENABLE SCRIPT
        this.enabled = true;

        Invoke("StartExtinguisher", inspectExtinguisherAnimLength);
    }

    void Update()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            ChangeImageStatus(keyboardSprite[0], keyboardSprite[1], keyboardSprite[2], keyboardSprite[3],keyboardSprite[4]);
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            ChangeImageStatus(gamepadSprite[0], gamepadSprite[1], gamepadSprite[2], gamepadSprite[3],gamepadSprite[4]);
        }
    }

    void StartExtinguisher()
    {
        extinguisherHUD.SetActive(true);
        inspectExtinguisherMode = true;
    }

    void ChangeImageStatus(Sprite twistSprite, Sprite pullSprite, Sprite aimSprite,
    Sprite squeezeSprite, Sprite sweepSprite)
    {
        TPASSHUD[0].sprite = twistSprite;
        TPASSHUD[1].sprite = pullSprite;
        TPASSHUD[2].sprite = aimSprite;
        TPASSHUD[3].sprite = squeezeSprite;
        TPASSHUD[4].sprite = sweepSprite;
    }
}

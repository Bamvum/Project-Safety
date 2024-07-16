using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using Unity.VisualScripting;

public class TPASS : MonoBehaviour
{
    PlayerControls playerControls;
    [Header("Scripts")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Interact interact;
    [SerializeField] Stamina stamina;
    [SerializeField] CinemachineInputProvider cinemachineInputProvider;
    [Space(10)]
    [SerializeField] TwistFireExtinguisher twistFE;
    [SerializeField] PullFireExtinguisher pullFE;

    [Header("Animation")]
    [SerializeField] AnimationClip inspectExtinguisherAnim;
    public float inspectExtinguisherAnimLength;

    [Header("Cinemachine")]
    [SerializeField] CinemachineVirtualCamera playerVC;
    [SerializeField] CinemachineVirtualCamera inspectExtinguisherVC;
    [SerializeField] CinemachineVirtualCamera twistExtinguisherVC;
    [SerializeField] CinemachineVirtualCamera pullExtinguisherVC;

    [Header("HUD")]
    [SerializeField] GameObject playerHUD;
    [SerializeField] GameObject extinguisherHUD;
    [SerializeField] Image blackImage;
    
    [Space(10)]
    [SerializeField] Image[] TPASSHUD;

    [Space(10)]
    [SerializeField] Sprite[] keyboardSprite;
    [SerializeField] Sprite[] gamepadSprite;
    [Space(10)]
    [SerializeField] GameObject[] fireExitinguisher;
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
        if(!twistFE.objectiveComplete)
        {
            if(inspectExtinguisherMode)
            {
                Debug.Log("To Twist Method!");
                extinguisherHUD.SetActive(false);

                blackImage.DOFade(1, inspectExtinguisherAnimLength).OnComplete(() =>
                {
                    // CINEMACHINE 
                    inspectExtinguisherVC.Priority = 0;
                    twistExtinguisherVC.Priority = 10;

                    // ANIMATION
                    playerMovement.playerAnim.SetBool("TwistExtinguisher", true);
                    
                    // EXTINGUISHER GAME OBJECT
                    fireExitinguisher[0].SetActive(false);
                    fireExitinguisher[1].SetActive(true);

                    blackImage.DOFade(1,inspectExtinguisherAnimLength).OnComplete(() =>
                    {
                        // FADE
                        blackImage.DOFade(0, inspectExtinguisherAnimLength);
                        twistFE.enabled = true;
                        this.enabled = false;
                    });
                });
            }


            // TODO - PROMPT THAT TWIST IS ALREADY DONE OR 
            inspectExtinguisherMode = false;
        }
    }
    private void ToPull(InputAction.CallbackContext context)
    {
        if(!pullFE.objectiveComplete)
        {
            if (inspectExtinguisherMode)
            {
                Debug.Log("To Pull Method!");
                extinguisherHUD.SetActive(false);

                blackImage.DOFade(1, inspectExtinguisherAnimLength).OnComplete(() =>
                {
                    // CINEMACHINE 
                    inspectExtinguisherVC.Priority = 0;
                    pullExtinguisherVC.Priority = 10;

                    // ANIMATION
                    // playerMovement.playerAnim.SetBool("PullExtinguisher", true);
                    playerMovement.playerAnim.SetBool("TwistExtinguisher", true);

                    // EXTINGUISHER GAME OBJECT
                    fireExitinguisher[0].SetActive(false);
                    fireExitinguisher[1].SetActive(true);

                    blackImage.DOFade(1, inspectExtinguisherAnimLength).OnComplete(() =>
                    {
                        // FADE
                        blackImage.DOFade(0, inspectExtinguisherAnimLength);
                        pullFE.enabled = true;
                        this.enabled = false;
                    });
                });

                inspectExtinguisherMode = false;
            }
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
        fireExitinguisher[0].SetActive(true);
        fireExitinguisher[1].SetActive(false);

        // ANIMATION
        playerMovement.playerAnim.SetBool("Idle", true);
        playerMovement.playerAnim.SetBool("Extinguisher", true);

        // HUD
        playerHUD.SetActive(false);


        // CINEMACHINE
        playerVC.Priority = 0;
        inspectExtinguisherVC.Priority = 10;
        twistExtinguisherVC.Priority = 0;
        pullExtinguisherVC.Priority = 0;        

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

        if(twistFE.objectiveComplete)
        {
            TPASSHUD[0].gameObject.SetActive(false);
        }

        if(pullFE.objectiveComplete)
        {
            TPASSHUD[1].gameObject.SetActive(false);
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

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;


public class TPASS : MonoBehaviour
{    
    PlayerControls playerControls;
    
    [Header("Script")]
    [SerializeField] TwistFireExtinguisher twistFE;
    [SerializeField] PullFireExtinguisher pullFE;
    // [SerializeField] TwistFireExtinguisher twistFE;

    [Header("Fire Extinguisher")]
    [SerializeField] GameObject fireExtinguisher;
    
    [Space(10)]
    [SerializeField] GameObject fireExtinguisherBody;
    [SerializeField] GameObject fireExtinguisherHose;
 
    [Space(10)]
    [SerializeField] GameObject fireExtinguisherToPickUp; // IF ACTIVE FALSE IT CAN BE USE

    [Space(10)]
    [SerializeField] int inputsPerformed;
    [SerializeField] int inputNeedToFinish;
    [SerializeField] bool[] buttonPressed;
    bool inTwistQTE;
    
    // [Header("Pull (TPASS)")]
    // [SerializeField] CanvasGroup pullHUD;
    // [SerializeField] RectTransform pullRectTransform;
    // [SerializeField] CanvasGroup pullCG;

    // [Space(10)]
    // [SerializeField] float pressedValue;
    // [SerializeField] float decreaseValue;

    // [Space(10)]
    // [SerializeField] Image pullControlImage;
    // [SerializeField] Sprite[] pullKeyboardSprite;
    // [SerializeField] Sprite[] pullGamepadSprite;
    // [SerializeField] Slider pullSlider;
    // [SerializeField] int roundNum;
    // bool inPullQTE;

    // [Header("Aim (TPASS)")]
    // [SerializeField] CanvasGroup aimHUD;
    // [SerializeField] RectTransform aimRectTransform;
    // [SerializeField] CanvasGroup aimCG;
    // bool inAimMode;

    [Header("TPASS status")]
    public bool twistAndPull;
    public bool aim;
    public bool squeezeAndSweep;

    [Header("Inputs")]
    bool equipFireExtinguisher;
    bool firstHalfDone;

    [Header("Flag")]
    bool canInput;

    [Space(10)]
    bool canUseFireExtinguisherInv;
    bool inGamePlay;

    void Awake()
    {
        playerControls = new PlayerControls();
    
        // twistRectTransform = twistHUD.GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        playerControls.Extinguisher.EquipExtinguisher.performed += ToEquip;
        playerControls.Extinguisher.PerformTPASS.performed += ToPerformTPASS;

        playerControls.Extinguisher.Enable();
    }

    private void ToEquip(InputAction.CallbackContext context)
    {
        if(!fireExtinguisherToPickUp.activeInHierarchy)
        {
            Debug.Log("To Equip");
            
            if (!equipFireExtinguisher)
            {
                if (firstHalfDone)
                {
                    Debug.Log("First Half Done");
                    // ANIMATION FIRE EXTINGUISHER IDLE WALK 
                    fireExtinguisherBody.SetActive(true);
                    fireExtinguisherHose.SetActive(true);
                }
                else
                {
                    PlayerScript.instance.playerMovement.playerAnim.SetBool("Extinguisher Walk", true);
                    fireExtinguisher.SetActive(true);
                }

                equipFireExtinguisher = true;
                canUseFireExtinguisherInv = true;
            }
            else
            {
                if (firstHalfDone)
                {
                    Debug.Log("First Half Done");
                    // ANIMATION FIRE EXTINGUISHER IDLE WALK 
                    fireExtinguisherBody.SetActive(false);
                    fireExtinguisherHose.SetActive(false);
                }
                else
                {
                    PlayerScript.instance.playerMovement.playerAnim.SetBool("Extinguisher Walk", false);
                    fireExtinguisher.SetActive(false);
                }

                equipFireExtinguisher = false;
                canUseFireExtinguisherInv = false;
            }
        }
    }
    
    private void ToPerformTPASS(InputAction.CallbackContext context)
    {
        if (equipFireExtinguisher)
        {
            if (!twistAndPull)
            {
                Debug.Log("Perform Twist and Pull QTE!");

                twistFE.enabled = true;
                twistFE.TwistFireExtinguisherTrigger();
                
                this.enabled = false;
            }
            else 
            {
                // PERFORM SQUEEZE AND SWEEP
            }
        }
    }
}


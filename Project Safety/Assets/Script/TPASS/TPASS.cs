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

    [Header("Fire Extinguisher")]
    [SerializeField] GameObject fireExtinguisher;
    
    [Space(10)]
    [SerializeField] GameObject fireExtinguisherBody;
    [SerializeField] GameObject fireExtinguisherHose;
 
    [Space(10)]
    [SerializeField] GameObject fireExtinguisherToPickUp; // IF ACTIVE FALSE IT CAN BE USE

    public bool aimMode;

    [Header("TPASS status")]
    public bool twistDone;
    public bool pullDone;
    // public bool aim;
    // public bool squeezeAndSweep;

    [Header("Inputs")]
    bool equipFireExtinguisher;
    public bool firstHalfDone;

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
        playerControls.TPASS.EquipExtinguisher.performed += ToEquip;
        playerControls.TPASS.PerformTPASS.performed += ToPerformTPASS;

        playerControls.TPASS.Enable();
    }

    private void ToEquip(InputAction.CallbackContext context)
    {
        if(!fireExtinguisherToPickUp.activeInHierarchy)
        {
            Debug.Log("To Equip");
            
            if (!equipFireExtinguisher)
            {
                if (twistDone && pullDone)
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
            if (!twistDone && !pullDone)
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

    void OnDisable()
    {
        playerControls.TPASS.Disable();
    }

}


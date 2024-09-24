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
    [SerializeField] AimFireExtinguisher aimFE;

    [Header("Fire Extinguisher")]
    public GameObject fireExtinguisher;
    
    [Space(10)]
    public GameObject fireExtinguisherBody;
    public GameObject fireExtinguisherHose;
 
    [Space(10)]
    [SerializeField] GameObject fireExtinguisherToPickUp;

    [Header("Cinemachine")]
    public CinemachineVirtualCamera twistAndPullVC;
    
    [Header("TPASS status")]
    public bool twistDone;
    public bool pullDone;
    // public bool aim;
    // public bool squeezeAndSweep;

    [Header("Inputs")]
    public bool equipFireExtinguisher;

    [Header("Flag")]
    public GameObject tpassHUD;
    public Image checkMarkDone;
    public AudioSource correctSFX;
    


    [Space(10)]
    bool canUseFireExtinguisherInv;

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
                    PlayerScript.instance.playerMovement.playerAnim.SetBool("Extinguisher Aim Walk", true);
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
                if (twistDone && pullDone)
                {
                    Debug.Log("First Half Done");
                    // ANIMATION FIRE EXTINGUISHER IDLE WALK 
                    PlayerScript.instance.playerMovement.playerAnim.SetBool("Extinguisher Aim Walk", false);
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


                PlayerScript.instance.playerMovement.enabled = false;
                PlayerScript.instance.cinemachineInputProvider.enabled = false;
                PlayerScript.instance.interact.enabled = false;
                PlayerScript.instance.stamina.enabled = false;

                PlayerScript.instance.playerVC.Priority = 0;
                twistAndPullVC.Priority = 10;

                tpassHUD.SetActive(true);
                
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

    void Update()
    {

    }

}


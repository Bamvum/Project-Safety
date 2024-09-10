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
    public static TPASS instance {get; private set;}

    void Awake()
    {
        instance = this;
        playerControls = new PlayerControls();
    }
    
    PlayerControls playerControls;
    
    [Header("Fire Extinguisher")]
    [SerializeField] GameObject fireExtinguisher;
    
    [Space(10)]
    [SerializeField] GameObject fireExtinguisherBody;
    [SerializeField] GameObject fireExtinguisherHose;
 
    [Space(10)]
    [SerializeField] GameObject fireExtinguisherToPickUp; // IF ACTIVE FALSE IT CAN BE USE

    [Header("TPASS status")]
    public bool twistAndPull;
    public bool aim;
    public bool squeezeAndSweep;

    [Header("Inputs")]
    bool equipFireExtinguisher;
    bool firstHalfDone;

    [Header("Flag")]
    bool canUseFireExtinguisherInv;



    void OnEnable()
    {
        playerControls.Extinguisher.EquipExtinguisher.performed += ToEquip;
        playerControls.Extinguisher.PerformTPASS.performed += ToPerformTPASS;

        playerControls.Extinguisher.TwistButton1.performed += Button1Pressed;
        playerControls.Extinguisher.TwistButton2.performed += Button2Pressed;
        playerControls.Extinguisher.TwistButton3.performed += Button3Pressed;
        playerControls.Extinguisher.TwistButton4.performed += Button4Pressed;

        playerControls.Extinguisher.Enable();
    }

    private void ToEquip(InputAction.CallbackContext context)
    {
        Debug.Log("To Equipt");
        if(!fireExtinguisherToPickUp.activeInHierarchy)
        {
            if(!equipFireExtinguisher)
            {
                if(firstHalfDone)
                {
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
                if(firstHalfDone)
                {
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
                Debug.Log("Perform Twist and Pull Sequence!");
                // PERFORM TWIST AND PULL MINIGAME
                firstHalfDone = true;
            }
        }
    }
    
    #region  - TWIST (TPASS) -

    private void Button1Pressed(InputAction.CallbackContext context)
    {

    }

    private void Button2Pressed(InputAction.CallbackContext context)
    {

    }

    private void Button3Pressed(InputAction.CallbackContext context)
    {

    }
    
    private void Button4Pressed(InputAction.CallbackContext context)
    {

    }

    #endregion

    void OnDisable()
    {
        playerControls.Extinguisher.Disable();
    }

    void Update()
    {

    }


}

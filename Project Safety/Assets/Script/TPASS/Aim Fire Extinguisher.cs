using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimFireExtinguisher : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Script")]
    [SerializeField] TPASS tpass;
    // [SerializeField] SqueezeAndSweep squeezeAndSweep;


    [Header("Fire Extinguisher Type")]

    // FIRE TYPES:
    // A - SOLID MATERIALS
    // B - INVOLVES LIQUIDS
    // C - INVOLVES ENERGIZED ELECTRICAL EQUIPMENT
    // D - INVOLVES METAL
    // K - INVOLVES COOKING MATERIAL

    public bool powder; // CLASS A, B, C, D FIRE ONLY
    public bool foam; // CLASS A, B FIRE ONLY
    public bool co2; // CLASS B, D FIRE ONLY
    public bool water; // CLASS A FIRE ONLY
    public bool wetChemical; // CLASS K FIRE ONLY

    [Header("HUD")]
    [SerializeField] CanvasGroup aimHUD;
    [SerializeField] RectTransform aimRectTransform;
    [SerializeField] CanvasGroup tpassBackgroundCG;

    [Space(10)]
    [SerializeField] GameObject extinguishImage;

    [Header("Raycast")]
    [SerializeField] float interactRange = 1.5f;
    [SerializeField] RaycastHit hit;


    void OnEnable()
    {
        // playerControls.AimFE.Action.performed += ctx => actionPressed = true;
        // playerControls.AimFE.Action.canceled += ctx => actionPressed = false;

        playerControls.AimFE.Action.performed += ToAction;
        playerControls.AimFE.Enable();

        // INSTANCE
        AimFireExtinguisherInstance();

        AimFireExtinguisherTrigger();
    }

    private void ToAction(InputAction.CallbackContext context)
    {
        
    }

    void OnDisable()
    {

        playerControls.AimFE.Disable();
    }

    public void AimFireExtinguisherInstance()
    {
        aimRectTransform.anchoredPosition = Vector3.zero;
        aimRectTransform.localScale = new Vector3(2, 2, 2);

    }

    public void AimFireExtinguisherTrigger()
    {
        // HUDManager.instance.playerHUD
        HUDManager.instance.missionHUD.SetActive(false);

        // PLAYER SCRIPTS
        PlayerScript.instance.interact.enabled = false;

        aimHUD.gameObject.SetActive(true);
        aimHUD.DOFade(1, 1).SetEase(Ease.Linear);

        Invoke("DisplayInstruction", 5);
    }

    void DisplayInstruction()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(aimRectTransform.DOAnchorPos(new Vector2(0f, 425f), .75f));
        sequence.Join(aimRectTransform.DOScale(new Vector3(1f, 1f, 1f), 1f));

        sequence.SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            // sequence.Join(twistCG.DOFade(1f, 1f));
            Debug.Log("Sequence Completed!");

            // canInput = true;
        });
    }

    void ResetPorperties()
    {
        // RAYCAST
    }

    void Update()
    {
        if (hit.collider != null)
        {
            extinguishImage.SetActive(false);
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            // 
            PowderFEType();

            if(powder)
            {
                Debug.Log("Powder Fire Extinguisher");
                PowderFEType();
            }

            if(foam)
            {
                Debug.Log("Foam Fire Extinguisher");
                FoamFEType();
            }

            if(co2)
            {
                Debug.Log("CO2 Fire Extinguisher");
                CO2FEType();
            }
            
            if(water)
            {
                Debug.Log("Water Fire Extinguisher");
                WaterFEType();
            }
        
            if(wetChemical)
            {
                Debug.Log("Wet Chemical Fire Extinguisher");
                WetChemicalFEType();
            }
        }
    }
    void PowderFEType()
    {
        /* 
            Powder Fire Extinguisher is SAFE for class:
            A (Ordinary Combustibles)        9
            B (Flammable Liquids or Gas)     10
            C (Electrical)                   11
            D (Metal Fires)                  12
        */

        if (hit.collider.gameObject.layer == 9 || hit.collider.gameObject.layer == 10 ||
            hit.collider.gameObject.layer == 11 || hit.collider.gameObject.layer == 12)
        {
            // SQUEEZE AND SWEEP FUNCTION
            extinguishImage.SetActive(true);
        }

        /* 
            Powder Fire Extinguisher is NOT SAFE for class:
            K (Kitchen Fires)                13
        */

        else if (hit.collider.gameObject.layer == 13)
        {
            // DISPLAY WRONG
        }
    }

    void FoamFEType()
    {
        /* 
            Foam Fire Extinguisher is SAFE for class:
            A (Ordinary Combustibles)        9
            B (Flammable Liquids or Gas)     10

        */

        if (hit.collider.gameObject.layer == 9 || hit.collider.gameObject.layer == 10)
        {
            // SQUEEZE AND SWEEP FUNCTION
            extinguishImage.SetActive(true);
        }

        /* 
            Foam Fire Extinguisher is NOT SAFE for class:
            C (Electrical)                   11
            D (Metal Fires)                  12
            K (Kitchen Fires)                13
        */

        else if (hit.collider.gameObject.layer == 11 || hit.collider.gameObject.layer == 12 ||
                hit.collider.gameObject.layer == 13)
        {
            // DISPLAY WRONG
        }
    }

    void CO2FEType()
    {
        /* 
            CO2 Fire Extinguisher is SAFE for class:
            B (Flammable Liquids or Gas)     10
            C (Electrical)                   11
            
        */

        if (hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 11 )
        {
            // SQUEEZE AND SWEEP FUNCTION
            extinguishImage.SetActive(true);
        }

        /* 
            CO2 Fire Extinguisher is NOT SAFE for class:
            A (Ordinary Combustibles)        9
            D (Metal Fires)                  12
            K (Kitchen Fires)                13
        */

        else if (hit.collider.gameObject.layer == 9 || hit.collider.gameObject.layer == 12 ||
                hit.collider.gameObject.layer == 13)
        {
            // DISPLAY WRONG
            extinguishImage.SetActive(true);
        }
    }

    void WaterFEType()
    {
         /* 
            Powder Fire Extinguisher is SAFE for class:
            A (Ordinary Combustibles)        9

        */

        if (hit.collider.gameObject.layer == 9)
        {
            // SQUEEZE AND SWEEP FUNCTION
            extinguishImage.SetActive(true);
        }

        /* 
            Powder Fire Extinguisher is NOT SAFE for class:
            B (Flammable Liquids or Gas)     10
            C (Electrical)                   11
            D (Metal Fires)                  12
            K (Kitchen Fires)                13
        */

        else if (hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 11 || 
                hit.collider.gameObject.layer == 12 || hit.collider.gameObject.layer == 13)
        {
            // DISPLAY WRONG
        }
    }

    void WetChemicalFEType()
    {
        /* 
            Powder Fire Extinguisher is SAFE for class:
            A (Ordinary Combustibles)        9
            K (Kitchen Fires)                13
        */

        if (hit.collider.gameObject.layer == 9 || hit.collider.gameObject.layer == 13)
        {
            // SQUEEZE AND SWEEP FUNCTION
            extinguishImage.SetActive(true);
        }

        /* 
            Powder Fire Extinguisher is NOT SAFE for class:
            B (Flammable Liquids or Gas)     10
            C (Electrical)                   11
            D (Metal Fires)                  12
        */

        else if (hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 11 || 
                hit.collider.gameObject.layer == 12)
        {
            // DISPLAY WRONG
            extinguishImage.SetActive(false);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class AimFireExtinguisher : MonoBehaviour
{
    PlayerControls playerControls;

    [SerializeField] GameObject fireExtinguisherInHand;
    [SerializeField] Rigidbody fireExtinguisherInHandRb;

    [Header("Script")]
    [SerializeField] TPASS tpass;
    [SerializeField] SqueezeandSweepFireExtinguisher squeezeAndSweepFE;

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
    public CanvasGroup aimHUD;
    public RectTransform aimRectTransform;

    [Space(10)]
    [SerializeField] GameObject extinguishImage;

    [Header("Raycast")]
    [SerializeField] float interactRange = 1.5f;
    [SerializeField] RaycastHit hit;
    
    [Space(10)]
    [SerializeField] bool canInput;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.AimFE.Action.performed += ToAction;
        playerControls.AimFE.Drop.performed += ToDrop;

        playerControls.AimFE.Enable();

        // INSTANCE
        AimFireExtinguisherInstance();

        AimFireExtinguisherTrigger();
    }

    private void ToAction(InputAction.CallbackContext context)
    {
        if(PlayerScript.instance.interact.inHandItem != null)
        {
            // SQUEEZE AND SWEEP FUNCTION
            if(canInput)
            {
                squeezeAndSweepFE.fireInteracted = hit.collider.gameObject;
                squeezeAndSweepFE.enabled = true;
                this.enabled = false;
            }
        }
    }

    private void ToDrop(InputAction.CallbackContext context)
    {
        Debug.Log("Drop!~");
        if(canInput)
        {
            if (PlayerScript.instance.interact.inHandItem != null)
            {
                HUDManager.instance.missionHUD.SetActive(true);
                PlayerScript.instance.interact.inHandItem.transform.SetParent(null);
                PlayerScript.instance.interact.inHandItem.layer = 6;

                if (PlayerScript.instance.interact.inHandItemRB != null)
                {
                    PlayerScript.instance.interact.inHandItemRB.isKinematic = false;
                }

                PlayerScript.instance.interact.inHandItem = null;
                PlayerScript.instance.interact.inHandItemRB = null;

                PlayerScript.instance.interact.leftHandExtinguisher.weight = 0;
                PlayerScript.instance.interact.rightHandExtinguisher.weight = 0;

                PlayerScript.instance.interact.enabled = true;
                this.enabled = false;
            }
        }
    }

    void OnDisable()
    {
        // tpass.tpassHUD.SetActive(false);
        aimHUD.gameObject.SetActive(false);

        playerControls.AimFE.Disable();
    }

    public void AimFireExtinguisherInstance()
    {
        aimHUD.alpha = 1;
        aimRectTransform.anchoredPosition = Vector3.zero;
        aimRectTransform.localScale = new Vector3(2, 2, 2);
        tpass.tpassBackgroundCG.alpha = 1;

        fireExtinguisherInHand = PlayerScript.instance.interact.inHandItem;
        fireExtinguisherInHandRb = PlayerScript.instance.interact.inHandItemRB;
    }

    public void AimFireExtinguisherTrigger()
    {
        // HUDManager.instance.playerHUD
        HUDManager.instance.missionHUD.SetActive(false);
        tpass.tpassHUD.SetActive(true);

        // PLAYER SCRIPTS
        PlayerScript.instance.interact.enabled = false;

        aimHUD.gameObject.SetActive(true);
        aimHUD.DOFade(1, 1).SetEase(Ease.Linear);

        Invoke("DisplayInstruction", 1.5f);
    }

    void DisplayInstruction()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(aimRectTransform.DOAnchorPos(new Vector2(0f, 425f), .75f));
        sequence.Join(aimRectTransform.DOScale(new Vector3(1f, 1f, 1f), 1f));

        sequence.SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            sequence.Join(tpass.tpassBackgroundCG.DOFade(0f, 1f));
            Debug.Log("Sequence Completed!");

            canInput = true;
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
            // DISPLAY VISUAL
            extinguishImage.SetActive(true);
            canInput = true;
        }

        /* 
            Powder Fire Extinguisher is NOT SAFE for class:
            K (Kitchen Fires)                13
        */

        else if (hit.collider.gameObject.layer == 13)
        {
            canInput = false;
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
            // DISPLAY VISUAL
            extinguishImage.SetActive(true);
            canInput = true;
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
            canInput = false;
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
            // DISPLAY VISUAL
            extinguishImage.SetActive(true);
            canInput = true;
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
            canInput = false;
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
            // DISPLAY VISUAL
            extinguishImage.SetActive(true);
            canInput = true;
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
            canInput = false;
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
            // DISPLAY VISUAL
            extinguishImage.SetActive(true);
            canInput = true;
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
            // DISPLAY VISUAL
            canInput = false;
        }
    }
}

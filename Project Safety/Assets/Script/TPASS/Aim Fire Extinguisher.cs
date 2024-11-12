using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.IO;

public class AimFireExtinguisher : MonoBehaviour
{
    PlayerControls playerControls;

    [SerializeField] GameObject fireExtinguisherInHand;
    [SerializeField] Rigidbody fireExtinguisherInHandRb;

    [Header("Script")]
    [SerializeField] TPASS tpass;
    [SerializeField] SqueezeandSweepFireExtinguisher squeezeAndSweepFE;
    public StageOfFire stageOfFire;

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
    [SerializeField] RectTransform aimRectTransform;
    [SerializeField] CanvasGroup aimCG;
    [SerializeField] RectTransform gameplayRectTransform;
    [SerializeField] CanvasGroup gameplayCG;
    [SerializeField] CanvasGroup examineFireCG;

    [Space(10)]
    [SerializeField] CanvasGroup titleStageOfFire;
    [SerializeField] TMP_Text stageOfFireTxt;

    [Space(5)]
    [SerializeField] RectTransform[] parentChoices;
    [SerializeField] Image[] choices;
    [SerializeField] Sprite[] keyboardSprite;
    [SerializeField] Sprite[] gamepadSprite;

    [Space(10)]
    [SerializeField] GameObject extinguishImage;

    [Header("Raycast")]
    [SerializeField] float interactRange = 1.5f;
    [SerializeField] RaycastHit hit;
    
    [Space(10)]
    [SerializeField] bool isExtinguishable;
    [SerializeField] bool canInput;
    [SerializeField] bool examineMode;
    [SerializeField] AudioSource wrongSFX;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        // playerControls.AimFE.Action.performed += ToAction;
        playerControls.AimFE.Drop.performed += ToDrop;
        playerControls.AimFE.Examine.performed += ToExamine;

        playerControls.AimFE.Option1.performed += ToOption1;
        playerControls.AimFE.Option2.performed += ToOption2;

        playerControls.AimFE.Enable();

        // INSTANCE
        AimFireExtinguisherInstance();

        AimFireExtinguisherTrigger();
    }

    #region - DROP -

    private void ToDrop(InputAction.CallbackContext context)
    {
        Debug.Log("Drop!~");

        if (!examineMode && PlayerScript.instance.interact.inHandItem != null)
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

    #endregion
    
    #region  - EXAMINE -
    
    private void ToExamine(InputAction.CallbackContext context)
    {
        if(!examineMode && PlayerScript.instance.interact.inHandItem != null)
        {
            if(canInput)
            {
                Debug.Log("Examine Fire!~");
                examineMode = true;

                Sequence sequence = DOTween.Sequence();

                // DISABLE MOVEMENT
                PlayerScript.instance.playerMovement.enabled = false;
                PlayerScript.instance.cinemachineInputProvider.enabled = false;
                PlayerScript.instance.stamina.enabled = false;

                // DISABLE AIM HUD
                aimCG.DOFade(0, 1).SetEase(Ease.Linear).SetUpdate(true);
                gameplayCG.DOFade(0, 1).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
                {
                    aimRectTransform.gameObject.SetActive(false);
                    gameplayRectTransform.gameObject.SetActive(false);

                    examineFireCG.gameObject.SetActive(true);

                    examineFireCG.DOFade(1, 1).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
                    {
                        titleStageOfFire.DOFade(1, 1).SetEase(Ease.Linear).SetUpdate(true);

                        choices[0].gameObject.SetActive(true);
                        choices[1].gameObject.SetActive(true);

                        parentChoices[0].DOScale(Vector3.one, .1f).OnComplete(() =>
                        {
                            parentChoices[0].DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1).SetUpdate(true);
                        });

                        parentChoices[1].DOScale(Vector3.one, .1f).OnComplete(() =>
                        {
                            parentChoices[1].DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1).SetUpdate(true);
                        });
                    });
                });


                // DISPLAY AIM - EXAMINE HUD  


                // DISPLAY FIRE'S STAGE
                if (stageOfFire.incipientStage)
                {
                    stageOfFireTxt.text = "INCIPIENT STAGE";
                }
                else if (stageOfFire.growthStage)
                {
                    stageOfFireTxt.text = "GROWTH STAGE";
                }
                else if (stageOfFire.fullyDevelopStage)
                {
                    stageOfFireTxt.text = "FULLY DEVELOP STAGE";
                }

                // IDENTIFY IF FIRE IS EXTINGUISHABLE






                // IDENTIFY IF CORRECT FIRE EXTINGUISHER IS USING AND WHAT TYPE OF FIRE
                // IS BURNING

                // CAN EXIT
            }

        }
    }

    #endregion

    
    #region - OPTIONS -

    private void ToOption1(InputAction.CallbackContext context)
    {
        if (examineFireCG.gameObject.activeSelf)
        {
            Debug.Log("Option 1 Pressed!");

            // EXIT AIM - EXAMINE HUD
            examineFireCG.gameObject.SetActive(false);

            // EXIT AIM - EXAMINE HUD
            AimFireExtinguisherInstance();
            AimFireExtinguisherTrigger();
            
            // DISABLE MOVEMENT
            PlayerScript.instance.playerMovement.enabled = true;
            PlayerScript.instance.cinemachineInputProvider.enabled = true;
            PlayerScript.instance.stamina.enabled = true;


            examineMode = false;
        }
    }
    
    private void ToOption2(InputAction.CallbackContext context)
    {
        if (examineFireCG.gameObject.activeSelf)
        {
            if (stageOfFire.fullyDevelopStage)
            {
                wrongSFX.Play();
            }
            else
            {
                Debug.Log("Extinguish! -  To Squeeze & Sweep");
                squeezeAndSweepFE.fireInteracted = hit.collider.gameObject;
                squeezeAndSweepFE.enabled = true;
                this.enabled = false;
                examineMode = false;

            }
        }

    }

    #endregion

    void OnDisable()
    {
        // tpass.tpassHUD.SetActive(false);
        aimHUD.gameObject.SetActive(false);

        playerControls.AimFE.Disable();

        canInput = false;
    }

    public void AimFireExtinguisherInstance()
    {
        aimCG.gameObject.SetActive(true);
        gameplayCG.gameObject.SetActive(true);
        examineFireCG.gameObject.SetActive(false);
        
        aimHUD.alpha = 1;
        aimCG.alpha = 1;
        gameplayCG.alpha = 1;
        examineFireCG.alpha = 0;
        
        parentChoices[0].localScale =  Vector3.zero;
        parentChoices[1].localScale =  Vector3.zero;

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
        });
    }
    void Update()
    {
        if (DeviceManager.instance.keyboardDevice)
        {
            ChangeImageStatus(keyboardSprite[0], keyboardSprite[1]);
        }
        else
        {
            ChangeImageStatus(gamepadSprite[0], gamepadSprite[1]);
        }

        if (hit.collider != null)
        {
            stageOfFire = null;
            extinguishImage.SetActive(false);
            canInput = false;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            // DISPLAY VISUAL
            stageOfFire = hit.collider.GetComponent<StageOfFire>();
            
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

    #region - FIRE EXITNGUISHER TYPE - 
    
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
            extinguishImage.SetActive(true);
            canInput = true;
        }

        /* 
            Powder Fire Extinguisher is NOT SAFE for class:
            K (Kitchen Fires)                13
        */

        // else if (hit.collider.gameObject.layer == 13)
        // {
        //     canInput = false;
        //     // DISPLAY WRONG
        // }

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

        // else if (hit.collider.gameObject.layer == 11 || hit.collider.gameObject.layer == 12 ||
        //         hit.collider.gameObject.layer == 13)
        // {
        //     // DISPLAY WRONG
        //     canInput = false;
        // }
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

        // else if (hit.collider.gameObject.layer == 9 || hit.collider.gameObject.layer == 12 ||
        //         hit.collider.gameObject.layer == 13)
        // {
        //     // DISPLAY WRONG
        //     canInput = false;
        // }
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

        // else if (hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 11 || 
        //         hit.collider.gameObject.layer == 12 || hit.collider.gameObject.layer == 13)
        // {
        //     // DISPLAY WRONG
        //     canInput = false;
        // }
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

        // else if (hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 11 || 
        //         hit.collider.gameObject.layer == 12)
        // {
        //     // DISPLAY VISUAL
        //     canInput = false;
        // }
    }

    #endregion 

    #region  - CHANGE STATUS IMAGE - 

    void ChangeImageStatus(Sprite extinguishSprite, Sprite exitSprite)
    {
        choices[0].sprite = exitSprite;
        choices[1].sprite = extinguishSprite;
    }

    #endregion
}

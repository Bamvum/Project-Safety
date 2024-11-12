using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SqueezeandSweepFireExtinguisher : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Script")]
    [SerializeField] TPASS tpass;
    [SerializeField] AimFireExtinguisher aimFE;

    
    [Header("HUD")]
    [SerializeField] CanvasGroup squeezeAndSweepHUD;
    [SerializeField] RectTransform squeezeAndSweepRectTransform;
    [SerializeField] CanvasGroup squeezeAndSweepCG;
    
    [Space(10)]
    [SerializeField] Slider squeezeAndSweepSlider;
    [SerializeField] Image fireHealthParent1;
    [SerializeField] Image fireHealthVisual1;
    [SerializeField] Image fireHealthParent2;
    [SerializeField] Image fireHealthVisual2;
    [SerializeField] Image fireHealthParent3;
    [SerializeField] Image fireHealthVisual3;
    [SerializeField] float fireMaxHealth;

    
    [Space(10)]
    [SerializeField] Image visualsImage;
    [SerializeField] Sprite[] visualSprite;
    
    [Header("Fire Extinguisher")]
    public GameObject fireInteracted;
    [SerializeField] ParticleSystem fireInteractedPS;
    [SerializeField] GameObject particleParent;
    [SerializeField] ParticleSystem.MinMaxCurve fireExtinguisherPSEmission;
    [SerializeField] ParticleSystem fireExtinguisherPS;

    [Space(10)]
    public GameObject fireSpread1;
    [SerializeField] ParticleSystem fireSpread1PS;
    public GameObject fireSpread2;
    [SerializeField] ParticleSystem fireSpread2PS;
    // [SerializeField] ParticleSystem.EmissionModule fireSpread1PSEmission;
    // [SerializeField] ParticleSystem.EmissionModule fireSpread2PSEmission;


    [Header("Flag")]
    [SerializeField] bool actionPressed;
    [SerializeField] int wrongInput;
    [SerializeField] bool stopFireSpread;
    [SerializeField] int fireSpread;
    [SerializeField] bool canInput;
    bool sliderLoop = true;
    [SerializeField] float sliderSpeed = .2f;

    [Space(10)]
    public float rotationSpeed = 2.0f; // Speed of rotation
    public float rotationRange = 15.0f; // Range of rotation (-15 to 15)

    [Header("Audio")]
    [SerializeField] AudioSource badSFX;
    [SerializeField] AudioSource goodSFX;
    [SerializeField] AudioSource perfectSFX;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.SqueezeSweepFE.Action.performed += ctx => actionPressed = true;
        playerControls.SqueezeSweepFE.Action.canceled += ctx => actionPressed = false;

        playerControls.SqueezeSweepFE.Enable();

        // INTANTIATE
        SqueezeandSweepFireExtinguisherInstance();

        SqueezeandSweepFireExtinguisherTrigger();

    }

    void OnDisable()
    {
        playerControls.SqueezeSweepFE.Disable();

        PlayerScript.instance.playerMovement.enabled = true;
        PlayerScript.instance.cinemachineInputProvider.enabled = true;
        PlayerScript.instance.stamina.enabled = true;
    }

    void SqueezeandSweepFireExtinguisherInstance()
    {
        squeezeAndSweepRectTransform.anchoredPosition = Vector3.zero;
        squeezeAndSweepRectTransform.localScale = new Vector3(2, 2, 2);
        squeezeAndSweepCG.alpha = 0;
        tpass.tpassBackgroundCG.alpha = 1;
        aimFE.aimHUD.gameObject.SetActive(false);

        fireInteractedPS = fireInteracted.GetComponent<ParticleSystem>();
        fireMaxHealth = fireInteractedPS.emission.rateOverTime.constant;

        fireHealthParent2.gameObject.SetActive(false);
        fireHealthParent3.gameObject.SetActive(false);
        fireHealthVisual1.fillAmount = fireMaxHealth / 100f; // If fireMaxHealth is out of 100
        fireHealthVisual2.fillAmount = fireMaxHealth / 100f; // If fireMaxHealth is out of 100
        fireHealthVisual3.fillAmount = fireMaxHealth / 100f; // If fireMaxHealth is out of 100
        // Set fireMaxHealth to Image Filled Amount

        rotationSpeed = 0;
        particleParent.transform.localRotation = Quaternion.Euler(0, particleParent.transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }

    void SqueezeandSweepFireExtinguisherTrigger()
    {
        // HUDS
        HUDManager.instance.playerHUD.SetActive(false);

        // PLAYER SCRIPTS
        PlayerScript.instance.playerMovement.enabled = false;
        PlayerScript.instance.cinemachineInputProvider.enabled = false;
        PlayerScript.instance.interact.enabled = false;
        PlayerScript.instance.stamina.enabled = false;

        // CINEMACHINE PRIORITY
        PlayerScript.instance.playerVC.Priority = 0;
        tpass.SqueezeAndSweepVC.Priority = 10;

        squeezeAndSweepHUD.gameObject.SetActive(true);
        squeezeAndSweepHUD.DOFade(1, 1).SetEase(Ease.Linear);

        Invoke("DisplayInstruction", 5);

    }

    void DisplayInstruction()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(squeezeAndSweepRectTransform.DOAnchorPos(new Vector2(0f, 425f), .75f));
        sequence.Join(squeezeAndSweepRectTransform.DOScale(new Vector3(1f, 1f, 1f), 1f));

        sequence.SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            sequence.Join(squeezeAndSweepCG.DOFade(1f, 1f));
            particleParent.SetActive(true);

            Debug.Log("Sequence Completed!");

            canInput = true;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (DeviceManager.instance.keyboardDevice)
        {
            visualsImage.sprite = visualSprite[0];
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            visualsImage.sprite = visualSprite[1];
        }


        var fireInteractedPSEmission = fireInteractedPS.emission;

        // if(fireSpread1 != null)
        // {
        //     var fireSpread1PSEmission = fireSpread1PS.emission;
        // }

        // if(fireSpread2 != null)
        // {
        //     var fireSpread2PSEmission = fireSpread2PS.emission;
        // }
        

        if (fireHealthVisual1.fillAmount <= 0)
        {
            if (canInput)
            {
                Debug.Log("Done");
                canInput = false;

                fireHealthVisual1.fillAmount = .01f;

                squeezeAndSweepCG.DOFade(0, 1).OnComplete(() =>
                {
                    tpass.checkMarkDone.gameObject.SetActive(true);
                    tpass.correctSFX.Play();

                    tpass.checkMarkDone.DOFade(1, 1).OnComplete(() =>
                    {
                        tpass.checkMarkDone.DOFade(0, 1).OnComplete(() =>
                        {
                            tpass.SqueezeAndSweepVC.Priority = 0;
                            PlayerScript.instance.playerVC.Priority = 10;

                            tpass.checkMarkDone.gameObject.SetActive(false);

                            squeezeAndSweepHUD.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
                            {
                                aimFE.aimHUD.gameObject.SetActive(true);

                                squeezeAndSweepHUD.gameObject.SetActive(false);

                                HUDManager.instance.playerHUD.SetActive(true);
                                particleParent.SetActive(false);

                                fireInteracted.gameObject.SetActive(false);

                                this.enabled = false;
                                aimFE.enabled = true;
                            });
                        });
                    });
                });
            }
        }
        else
        {
            if (canInput)
            {
                if (actionPressed)
                {
                    if (squeezeAndSweepSlider.value >= 0.475f && squeezeAndSweepSlider.value <= 0.525f)
                    {
                        Debug.Log("Perfect!");
                        perfectSFX.Play();
                        

                        fireHealthVisual1.fillAmount -= .09f;
                        rotationSpeed += 15;
                        fireInteractedPSEmission.rateOverTime = fireInteractedPSEmission.rateOverTime.constant - 15;

                        // if (fireSpread1 != null)
                        // {
                        //     fireHealthVisual2.fillAmount -= .09f;
                        //     fireSpread1PSEmission.rateOverTime = fireSpread1PSEmission.rateOverTime.constant - 15;
                        // }

                        // if(fireSpread2 != null)
                        // {
                        //     fireHealthVisual3.fillAmount -= .09f;
                        //     fireSpread2PSEmission.rateOverTime = fireSpread2PSEmission.rateOverTime.constant - 15;
                        // }
                    }
                    else if ((squeezeAndSweepSlider.value >= 0.3f && squeezeAndSweepSlider.value <= 0.475f) || (squeezeAndSweepSlider.value > 0.525f && squeezeAndSweepSlider.value <= 0.7f))
                    {
                        Debug.Log("Good!");
                        goodSFX.Play();

                        fireHealthVisual1.fillAmount -= .075f;
                        rotationSpeed += 7.5f;
                        fireInteractedPSEmission.rateOverTime = fireInteractedPSEmission.rateOverTime.constant - 7.5f;

                        // if (fireSpread1 != null)
                        // {
                        //     fireHealthVisual2.fillAmount -= .075f;
                        //     fireSpread1PSEmission.rateOverTime = fireSpread1PSEmission.rateOverTime.constant - 7.5f;
                        // }

                        // if(fireSpread2 != null)
                        // {
                        //     fireHealthVisual3.fillAmount -= .075f;
                        //     fireSpread2PSEmission.rateOverTime = fireSpread2PSEmission.rateOverTime.constant - 7.5f;
                        // }

                    }
                    else if ((squeezeAndSweepSlider.value >= 0f && squeezeAndSweepSlider.value <= 0.3f) || (squeezeAndSweepSlider.value >= 0.7f && squeezeAndSweepSlider.value <= 1f))
                    {
                        Debug.Log("Bad!");
                        badSFX.Play();

                        fireHealthVisual1.fillAmount -= .001f;
                        rotationSpeed += 1;
                        wrongInput++;
                        fireInteractedPSEmission.rateOverTime = fireInteractedPSEmission.rateOverTime.constant - 1;

                        // if (fireSpread1 != null)
                        // {
                        //     fireHealthVisual2.fillAmount -= .001f;
                        //     fireSpread1PSEmission.rateOverTime = fireSpread1PSEmission.rateOverTime.constant - 1;
                        // }

                        // if(fireSpread2 != null)
                        // {
                        //     fireHealthVisual3.fillAmount -= .001f;
                        //     fireSpread2PSEmission.rateOverTime = fireSpread2PSEmission.rateOverTime.constant - 1;
                        // }
                    }

                    if (wrongInput == 2)
                    {
                        wrongInput = 0;

                        // if (fireSpread == 0)
                        // {
                        //     fireSpread1 = Instantiate(fireInteracted, fireInteracted.transform);
                        //     fireSpread1.transform.localPosition = new Vector3(-0.2f, 0, 0);
                        //     fireSpread1.transform.localRotation = Quaternion.identity;
                        //     fireSpread1.transform.localScale = Vector3.one;

                        //     fireSpread1PS = fireSpread1.GetComponent<ParticleSystem>();
                            
                        //     fireHealthParent2.gameObject.SetActive(true);
                        //     fireHealthVisual2.fillAmount = fireHealthVisual1.fillAmount;

                        //     fireSpread++;
                        // }
                        // else if (fireSpread == 1)
                        // {
                           
                        //     fireSpread2 = Instantiate(fireSpread1, fireInteracted.transform);
                        //     fireSpread2.transform.localPosition = new Vector3(0.2f, 0, 0);
                        //     fireSpread2.transform.localRotation = Quaternion.identity;
                        //     fireSpread2.transform.localScale = Vector3.one;

                        //     fireSpread2PS = fireSpread2.GetComponent<ParticleSystem>();
                            
                        //     fireHealthParent3.gameObject.SetActive(true);
                        //     fireHealthVisual3.fillAmount = fireHealthVisual1.fillAmount;

                        //     fireSpread++;
                        // }

                        if (aimFE.stageOfFire.incipientStage)
                        {
                            aimFE.stageOfFire.incipientStage = false;
                            aimFE.stageOfFire.growthStage = true;
                        }
                        else if (aimFE.stageOfFire.growthStage) 
                        {
                            // GAME OVER
                        }
                        else if (aimFE.stageOfFire.fullyDevelopStage)
                        {

                        }

                    }


                    StartCoroutine(DelayCanInput());
                }
            }
        }
        
        // SLIDER

        if (sliderLoop)
        {
            squeezeAndSweepSlider.value += sliderSpeed * Time.deltaTime;
            if (squeezeAndSweepSlider.value >= 1f)
            {
                squeezeAndSweepSlider.value = 1f;
                sliderLoop = false; // Change direction
            }
        }
        else
        {
            squeezeAndSweepSlider.value -= sliderSpeed * Time.deltaTime;
            if (squeezeAndSweepSlider.value <= 0f)
            {
                squeezeAndSweepSlider.value = 0f;
                sliderLoop = true; // Change direction
            }
        }
    
        float rotationX = Mathf.PingPong(Time.time * rotationSpeed, rotationRange * 2) - rotationRange;
        particleParent.transform.localRotation = Quaternion.Euler(rotationX, particleParent.transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
          
    }


    IEnumerator DelayCanInput()
    {
        // HIDE VISUAL
        visualsImage.gameObject.SetActive(false);
        canInput = false;

        yield return new WaitForSeconds(2);
        
        visualsImage.gameObject.SetActive(true);
        canInput = true;
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PullFireExtinguisher : MonoBehaviour
{
    PlayerControls playerControls;
    [Header("Scripts")]
    [SerializeField] TPASS tpass;
    // [SerializeField] AimFireExtinguisher aimFE;

    [Header("Value")]
    [SerializeField] float pressedValue = 0.99f;
    [SerializeField] float decreaseValue = 0.01f;

    [Header("HUD")]
    [SerializeField] CanvasGroup pullHUD;
    [SerializeField] RectTransform pullRectTransform;
    [SerializeField] CanvasGroup pullCG;

    [Space(10)]
    [SerializeField] Slider pullSlider;
    [SerializeField] TMP_Text roundText;
    
    [Space(10)]
    [SerializeField] Image imageHUD;
    [SerializeField] Sprite[] keyboardSprite;
    [SerializeField] Sprite[] gamepadSprite;

    [Header("Fire Extinguisher")]
    [SerializeField] GameObject pullPin;


    bool canInput;
    bool actionLock;
    int roundNum;


    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.PullFE.Action.performed += ToAction;
        playerControls.PullFE.ActionLock.performed += ToActionLock;

        playerControls.PullFE.Enable();

        // INSTANTIATE
        PullFireExtinguisherInstance();

        PullFireExtinguisherTrigger();
    }

    private void ToAction(InputAction.CallbackContext context)
    {
        if(canInput)
        {
            if(roundNum == 0)
            {
                pressedValue = 8.5f;
            }
            else if (roundNum == 1)
            {
                pressedValue = 5;
            }
            else if (roundNum == 2)
            {
                pressedValue = 3.5f;
            }

            pullSlider.value += pressedValue * Time.deltaTime;
        }
    }

    private void ToActionLock(InputAction.CallbackContext context)
    {
        if(canInput)
        {
            if(roundNum <= 2)
            {
                if (pullSlider.value >= 0.985f && pullSlider.value <= pullSlider.maxValue)
                {
                    Debug.Log("Action Lock Pressed!");
                    // roundText.text = roundNum + 1  + " / 3"; 

                    canInput = false;
                    roundNum++;

                    pullCG.DOFade(0, 1).OnComplete(() =>
                    {
                        pullCG.DOFade(0, 1).OnComplete(() =>
                        {
                            pullSlider.value = 0;

                            pullCG.DOFade(1, 1).OnComplete(() =>
                            {
                                CanInputDelay();
                            });
                        });

                    });
                }
            }

        }
    }
    
    
    void OnDisable()
    {
        playerControls.PullFE.Disable();

        // PLAYER SCRIPTS
        PlayerScript.instance.playerMovement.enabled = true;
        PlayerScript.instance.cinemachineInputProvider.enabled = true;
        PlayerScript.instance.interact.enabled = true;
        PlayerScript.instance.stamina.enabled = true;
    }

    public void PullFireExtinguisherInstance()
    {
        pullRectTransform.anchoredPosition = Vector3.zero;
        pullRectTransform.localScale = new Vector3(2, 2, 2);
        pullCG.alpha = 0;
    }

    public void PullFireExtinguisherTrigger()
    {
        pullHUD.gameObject.SetActive(true);
        pullSlider.value = 0;
        pullHUD.DOFade(1, 1).SetEase(Ease.Linear);
        
        Invoke("DisplayInstruction", 5);
    }

    void Update()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            ChangeImageStatus(keyboardSprite[0], keyboardSprite[1]);
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            ChangeImageStatus(gamepadSprite[0], gamepadSprite[1]);
        }

        if(canInput)
        {
            SliderFunction();
        }
    }

    void SliderFunction()
    {
        if(canInput)
        {
            pullSlider.value -= decreaseValue * Time.deltaTime;
        }

        if(roundNum >= 3)
        {
            PlayerScript.instance.playerMovement.playerAnim.SetBool("TwistExtinguisher", false);
            
            tpass.twistAndPull = true;
            canInput = false;

            pullCG.DOFade(0, 1).OnComplete(() =>
            {
                tpass.checkMarkDone.gameObject.SetActive(true);
                tpass.correctSFX.Play();

                tpass.checkMarkDone.DOFade(1, 1).OnComplete(() =>
                {
                    tpass.checkMarkDone.DOFade(0, 1).OnComplete(() =>
                    {
                        PlayerScript.instance.playerVC.Priority = 10;
                        tpass.twistAndPullVC.Priority = 0;

                        tpass.checkMarkDone.gameObject.SetActive(false);

                        pullHUD.DOFade(0 , 1).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            tpass.tpassHUD.SetActive(false);
                            pullHUD.gameObject.SetActive(false);

                            HUDManager.instance.playerHUD.SetActive(true);
                            HUDManager.instance.missionHUD.SetActive(true);

                            this.enabled = false;
                            tpass.enabled = true;
                        });
                    });
                });
            });
        }
    }

    void CanInputDelay()
    {

        canInput = true;
    }

    void DisplayInstruction()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(pullRectTransform.DOAnchorPos(new Vector2(0f, 425f), .75f));
        sequence.Join(pullRectTransform.DOScale(new Vector3(1f, 1f, 1f), 1f));

        sequence.SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            sequence.Join(pullCG.DOFade(1f, 1f));
            Debug.Log("Sequence Completed!");

            canInput = true;
        });
    }



    void ChangeImageStatus(Sprite button1Sprite, Sprite button2Sprite)
    {
        if(pullSlider.value >= 0.985f && pullSlider.value <= pullSlider.maxValue)
        {
            imageHUD.sprite = button2Sprite;
        }
        else
        {
            imageHUD.sprite = button1Sprite;
        }
    }
}


                    //     LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

                    //     LoadingSceneManager.instance.fadeImage.DOFade(1, 1).OnComplete(() =>
                    //     {
                    //         PlayerScript.instance.playerVC.Priority = 10;
                    //         tpass.twistAndPullVC.Priority = 0;

                    //         tpass.tpassHUD.SetActive(false);
                    //         pullHUD.gameObject.SetActive(false);

                    //         tpass.checkMarkDone.gameObject.SetActive(false);

                    //         this.enabled = false;
                    //         tpass.enabled = true;
                    //         pullHUD.DOFade(0 , 1).SetEase(Ease.Linear).OnComplete(() =>
                    //         {


                    //             // LoadingSceneManager.instance.fadeImage.DOFade(0, 1).OnComplete(() =>
                    //             // {
                    //             //     LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
                    //             // });
                    //         });

                    //     });
                    // });
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

    [Header("Value")]
    [SerializeField] float pressedValue = 0.99f;
    [SerializeField] float decreaseValue = 0.01f;

    [Header("HUD")]
    [SerializeField] GameObject pullFEHUD;
    [SerializeField] RectTransform instructionHUD;
    [SerializeField] CanvasGroup pullFEGameplay;

    [Space(10)]
    [SerializeField] Slider pullSlider;
    [SerializeField] TMP_Text roundText;
    
    [Space(10)]
    [SerializeField] Image imageHUD;
    [SerializeField] Sprite[] keyboardSprite;
    [SerializeField] Sprite[] gamepadSprite;

    bool canInput;
    bool actionLock;
    int roundNum;
    public bool objectiveComplete;


    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.PullFE.Action.performed += ToAction;
        playerControls.PullFE.ActionLock.performed += ToActionLock;

        playerControls.PullFE.Enable();

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

                    pullFEGameplay.DOFade(0, 1).OnComplete(() =>
                    {
                        pullFEGameplay.DOFade(0, 1).OnComplete(() =>
                        {
                            pullSlider.value = 0;

                            pullFEGameplay.DOFade(1, 1).OnComplete(() =>
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

        pullFEHUD.SetActive(false);

        // tpass.ExtinguisherTrigger();
    }

    public void PullFireExtinguisherTrigger()
    {
        pullFEHUD.SetActive(true);
        pullSlider.value = 0;

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

        SliderFunction();

    }

    void SliderFunction()
    {
        if(canInput)
        {
            pullSlider.value -= decreaseValue * Time.deltaTime;
        }

        if(roundNum >= 3)
        {
            objectiveComplete = true;
            PlayerScript.instance.playerMovement.playerAnim.SetBool("TwistExtinguisher", false);
            
            tpass.firstHalfDone = true;
            tpass.aimMode = true;
            this.enabled = false;
        }
    }

    void CanInputDelay()
    {

        canInput = true;
    }

    void DisplayInstruction()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(instructionHUD.DOAnchorPos(new Vector2(0f, 425f), .75f));
        sequence.Join(instructionHUD.DOScale(new Vector3(1f, 1f, 1f), 1f));

        sequence.SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            sequence.Join(pullFEGameplay.DOFade(1f, 1f));
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

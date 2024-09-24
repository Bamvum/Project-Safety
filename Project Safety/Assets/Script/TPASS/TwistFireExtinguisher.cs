using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;

public class TwistFireExtinguisher : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Script")]
    [SerializeField] TPASS tpass;
    [SerializeField] PullFireExtinguisher pullFE;



    [Header("HUD")]
    [SerializeField] CanvasGroup twistHUD;
    [SerializeField] RectTransform twistRectTransform;
    [SerializeField] CanvasGroup twistCG;

    [Space(10)]
    [SerializeField] Image[] twistControlImage;
    [SerializeField] Sprite[] twistKeyboardSprite;
    [SerializeField] Sprite[] twistGamepadSprite;
    
    [Space(10)]
    [SerializeField] int inputsPerformed;
    [SerializeField] int inputNeedToFinish;
    [SerializeField] bool[] buttonPressed;

    [Header("Flag")]
    bool canInput;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.TwistFE.Button1.performed += Button1Pressed;
        playerControls.TwistFE.Button2.performed += Button2Pressed;
        playerControls.TwistFE.Button3.performed += Button3Pressed;
        playerControls.TwistFE.Button4.performed += Button4Pressed;

        playerControls.TwistFE.Enable();

        TwistFireExtinguisherTrigger();
    }

    private void Button1Pressed(InputAction.CallbackContext context)
    {
        if (canInput && !buttonPressed[1] && !buttonPressed[2] && !buttonPressed[3] && (buttonPressed[0] || inputsPerformed != inputNeedToFinish))
        {
            Debug.Log("Button 1 is Pressed!");

            twistControlImage[0].color = new Color(twistControlImage[0].color.r, twistControlImage[0].color.g, twistControlImage[0].color.b, .3f);
            twistControlImage[1].color = new Color(twistControlImage[1].color.r, twistControlImage[1].color.g, twistControlImage[1].color.b, 1);
            twistControlImage[2].color = new Color(twistControlImage[2].color.r, twistControlImage[2].color.g, twistControlImage[2].color.b, .3f);
            twistControlImage[3].color = new Color(twistControlImage[3].color.r, twistControlImage[3].color.g, twistControlImage[3].color.b, .3f);

            buttonPressed[0] = false;
            buttonPressed[1] = true;
            buttonPressed[2] = false;
            buttonPressed[3] = false;

            inputsPerformed++;

        }

    }

    private void Button2Pressed(InputAction.CallbackContext context)
    {
        if (canInput && !buttonPressed[0] && !buttonPressed[2] && !buttonPressed[3] && (buttonPressed[1] || inputsPerformed != inputNeedToFinish))
        {
            Debug.Log("Button 2 is Pressed!");

            twistControlImage[0].color = new Color(twistControlImage[0].color.r, twistControlImage[0].color.g, twistControlImage[0].color.b, .3f);
            twistControlImage[1].color = new Color(twistControlImage[1].color.r, twistControlImage[1].color.g, twistControlImage[1].color.b, .3f);
            twistControlImage[2].color = new Color(twistControlImage[2].color.r, twistControlImage[2].color.g, twistControlImage[2].color.b, 1);
            twistControlImage[3].color = new Color(twistControlImage[3].color.r, twistControlImage[3].color.g, twistControlImage[3].color.b, .3f);

            buttonPressed[0] = false;
            buttonPressed[1] = false;
            buttonPressed[2] = true;
            buttonPressed[3] = false;

            inputsPerformed++;
        }
    }

    private void Button3Pressed(InputAction.CallbackContext context)
    {
        if (canInput && !buttonPressed[0] && !buttonPressed[1] && !buttonPressed[3] && (buttonPressed[2] || inputsPerformed != inputNeedToFinish))
        {
            Debug.Log("Button 3 is Pressed!");

            twistControlImage[0].color = new Color(twistControlImage[0].color.r, twistControlImage[0].color.g, twistControlImage[0].color.b, .3f);
            twistControlImage[1].color = new Color(twistControlImage[1].color.r, twistControlImage[1].color.g, twistControlImage[1].color.b, .3f);
            twistControlImage[2].color = new Color(twistControlImage[2].color.r, twistControlImage[2].color.g, twistControlImage[2].color.b, .3f);
            twistControlImage[3].color = new Color(twistControlImage[3].color.r, twistControlImage[3].color.g, twistControlImage[3].color.b, 1);

            buttonPressed[0] = false;
            buttonPressed[1] = false;
            buttonPressed[2] = false;
            buttonPressed[3] = true;

        }
    }

    private void Button4Pressed(InputAction.CallbackContext context)
    {
        if (canInput && !buttonPressed[0] && !buttonPressed[1] && !buttonPressed[2] && (buttonPressed[3] || inputsPerformed != inputNeedToFinish))
        {
            Debug.Log("Button 4 is Pressed!");

            twistControlImage[0].color = new Color(twistControlImage[0].color.r, twistControlImage[0].color.g, twistControlImage[0].color.b, 1);
            twistControlImage[1].color = new Color(twistControlImage[1].color.r, twistControlImage[1].color.g, twistControlImage[1].color.b, .3f);
            twistControlImage[2].color = new Color(twistControlImage[2].color.r, twistControlImage[2].color.g, twistControlImage[2].color.b, .3f);
            twistControlImage[3].color = new Color(twistControlImage[3].color.r, twistControlImage[3].color.g, twistControlImage[3].color.b, .3f);

            buttonPressed[0] = true;
            buttonPressed[1] = false;
            buttonPressed[2] = false;
            buttonPressed[3] = false;

            inputsPerformed++;
        }
    }

    void OnDisable()
    {
        playerControls.TwistFE.Disable();
    }

    void Update()
    {
        if (DeviceManager.instance.keyboardDevice)
        {
            ChangeImageStatus(twistKeyboardSprite[0], twistKeyboardSprite[1], twistKeyboardSprite[2], twistKeyboardSprite[3]);
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            ChangeImageStatus(twistGamepadSprite[0], twistGamepadSprite[1], twistGamepadSprite[2], twistGamepadSprite[3]);
        }

        if(canInput)
        {
            TimerFunction();
        }


    }

    public void TwistFireExtinguisherTrigger()
    {
        HUDManager.instance.playerHUD.SetActive(false);


        twistHUD.gameObject.SetActive(true);
        twistHUD.DOFade(1, 1);

        Invoke("DisplayInstruction", 5);
    }

    void DisplayInstruction()
    {
        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(twistRectTransform.DOAnchorPos(new Vector2(0f, 425f), .75f));
        sequence.Join(twistRectTransform.DOScale(new Vector3(1f, 1f, 1f), 1f));

        sequence.SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            sequence.Join(twistCG.DOFade(1f, 1f));
            Debug.Log("Sequence Completed!");

            canInput = true;
        });
    }

    void ChangeImageStatus(Sprite button1Sprite, Sprite button2Sprite, Sprite button3Sprite, Sprite button4Sprite)
    {
        twistControlImage[0].sprite = button1Sprite;
        twistControlImage[1].sprite = button2Sprite;
        twistControlImage[2].sprite = button3Sprite;
        twistControlImage[3].sprite = button4Sprite;
    }

    void TimerFunction()
    {
        if (inputsPerformed == inputNeedToFinish)
        {
            inputsPerformed = inputNeedToFinish;
            canInput = false;

            tpass.twistDone = true;

            twistCG.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
            {
                tpass.checkMarkDone.gameObject.SetActive(true);
                tpass.correctSFX.Play();
                tpass.checkMarkDone.DOFade(1, 1).SetEase(Ease.Linear).OnComplete(() =>
                {
                    tpass.checkMarkDone.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        twistHUD.gameObject.SetActive(false);
                        
                        tpass.checkMarkDone.gameObject.SetActive(false);

                        pullFE.enabled = true;
                        pullFE.PullFireExtinguisherTrigger();

                        this.enabled =false;
                    });
                });
            });
        }
    }
}



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;


public class TwistFireExtinguisher : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("HUD")]
    [SerializeField] GameObject twistFEHUD;
    [SerializeField] RectTransform instructionHUD;
    [SerializeField] CanvasGroup twistFEGameplay;
    [Space(10)]
    [SerializeField] Image blackImage;

    [Space(10)]
    [SerializeField] Image[] imageHUD;
    [SerializeField] Sprite[] keyboardSprite;
    [SerializeField] Sprite[] gamepadSprite;

    [Header("Timer")]
    [SerializeField] Image timer;
    [SerializeField] float twistTime = 100f;
    [SerializeField] float maxTwistTime = 100f;
    [Space(10)]
    [SerializeField] int inputNeedToFinish;
    [SerializeField] int inputsPerformed;
    [SerializeField] bool[] buttonPressed;
    bool canInput;
    public bool objectiveComplete;


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

            imageHUD[0].color = new Color(imageHUD[0].color.r, imageHUD[0].color.g, imageHUD[0].color.b, .3f);
            imageHUD[1].color = new Color(imageHUD[1].color.r, imageHUD[1].color.g, imageHUD[1].color.b, 1);
            imageHUD[2].color = new Color(imageHUD[2].color.r, imageHUD[2].color.g, imageHUD[2].color.b, .3f);
            imageHUD[3].color = new Color(imageHUD[3].color.r, imageHUD[3].color.g, imageHUD[3].color.b, .3f);

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

            imageHUD[0].color = new Color(imageHUD[0].color.r, imageHUD[0].color.g, imageHUD[0].color.b, .3f);
            imageHUD[1].color = new Color(imageHUD[1].color.r, imageHUD[1].color.g, imageHUD[1].color.b, .3f);
            imageHUD[2].color = new Color(imageHUD[2].color.r, imageHUD[2].color.g, imageHUD[2].color.b, 1);
            imageHUD[3].color = new Color(imageHUD[3].color.r, imageHUD[3].color.g, imageHUD[3].color.b, .3f);

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

            imageHUD[0].color = new Color(imageHUD[0].color.r, imageHUD[0].color.g, imageHUD[0].color.b, .3f);
            imageHUD[1].color = new Color(imageHUD[1].color.r, imageHUD[1].color.g, imageHUD[1].color.b, .3f);
            imageHUD[2].color = new Color(imageHUD[2].color.r, imageHUD[2].color.g, imageHUD[2].color.b, .3f);
            imageHUD[3].color = new Color(imageHUD[3].color.r, imageHUD[3].color.g, imageHUD[3].color.b, 1);

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

            imageHUD[0].color = new Color(imageHUD[0].color.r, imageHUD[0].color.g, imageHUD[0].color.b, 1);
            imageHUD[1].color = new Color(imageHUD[1].color.r, imageHUD[1].color.g, imageHUD[1].color.b, .3f);
            imageHUD[2].color = new Color(imageHUD[2].color.r, imageHUD[2].color.g, imageHUD[2].color.b, .3f);
            imageHUD[3].color = new Color(imageHUD[3].color.r, imageHUD[3].color.g, imageHUD[3].color.b, .3f);

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

        twistFEHUD.SetActive(false);

        // TPASS.instance.ExtinguisherTrigger();
    }

    void Update()
    {
        if (DeviceManager.instance.keyboardDevice)
        {
            ChangeImageStatus(keyboardSprite[0], keyboardSprite[1], keyboardSprite[2], keyboardSprite[3]);
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            ChangeImageStatus(gamepadSprite[0], gamepadSprite[1], gamepadSprite[2], gamepadSprite[3]);
        }

        TimerFunction();

    }

    public void TwistFireExtinguisherTrigger()
    {
        twistFEHUD.SetActive(true);
        Invoke("DisplayInstruction", 5);
    }

    void DisplayInstruction()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(instructionHUD.DOAnchorPos(new Vector2(0f, 425f), .75f));
        sequence.Join(instructionHUD.DOScale(new Vector3(1f, 1f, 1f), 1f));

        sequence.SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            sequence.Join(twistFEGameplay.DOFade(1f, 1f));
            Debug.Log("Sequence Completed!");

            canInput = true;
        });
    }

    void ChangeImageStatus(Sprite button1Sprite, Sprite button2Sprite, Sprite button3Sprite, Sprite button4Sprite)
    {
        imageHUD[0].sprite = button1Sprite;
        imageHUD[1].sprite = button2Sprite;
        imageHUD[2].sprite = button3Sprite;
        imageHUD[3].sprite = button4Sprite;
    }

    void TimerFunction()
    {
        if (inputsPerformed > inputNeedToFinish)
        {
            inputsPerformed = inputNeedToFinish;

            objectiveComplete = true;
            PlayerScript.instance.playerMovement.playerAnim.SetBool("TwistExtinguisher", false);
            // TPASS.instance.twist = true;
            
            // blackImage.DOFade(1, tpass.inspectExtinguisherAnimLength).OnComplete(() =>
            // {

            //     blackImage.DOFade(1, tpass.inspectExtinguisherAnimLength).OnComplete(() =>
            //     {
            //         // FADE
            //         blackImage.DOFade(0, tpass.inspectExtinguisherAnimLength);
            //     });
            // });
            this.enabled = false;
        }


        if (canInput)
        {
            twistTime -= 5 * Time.deltaTime;
            timer.fillAmount = twistTime / maxTwistTime;
        }

        if (twistTime == 0)
        {
            twistTime = 0;
            Debug.LogError("Time Ended");
        }
    }


    // TODO - GAME OVER (TIMER ENDED)
    //      - DOTWEEN WHEN FINISH
    //      - FADE IN AND OUT WHEN FINISH

}



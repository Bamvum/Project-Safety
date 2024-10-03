using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;

public class StayCalm : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Dialogue Trigger")]

    [SerializeField] DialogueTrigger AccessPhone;

    [Header("HUD")]
    [SerializeField] TMP_Text playerHealthText;

    [Space(10)]
    [SerializeField] GameObject stayCalmHUD;
    [SerializeField] CanvasGroup stayCalmHUDCG;
    [SerializeField] RectTransform stayCalmRectTransform;
    [SerializeField] CanvasGroup stayCalmCG;

    [Space(10)]
    [SerializeField] Image checkMarkDone;
    [SerializeField] AudioSource correctSFX;

    [Space(10)]
    [SerializeField] Image stayCalmImage;
    [SerializeField] Sprite[] stayCalmKeyboard;
    [SerializeField] Sprite[] stayCalmGamepad;

    [Space(10)]
    [SerializeField] int inputsPerformed;
    [SerializeField] int inputNeedToFinish;
    [SerializeField] bool[] buttonToPress;

    [Header("Flag")]
    [SerializeField] int playerHealth;
    [SerializeField] bool canInput;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.StayCalm.Button1.performed += Button1Pressed;
        playerControls.StayCalm.Button2.performed += Button2Pressed;
        playerControls.StayCalm.Button3.performed += Button3Pressed;
        playerControls.StayCalm.Button4.performed += Button4Pressed;

        playerControls.StayCalm.Enable();

        // INSTANCE
        StayCalmInstance();

        StayCalmTrigger();
    }

    private void Button1Pressed(InputAction.CallbackContext context)
    {
        if(canInput && inputsPerformed != inputNeedToFinish)
        {
            if(buttonToPress[0])
            {
                Debug.Log("Button Pressed 1");

                RandomizeButtonToPress(buttonToPress);
                inputsPerformed++;
            }
        }
    }

    private void Button2Pressed(InputAction.CallbackContext context)
    {
        if(canInput && inputsPerformed != inputNeedToFinish)
        {
            if(buttonToPress[1])
            {
                Debug.Log("Button Pressed 2");

                RandomizeButtonToPress(buttonToPress);
                inputsPerformed++;
            }
        }
    }

    private void Button3Pressed(InputAction.CallbackContext context)
    {
        if(canInput && inputsPerformed != inputNeedToFinish)
        {
            if(buttonToPress[2])
            {
                Debug.Log("Button Pressed 3");
            
                RandomizeButtonToPress(buttonToPress);
                inputsPerformed++;
            }
        }
    }

    private void Button4Pressed(InputAction.CallbackContext context)
    {
        if(canInput && inputsPerformed != inputNeedToFinish)
        {
            if(buttonToPress[3])
            {
                Debug.Log("Button Pressed 4");

                RandomizeButtonToPress(buttonToPress);
                inputsPerformed++;
            }
        }
    }

    void StayCalmInstance()
    {
        RandomizeButtonToPress(buttonToPress);
    }

    void StayCalmTrigger()
    {
        HUDManager.instance.playerHUD.SetActive(false);

        stayCalmHUD.SetActive(true);
        stayCalmHUDCG.DOFade(1, 1).SetEase(Ease.Linear);

        Invoke("DisplayInstruction", 5);
    }

    void DisplayInstruction()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(stayCalmRectTransform.DOAnchorPos(new Vector2(0f, 425f), .75f));
        sequence.Append(stayCalmRectTransform.DOScale(new Vector2(1f, 1f), 1f));

        sequence.SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            sequence.Join(stayCalmCG.DOFade(1, 1));
            Debug.Log("Sequence Completed");

            canInput = true;
        });
    }
    
    // Update is called once per frame
    void Update()
    {
        DeviceChecker();

        // FINISH CHECKER

        if(inputsPerformed == inputNeedToFinish)
        {
            inputsPerformed = inputNeedToFinish;
            canInput = false;

            stayCalmCG.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
            {
                checkMarkDone.gameObject.SetActive(true);
                correctSFX.Play();

                checkMarkDone.DOFade(1, 1).SetEase(Ease.Linear).OnComplete(() =>
                {
                    checkMarkDone.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        checkMarkDone.gameObject.SetActive(false);
                        stayCalmHUD.SetActive(false);
                        
                        this.enabled = false;
                        
                    });
                });
            });
        }
    }

    void DeviceChecker()
    {
        if (DeviceManager.instance.keyboardDevice)
        {
            if(buttonToPress[0])
            {
                stayCalmImage.sprite = stayCalmKeyboard[0];
            }
            else if(buttonToPress[1])
            {
                stayCalmImage.sprite = stayCalmKeyboard[1];
            }
            else if(buttonToPress[2])
            {
                stayCalmImage.sprite = stayCalmKeyboard[2];
            }
            else if(buttonToPress[3])
            {
                stayCalmImage.sprite = stayCalmKeyboard[3];
            }
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            if(buttonToPress[0])
            {
                stayCalmImage.sprite = stayCalmGamepad[0];
            }
            else if(buttonToPress[1])
            {
                stayCalmImage.sprite = stayCalmGamepad[1];
            }
            else if(buttonToPress[2])
            {
                stayCalmImage.sprite = stayCalmGamepad[2];
            }
            else if(buttonToPress[3])
            {
                stayCalmImage.sprite = stayCalmGamepad[3];
            }
        }
    }

    void RandomizeButtonToPress(bool[] ButtonToPress)
    {
        // Step 1: Set all elements to false
        for (int i = 0; i < ButtonToPress.Length; i++)
        {
            ButtonToPress[i] = false;
        }

        // Step 2: Randomly pick one index to set to true
        int randomIndex = UnityEngine.Random.Range(0, ButtonToPress.Length);
        ButtonToPress[randomIndex] = true;
    }
}

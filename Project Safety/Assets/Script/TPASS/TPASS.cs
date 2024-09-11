using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;


public class TPASS : MonoBehaviour
{
    public static TPASS instance {get; private set;}

    void Awake()
    {
        instance = this;
        playerControls = new PlayerControls();
    
        // twistRectTransform = twistHUD.GetComponent<RectTransform>();
    }
    
    PlayerControls playerControls;
    
    [Header("Fire Extinguisher")]
    [SerializeField] GameObject fireExtinguisher;
    
    [Space(10)]
    [SerializeField] GameObject fireExtinguisherBody;
    [SerializeField] GameObject fireExtinguisherHose;
 
    [Space(10)]
    [SerializeField] GameObject fireExtinguisherToPickUp; // IF ACTIVE FALSE IT CAN BE USE

    [Header("Cinemachine")]
    [SerializeField] CinemachineVirtualCamera twistAndPullVC;

    [Header("Twist (TPASS)")]
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
    [SerializeField] bool twistObjectiveCompleted;
    bool inTwistQTE;
    
    [Header("Pull (TPASS)")]
    [SerializeField] CanvasGroup pullHUD;
    [SerializeField] RectTransform pullRectTransform;
    [SerializeField] CanvasGroup pullCG;

    [Space(10)]
    [SerializeField] float pressedValue;
    [SerializeField] float decreaseValue;

    [Space(10)]
    [SerializeField] Image pullControlImage;
    [SerializeField] Sprite[] pullKeyboardSprite;
    [SerializeField] Sprite[] pullGamepadSprite;
    [SerializeField] Slider pullSlider;
    [SerializeField] TMP_Text roundText;
    [SerializeField] int roundNum;
    [SerializeField] bool pullObjectiveCompleted;

    bool inPullQTE;

    [Header("TPASS status")]
    public bool twistAndPull;
    public bool aim;
    public bool squeezeAndSweep;

    [Header("Inputs")]
    bool equipFireExtinguisher;
    bool firstHalfDone;

    [Header("Flag")]
    bool canInput;

    [Space(10)]
    bool canUseFireExtinguisherInv;
    bool inGamePlay;
    

    void OnEnable()
    {
        playerControls.Extinguisher.EquipExtinguisher.performed += ToEquip;
        playerControls.Extinguisher.PerformTPASS.performed += ToPerformTPASS;

        playerControls.Extinguisher.Enable();
    }

    private void ToEquip(InputAction.CallbackContext context)
    {
        Debug.Log("To Equipt");
        if(!fireExtinguisherToPickUp.activeInHierarchy)
        {
            if (!equipFireExtinguisher)
            {
                if (firstHalfDone)
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
                if (firstHalfDone)
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
                Debug.Log("Perform Twist and Pull QTE!");

                StartCoroutine(DisplayTwist());
            }
        }
    }
    
    #region - TWIST (TPASS) - 

        #region  - TWIST CONTROLS -
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

        #endregion
    
    IEnumerator DisplayTwist()
    {
        playerControls.Extinguisher.EquipExtinguisher.Disable();
        playerControls.Extinguisher.PerformTPASS.Disable();

        // TWIST CONTROL
        playerControls.Extinguisher.TwistButton1.performed += Button1Pressed;
        playerControls.Extinguisher.TwistButton2.performed += Button2Pressed;
        playerControls.Extinguisher.TwistButton3.performed += Button3Pressed;
        playerControls.Extinguisher.TwistButton4.performed += Button4Pressed;

        HUDManager.instance.playerHUD.SetActive(false);

        PlayerScript.instance.playerMovement.enabled = false;
        PlayerScript.instance.cinemachineInputProvider.enabled = false;
        PlayerScript.instance.interact.enabled = false;
        PlayerScript.instance.stamina.enabled = false;

        PlayerScript.instance.playerVC.Priority = 0;
        twistAndPullVC.Priority = 10;

        twistHUD.gameObject.SetActive(true);
        twistHUD.DOFade(1, 1);

        yield return new WaitForSeconds(5);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(twistRectTransform.DOAnchorPos(new Vector2(0f, 425f), .75f));
        sequence.Join(twistRectTransform.DOScale(new Vector3(1f, 1f, 1f), 1f));

        sequence.SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            sequence.Join(twistCG.DOFade(1f, 1f));
            Debug.Log("Sequence Completed!");

            canInput = true;
            inTwistQTE = true;
        });
    }

    void TwistQTEFunction()
    {
        if (inputsPerformed > inputNeedToFinish)
        {
            inputsPerformed = inputNeedToFinish;

            canInput = false;
            twistObjectiveCompleted = true;

            // TWIST CONTROL
            playerControls.Extinguisher.TwistButton1.Disable();
            playerControls.Extinguisher.TwistButton2.Disable();
            playerControls.Extinguisher.TwistButton3.Disable();
            playerControls.Extinguisher.TwistButton4.Disable();

            // GO TO PULL FUNCTION
            StartCoroutine(DisplayPull());
            inTwistQTE = false;
        }
    }

    void ChangeTwistImage(Sprite button1Sprite, Sprite button2Sprite, Sprite button3Sprite, Sprite button4Sprite)
    {
        twistControlImage[0].sprite = button1Sprite;
        twistControlImage[1].sprite = button2Sprite;
        twistControlImage[2].sprite = button3Sprite;
        twistControlImage[3].sprite = button4Sprite;
    }

    #endregion
    
    #region - PULL (TPASS) -

    private void ToPullAction(InputAction.CallbackContext context)
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
        if (canInput)
        {
            if(roundNum <= 2)
            {
                if (pullSlider.value >= 0.985f && pullSlider.value <= pullSlider.maxValue)
                {
                    Debug.Log("Action Lock Pressed!");
                    roundText.text = roundNum + 1  + " / 3"; 

                    canInput = false;
                    roundNum++;

                    pullCG.DOFade(0, 1).OnComplete(() =>
                    {
                        pullCG.DOFade(0, 1).OnComplete(() =>
                        {
                            pullSlider.value = 0;

                            pullCG.DOFade(1, 1).OnComplete(() =>
                            {
                                canInput = true;
                            });
                        });

                    });
                }
            }

        }
    }
    
    IEnumerator DisplayPull()
    {
        // PULL CONTROL
        playerControls.Extinguisher.Action.performed += ToPullAction;
        playerControls.Extinguisher.ActionLock.performed += ToActionLock;

        twistHUD.DOFade(0, 1).OnComplete(() =>
        {
            twistHUD.gameObject.SetActive(false);
            pullHUD.gameObject.SetActive(true);
            pullSlider.value = 0;
            pullHUD.DOFade(1, 1);
        });

        yield return new WaitForSeconds(5);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(pullRectTransform.DOAnchorPos(new Vector2(0f, 425f), .75f));
        sequence.Join(pullRectTransform.DOScale(new Vector3(1f, 1f, 1f), 1f));

        sequence.SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            sequence.Join(pullCG.DOFade(1f, 1f));
            Debug.Log("Sequence Completed!");

            canInput = true;
            inPullQTE = true;
        });
    }

    void PullQTEFunction()
    {
        if(canInput)
        {
            pullSlider.value -= decreaseValue * Time.deltaTime;
        }

        if(roundNum >= 3)
        {
            canInput = false;
            pullObjectiveCompleted = true;

            // PULL CONTROL
            playerControls.Extinguisher.Action.Disable();
            playerControls.Extinguisher.ActionLock.Disable();
            
            inPullQTE = false;
            firstHalfDone = true;
        }
    }

    void ChangePullImage(Sprite button1Sprite, Sprite button2Sprite)
    {
        if(pullSlider.value >= 0.985f && pullSlider.value <= pullSlider.maxValue)
        {
            pullControlImage.sprite = button2Sprite;
        }
        else
        {
            pullControlImage.sprite = button1Sprite;
        }
    }

    #endregion 

    void OnDisable()
    {
        playerControls.Extinguisher.Disable();
    }

    void Update()
    {
        // DISPLAY PROMPT "TO TPASS" IF FIRE EXTINGUISHER IS EQUIP...

        // DEVICE MANAGER
        DeviceChecker();

        if(inTwistQTE)
        {
            TwistQTEFunction();
        }
        else if(inPullQTE)
        {
            PullQTEFunction();
        }
    }

    void DeviceChecker()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            if (inTwistQTE)
            {
                ChangeTwistImage(twistKeyboardSprite[0], twistKeyboardSprite[1], twistKeyboardSprite[2], twistKeyboardSprite[3]);
            }
            else if(inPullQTE)
            {
                ChangePullImage(pullKeyboardSprite[0], pullKeyboardSprite[1]);
            }

        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            if (inTwistQTE)
            {
                ChangeTwistImage(twistGamepadSprite[0], twistGamepadSprite[1], twistGamepadSprite[2], twistGamepadSprite[3]);
            }
            else if (inPullQTE)
            {
                ChangePullImage(pullGamepadSprite[0], pullGamepadSprite[1]);
            }
        }

    }
}


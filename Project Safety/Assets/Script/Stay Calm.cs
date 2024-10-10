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

    [Header("Player")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerSitted;

    [Header("Dialogue Trigger")]
    [SerializeField] DialogueTrigger AccessPhone;

    [Header("HUD")]
    [SerializeField] TMP_Text playerHealthText;

    [Space(5)]
    [SerializeField] CanvasGroup stayCalmHUD;
    [SerializeField] RectTransform stayCalmRectTransform;
    [SerializeField] CanvasGroup stayCalmRectTransformCG;
    [SerializeField] CanvasGroup stayCalmCG;
    
    [Space(5)]
    [SerializeField] Image stayCalmImage;
    [SerializeField] Sprite[] stayCalmKeyboard;
    [SerializeField] Sprite[] stayCalmGamepad;

    [Space(5)]
    [SerializeField] Image checkMarkDone;

    [Space(10)]
    [SerializeField] Image stayCalmTimer;
    [SerializeField] float timerDescrease = .1f;
    [SerializeField] int inputsPerformed;
    [SerializeField] int inputNeedToFinish;
    [SerializeField] bool[] buttonToPress;

    [Header("Audio")]
    [SerializeField] AudioSource doneSFX;
    [SerializeField] AudioSource wrongSFX;
    [SerializeField] AudioSource heartBeatSFX;
    


    [Header("Flag")]
    [SerializeField] int playerHealth;
    [SerializeField] int playerMaxHealth;
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
        RandomizeButtonToPress(buttonToPress);
        heartBeatSFX.Play();

        StayCalmTrigger();
    }

    void OnDisable()
    {
        heartBeatSFX.Stop();
        playerControls.StayCalm.Disable();
    }
    private void Button1Pressed(InputAction.CallbackContext context)
    {
        if (playerHealth != playerMaxHealth && canInput && inputsPerformed != inputNeedToFinish)
        {
            if(buttonToPress[0])
            {
                Debug.Log("Button Pressed 1");

                RandomizeButtonToPress(buttonToPress);
                inputsPerformed++;
            }
            else
            {
                VibrateGamepad();
                wrongSFX.Play();
                RandomizeButtonToPress(buttonToPress);
                playerHealth++;
            }

            stayCalmTimer.fillAmount = 1;
        }
    }

    private void Button2Pressed(InputAction.CallbackContext context)
    {
        if (playerHealth != playerMaxHealth && canInput && inputsPerformed != inputNeedToFinish)
        {
            
            if(buttonToPress[1])
            {
                Debug.Log("Button Pressed 2");

                RandomizeButtonToPress(buttonToPress);
                inputsPerformed++;
            }
            else
            {
                VibrateGamepad();
                wrongSFX.Play();
                RandomizeButtonToPress(buttonToPress);
                playerHealth++;
            }

            stayCalmTimer.fillAmount = 1;
        }
    }

    private void Button3Pressed(InputAction.CallbackContext context)
    {
        if (playerHealth != playerMaxHealth && canInput && inputsPerformed != inputNeedToFinish)
        {
            if(buttonToPress[2])
            {
                Debug.Log("Button Pressed 3");
            
                RandomizeButtonToPress(buttonToPress);
                inputsPerformed++;
            }
            else
            {
                VibrateGamepad();
                wrongSFX.Play();
                RandomizeButtonToPress(buttonToPress);
                playerHealth++;
            }

            stayCalmTimer.fillAmount = 1;
        }
    }

    private void Button4Pressed(InputAction.CallbackContext context)
    {
        if (playerHealth != playerMaxHealth && canInput && inputsPerformed != inputNeedToFinish)
        {
            if(buttonToPress[3])
            {
                Debug.Log("Button Pressed 4");

                RandomizeButtonToPress(buttonToPress);
                inputsPerformed++;
            }
            else
            {
                VibrateGamepad();
                wrongSFX.Play();
                RandomizeButtonToPress(buttonToPress);
                playerHealth++;
            }

            stayCalmTimer.fillAmount = 1;
        }
    }

    void StayCalmTrigger()
    {
        HUDManager.instance.playerHUD.SetActive(false);

        stayCalmHUD.gameObject.SetActive(true);
        stayCalmHUD.DOFade(1, 1).OnComplete(() =>
        {
            stayCalmRectTransformCG.DOFade(1,0).OnComplete(() =>
            {
                Invoke("DisplayInstruction", 2.5f);
            });
        });

        
    }

    void DisplayInstruction()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(stayCalmRectTransform.DOAnchorPos(new Vector2(0f, 425f), .75f));
        sequence.Join(stayCalmRectTransform.DOScale(new Vector2(1f, 1f), 1f));

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
        Gameplay();
    }

    void VibrateGamepad()
    {
        if(Gamepad.current != null)
        {
            // Set a short vibration
            Gamepad.current.SetMotorSpeeds(0.5f, 0.5f); // Adjust the intensity here
            Invoke("StopVibration", 0.3f); // Stops vibration after 0.1 seconds
        }

    }

    private void StopVibration()
    {
        Gamepad.current.SetMotorSpeeds(0, 0);
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


    void Gameplay()
    {
        playerHealthText.text = playerHealth + " / " + playerMaxHealth;

        if (playerHealth != playerMaxHealth)
        {
            if (stayCalmCG.alpha == 1)
            {
        
                stayCalmTimer.fillAmount -= timerDescrease * Time.deltaTime;

                if (stayCalmTimer.fillAmount <= 0)
                {
                    RandomizeButtonToPress(buttonToPress);
                    playerHealth++;
                    stayCalmTimer.fillAmount = 1;
                }
            }
        }
        else
        {
            stayCalmCG.DOFade(0, 1).OnComplete(() =>
            {
                GameOver.instance.ShowGameOver();
                PlayerScript.instance.DisablePlayerScripts();
                this.enabled = false;
                Debug.Log("Game OVer!");

            });

        }


        // FINISH CHECKER
        if(canInput)
        {
            if (inputsPerformed == inputNeedToFinish)
            {
                inputsPerformed = inputNeedToFinish;
                canInput = false;

                stayCalmCG.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
                {
                    checkMarkDone.gameObject.SetActive(true);
                    doneSFX.Play();

                    playerSitted.SetActive(false);
                    player.SetActive(true);

                    checkMarkDone.DOFade(1, 1).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        checkMarkDone.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            checkMarkDone.gameObject.SetActive(false);
                            stayCalmHUD.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
                            {
                                stayCalmHUD.gameObject.SetActive(false);
                                AccessPhone.StartDialogue();
                                this.enabled = false;
                            });

                        });
                    });
                });
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

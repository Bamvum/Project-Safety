using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;


public class DoorChecking : MonoBehaviour
{
    PlayerControls playerControls;

    public static DoorChecking instance {get; private set;}

    void Awake()
    {
        instance = this;
        playerControls = new PlayerControls();
    }

    [Header("HUD")]
    [SerializeField] CanvasGroup checkDoorHUDCG;

    [Space(5)] // HUD DOOR STATUS
    [SerializeField] RectTransform doorStatusParentRectTransform;
    [SerializeField] CanvasGroup doorHand;
    [SerializeField] CanvasGroup[] doorTestStatus;
    
    [Space(5)] // HUD CHOICES
    [SerializeField] RectTransform openChoiceRectTransform;
    [SerializeField] RectTransform testChoiceRectTransform;
    [SerializeField] RectTransform exitChoiceRectTransform;
    
    [Space(5)] // HUD CHOICES INPUT
    [SerializeField] Image[] checkDoorImage;
    [SerializeField] Sprite[] keyboardSprite;
    [SerializeField] Sprite[] gamepadSprite;

    [Header("Animator")]
    [SerializeField] Animator doorAnimator;

    [Header("Flag")]
    [SerializeField] bool isSafeToOpen;
    [SerializeField] bool canInput;

    [Header("SFX")]
    [SerializeField] AudioSource hurtSFX;

    void Start()
    {
        doorAnimator = GetComponent<Animator>();    
    }

    void OnEnable()
    {
        playerControls.CheckDoor.Open.performed += ToOpen;
        playerControls.CheckDoor.Exit.performed += ToExit;
        playerControls.CheckDoor.Test.performed += ToTest;

        playerControls.CheckDoor.Enable();

        // DISABLE HUD/UI
        HUDManager.instance.playerHUD.SetActive(false);

        // DISABLE PLAYER SCRIPTS
        PlayerScript.instance.DisablePlayerScripts();
        
        DisplayCheckingDoorHUD();
    }

    void OnDisable()
    {
        playerControls.CheckDoor.Disable();
    }

    #region - BUTTON PRESSED FUNCTION -

    private void ToOpen(InputAction.CallbackContext context)
    {
        if(canInput)
        {
            if(!Pause.instance.pauseHUDRectTransform.gameObject.activeSelf)
            {
                Debug.Log("To Door Method");

                if(isSafeToOpen)
                {
                    DoorOpen();
                    HideCheckingDoorHUD(true);
                }
                else
                {
                    DoorDamage();
                }
            }
        }
    }

    private void ToTest(InputAction.CallbackContext context)
    {
        if (canInput)
        {
            if(!Pause.instance.pauseHUDRectTransform.gameObject.activeSelf)
            {
                Debug.Log("To Test Method");

                if(isSafeToOpen)
                {
                    StartCoroutine(DelayOnDisplayDoorStatus(true));
                }
                else
                {
                    StartCoroutine(DelayOnDisplayDoorStatus(false));
                }
            }
        }
    }

    private void ToExit(InputAction.CallbackContext context)
    {
        if (canInput)
        {
            if (!Pause.instance.pauseHUDRectTransform.gameObject.activeSelf)
            {
                Debug.Log("To Exit Method");

                HideCheckingDoorHUD(false);
            }
        }
    }

    #endregion

    void Update()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            ChangeImageStatus(keyboardSprite[0], keyboardSprite[1], keyboardSprite[2]);
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            ChangeImageStatus(gamepadSprite[0], gamepadSprite[1], gamepadSprite[2]);
        }
    }

    #region - UI CHANGES -

    void DisplayCheckingDoorHUD()
    {
        checkDoorHUDCG.gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(checkDoorHUDCG.DOFade(1, 1f).SetEase(Ease.Linear).SetUpdate(true));

        sequence.Append(openChoiceRectTransform.DOScale(Vector3.one, .5f).SetUpdate(true));
        sequence.Join(exitChoiceRectTransform.DOScale(Vector3.one, .5f).SetUpdate(true));
        sequence.Join(testChoiceRectTransform.DOScale(Vector3.one, .5f).SetUpdate(true));
        sequence.Join(doorStatusParentRectTransform.DOScale(Vector3.one, .5f).SetUpdate(true));        

        sequence.SetEase(Ease.InOutQuad).SetUpdate(true).OnComplete(() =>
        {
            canInput = true;
        });
    }

    void HideCheckingDoorHUD(bool isremoveScript)
    {
        this.enabled = false;
     
        ResetCheckingDoorHUD();

        if(isremoveScript)
        {
            this.gameObject.layer = 0;
        }
    }

    
    void ResetCheckingDoorHUD()
    {
        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(doorStatusParentRectTransform.DOScale(Vector3.zero, .5f).SetUpdate(true));
        sequence.Join(doorHand.DOFade(0, .5f).SetUpdate(true));
        sequence.Join(doorTestStatus[0].DOFade(0, .5f).SetUpdate(true));
        sequence.Join(doorTestStatus[1].DOFade(0, .5f).SetUpdate(true));
        sequence.Join(openChoiceRectTransform.DOScale(Vector3.zero, .5f).SetUpdate(true));
        sequence.Join(testChoiceRectTransform.DOScale(Vector3.zero, .5f).SetUpdate(true));
        sequence.Join(exitChoiceRectTransform.DOScale(Vector3.zero, .5f).SetUpdate(true));

        sequence.SetEase(Ease.InOutQuad).SetUpdate(true).OnComplete(() =>
        {
            HUDManager.instance.playerHUD.SetActive(true);
            checkDoorHUDCG.gameObject.SetActive(false);

            // ENABLE PLAYER SCIRPT
            PlayerScript.instance.playerMovement.enabled = true;
            PlayerScript.instance.cinemachineInputProvider.enabled = true;
            PlayerScript.instance.interact.enabled = true;
            PlayerScript.instance.stamina.enabled = true;

            this.enabled = false;
        });
        
        
    }


    IEnumerator DelayOnDisplayDoorStatus(bool doorStatus)
    {
        canInput = false;

        Sequence sequence = DOTween.Sequence();

        // DOTWEEN OF DOOR STATUS
        sequence.Append(doorHand.DOFade(1, 1).SetUpdate(true)) // Fade in the door hand
                .AppendInterval(1f); // Instead of WaitForSeconds, use DoTween's interval for delay

        if (doorStatus)
        {
            // SAFE
            sequence.Append(doorTestStatus[0].DOFade(1, 1).SetUpdate(true)); // Fade in SAFE status
        }
        else
        {
            // NOT SAFE
            sequence.Append(doorTestStatus[1].DOFade(1, 1).SetUpdate(true)); // Fade in NOT SAFE status
        }

        sequence.AppendInterval(1f) // Delay after showing status
                .SetEase(Ease.Linear) // Apply easing to the whole sequence
                .SetUpdate(true) // Ensures the tween continues if timeScale = 0
                .OnComplete(() =>
                {
                    canInput = true; // Reactivate input after the sequence is complete
                });

        // Wait for the sequence to finish before exiting the coroutine
        yield return sequence.WaitForCompletion();
    }


    void ChangeImageStatus(Sprite openSprite, Sprite exitSprite, Sprite testSprite)
    {
        checkDoorImage[0].sprite = openSprite;
        checkDoorImage[1].sprite = exitSprite;
        checkDoorImage[2].sprite = testSprite;
    }


    #endregion

    #region - ANIMATION -

    void DoorOpen()
    {
        Debug.Log("Door Open!");
        doorAnimator.SetBool("Door Open", true);
    }

    #endregion 

    #region  - DAMAGE -

    void DoorDamage()
    {
        Act2Scene2SceneManager.instance.playerHealth -= 7.5f;
        hurtSFX.Play();

        if(Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(.5f, .5f);
            Invoke("StopVibration", .25f);
        }

        // StartCoroutine(CheckVibration());
    }

    void StopVibration()
    {
        Gamepad.current.SetMotorSpeeds(0, 0);
        HideCheckingDoorHUD(true);
    }

    #endregion 

    // void OnEnable()
    // {
    //     playerControls.CheckDoor.Open.performed += ToOpen;
    //     playerControls.CheckDoor.Exit.performed += ToExit;
    //     playerControls.CheckDoor.Test.performed += ToTest;

    //     playerControls.CheckDoor.Enable();

    //     // HUD DISABLE
    //     HUDManager.instance.playerHUD.SetActive(false);

    //     DisplayCheckingDoorHUD();

    //     PlayerScript.instance.DisablePlayerScripts();
    // }

    // #region - BUTTON PRESSED FUNCTION -

    // private void ToOpen(InputAction.CallbackContext context)
    // {
    //     Debug.Log("To Door Method");   

    //     if(canInput)
    //     {
    //         if (isSafeToOpen)
    //         {
    //             DoorOpen();
    //             HideCheckingDoorHUD(true);
    //         }
    //         else
    //         {
    //             // VIBRATE CONTROLLER
    //             DoorDamage();
    //         }
    //     }
    // }

    // private void ToExit(InputAction.CallbackContext context)
    // {
    //     Debug.Log("To Exit Method");

    //     HideCheckingDoorHUD(false);
    // }

    // private void ToTest(InputAction.CallbackContext context)
    // {
    //     // CAN OPEN - DISPLAY OPENING HAND UI
    //     Debug.Log("To Test Door Method");
    //     // DISPLAY IF DOOR IS SAFE OR NOT
    //     // HIDE CHECKING DOOR HUD THEN SHOW IT AGAIN

    //     if(canInput)
    //     {
    //         if (isSafeToOpen)
    //         {
    //             StartCoroutine(DelayOnDisplayDoorStatus("The Door is Safe!"));
    //         }
    //         else
    //         {
    //             StartCoroutine(DelayOnDisplayDoorStatus("The Door is Hot!"));
    //         }
    // }


    // }

    // #endregion

    // void OnDisable()
    // {
    //     // HUD ENABLE
    //     HUDManager.instance.playerHUD.SetActive(true);

    //     playerControls.CheckDoor.Disable();
    // }

    // void Update()
    // {
    //     if(DeviceManager.instance.keyboardDevice)
    //     {
    //         ChangeImageStatus(keyboardSprite[0], keyboardSprite[1], keyboardSprite[2]);
    //     }
    //     else if(DeviceManager.instance.gamepadDevice)
    //     {
    //         ChangeImageStatus(gamepadSprite[0], gamepadSprite[1], gamepadSprite[2]);
    //     }
    // }

    // #region - DAMAGE -

    // void DoorDamage()
    // {
    //     Act2Scene2SceneManager.instance.playerHealth -= 10;
    //     hurtSFX.Play();

    //     if (Gamepad.current != null)
    //     {
    //         Gamepad.current.SetMotorSpeeds(0.5f, 0.5f);
    //         Invoke("StopVibration", 0.25f);
    //     }

    //     StartCoroutine(CheckVibration());

    // }

    // void StopVibration()
    // {
    //     Gamepad.current.SetMotorSpeeds(0,0);
    // }

    // IEnumerator CheckVibration()
    // {
    //     yield return new WaitForSeconds(0.275f); // Wait for the vibration duration

    //     HideCheckingDoorHUD(true); // Do your action
    // }

    // #endregion

    // #region  - DOOR ANIMATION -

    // void DoorOpen()
    // {
    //     Debug.Log("Door Open!");
    //     doorAnimator.SetBool("Door Open", true);
    // }

    // #endregion 


    // #region - UI CHANGES -

    // void DisplayCheckingDoor ()
    // {
    //     checkDoorHUD.SetActive(true);
    //     openChoiceRectTransform.gameObject.SetActive(true);
    //     testChoiceRectTransform.gameObject.SetActive(true);

    //     openChoiceRectTransform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
    //     {
    //         openChoiceRectTransform.DOPunchScale(Vector3.one * punchScale, punchDuration, 10, 1).OnComplete(() =>
    //         {
    //             exitChoiceRectTransform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
    //             {
    //                 exitChoiceRectTransform.DOPunchScale(Vector3.one * punchScale, punchDuration, 10, 1).OnComplete(() =>
    //                 {
    //                     testChoiceRectTransform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
    //                     {
    //                         testChoiceRectTransform.DOPunchScale(Vector3.one * punchScale, punchDuration, 10, 1).OnComplete(() =>
    //                         {
    //                             canInput = true;
    //                         });
    //                     });
    //                 });
    //             });
    //         });
    //     });
    // }

    // void HideCheckingDoorHUD(bool removeScript)
    // {
    //     this.enabled = false;

    //     checkDoorHUD.SetActive(false);
    //     ResetRectTransformScale();

    //     // PLAYER SCRIPT ENABLE
    //     PlayerScript.instance.playerMovement.enabled = true;
    //     PlayerScript.instance.cinemachineInputProvider.enabled = true;
    //     PlayerScript.instance.interact.enabled = true;
    //     PlayerScript.instance.stamina.enabled = true;

    //     if(removeScript)
    //     {
    //         this.gameObject.layer = 0;
    //         Destroy(this);
    //     }
    // }

    // void ChangeImageStatus(Sprite openSprite, Sprite exitSprite, Sprite testSprite)
    // {
    //     checkDoorImage[0].sprite = openSprite;
    //     checkDoorImage[1].sprite = exitSprite;
    //     checkDoorImage[2].sprite = testSprite;
    // }

    // IEnumerator DelayOnDisplayDoorStatus(string doorStatus)
    // {
    //     canInput = false;

    //     testChoiceRectTransform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
    //     {
    //         testChoiceRectTransform.DOPunchScale(Vector3.zero * punchScale, punchDuration, 10, 1).OnComplete(() =>
    //         {
    //             exitChoiceRectTransform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
    //             {
    //                 exitChoiceRectTransform.DOPunchScale(Vector3.zero * punchScale, punchDuration, 10, 1).OnComplete(() =>
    //                 {
    //                     openChoiceRectTransform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
    //                     {
    //                         openChoiceRectTransform.DOPunchScale(Vector3.zero * punchScale, punchDuration, 10, 1);
    //                     });
    //                 });
    //             });

    //         });
    //     });

    //     yield return new WaitForSeconds(2.5f);

    //     DisplayDoorStatus(doorStatus);

    //     DisplayCheckingDoorHUD();
    // }

    // void DisplayDoorStatus(string doorStatus)
    // {
    //     statusOfDoor.text = doorStatus;
    // }

    // void ResetRectTransformScale()
    // {
    //     openChoiceRectTransform.localScale = Vector3.zero;
    //     exitChoiceRectTransform.localScale = Vector3.zero;
    //     testChoiceRectTransform.localScale = Vector3.zero;
    //     statusOfDoor.text = string.Empty;
    // }

    // #endregion
}
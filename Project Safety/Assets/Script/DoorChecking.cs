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
    [SerializeField] GameObject checkDoorHUD;
    [SerializeField] TMP_Text statusOfDoor;

    [Space(5)]
    [SerializeField] RectTransform openChoiceRectTransform;
    [SerializeField] RectTransform testChoiceRectTransform;
    [SerializeField] RectTransform exitChoiceRectTransform;
    
    [Space(5)]
    [SerializeField] Image[] checkDoorImage;
    [SerializeField] Sprite[] keyboardSprite;
    [SerializeField] Sprite[] gamepadSprite;

    [Header("Animator")]
    [SerializeField] Animator doorAnimator;

    [Header("Flag")]
    [SerializeField] bool isSafeToOpen;
    [SerializeField] bool isDoorInteracted;

    [Header("SFX")]
    [SerializeField] AudioSource hurtSFX;

    [Header("DoTween")]
    [SerializeField] float punchDuration = 0.3f;
    [SerializeField] float punchScale = 0.2f;
    
    void Start()
    {
        doorAnimator = GetComponent<Animator>();    
    }

    void OnEnable()
    {        
        playerControls.CheckDoor.Enable();

        // HUD DISABLE
        HUDManager.instance.playerHUD.SetActive(false);
        HUDManager.instance.examineHUD.SetActive(false);
        HUDManager.instance.dialogueHUD.SetActive(false);
        HUDManager.instance.missionHUD.SetActive(false);

        ResetRectTransformScale();
        DisplayCheckingDoorHUD();
        
        PlayerScript.instance.DisablePlayerScripts();
    }

    private void ToOpen(InputAction.CallbackContext context)
    {
        Debug.Log("To Door Method");   

        if(isSafeToOpen)
        {
            DoorOpen();
            HideCheckingDoorHUD();
        }
        else
        {
            Act2Scene2SceneManager.instance.playerHealth -= 10;
            hurtSFX.Play();
            HideCheckingDoorHUD();
        }
        

    }

    private void ToTest(InputAction.CallbackContext context)
    {
        // CAN OPEN - DISPLAY OPENING HAND UI
        
        Debug.Log("To Test Door Method");


        // DISPLAY IF DOOR IS SAFE OR NOT
        // HIDE CHECKING DOOR HUD THEN SHOW IT AGAIN

        if(isSafeToOpen)
        {
            StartCoroutine(DelayOnDisplayDoorStatus("The Door is Safe!"));
        }
        else
        {
            StartCoroutine(DelayOnDisplayDoorStatus("The Door is Hot!"));
        }
        
    }

    void OnDisable()
    {
        playerControls.CheckDoor.Disable();
    }

    void DisableScript()
    {
        this.enabled = false;
        this.gameObject.layer = 0;

        Destroy(this);

        // PLAYER SCRIPT ENABLE
        PlayerScript.instance.playerMovement.enabled = true;
        PlayerScript.instance.cinemachineInputProvider.enabled = true;
        PlayerScript.instance.interact.enabled = true;
        PlayerScript.instance.stamina.enabled = true;
        
        ResetRectTransformScale();
    }


    void HideCheckingDoorHUD()
    {
        DisableScript();
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
    }

    #region  - DOOR ANIMATION -

    void DoorOpen()
    {
        Debug.Log("Door Open!");
        doorAnimator.SetBool("Door Open", true);
    }

    #endregion 


    #region - UI CHANGES -

    void DisplayCheckingDoorHUD()
    {
        checkDoorHUD.SetActive(true);
        openChoiceRectTransform.gameObject.SetActive(true);
        testChoiceRectTransform.gameObject.SetActive(true);

        openChoiceRectTransform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
        {
            openChoiceRectTransform.DOPunchScale(Vector3.one * punchScale, punchDuration, 10, 1).OnComplete(() =>
            {
                exitChoiceRectTransform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
                {
                    exitChoiceRectTransform.DOPunchScale(Vector3.one * punchScale, punchDuration, 10, 1).OnComplete(() =>
                    {
                        testChoiceRectTransform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
                        {
                            testChoiceRectTransform.DOPunchScale(Vector3.one * punchScale, punchDuration, 10, 1).OnComplete(() =>
                            {
                                playerControls.CheckDoor.Open.performed += ToOpen;
                                playerControls.CheckDoor.Test.performed += ToTest;
                            });
                        });
                    });
                });
            });
        });
    }

    void ChangeImageStatus(Sprite openSprite, Sprite testSprite)
    {
        checkDoorImage[0].sprite = openSprite;
        checkDoorImage[1].sprite = testSprite;
    }

    IEnumerator DelayOnDisplayDoorStatus(string doorStatus)
    {
        playerControls.CheckDoor.Open.Disable();
        playerControls.CheckDoor.Test.Disable();

        testChoiceRectTransform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
        {
            testChoiceRectTransform.DOPunchScale(Vector3.zero * punchScale, punchDuration, 10, 1).OnComplete(() =>
            {
                exitChoiceRectTransform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
                {
                    exitChoiceRectTransform.DOPunchScale(Vector3.zero * punchScale, punchDuration, 10, 1).OnComplete(() =>
                    {
                        openChoiceRectTransform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
                        {
                            openChoiceRectTransform.DOPunchScale(Vector3.zero * punchScale, punchDuration, 10, 1);
                        });
                    });
                });

            });
        });

        yield return new WaitForSeconds(2.5f);

        DisplayDoorStatus(doorStatus);
    
        DisplayCheckingDoorHUD();
    }

    void DisplayDoorStatus(string doorStatus)
    {
        statusOfDoor.text = doorStatus;
    }

    void ResetRectTransformScale()
    {
        openChoiceRectTransform.localScale = Vector3.zero;
        testChoiceRectTransform.localScale = Vector3.zero;
        statusOfDoor.text = string.Empty;
    }

    #endregion
}

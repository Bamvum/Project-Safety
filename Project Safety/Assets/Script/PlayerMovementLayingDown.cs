using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;

public class PlayerMovementLayingDown : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Trigger Dialogue")]
    [SerializeField] DialogueTrigger startEnglishDialogue;
    [SerializeField] DialogueTrigger startTagalogDialogue;

    [Header("Script")]
    [SerializeField] CinemachineInputProvider inputProviderSleeping;

    [Space(5)]
    [SerializeField] CinemachineVirtualCamera playerLayingDownVC;

    [Space(10)]
    [SerializeField] GameObject prompt; 
    [SerializeField] Image promptImg; 
    [SerializeField] Sprite[] promptSprite; 

    [Space(10)]
    [SerializeField] GameObject sleepingCharacter;
    [SerializeField] AudioSource bedSheetSFX;

    [SerializeField] Animator sleepingAnimation;
    [SerializeField] float sleepingCharacterSpeed;

    [Header("Inputs")]
    bool actionPressed;
    Vector3 lookInput;
    bool canMove;
    bool toRepeat = true;
    bool toRepeatInstruction = true;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.PlayerLayingDown.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();

        playerControls.PlayerLayingDown.Action.performed += ctx => actionPressed = true;
        playerControls.PlayerLayingDown.Action.canceled += ctx => actionPressed = false;

        playerControls.PlayerLayingDown.Enable();
    }

    void OnDisable()
    {
        playerControls.PlayerLayingDown.Disable();
    }

    void Update()
    {
        CameraDeviceChecker();
        
        if(toRepeat)
        {
            if (PrologueSceneManager.instance.toGetUp)
            {
                if (toRepeatInstruction)
                {
                    Debug.Log("Display Instruction");
                    InstructionManager.instance.enabled = true;
                    InstructionManager.instance.ShowInstruction();
                    
                    toRepeatInstruction = false;
                }

                if(canMove)
                {
                    prompt.SetActive(true);

                    if(actionPressed)
                    {
                        prompt.SetActive(false);
                        sleepingAnimation.SetBool("To stand up", true);
                        
                        StartCoroutine(DisableScript());
                        toRepeat = false;
                    }
                }
            }
        }
        
        if (sleepingAnimation.GetBool("To stand up"))
        {
            sleepingCharacter.transform.position = Vector3.MoveTowards(sleepingCharacter.transform.position, new Vector3(sleepingCharacter.transform.position.x, 3.15f, -13.2f), Time.deltaTime * sleepingCharacterSpeed);
        }
    }

    IEnumerator DisableScript()
    {
        Debug.Log("Play Bed sheet SFX");
        bedSheetSFX.Play();

        yield return new WaitForSeconds(4);

        Debug.Log("Disable Script");
        
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
        // FADE IN
        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
            PlayerScript.instance.playerMovement.gameObject.SetActive(true);
            PlayerScript.instance.playerMovement.enabled = false;
            PlayerScript.instance.cinemachineInputProvider.enabled = false;
            PlayerScript.instance.interact.enabled = false;
            PlayerScript.instance.stamina.enabled = false;
            HUDManager.instance.playerHUD.SetActive(true);
            
            // FADE OUT
            LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
            {
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
                
                if(PrologueSceneManager.instance.languageIndex == 0)
                {
                    startEnglishDialogue.StartDialogue();
                }
                else
                {
                    startTagalogDialogue.StartDialogue();
                }

                Pause.instance.PauseCanInput(true);
            });

        });
    }

    void CameraDeviceChecker()
    {
        var pov =   playerLayingDownVC.GetCinemachineComponent<CinemachinePOV>();

        if(DeviceManager.instance.keyboardDevice)
        {
            promptImg.sprite = promptSprite[0]; 
            pov.m_HorizontalAxis.m_MaxSpeed = .1f;
            pov.m_VerticalAxis.m_MaxSpeed = .1f;
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            promptImg.sprite = promptSprite[1]; 
            pov.m_VerticalAxis.m_MaxSpeed = 1f;
            pov.m_HorizontalAxis.m_MaxSpeed = 1f;
        }
    }

    public void PlayerMove(bool active)
    {
        StartCoroutine(DelayCanMove(active));
    }

    IEnumerator DelayCanMove(bool active)
    {
        yield return new WaitForSeconds(2.5f);
        canMove = active;

    }
}

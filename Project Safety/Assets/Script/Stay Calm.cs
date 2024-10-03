using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using System;
using Microsoft.Unity.VisualStudio.Editor;

public class StayCalm : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("HUD")]
    [SerializeField] CanvasGroup stayCalmHUD;
    [SerializeField] RectTransform stayCalmRectTransform;
    [SerializeField] CanvasGroup stayCalmCG;

    [Space(10)]
    [SerializeField] Image stayCalmVisualImage;
    [SerializeField] Sprite[] stayCalmVisualKeyboard;
    [SerializeField] Sprite[] stayCalmVisualGamepad;

    [Space(10)]
    [SerializeField] int inputsPerformed;
    [SerializeField] int inputNeedToFinish;

    [Header("Flag")]
    bool canInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        playerControls.StayCalm.Button1.performed += Button1Pressed;
        playerControls.StayCalm.Button2.performed += Button2Pressed;
        playerControls.StayCalm.Button3.performed += Button3Pressed;
        playerControls.StayCalm.Button4.performed += Button4Pressed;
        
        playerControls.StayCalm.Enable();

        StayCalmTrigger();

    }

    private void Button1Pressed(InputAction.CallbackContext context)
    {
        if(canInput)
        {

        }
    }

    private void Button2Pressed(InputAction.CallbackContext context)
    {
        if(canInput)
        {

        }
    }

    private void Button3Pressed(InputAction.CallbackContext context)
    {
        if(canInput)
        {

        }
    }

    private void Button4Pressed(InputAction.CallbackContext context)
    {
        if(canInput)
        {

        }
    }

    void StayCalmTrigger()
    {
        HUDManager.instance.playerHUD.SetActive(false);

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
        if(DeviceManager.instance.keyboardDevice)
        {
            
        }
        else if(DeviceManager.instance.gamepadDevice)
        {

        }
    }

    void ChangeImageStatus()
    {
        
    }
}

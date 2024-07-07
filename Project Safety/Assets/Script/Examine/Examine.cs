using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Examine : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Script")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Interact interact;
    [SerializeField] Stamina stamina;
    [SerializeField] CinemachineInputProvider cinemachineInputProvider;
    [Space(10)]
    Item item;
    
    [Header("Examine")]
    [SerializeField] GameObject playerHUD;
    [SerializeField] GameObject examineHUD;
    [SerializeField] GameObject itemDescriptionHUD;
    [SerializeField] GameObject itemNameHUD;
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] TMP_Text itemDescriptionText;
    bool examineMode;
    
    [Space(5)]
    [SerializeField] Image returnImageHUD;
    [SerializeField] Image readImageHUD;
    [SerializeField] GameObject readGameObjectText;
    [SerializeField] Image rotateImage1HUD;
    [SerializeField] GameObject rotateGameObjectText;
    [SerializeField] Image rotateImage2HUD;

    [Space(5)]
    [SerializeField] Sprite escSprite;
    [SerializeField] Sprite eSprite;
    [SerializeField] Sprite lmbSprite;

    [Space(5)]
    [SerializeField] Sprite circleSprite;
    [SerializeField] Sprite triangleSprite;
    [SerializeField] Sprite leftShoulderSprite;
    [SerializeField] Sprite rightStickSprite;

    // EXAMINE OBJECT POSITION AND ROTATION
    [Space(10)]
    // GameObject examineObject;
    Vector3 originalPosition;
    Vector3 originalRotation;
    Vector3 targetPosition;
    
    // LERP
    [Space(10)]
    [SerializeField]float lerpStartTime;
    [SerializeField]float lerpDuration = 2f;
    [SerializeField]bool isLerping;
    
    // EXAMINED ITEM OBJECT
    [Space(10)]
    [SerializeField] float rotationSpeed;
    Vector2 rotationInput;
    Vector2 gamepadRotationInput;
    float xAxis;
    float yAxis;
    bool isLock;
    // [Header("DoTween")]
    // [SerializeField] float punchDuration = 0.3f;
    // [SerializeField] float punchScale = 0.2f;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Examine.Lock.performed += ctx => isLock = true;
        playerControls.Examine.Lock.canceled += ctx => isLock = false;

        playerControls.Examine.Rotation.performed += ctx => rotationInput = ctx.ReadValue<Vector2>();

        playerControls.Examine.GamepadRotation.performed += ctx => gamepadRotationInput = ctx.ReadValue<Vector2>();
        
        playerControls.Examine.Read.performed += ToRead;
        playerControls.Examine.Back.performed += ToBack;

        playerControls.Examine.Enable();
    }

    #region - TO READ -

    private void ToRead(InputAction.CallbackContext context)
    {
        if(examineMode)
        {
            if(item.itemSO.isReadble)
            {
                itemDescriptionHUD.SetActive(true);
                
                itemDescriptionText.text = item.itemSO.itemDescription;
            }
        }
    }
    
    #endregion

    #region  - TO BACK -

    private void ToBack(InputAction.CallbackContext context)
    {
        if(itemDescriptionHUD.activeSelf)
        {
            itemDescriptionHUD.SetActive(false);
        }
        else
        {
            examineMode = false;

            interact.interactObject.transform.position = originalPosition;
            interact.interactObject.transform.eulerAngles = originalRotation;

            interact.interactObject = null;
            // examineObject = null;
            item = null;
            playerMovement.playerAnim.enabled = true;

            playerHUD.SetActive(true);
            examineHUD.SetActive(false);

            // DISABLE SCRIPT
            this.enabled = false;

            // ENABLE SCRIPT
            playerMovement.enabled = true;
            interact.enabled = true;
            stamina.enabled = true;
            cinemachineInputProvider.enabled = true;
        }
    }

    #endregion

    void OnDisable()
    {   
        playerControls.Examine.Disable();
    }

    void Update()
    {
        if (!examineMode && !isLerping)
        {
            item = interact.interactObject.GetComponent<Item>();

            itemNameText.text = item.itemSO.itemName;

            originalPosition = interact.interactObject.transform.position;
            originalRotation = interact.interactObject.transform.rotation.eulerAngles;
            //targetObjectPosition = Camera.main.transform.position + (Camera.main.transform.forward * item.itemSO.itemDistanceToPlayer) - (Camera.main.transform.right * xOffset);
            targetPosition = Camera.main.transform.position + (Camera.main.transform.forward * item.itemSO.itemDistanceToPlayer);

            // Vector3 lookAtDirection = (Camera.main.transform.position - target.transform.position).normalized;

            // Quaternion targetRotation = Quaternion.LookRotation(lookAtDirection, Vector3.up);
            // interact.interactObject.transform.rotation = targetRotation;

            isLerping = true;
            lerpStartTime = Time.time;
        }

        if (isLerping)
        {
            
            float lerpTime = Time.time - lerpStartTime;
            float t = Mathf.Clamp01(lerpTime / lerpDuration);

            // Interpolate the position
            interact.interactObject.transform.position = Vector3.Lerp(originalPosition, targetPosition, t);

            if (t >= 1f)
            {
                // Ensure object reaches exactly to the target position
                interact.interactObject.transform.position = targetPosition;
                isLerping = false;
                examineMode = true;
                playerMovement.playerAnim.enabled = false;
            }
        }

        HUDDisplay();
        RotationInput();
    }


    void HUDDisplay()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            ChangeImageStatus(false, item.itemSO.isReadble,escSprite, eSprite, null, lmbSprite);

        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            ChangeImageStatus(true, item.itemSO.isReadble, circleSprite, triangleSprite, leftShoulderSprite, rightStickSprite);
            
            // if(item.itemSO.isReadble)
            // {
            //     ChangeImageStatus(true, circleSprite, triangleSprite, leftShoulderSprite, rightStickSprite);
            // }
        }        
    }


    void RotationInput()
    {
        if (examineMode && isLock)
        {
            if (DeviceManager.instance.keyboardDevice)
            {
                xAxis = rotationInput.x * 0.5f;
                yAxis = rotationInput.y * 0.5f;
            }
            else if (DeviceManager.instance.gamepadDevice) 
            {
                xAxis = gamepadRotationInput.x * (rotationSpeed * 20f);
                yAxis = gamepadRotationInput.y * (rotationSpeed * 20f);
            }
            
            interact.interactObject.transform.Rotate(Vector3.up, -xAxis, Space.World);
            interact.interactObject.transform.Rotate(Vector3.right, -yAxis, Space.World);
        }
    }

    void ChangeImageStatus(bool leftShoulderActive, bool isReadble, Sprite returnSprite, Sprite readSprite, Sprite rotateSprite1, Sprite rotateSprite2)
    {
        rotateImage1HUD.gameObject.SetActive(leftShoulderActive);
        rotateGameObjectText.SetActive(leftShoulderActive);
        readImageHUD.gameObject.SetActive(isReadble);
        readGameObjectText.SetActive(isReadble);

        returnImageHUD.sprite = returnSprite;
        readImageHUD.sprite = readSprite;
        rotateImage1HUD.sprite = rotateSprite1;
        rotateImage2HUD.sprite = rotateSprite2;
    }
}


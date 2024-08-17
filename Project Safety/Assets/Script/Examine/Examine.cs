using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Examine : MonoBehaviour
{
    [Header("Script")]
    Item item;
    
    [Header("Examine")]
    [SerializeField] GameObject itemDescriptionHUD;
    [SerializeField] GameObject itemNameHUD;
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] TMP_Text itemDescriptionText;
    bool examineMode;
    
    [Space(5)]
    [SerializeField] GameObject[] examineInstruction;

    [Space(5)]
    [SerializeField] Image[] sprite;
    [Space(5)]
    [SerializeField] Sprite[] keyboardSprite;

    [Space(5)]
    [SerializeField] Sprite[] gamepadSprite;


    // EXAMINE OBJECT POSITION AND ROTATION
    [Space(10)]
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

    void OnEnable()
    {
        ScriptManager.instance.playerControls.Examine.Lock.performed += ctx => isLock = true;
        ScriptManager.instance.playerControls.Examine.Lock.canceled += ctx => isLock = false;

        ScriptManager.instance.playerControls.Examine.Rotation.performed += ctx => rotationInput = ctx.ReadValue<Vector2>();

        ScriptManager.instance.playerControls.Examine.GamepadRotation.performed += ctx => gamepadRotationInput = ctx.ReadValue<Vector2>();
        
        ScriptManager.instance.playerControls.Examine.Read.performed += ToRead;
        ScriptManager.instance.playerControls.Examine.Back.performed += ToBack;

        ScriptManager.instance.playerControls.Examine.Enable();
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

            ScriptManager.instance.interact.interactObject.transform.position = originalPosition;
            ScriptManager.instance.interact.interactObject.transform.eulerAngles = originalRotation;

            ScriptManager.instance.interact.interactObject = null;
            // examineObject = null;
            item = null;
            ScriptManager.instance.playerMovement.playerAnim.enabled = true;

            HUDManager.instance.playerHUD.SetActive(true);
            HUDManager.instance.examineHUD.SetActive(false);

            // DISABLE SCRIPT
            this.enabled = false;

            // ENABLE SCRIPT
            ScriptManager.instance.playerMovement.enabled = true;
            ScriptManager.instance.interact.enabled = true;
            ScriptManager.instance.stamina.enabled = true;
            ScriptManager.instance.cinemachineInputProvider.enabled = true;
        }
    }

    #endregion

    void OnDisable()
    {   
        ScriptManager.instance.playerControls.Examine.Disable();
    }

    void Update()
    {
        
        if (!examineMode && !isLerping)
        {
            item = ScriptManager.instance.interact.interactObject.GetComponent<Item>();

            itemNameText.text = item.itemSO.itemName;

            originalPosition = ScriptManager.instance.interact.interactObject.transform.position;
            originalRotation = ScriptManager.instance.interact.interactObject.transform.rotation.eulerAngles;
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
            ScriptManager.instance.interact.interactObject.transform.position = Vector3.Lerp(originalPosition, targetPosition, t);

            if (t >= 1f)
            {
                // Ensure object reaches exactly to the target position
                ScriptManager.instance.interact.interactObject.transform.position = targetPosition;
                isLerping = false;
                examineMode = true;
                ScriptManager.instance.playerMovement.playerAnim.enabled = false;
            }
        }

        HUDDisplay();
        RotationInput();
    }


    void HUDDisplay()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            ChangeImageStatus(false, item.itemSO.isReadble, keyboardSprite[0], keyboardSprite[1], null, keyboardSprite[2]);
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            ChangeImageStatus(true, item.itemSO.isReadble, gamepadSprite[0], gamepadSprite[1], gamepadSprite[2], gamepadSprite[3]);
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
            
            ScriptManager.instance.interact.interactObject.transform.Rotate(Vector3.up, -xAxis, Space.World);
            ScriptManager.instance.interact.interactObject.transform.Rotate(Vector3.right, -yAxis, Space.World);
        }
    }

    void ChangeImageStatus(bool isActiveImageHUD, bool isReadble, Sprite returnSprite, Sprite readSprite, Sprite rotateSprite1, Sprite rotateSprite2)
    {
        // [SerializeField] GameObject[] examineInstruction;
        //  [0] - Return
        //  [1] - Read
        //  [2] - Read Text
        //  [3] - Rotate 1
        //  [4] - Rotate Text
        //  [5] - Rotate 2

        if(DeviceManager.instance.keyboardDevice)
        {
            examineInstruction[3].SetActive(false);
            examineInstruction[4].SetActive(false);
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            examineInstruction[3].SetActive(true);
            examineInstruction[4].SetActive(true);
        }

        examineInstruction[1].SetActive(isReadble);
        examineInstruction[2].SetActive(isReadble);
        examineInstruction[3].SetActive(isActiveImageHUD);
        examineInstruction[4].SetActive(isActiveImageHUD);

        // rotateImage1HUD.gameObject.SetActive(leftShoulderActive);
        // rotateGameObjectText.SetActive(leftShoulderActive);
        // readImageHUD.gameObject.SetActive(isReadble);
        // readGameObjectText.SetActive(isReadble);

        sprite[0].sprite = returnSprite;
        sprite[1].sprite = readSprite;
        sprite[2].sprite = rotateSprite1;
        sprite[3].sprite = rotateSprite2;
        // returnImageHUD.sprite = returnSprite;
        // readImageHUD.sprite = readSprite;
        // rotateImage1HUD.sprite = rotateSprite1;
        // rotateImage2HUD.sprite = rotateSprite2;
    }
}


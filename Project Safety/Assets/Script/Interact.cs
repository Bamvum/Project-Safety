using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class Interact : MonoBehaviour
{
    PlayerControls playerControls;

    [SerializeField] Transform handIKTarget;
    [SerializeField] Transform handTarget;

    [Header("Scripts")]     
    Item item;
    [HideInInspector] public DialogueTrigger dialogueTrigger;
    Interactable interactable;

    [Header("Interact")]     
    [SerializeField] float interactRange = 1.5f;
    [HideInInspector] public GameObject interactObject;
    public RaycastHit hit;

    [Header("Interact HUD")]
    public Image[] interactImage;
    public Sprite[] sprite;

    void Awake()
    {
        playerControls = new PlayerControls();
    }
    void OnEnable()
    {
        playerControls.Player.Interact.performed += ToInteract;
        playerControls.Player.Examine.performed += ToExamine;

        playerControls.Player.Enable();
    }
    
    void OnDisable()
    {
        playerControls.Player.Disable();
    }

    #region - TO INTERACT -

    private void ToInteract(InputAction.CallbackContext context)
    {   
        if(hit.collider != null)
        {
            if(item != null)
            {
                if(item.itemSO.isTakable)
                {
                    // ITEM INTRACTION
                    handIKTarget.position = hit.collider.transform.position;

                    PlayerScript.instance.playerMovement.playerAnim.SetTrigger("Grab");
                    Debug.Log("Item Interact!");
                }
            }
            else if (dialogueTrigger != null)
            {
                // NPC INTRACTION
                
                Debug.Log("NPC Interact!");
                dialogueTrigger.StartDialogue(); 
            }
            else if(interactable != null)
            {
                if(interactable.isAlarm)
                {
                    interactable.Alarm();
                }
                else if (interactable.isLightSwitch)
                {
                    interactable.LightSwitchTrigger();
                }
                else if (interactable.isDoor)
                {
                    interactable.DoorTrigger();
                }
                else if (interactable.isPC)
                {
                    interactable.PC();
                }
                else if (interactable.isMonitor)
                {
                    interactable.AccessMonitor();
                }
                else if(interactable.isSocketPlug)
                {
                    interactable.Unplug();
                }
                else if(interactable.isWardrobe)
                {
                    interactable.ChangeClothes();
                }
                else if(interactable.isOutsideDoor)
                {
                    interactable.GoOutside();

                }
                else if (interactable.isBus)
                {
                    interactable.BussEnter();
                }
            }
        }
    }
    
    #endregion

    #region - TO EXAMINE -

    private void ToExamine(InputAction.CallbackContext context)
    {
        if(hit.collider != null && item != null)
        {
            Debug.Log("Item Examine!!");

            interactObject = hit.collider.gameObject;

            HUDManager.instance.playerHUD.SetActive(false);
            HUDManager.instance.examineHUD.SetActive(true);

            // TODO - DISABLE SCRIPT
            this.enabled = false;
            PlayerScript.instance.playerMovement.enabled = false;
            // ScriptManager.instance.stamina.enabled = false;
            PlayerScript.instance.cinemachineInputProvider.enabled = false;
            
            // // TODO - ENABLE SCRIPT
            // ScriptManager.instance.examine.enabled = true;
        }
    }
    
    #endregion

    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactRange, Color.green);

        if(hit.collider != null)
        {
            ResetPorperties();
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, interactRange))
        {

            if(hit.collider.gameObject.layer == 6)
            {
                Debug.Log("Item");
                ItemRaycast();
            }

            if(hit.collider.gameObject.layer == 7)
            {
                Debug.Log("Dialogue");
                DialogueRaycast();
            }
    
            if(hit.collider.gameObject.layer == 8)
            {
                Debug.Log("Interactable");
                InteractableRaycast();
            }
        }
    }

    #region  - INTERACT RESET PROPERTIES (NULL) -

    void ResetPorperties()
    {
        item = null;
        dialogueTrigger = null;
        interactable = null;

        PlayerScript.instance.playerMovement.playerAnim.SetBool("Interact", false);

        // REMOVE SPRITE IN IMAGE 
        ChangeImageStatus(false, false, null);
    }

    #endregion

    #region  - ITEM RAYCAST -

    void ItemRaycast()
    {
        item = hit.collider.GetComponent<Item>();

        if (item.itemSO.isTakable)
        {
            ChangeImageStatus(true, true, sprite[0]);
        }
        else
        {
            ChangeImageStatus(true, false, null);
        }
    }

    #endregion

    #region  - DIALOGUE RAYCAST -

    void DialogueRaycast()
    {
        dialogueTrigger = hit.collider.GetComponent<DialogueTrigger>();

        ChangeImageStatus(false, true, sprite[1]);
    }

    #endregion

    #region  - INTERACTABLE RAYCAST -

    void InteractableRaycast()
    {
        interactable = hit.collider.GetComponent<Interactable>();

        ChangeImageStatus(false, true, sprite[0]);
    }

    #endregion
    
    void ChangeImageStatus(bool activeLeftIMGStatus, bool activeRightIMGStatus, Sprite imgSprite)
    {
        interactImage[0].gameObject.SetActive(activeLeftIMGStatus);
        interactImage[1].gameObject.SetActive(activeRightIMGStatus);
        interactImage[1].sprite = imgSprite;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class Interact : MonoBehaviour
{
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
    
    void OnEnable()
    {
        PlayerScript.instance.playerControls.Player.Interact.performed += ToInteract;
        PlayerScript.instance.playerControls.Player.Examine.performed += ToExamine;

        PlayerScript.instance.playerControls.Player.Enable();
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
                if (interactable.isLightSwitch)
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

    void OnDisable()
    {
        PlayerScript.instance.playerControls.Player.Disable();
    }

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

    void ResetPorperties()
    {
        item = null;
        dialogueTrigger = null;
        interactable = null;

        PlayerScript.instance.playerMovement.playerAnim.SetBool("Interact", false);

        // REMOVE SPRITE IN IMAGE 
        ChangeImageStatus(false, false, null);
    }

    void ItemRaycast()
    {
        item = hit.collider.GetComponent<Item>();

        if (item.itemSO.isTakable)
        {
            ChangeImageStatus(true, true, HUDManager.instance.sprite[0]);
        }
        else
        {
            ChangeImageStatus(true, false, null);
        }
    }

    void DialogueRaycast()
    {
        dialogueTrigger = hit.collider.GetComponent<DialogueTrigger>();

        

        ChangeImageStatus(false, true, HUDManager.instance.sprite[1]);
    }

    void InteractableRaycast()
    {
        interactable = hit.collider.GetComponent<Interactable>();

        ChangeImageStatus(false, true, HUDManager.instance.sprite[0]);
    }
    
    void ChangeImageStatus(bool activeLeftIMGStatus, bool activeRightIMGStatus, Sprite imgSprite)
    {
        HUDManager.instance.interactImage[0].gameObject.SetActive(activeLeftIMGStatus);
        HUDManager.instance.interactImage[1].gameObject.SetActive(activeRightIMGStatus);
        HUDManager.instance.interactImage[1].sprite = imgSprite;
    }
}

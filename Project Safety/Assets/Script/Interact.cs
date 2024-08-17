using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class Interact : MonoBehaviour
{
    [SerializeField] Animator playerAnim;
    [SerializeField] Transform handIKTarget;
    [SerializeField] Transform handTarget;

    [Header("Scripts")]     
    Item item;
    [HideInInspector] public DialogueTrigger dialogueTrigger;
    Interactable interactable;

    
    [Header("Interact")]     
    [SerializeField] float interactRange = 2.5f;
    [HideInInspector] public GameObject interactObject;
    RaycastHit hit;

    
    [Header("Interact/Examine HUD")]     
    [SerializeField] Image[] interactImage;
    [SerializeField] Sprite[] sprite;


    void Awake()
    {
        playerAnim = GetComponent<Animator>();

        ScriptManager.instance.playerControls = new PlayerControls();
    }
    
    void OnEnable()
    {
        ScriptManager.instance.playerControls.Player.Interact.performed += ToInteract;
        ScriptManager.instance.playerControls.Player.Examine.performed += ToExamine;

        ScriptManager.instance.playerControls.Player.Enable();
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
                    playerAnim.SetTrigger("Grab");
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
                // INTERACTABLE INTRACTION
                // handIKTarget.position = hit.collider.transform.position;
                // playerAnim.SetTrigger("Interact");

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

            // handIKTarget.position = hit.collider.transform.position;
            // playerAnim.SetTrigger("Interact");
            
            interactObject = hit.collider.gameObject;

            HUDManager.instance.playerHUD.SetActive(false);
            HUDManager.instance.examineHUD.SetActive(true);

            // TODO - DISABLE SCRIPT
            this.enabled = false;
            ScriptManager.instance.playerMovement.enabled = false;
            ScriptManager.instance.stamina.enabled = false;
            ScriptManager.instance.cinemachineInputProvider.enabled = false;
            
            // TODO - ENABLE SCRIPT
            ScriptManager.instance.examine.enabled = true;
        }
    }
    
    #endregion

    void OnDisable()
    {
        ScriptManager.instance.playerControls.Player.Disable();
    }

    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactRange, Color.green);

        if(hit.collider != null)
        {
            item = null;
            dialogueTrigger = null;
            interactable = null;

            playerAnim.SetBool("Interact", false);

            // REMOVE SPRITE IN IMAGE 
            ChangeImageStatus(false, false, null);
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            #region - ITEM RAYCAST -

            if(hit.collider.gameObject.layer == 6)
            {
                item = hit.collider.GetComponent<Item>();

                if(item.itemSO.isTakable)
                {
                    ChangeImageStatus(true, true, sprite[0]);
                }
                else
                {
                    ChangeImageStatus(true, false, null);
                }
            }
        
            #endregion

            #region - DIALOGUE RAYCAST -

            if(hit.collider.gameObject.layer == 7)
            {
                dialogueTrigger = hit.collider.GetComponent<DialogueTrigger>();

                Debug.Log("NPC Name: " + hit.collider.name);

                ChangeImageStatus(false, true, sprite[1]);
            }
        
            #endregion

            #region - INTERACTABLE ENVIRONMENT RAYCAST -

            if(hit.collider.gameObject.layer == 8)
            {
                interactable = hit.collider.GetComponent<Interactable>();

                ChangeImageStatus(false, true, sprite[0]);
            }

            #endregion
        }
    }

    void ChangeImageStatus(bool activeLeftIMGStatus, bool activeRightIMGStatus, Sprite imgSprite)
    {
        interactImage[0].gameObject.SetActive(activeLeftIMGStatus);
        interactImage[1].gameObject.SetActive(activeRightIMGStatus);
        interactImage[1].sprite = imgSprite;
    }
}

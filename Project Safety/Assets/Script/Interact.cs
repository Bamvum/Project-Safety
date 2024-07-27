using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;

public class Interact : MonoBehaviour
{
    PlayerControls playerControls;

    [SerializeField] Animator playerAnim;
    [SerializeField] Transform handIKTarget;
    [SerializeField] Transform handTarget;

    [Header("Scripts")]     
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Examine examine;
    [SerializeField] Stamina stamina;
    [SerializeField] CinemachineInputProvider cinemachineInputProvider;
    Item item;
    [HideInInspector] public DialogueTrigger dialogueTrigger;
    Interactable interactable;

    
    [Header("Interact")]     
    [SerializeField] GameObject playerHUD;
    [SerializeField] float interactRange = 2.5f;
    [HideInInspector] public GameObject interactObject;
    RaycastHit hit;

    
    [Header("Interact/Examine HUD")]     
    [SerializeField] GameObject examineHUD;
    [SerializeField] Image leftInteractHUDImage; 
    [SerializeField] Image rightInteractHUDImage; 
    [SerializeField] Sprite interactSprite;
    [SerializeField] Sprite talkSprite;
    [SerializeField] Sprite examineSprite;

    void Awake()
    {
        playerAnim = GetComponent<Animator>();

        playerControls = new PlayerControls();
    }
    
    void OnEnable()
    {
        playerControls.Player.Interact.performed += ToInteract;
        playerControls.Player.Examine.performed += ToExamine;

        playerControls.Player.Enable();
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
                    handIKTarget.position = hit.collider.transform.position;
                    playerAnim.SetTrigger("Interact");
                    Debug.Log("Item Interact!");
                }
            }
            else if (dialogueTrigger != null)
            {
                Debug.Log("NPC Interact!");
                dialogueTrigger.StartDialogue();

                // NOTE:
                // In Dialogue Trigger script, Make sure to assign PlayerMovement, Interact,
                // and Camera Movement script to Dialogue Event and disable the script. 
            }
            else if(interactable != null)
            {  
                Debug.Log("Interactable Interact!");
                
                if(interactable.isLightSwitch)
                {
                    Debug.Log("Light Switch Interacted!");
                    interactable.LightSwitchTrigger();
                }
                else if(interactable.isDoor)
                {

                }
                else if(interactable.isPC)
                {
                    interactable.PC();
                }
                else if(interactable.isMonitor)
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

            playerHUD.SetActive(false);
            examineHUD.SetActive(true);

            // TODO - DISABLE SCRIPT
            this.enabled = false;
            playerMovement.enabled = false;
            stamina.enabled = false;
            cinemachineInputProvider.enabled = false;
            
            // TODO - ENABLE SCRIPT
            examine.enabled = true;
        }
    }
    
    #endregion

    void OnDisable()
    {
        
        playerControls.Player.Disable();
    }

    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactRange, Color.green);

        if(hit.collider != null)
        {
            item = null;
            dialogueTrigger = null;
            interactable = null;

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
                    ChangeImageStatus(true, true, interactSprite);
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

                ChangeImageStatus(false, true, talkSprite);
            }
        
            #endregion

            #region - INTERACTABLE ENVIRONMENT RAYCAST -

            if(hit.collider.gameObject.layer == 8)
            {
                interactable = hit.collider.GetComponent<Interactable>();

                ChangeImageStatus(false, true, interactSprite);
            }

            #endregion
        }
    }

    void ChangeImageStatus(bool activeLeftIMGStatus, bool activeRightIMGStatus, Sprite imgSprite)
    {
        leftInteractHUDImage.gameObject.SetActive(activeLeftIMGStatus);
        rightInteractHUDImage.gameObject.SetActive(activeRightIMGStatus);
        rightInteractHUDImage.sprite = imgSprite;
    }
}

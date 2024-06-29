using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    PlayerControls playerControls;

    [SerializeField] Animator playerAnim;
    [SerializeField] Transform handIKTarget;
    [SerializeField] Transform handTarget;

    [Header("Scripts")]     
    Item item;
    public DialogueTrigger dialogueTrigger;

    
    [Header("Interact")]     
    [SerializeField] float interactRange = 2.5f;
    RaycastHit hit;

    
    [Header("Interact HUD")]     
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


    private void ToInteract(InputAction.CallbackContext context)
    {   
        if(hit.collider != null)
        {
            if(item != null)
            {
                handIKTarget.position = hit.collider.transform.position;
                playerAnim.SetTrigger("Interact");
                Debug.Log("Item Interact!");
            }
            else if (dialogueTrigger != null)
            {
                Debug.Log("NPC Interact!");
                dialogueTrigger.StartDialogue();

                // NOTE:
                // In Dialogue Trigger script, Make sure to assign PlayerMovement, Interact,
                // and Camera Movement script to Dialogue Event and disable the script. 
            }
        }
    }
    
    private void ToExamine(InputAction.CallbackContext context)
    {
        if(hit.collider != null && item != null)
        {
            Debug.Log("Item Examine!!");
        }
    }
    
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
        }
    }

    void ChangeImageStatus(bool activeLeftIMGStatus, bool activeRightIMGStatus, Sprite imgSprite)
    {
        leftInteractHUDImage.gameObject.SetActive(activeLeftIMGStatus);
        rightInteractHUDImage.gameObject.SetActive(activeRightIMGStatus);
        rightInteractHUDImage.sprite = imgSprite;

        
    }
}

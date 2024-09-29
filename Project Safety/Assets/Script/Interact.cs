using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using System;
using Unity.VisualScripting;

public class Interact : MonoBehaviour
{
    PlayerControls playerControls;

    [SerializeField] Transform handIKTarget;
    [SerializeField] Transform handParent;
    [SerializeField] TwoBoneIKConstraint leftHandExtinguisher;
    [SerializeField] TwoBoneIKConstraint rightHandExtinguisher;

    [Header("Scripts")]     
    Item item;
    [HideInInspector] public DialogueTrigger dialogueTrigger;
    Interactable interactable;

    [Header("Interact")]     
    [SerializeField] float interactRange = 1.5f;
    public GameObject inHandItem;
    public Rigidbody inHandItemRB;
    public GameObject interactObject;
    public Rigidbody rb;
    public RaycastHit hit;

    [Header("Interact HUD")]
    public Image[] interactImage;
    public Sprite[] sprite;

    [Header("Animation")]
    public AnimationClip grab;
    public float grabLength;

    void Awake()
    {
        grabLength = grab.length;

        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Player.Interact.performed += ToInteract;
        playerControls.Player.Examine.performed += ToExamine;
        playerControls.Player.Drop.performed += ToDrop;
        playerControls.Player.Use.performed += ToUse;

        playerControls.Player.Enable();
    }


    void OnDisable()
    {
        playerControls.Player.Disable();
    }

    #region - TO INTERACT -

    
    IEnumerator PickUpItem(float duration)
    {
        yield return new WaitForSeconds(duration - 0.7f);
        
        inHandItem.transform.SetParent(handParent, true);
        inHandItem.transform.localPosition = new Vector3(0.248f, -0.113f, 0.077f);
        inHandItem.transform.localRotation = Quaternion.Euler(-146.812f, 29.77299f, 214.768f);

        yield return new WaitForSeconds(duration);

        leftHandExtinguisher.weight = 1;
        rightHandExtinguisher.weight = 1;
    }


    private void ToInteract(InputAction.CallbackContext context)
    {   
        if(hit.collider != null)
        {
            if(item != null && inHandItem == null )
            {
                if (item.itemSO.isTakable)
                {
                    // ITEM INTRACTION
                    handIKTarget.position = hit.collider.transform.position;

                    // ANIMATION
                    PlayerScript.instance.playerMovement.playerAnim.SetTrigger("Grab");

                    inHandItem = hit.collider.gameObject;
                    inHandItemRB = inHandItem.GetComponent<Rigidbody>();
                    inHandItem.layer = 0;

                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }

                    float clipDuration = grab.length;
                    StartCoroutine(PickUpItem(clipDuration));
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
            
            if(rb != null)
            {
                rb.isKinematic = true;
            }

            HUDManager.instance.playerHUD.SetActive(false);
            HUDManager.instance.examineHUD.SetActive(true);

            // TODO - DISABLE SCRIPT
            this.enabled = false;
            PlayerScript.instance.playerMovement.enabled = false;
            PlayerScript.instance.stamina.enabled = false;
            PlayerScript.instance.cinemachineInputProvider.enabled = false;
            
            // TODO - ENABLE SCRIPT
            PlayerScript.instance.examine.enabled = true;
        }
    }
    
    #endregion

    #region - TO DROP -

    private void ToDrop(InputAction.CallbackContext context)
    {
        if(inHandItem != null)
        {
            // DROP INHANDITEM
            inHandItem.transform.SetParent(null);
            inHandItem.layer = 6;
            
            if (inHandItemRB != null)
            {
                inHandItemRB.isKinematic = false;
            }

            inHandItem = null;
            inHandItemRB = null;

            leftHandExtinguisher.weight = 0;
            rightHandExtinguisher.weight = 0;

            StartCoroutine(DropItem());
        }
    }

    IEnumerator DropItem()
    {
        yield return null;
    }
    

    #endregion
    
    #region - TO USE -

    private void ToUse(InputAction.CallbackContext context)
    {
        if(inHandItem != null)
        {
            IUsable usable = inHandItem.GetComponent<IUsable>();
            if(usable != null)
            {
                usable.Use(this.gameObject);
            }
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
        interactObject = null;
        rb = null;
        
        PlayerScript.instance.playerMovement.playerAnim.SetBool("Interact", false);

        // REMOVE SPRITE IN IMAGE 
        ChangeImageStatus(false, false, null);
    }

    #endregion

    #region  - ITEM RAYCAST -

    void ItemRaycast()
    {
        item = hit.collider.GetComponent<Item>();
        rb = hit.collider.GetComponent<Rigidbody>();
        interactObject = hit.collider.gameObject;

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

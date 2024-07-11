using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class EmergencyHotline : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Scripts")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Interact interact;
    [SerializeField] Stamina stamina;
    [SerializeField] CinemachineInputProvider cinemachineInputProvider;
    
    [Header("Cinemachine")]
    [SerializeField] CinemachineVirtualCamera playerVC;
    [SerializeField] CinemachineVirtualCamera phoneVC;
    
    [Header("HUD")]
    [SerializeField] GameObject playerHUD;
    [SerializeField] GameObject contactHUD;
    
    // [Space(10)]

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Contact.Enable();
    }

    void OnDisable()
    {
        playerControls.Contact.Disable();
    }
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TPASS : MonoBehaviour
{
    PlayerControls playerControls;
    [Header("Scripts")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Interact interact;
    
    [SerializeField] Stamina stamina;
    [SerializeField] CinemachineInputProvider cinemachineInputProvider;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {
        
    }
}

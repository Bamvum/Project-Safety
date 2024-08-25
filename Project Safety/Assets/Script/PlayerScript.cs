using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript instance {get; private set;}

    void Awake()
    {
        instance = this;
    
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        playerControls = new PlayerControls();
    }

    public PlayerControls playerControls;

    [Header("Player")]
    public PlayerMovement playerMovement;
    [Space(10)]
    public CinemachineVirtualCamera playerVC;
    public CinemachineBrain cinemachineBrain;
    public CinemachineInputProvider cinemachineInputProvider;
    public Interact interact;
    public Examine examine;
    public Stamina stamina;

    [Space(10)]
    public AudioSource missionSFX;
    

    public void DisablePlayerScripts()
    {
        playerMovement.enabled = false;
        cinemachineInputProvider.enabled = false;
        interact.enabled = false;
        examine.enabled = false;
        stamina.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set;}

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        playerControls = new PlayerControls();
    }

    [Header("Scripts")]

    public PlayerControls playerControls;
    public PlayerMovement playerMovement;
    public CinemachineInputProvider cinemachineInputProvider;
    public Interact interact;
    public Examine examine;
    public Stamina stamina;
    
    [Space(10)]
    public DialogueManager dialogueManager;
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public static ScriptManager instance { get; private set;}

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

        // playerControls = new PlayerControls();
    }
    
    // [Header("Player")]
    // public PlayerControls playerControls;
    // public PlayerMovement playerMovement;

    // public CinemachineInputProvider cinemachineInputProvider;
    // public Interact interact;
    // public Examine examine;
    // public Stamina stamina;
    
    // [Space(10)]
    // public DialogueManager dialogueManager;e

    // [Space(10)]
    // public TransitionManager transitionManager;
    // public Mission mission;


}

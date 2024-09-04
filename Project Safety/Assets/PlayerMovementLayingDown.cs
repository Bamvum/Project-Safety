using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerMovementLayingDown : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Script")]
    [SerializeField] CinemachineInputProvider inputProviderSleeping;

    [Space(10)]
    [SerializeField] GameObject sleepingCharacter;
    [SerializeField] Animator sleepingAnimation;
    [SerializeField] float sleepingCharacterSpeed;

    [Header("Inputs")]
    bool actionPressed;
    Vector3 lookInput;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.PlayerLayingDown.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();

        playerControls.PlayerLayingDown.Action.performed += ctx => actionPressed = true;
        playerControls.PlayerLayingDown.Action.canceled += ctx => actionPressed = false;

        playerControls.PlayerLayingDown.Enable();
    }

    void OnDisable()
    {
        playerControls.PlayerLayingDown.Disable();
    }

    void Start()
    {

    }

    void Update()
    {
        if(actionPressed)
        {
            sleepingAnimation.SetBool("To stand up", true);

            sleepingCharacter.transform.position = Vector3.MoveTowards(sleepingCharacter.transform.position, new Vector3 (sleepingCharacter.transform.position.x, 3.15f, -13.2f), Time.deltaTime * sleepingCharacterSpeed);
        }
    }
}

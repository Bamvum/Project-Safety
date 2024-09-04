using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class PlayerMovementLayingDown : MonoBehaviour
{

    PlayerControls playerControls;

    [Header("Trigger Dialogue")]
    [SerializeField] DialogueTrigger startDialogue;

    [Header("Script")]
    [SerializeField] CinemachineInputProvider inputProviderSleeping;

    [Space(10)]
    [SerializeField] GameObject sleepingCharacter;
    [SerializeField] Animator sleepingAnimation;
    [SerializeField] float sleepingCharacterSpeed;

    [Header("Inputs")]
    bool actionPressed;
    Vector3 lookInput;
    bool toRepeat = true;

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
        if(toRepeat)
        {
            // PROMPT DISPLAY 
            if (PrologueSceneManager.instance.toGetUp && actionPressed)
            {
                sleepingAnimation.SetBool("To stand up", true);

                StartCoroutine(DisableScript());
                toRepeat = false;
            }
        }

        if (sleepingAnimation.GetBool("To stand up"))
        {
            sleepingCharacter.transform.position = Vector3.MoveTowards(sleepingCharacter.transform.position, new Vector3(sleepingCharacter.transform.position.x, 3.15f, -13.2f), Time.deltaTime * sleepingCharacterSpeed);
        }
    }

    IEnumerator DisableScript()
    {
        yield return new WaitForSeconds(5);

        Debug.Log("Disable Script");
        // FADE IN
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
        LoadingSceneManager.instance.fadeImage
                .DOFade(1, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
            PlayerScript.instance.playerMovement.gameObject.SetActive(true);
            // FADE OUT
            LoadingSceneManager.instance.fadeImage
                .DOFade(0, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
            {
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
                startDialogue.StartDialogue();
            });

        });

    }
}

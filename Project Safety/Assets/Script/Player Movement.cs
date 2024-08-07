using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    PlayerControls playerControls;
    
    [Header("Player")]
    [SerializeField] CharacterController characterController;
    [SerializeField] Transform playerBody;
    
    [Header("POV/Camera")]
    public CinemachineVirtualCamera playerVC;
    [Space(10)]
    [SerializeField] GameObject cameraRoot;
    [SerializeField] float upperLimit = -40f;
    [SerializeField] float bottomLimit = 70f;
    [Range(0, 50)]
    [SerializeField] float mouseSensitivity = 21.9f;

     [Header("Movement")]
    [SerializeField] float movementSpeed;
    [SerializeField] float walkSpeed = 3.5f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float crouchSpeed = 1.8f;
    [SerializeField] float jumpForce = 8;
    [SerializeField] float gravity = -9.81f;
   
    [Space(10)]
    public bool runInput;
    bool jumpInput;
    public bool crouchInput;
    Vector3 movementInput;
    float horizontalMovementInput;
    float verticalMovementInput;
    Vector3 lookInput;
    public float horizontalLookInput;
    public float verticalLookInput;
    

    [Header("Animation")]
    public Animator playerAnim;
    bool hasAnim;
    int xVelocityHash;
    int yVelocityHash;
    int zVelocityHash;
    int jumpHash;
    int groundHash;
    int fallingHash;
    int crouchHash;
    float xRotation;
    Vector2 currentVelocity;
    Vector3 velocity;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Player.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();

        playerControls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();

        playerControls.Player.Sprint.performed += ctx => runInput = true;
        playerControls.Player.Sprint.canceled += ctx => runInput = false;

        playerControls.Player.Jump.performed += ctx => jumpInput = true;
        playerControls.Player.Jump.canceled += ctx => jumpInput = false;

        playerControls.Player.Crouch.performed += ctx => crouchInput = true;
        playerControls.Player.Crouch.canceled += ctx => crouchInput = false;
        
        playerControls.Player.Enable();
    }

    void OnDisable()
    {
        ResetInputValue();   
        playerControls.Player.Disable();
    }

    void  ResetInputValue()
    {
        playerAnim.SetFloat(xVelocityHash, 0);
        movementInput = Vector3.zero;
        horizontalMovementInput = 0;
        verticalMovementInput = 0;

        playerAnim.SetFloat(yVelocityHash, 0);
        lookInput = Vector3.zero;
        horizontalLookInput = 0;
        verticalLookInput = 0;
    }

    void Start()
    {
        xVelocityHash = Animator.StringToHash("X_Velocity");
        yVelocityHash = Animator.StringToHash("Y_Velocity");
        zVelocityHash = Animator.StringToHash("Z_Velocity");
        jumpHash = Animator.StringToHash("Jump");
        groundHash = Animator.StringToHash("Grounded");
        fallingHash = Animator.StringToHash("Falling");
        crouchHash = Animator.StringToHash("Crouch");

        Cursor.lockState = CursorLockMode.Locked;  
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        var pov = playerVC.GetCinemachineComponent<CinemachinePOV>();
        
        if(DeviceManager.instance.keyboardDevice)
        {
            pov.m_VerticalAxis.m_MaxSpeed = .1f;
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            pov.m_VerticalAxis.m_MaxSpeed = 1f;
        }
    }

    void LateUpdate()
    {
        CamMovement();
    }

    void Movement()
    {   
        horizontalMovementInput = movementInput.x;
        verticalMovementInput = movementInput.y;

        Vector3 move = new Vector3(horizontalMovementInput, 0, verticalMovementInput);
        move = Camera.main.transform.forward * move.z + Camera.main.transform.right * move.x;
        move.y = 0;
        move.Normalize();

        playerAnim.SetBool(fallingHash, !characterController.isGrounded);
        playerAnim.SetBool(groundHash, characterController.isGrounded);

        #region - JUMP INPUT -

        if(characterController.isGrounded && velocity.y < -2f)
        {
            // LANDING
            playerAnim.SetFloat(zVelocityHash, velocity.y);
            playerAnim.ResetTrigger(jumpHash);
            velocity.y = -3.5f;
        }

        if(SceneManager.GetActiveScene().name != "Prologue")
        {
            if (jumpInput && !crouchInput && characterController.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                playerAnim.SetTrigger("Jump");
            }
        }

        #endregion

        #region - RUN & CROUCH INPUT -
        
        if(crouchInput)
        {
            if(runInput)
            {
                movementSpeed = crouchSpeed;
            }
            else
            {
                movementSpeed = crouchSpeed;
            }

            characterController.center = new Vector3(0,  0.55f, 0.28f);
            characterController.height = 1f;

            playerAnim.SetBool(crouchHash, true);
        }
        else if (SceneManager.GetActiveScene().name != "Prologue" && runInput && !PlayerManager.instance.stamina.outOfStamina)
        {
            movementSpeed = runSpeed;
            playerAnim.SetBool(crouchHash, false);

            characterController.center = new Vector3(0,  1, 0.28f);
            characterController.height = 2;
            
        }
        else
        {
            movementSpeed = walkSpeed;
            playerAnim.SetBool(crouchHash, false);
            
            characterController.center = new Vector3(0,  1, 0.28f);
            characterController.height = 2;
        }

        characterController.Move(move * movementSpeed * Time.fixedDeltaTime);

        #endregion

        currentVelocity.x = Mathf.Lerp(currentVelocity.x, horizontalMovementInput * movementSpeed, 8.9f * Time.fixedDeltaTime);
        currentVelocity.y = Mathf.Lerp(currentVelocity.y, verticalMovementInput * movementSpeed, 8.9f * Time.fixedDeltaTime);

        playerAnim.SetFloat(xVelocityHash, currentVelocity.x);
        playerAnim.SetFloat(yVelocityHash, currentVelocity.y);

        velocity.y += gravity * Time.fixedDeltaTime;
        characterController.Move(velocity * Time.fixedDeltaTime);
    }

    void CamMovement()
    {
        // TODO - FINALIK HEAD MOVEMENT

        if(DeviceManager.instance.keyboardDevice)
        {
            mouseSensitivity = 21.9f;
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            mouseSensitivity = 121.9f;
        }

        horizontalLookInput = lookInput.x;
        verticalLookInput = lookInput.y;

        // Camera.main.transform.position = cameraRoot.transform.position;

        xRotation -= verticalLookInput * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, upperLimit, bottomLimit);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0,0);
        transform.Rotate(Vector3.up, horizontalLookInput * mouseSensitivity * Time.deltaTime);
    }
}

// TODO - NOT MOVE ON AIR
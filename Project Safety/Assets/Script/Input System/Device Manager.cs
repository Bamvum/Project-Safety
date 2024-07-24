using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class DeviceManager : MonoBehaviour
{
    public static DeviceManager instance { get; private set; }

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
    }

    [Header("Device in Use")]
    public bool keyboardDevice;
    public bool gamepadDevice;

    void Update()
    {
        CheckKeyboardMouseInput();
        CheckGamepadInput();
    }

    void CheckKeyboardMouseInput()
    {
        if (Keyboard.current.anyKey.isPressed || Mouse.current.delta.ReadValue() != Vector2.zero ||
            Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed ||
            Mouse.current.middleButton.isPressed)
        {
            Debug.Log("Using Keyboard & Mouse Device");
            keyboardDevice = true;
            gamepadDevice = false;
        }

    }

    void CheckGamepadInput()
    {
        // CHECK IF THERE IS GAMEPAD CONNECTED
        if (Gamepad.current != null)
        {
            foreach (GamepadButton button in System.Enum.GetValues(typeof(GamepadButton)))
            {
                if (Gamepad.current[button].wasPressedThisFrame ||
                    Gamepad.current.leftStick.ReadValue() != Vector2.zero ||
                    Gamepad.current.rightStick.ReadValue() != Vector2.zero)
                {
                    Debug.Log("Using Gamepad Device");
                    keyboardDevice = false;
                    gamepadDevice = true;
                }
            }
        }
    }
}

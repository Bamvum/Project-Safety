using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnAndOffGameObject : MonoBehaviour
{
    [SerializeField] GameObject lightObject;
    [SerializeField] float toggleSpeed = 1.0f; // Time in seconds between toggles
    public bool isToggling = true;   // To start or stop toggling     
    private float timer = 0f;

    // Gamepad vibration variables
    [SerializeField] float vibrationDuration = 0.1f;  // Duration of vibration in seconds
    [SerializeField] float lowFrequency = 0.5f;       // Low-frequency motor intensity
    [SerializeField] float highFrequency = 0.5f;      // High-frequency motor intensity
    
    private void Update()
    {
        if (isToggling)
        {
            // Increase the timer by the time passed since the last frame
            timer += Time.deltaTime;

            // If the timer exceeds the toggleSpeed, toggle the object
            if (timer >= toggleSpeed)
            {
                lightObject.SetActive(!lightObject.activeSelf);  // Toggle the active state
                VibrateGamepad();  // Trigger vibration
                timer = 0f;  // Reset the timer
            }
        }
    }

    // Call this function to stop toggling (optional)
    public void StopToggling()
    {
        isToggling = false;
    }

    // Call this function to start toggling (optional)
    public void StartToggling()
    {
        isToggling = true;
        timer = 0f;  // Reset the timer to avoid immediate toggling
    }

    // Function to trigger gamepad vibration
    private void VibrateGamepad()
    {
        if (Gamepad.current != null)  // Ensure a gamepad is connected
        {
            Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);  // Set vibration intensity
            Invoke("StopVibration", vibrationDuration);  // Stop vibration after the specified duration
        }
    }

    // Function to stop gamepad vibration
    private void StopVibration()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);  // Stop vibration
        }
    }
}

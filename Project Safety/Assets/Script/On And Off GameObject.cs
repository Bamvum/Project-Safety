using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAndOffGameObject : MonoBehaviour
{
    [SerializeField] GameObject lightObject;
    [SerializeField] float toggleSpeed = 1.0f; // Time in seconds between toggles
    public bool isToggling = true;   // To start or stop toggling     
    private float timer = 0f;

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

}

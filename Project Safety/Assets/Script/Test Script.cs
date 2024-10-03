using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    [SerializeField] GameObject selectedGameObject;
    bool isGamepad;
    void Update()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            Cursor.lockState = CursorLockMode.None;
            isGamepad = false;
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            Cursor.lockState = CursorLockMode.Locked;

            if(!isGamepad)
            {
                EventSystem.current.SetSelectedGameObject(selectedGameObject);
                isGamepad = true;
            }
        }
        
    }
}

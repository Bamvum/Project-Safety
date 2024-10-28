using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwapper : MonoBehaviour
{
    [SerializeField] RectTransform swapButton1;
    [SerializeField] RectTransform swapButton2;
    
    public void SwapPosition()
    {
        // Store the current position of button1
        Vector3 tempPosition = swapButton1.anchoredPosition;

        // Swap positions
        swapButton2.anchoredPosition = swapButton2.anchoredPosition;
        swapButton2.anchoredPosition = tempPosition;
    }
}

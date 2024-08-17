using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Fade : MonoBehaviour
{
    PlayerControls playerControls;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Player.Enable();
    }

    void OnDisable()
    {
        playerControls.Player.Disable();
    }
}

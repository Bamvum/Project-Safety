using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

public class TPASS : MonoBehaviour
{
    public static TPASS instance {get; private set;}

    void Awake()
    {
        instance = this;
        playerControls = new PlayerControls();
    }
    
    PlayerControls playerControls;

    [SerializeField] GameObject fireExtinguisherInv;



    void Update()
    {
        // if(fireExtinguisherInv.activeInHierarchy)
        // {

        // }

        // TODO - START TPASS
        //      - TWIST
        //      - PULL
        //      - AIM + WALK MODE
        //      - SQUEEZE & SWEEP MINI GAME
    }


}

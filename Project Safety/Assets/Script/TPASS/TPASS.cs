using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TPASS : MonoBehaviour
{
    [Header("TPASS HUD")]
    public CanvasGroup tpassBackgroundCG;
    
    [Header("TPASS Scripts")]
    [SerializeField] TwistFireExtinguisher twistFE;
    [SerializeField] AimFireExtinguisher aimFE;

    [Header("Fire Extinguisher VC")]
    public CinemachineVirtualCamera twistAndPullVC;
    public CinemachineVirtualCamera SqueezeAndSweepVC;

    [Header("TPASS Status")]
    public bool twistAndPull;
    public bool aimSqueezeAndSweep;

    [Space(10)]
    public GameObject tpassHUD;
    public Image checkMarkDone;
    public AudioSource correctSFX;


    public void CheckTPASS()
    {
        if(!Pause.instance.pauseHUDRectTransform.gameObject.activeSelf)
        {
            if (!twistAndPull)
            {
                twistFE.enabled = true;
                this.enabled = false;
            }
            else
            {
                aimFE.enabled = true;
                this.enabled = false;
            }
        }
        
    }
    
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class TPASS : MonoBehaviour
{
    [Header("TPASS Scripts")]
    [SerializeField] TwistFireExtinguisher twistFE;

    [Header("Fire Extinguisher Type")]
    public bool waterBase; // CLASS A FIRE ONLY
    public bool AFFF; // CLASS A & B FIRE ONLY
    public bool DryChemical; // CLASS A & B FIRE ONLY

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
        if(!twistAndPull)
        {
            twistFE.enabled = true;
            this.enabled = false;
        }
        else
        {
            // GO TO AIM AND SQUEEZE AND SWEEP
        }
    }
    
}

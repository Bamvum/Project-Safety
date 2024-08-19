using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance { get; private set;}

    // void Awake()
    // {
    //     if(instance == null)
    //     {
    //         instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    [Header("Loading")]
    public GameObject loadingUI;
    
    [Header("HUD/UI")]
    public GameObject playerHUD;
    public GameObject examineHUD;
    public GameObject dialogueHUD;
    
    [Space(10)]
    public GameObject instructionHUD;
    public GameObject missionHUD;

    [Header("Prologue HUD/UI")]
    public GameObject homeworkHUD;
    
    public GameObject logInUI;
    public GameObject mainMenuUI;
    public GameObject loadingScreenUI;
}

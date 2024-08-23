using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance { get; private set;}

    // [Header("Dialogue Manager")]
    // public GameObject triggerDialogues;
    
    void Awake()
    {
        instance = this;
        // if (instance == null)
        // {
        //     DontDestroyOnLoad(triggerDialogues);
        // }
        // else
        // {
        //     Destroy(gameObject);
        // }
    }

    // TODO -   CURSOR STATE
    //      -   IF STATEMENT (DIALOGUE HUD, PLAYER HUD,  IS ACTIVE) 


    [Header("Player Related HUD")]
    public GameObject playerHUD;
    public GameObject examineHUD;
    public GameObject dialogueHUD;
    
    [Header("Interact HUD")]
    public Image[] interactImage;
    public Sprite[] sprite;

    

    [Header("Instruction HUD")]
    public GameObject[] instructionPage;
    // 0 - Left
    // 1 - Right
    // 2 - Done
    public GameObject[] instructionButton;
    
    [Space(5)]
    public GameObject instructionHUD;
    public Image instructionBG;
    public RectTransform instructionBGRectTransform;
    public GameObject instructionContent;
    public CanvasGroup instructionContentCG;

    [Space(10)]
    public GameObject keyboardInstruction;
    public GameObject gamepadInstruction;

    [Space(10)]
    public Image[] imageHUD;
    public Sprite[] keyboardSprite;
    public Sprite[] gamepadSprite;

    [Header("Mission HUD")]
    public TMP_Text missionText;
    public RectTransform missionRectTransform;
    public CanvasGroup missionCG;
    
    [Header("Homework HUD")]
    public GameObject homeworkHUD;

    void Start()
    {
        instructionBGRectTransform = instructionBG.GetComponent<RectTransform>();
        instructionContentCG = instructionContent.GetComponent<CanvasGroup>();
        
        InstructionPropertiesReset();
        MissionPropertiesReset();
    }

    void InstructionPropertiesReset()
    {
        // ASSIGN
        instructionBGRectTransform.sizeDelta = new Vector2(0, instructionBGRectTransform.sizeDelta.y);
        instructionContentCG.alpha = 0;
    }

    void MissionPropertiesReset()
    {
        // ASSIGN
        missionRectTransform.anchoredPosition = new Vector2(-325, missionRectTransform.anchoredPosition.y);
        missionCG.alpha = 0;
    }
}

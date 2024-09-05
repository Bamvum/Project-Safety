using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

public class InstructionManager : MonoBehaviour
{
    public static InstructionManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }

    [SerializeField] GameObject instructionHUD;
    [SerializeField] Image instructionBG;
    [SerializeField] RectTransform instructionBGRectTransform;
    [SerializeField] GameObject instructionContent;
    [SerializeField] CanvasGroup instructionContentCG;
    [SerializeField] InstructionSO instructionsSO;
    [SerializeField] GameObject[] instructionButton;
    // 0 - Left
    // 1 - Right
    // 2 - Done
    
    [Space(10)]
    [SerializeField] Image instructionImage;
    [SerializeField] TMP_Text instructionText;

    int counter;

    void Start()
    {
        Debug.Log(instructionsSO.instructions.Count);

        // Display the first instruction at the start
        DisplayInstruction();

        // Ensure buttons are set correctly
        UpdateButtonStates();

        instructionBGRectTransform.sizeDelta = new Vector2(0, instructionBGRectTransform.sizeDelta.y);
        instructionContentCG.alpha = 0;
    }
    
    void Update()
    {
        DeviceChecker();
    }
    
    void DeviceChecker()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            
        }
    }

    public void NextInstruction()
    {
        if (counter < instructionsSO.instructions.Count - 1)
        {
            counter++;
            DisplayInstruction();
            UpdateButtonStates();
        }
    }

    public void PreviousInstruction()
    {
        if (counter > 0)
        {
            counter--;
            DisplayInstruction();
            UpdateButtonStates();
        }
    }

    private void DisplayInstruction()
    {
        // Check if counter is within bounds
        if (counter >= 0 && counter < instructionsSO.instructions.Count)
        {
            instructionImage.sprite = instructionsSO.instructions[counter].instructionSprite;
            instructionText.text = instructionsSO.instructions[counter].instructionString;
        }
    }

    private void UpdateButtonStates()
    {
        // Update button visibility based on current index
        instructionButton[0].SetActive(counter > 0); // Show "Previous" button if not at the start
        instructionButton[1].SetActive(counter < instructionsSO.instructions.Count - 1); // Show "Next" button if not at the end
        instructionButton[2].SetActive(counter == instructionsSO.instructions.Count - 1); // Show "Finish" button if at the end
    }
    public void ShowInstruction()
    {
        Time.timeScale = 0;
        // Bool instructionHUDActive = true (FOR PAUSE FUNCTION)

        Debug.Log("Display Instruction!");
        instructionHUD.SetActive(true);

        instructionBGRectTransform.DOSizeDelta(new Vector2(1920, instructionBGRectTransform.sizeDelta.y), .5f)
            .SetEase(Ease.InQuad)
            .SetUpdate(true)
            .OnComplete(() =>
        {
            instructionContent.SetActive(true);
            instructionContentCG.DOFade(1, .75f).SetUpdate(true);
        });
    }

    public void HideInstruction()
    {
        instructionContentCG.DOFade(1, .75f).SetUpdate(true).OnComplete(() =>
        {
            instructionContent.SetActive(false);
            instructionBGRectTransform.DOSizeDelta(new Vector2(0, instructionBGRectTransform.sizeDelta.y), .5f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .OnComplete(() =>
            {
                instructionHUD.SetActive(false);

                // ENABLE PLAYER SCRIPTS
                PlayerScript.instance.playerMovement.enabled = true;
                PlayerScript.instance.cinemachineInputProvider.enabled = true;
                //PlayerScript.instance.interact.enabled = true;

                if (PlayerScript.instance.canRunInThisScene)
                {
                    PlayerScript.instance.stamina.enabled = true;
                }

                //Cursor.lockState = CursorLockMode.Locked;
                
                Time.timeScale = 1;
                // Bool instructionHUDActive = false (FOR PAUSE FUNCTION)
            });
        });
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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
    public InstructionSO instructionsSO;
    [SerializeField] GameObject[] instructionButton;
    // 0 - Left
    // 1 - Right
    // 2 - Done
    
    [Space(10)]
    [SerializeField] Image instructionImage;
    [SerializeField] TMP_Text instructionText;

    [Header("Flag")]
    bool isGamepad = false;
    int counter;


    // TESTING DELETE WHEN AUDIO MANAGER IS MADE
    [SerializeField] AudioSource alarmSFX;

    void Start()
    {
        Debug.Log(instructionsSO.instructions.Count);

        UpdateButtonStates();
    }
    
    void Update()
    {
        DeviceChecker();
    }
    
    void DeviceChecker()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            if(instructionBGRectTransform.gameObject.activeSelf)
            {
                Debug.Log("Keyboard");
                DisplayInstruction("keyboard");

                Cursor.lockState = CursorLockMode.None;
                EventSystem.current.SetSelectedGameObject(null);
                isGamepad = false;
            }
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            if(instructionBGRectTransform.gameObject.activeSelf)
            {
                Debug.Log("Gamepad");
                DisplayInstruction("gamepad");

                Cursor.lockState = CursorLockMode.Locked;

                if (!isGamepad)
                {
                    if (counter == instructionsSO.instructions.Count - 1)
                    {
                        EventSystem.current.SetSelectedGameObject(instructionButton[2]);
                        Debug.Log("Selected Instruction Button - Done");
                    }
                    else
                    {
                        EventSystem.current.SetSelectedGameObject(instructionButton[1]);
                        Debug.Log("Selected Instruction Button - Right");
                    }

                    isGamepad = true;
                }
            }
        }
    }

    public void NextInstruction()
    {
        if (counter < instructionsSO.instructions.Count - 1)
        {
            counter++;
            DeviceChecker();
            UpdateButtonStates();
            isGamepad = false;
        }
    }

    public void PreviousInstruction()
    {
        if (counter > 0)
        {
            counter--;
            DeviceChecker();
            UpdateButtonStates();

            if(counter == 0)
            {
                EventSystem.current.SetSelectedGameObject(instructionButton[0]);
            }

            isGamepad = false;
        }
    }

    // Modified DisplayInstruction method to accept device type
    private void DisplayInstruction(string deviceType)
    {
        // Check if counter is within bounds
        if (counter >= 0 && counter < instructionsSO.instructions.Count)
        {
            instructionImage.sprite = instructionsSO.instructions[counter].instructionSprite;

            // Display the correct instruction text based on the device type
            if (deviceType == "keyboard")
            {
                instructionText.text = instructionsSO.instructions[counter].instructionKeyboard;
            }
            else if (deviceType == "gamepad")
            {
                instructionText.text = instructionsSO.instructions[counter].instructionGampad;
            }
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
        
        if(alarmSFX != null)
        {
            alarmSFX.Pause();
        }

        // Bool instructionHUDActive = true (FOR PAUSE FUNCTION)

        Debug.Log("Display Instruction!");
        instructionHUD.SetActive(true);
        
        // FOR NEW LOOK FOR INSTRUCTION HUD

        instructionBGRectTransform.DOScale(Vector3.one, .5f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true)
            .OnComplete(() =>
        {
            instructionContent.SetActive(true);
            instructionContentCG.interactable = true;
        });

        


        // instructionBGRectTransform.DOSizeDelta(new Vector2(1920, instructionBGRectTransform.sizeDelta.y), .5f)
        //     .SetEase(Ease.InQuad)
        //     .SetUpdate(true)
        //     .OnComplete(() =>
        // {
        //     instructionContent.SetActive(true);
        //     instructionContentCG.DOFade(1, .75f).SetUpdate(true);
        // });
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

                Cursor.lockState = CursorLockMode.Locked;
                
                Pause.instance.PauseCanInput(true);
                
                HUDManager.instance.playerHUD.SetActive(true);
                
                // ENABLE PLAYER SCRIPTS
                PlayerScript.instance.playerMovement.enabled = true;
                PlayerScript.instance.cinemachineInputProvider.enabled = true;
                PlayerScript.instance.interact.enabled = true;

                // FOR SCENES WHERE PLAYER CAN RUN
                if (PlayerScript.instance.canRunInThisScene)
                {
                    PlayerScript.instance.stamina.enabled = true;
                }
                
                // PROLOGUE
                if(alarmSFX != null)
                {
                    alarmSFX.UnPause();
                }

                Time.timeScale = 1;
                this.enabled = false;
            });
        });
    }
}

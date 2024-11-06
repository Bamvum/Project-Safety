using System.Collections;
using System.Collections.Generic;
using System.Timers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance {get; private set;}

    PlayerControls playerControls;

    void Awake()
    {
        instance = this;
        playerControls = new PlayerControls();
    }
    
    [Header("Dialogue")]
    bool actionInput;
    bool option1Input;
    bool option2Input;
    bool option3Input;
    
    [Range(0.01f, 0.09f)]
    public float typingSpeed = 0.03f;
    List<DialogueProperties> dialogueList;
    int currentDialogueIndex;
    float elpasedTime;
    float maximumElapsedTime;

    [Header("Dialogue HUD")]
    [SerializeField] RectTransform dialogueBackground;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] TMP_Text dialogueName;

    [Space(10)]
    [SerializeField] GameObject[] dialogueChoices;
    [SerializeField] GameObject actionOption;
    [SerializeField] GameObject leftOption;
    [SerializeField] GameObject upOption;
    [SerializeField] GameObject rightOption;

    [Space(10)]
    [SerializeField] Image[] choicesImage;
    [SerializeField] Image actionImageHUD;

    [Space(10)]
    [SerializeField] Sprite[] keyboardSprite;

    [Space(5)]
    [SerializeField] Sprite[] gamepadSprite;

    [Space(10)]
    
    bool isInDialogue;
    bool isSpecialEvent;
    [SerializeField] bool isFloatDelay;

    [Header("Audio")]
    [SerializeField] AudioSource dialogueSFX;
    [SerializeField] AudioSource dialogueSpeechSFX;

    [Header("DoTween")]
    float punchDuration = 0.3f;
    float punchScale = 0.2f;


    void Start()
    {
        
    }

    void OnEnable()
    {
        playerControls.SpeechDialogue.Action.performed += ctx => actionInput = true;
        playerControls.SpeechDialogue.Action.canceled += ctx => actionInput = false;

        playerControls.SpeechDialogue.Option1.performed += ctx => option1Input = true;
        playerControls.SpeechDialogue.Option1.canceled += ctx => option1Input = false;

        playerControls.SpeechDialogue.Option2.performed += ctx => option2Input = true;
        playerControls.SpeechDialogue.Option2.canceled += ctx => option2Input = false;

        playerControls.SpeechDialogue.Option3.performed += ctx => option3Input = true;
        playerControls.SpeechDialogue.Option3.canceled += ctx => option3Input = false;

        playerControls.SpeechDialogue.Enable();

       
    }

    void OnDisable()
    {
        playerControls.SpeechDialogue.Disable();
    }
    
    void Update()
    {
        if(isInDialogue)
        {
            if(DeviceManager.instance.keyboardDevice)
            {
                ChangeImageStatus(keyboardSprite[0], keyboardSprite[1], keyboardSprite[2], keyboardSprite[3]);
            }
            else if (DeviceManager.instance.gamepadDevice)
            {
                ChangeImageStatus(gamepadSprite[0], gamepadSprite[1], gamepadSprite[2], gamepadSprite[3]);
            }

            if(isFloatDelay)
            {
                if (elpasedTime != maximumElapsedTime)
                {
                    elpasedTime += Time.deltaTime;
                }
                else
                {
                    isFloatDelay = false;
                }
            }
        }        
    }

    public void DialogueStart(List<DialogueProperties> textToPrint)
    {
        isInDialogue = true;

        // DISABLE OTHER SCRIPTS

        PlayerScript.instance.playerMovement.enabled = false;
        PlayerScript.instance.interact.enabled = false;
        PlayerScript.instance.stamina.enabled = false;
        PlayerScript.instance.cinemachineInputProvider.enabled = false;

        DialogueHUDShow();

        dialogueList = textToPrint;
        currentDialogueIndex = 0; 

        StartCoroutine(PrintDialogue());
    }

    bool optionSelected = false;

    IEnumerator PrintDialogue()
    {
        while (currentDialogueIndex < dialogueList.Count)
        {
            DialogueProperties line = dialogueList[currentDialogueIndex];
            isSpecialEvent =  line.isOtherEvent;
            maximumElapsedTime = line.delayNextDialogue;
            dialogueSpeechSFX.clip = line.dialogueSpeech;
            dialogueSFX.clip = line.dialogouAudio;
            dialogueSFX.Play();
            elpasedTime = 0;
            // RESET ELAPSED TIME

            dialogueName.text = line.npcName;

            line.startDialogueEvent?.Invoke();
            
            if(line.isDialogueAQuestion)
            {
                yield return StartCoroutine(TypeText(line.dialogue));

                leftOption.GetComponentInChildren<TMP_Text>().text = line.choiceAnswer1;
                rightOption.GetComponentInChildren<TMP_Text>().text = line.choiceAnswer3;


                if(line.isDialogueA3ChoicesQuestion)
                {
                    ChoiceHUDStatus(true, true);

                    upOption.GetComponentInChildren<TMP_Text>().text = line.choiceAnswer2;
                }
                else
                {
                    ChoiceHUDStatus(true, false);
                }

                yield return new WaitUntil(() => CheckForInput(line));

                optionSelected = false;
            }
            else
            {
                yield return StartCoroutine(TypeText(line.dialogue));
            }
        
        line.endDialogueEvent?.Invoke();

        }
        
        if(!isSpecialEvent)
        {
            DialogueStop();
        }
        else
        {
            StopAllCoroutines();
            DialogueHUDHide();
        }
    }    
    
    IEnumerator TypeText(string text)
    {
        dialogueText.text = string.Empty;
        actionOption.SetActive(false);
        dialogueBackground.gameObject.SetActive(true);

        // DELAY UNTIL NEXT DIALOGUE
        if(dialogueList[currentDialogueIndex].delayNextDialogue != 0)
        {
            isFloatDelay = true;
        }
        else
        {
            isFloatDelay = false;
        }


        // DIALOGUE TEXT SPEED
        foreach(char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            // yield return new WaitForSeconds(typingSpeed);
            yield return new WaitForSeconds(SettingMenu.instance.dialogueSpeedSlider.value);
        }

        // OTHER EVENT IN DIALOGUE
        if(dialogueList[currentDialogueIndex].isOtherEvent)
        {
            yield return new WaitUntil(() => !dialogueSFX.isPlaying);
            yield return new WaitUntil(() => !PlayerScript.instance.cinemachineBrain.IsBlending);
            yield return new WaitUntil(() => elpasedTime >= dialogueList[currentDialogueIndex].delayNextDialogue);
        
            actionOption.SetActive(true);
            yield return new WaitUntil(() => actionInput == true);
            dialogueBackground.gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitUntil(() => !dialogueSFX.isPlaying);
            yield return new WaitUntil(() => !PlayerScript.instance.cinemachineBrain.IsBlending);
            yield return new WaitUntil(() => elpasedTime >= dialogueList[currentDialogueIndex].delayNextDialogue);

            actionOption.SetActive(true);
            yield return new WaitUntil(() => actionInput == true);
            dialogueBackground.gameObject.SetActive(false);
        }

        // DIALOGUE END
        if(dialogueList[currentDialogueIndex].isEnd)
        {
            dialogueList[currentDialogueIndex].endDialogueEvent?.Invoke();
            DialogueStop();
        }

        if(isSpecialEvent)
        {
            dialogueList[currentDialogueIndex].endDialogueEvent?.Invoke();
        }

        currentDialogueIndex++;
    }

    bool CheckForInput(DialogueProperties line)
    {
        if (option1Input)
        {
            HandleOptionSelected(line.choice1JumpTo);
            return true;
        }
        else if (option3Input)
        {
            HandleOptionSelected(line.choice3JumpTo);
            return true;
        }
        else if (line.isDialogueA3ChoicesQuestion && option2Input)
        {
            HandleOptionSelected(line.choice2JumpTo);
            return true;
        }

        return false;
    }

    void HandleOptionSelected(int indexJump)
    {
        Debug.Log("Inside HandleOptionSelected");
        optionSelected = true;
        
        leftOption.SetActive(false);
        upOption.SetActive(false);
        rightOption.SetActive(false);

        currentDialogueIndex = indexJump;
    }

    void DialogueStop()
    {
        StopAllCoroutines();

        dialogueText.text = string.Empty;
        isInDialogue = false;

        PlayerScript.instance.playerMovement.enabled = true;
        PlayerScript.instance.interact.enabled = true;
        PlayerScript.instance.cinemachineInputProvider.enabled = true;
        
        if(PlayerScript.instance.canRunInThisScene)
        {
            PlayerScript.instance.stamina.enabled = true;
        }
        else
        {
            PlayerScript.instance.stamina.enabled = false;
        }

        // DOTWEENING
        DialogueHUDHide();
    }   

    #region - TWEENING -

    void DialogueHUDShow()
    {
        Debug.Log("Dialogue Show!");
        HUDManager.instance.dialogueHUD.SetActive(true);
        HUDManager.instance.playerHUD.SetActive(false);
    
        dialogueBackground.localScale = Vector3.zero;

        dialogueBackground.DOScale(Vector3.one, 0.1f).OnComplete(() => 
        {
            dialogueBackground.DOPunchScale(Vector3.one * punchScale, punchDuration, 10, 1).SetEase(Ease.InFlash);
        });
    }

    void DialogueHUDHide()
    {
        dialogueBackground.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => 
        {
            HUDManager.instance.dialogueHUD.SetActive(false);
            if(!isSpecialEvent)
            {
                HUDManager.instance.playerHUD.SetActive(true);
            }
        });
    }

    
    void ChoiceHUDStatus(bool activeStatus, bool is3Question)
    {
        RectTransform leftOptionRectTransform = leftOption.GetComponent<RectTransform>();
        RectTransform upOptionRectTransform = upOption.GetComponent<RectTransform>();
        RectTransform rightOptionRectTransform = rightOption.GetComponent<RectTransform>();

        // TODO - FIX CODE MAKE IT SHORTER
        leftOptionRectTransform.localScale = Vector3.zero;
        upOptionRectTransform.localScale = Vector3.zero;
        rightOptionRectTransform.localScale = Vector3.zero;

        
        if(activeStatus)
        {
            if(!is3Question)
            {
                leftOption.SetActive(true);
                rightOption.SetActive(true);
                
                leftOptionRectTransform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
                {
                    leftOptionRectTransform.DOPunchScale(Vector3.one * punchScale, punchDuration, 10, 1).OnComplete(() =>
                    {
                        rightOptionRectTransform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
                        {
                            rightOptionRectTransform.DOPunchScale(Vector3.one * punchScale, punchDuration, 10, 1);
                        });
                    });
                });
            }
            else
            {
                leftOption.SetActive(true);
                upOption.SetActive(true);
                rightOption.SetActive(true);
                
                leftOptionRectTransform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
                {
                    leftOptionRectTransform.DOPunchScale(Vector3.one * punchScale, punchDuration, 10, 1).OnComplete(() =>
                    {
                        upOptionRectTransform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
                        {
                            upOptionRectTransform.DOPunchScale(Vector3.one * punchScale, punchDuration, 10, 1).OnComplete(() =>
                            {
                                rightOptionRectTransform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
                                {
                                    rightOptionRectTransform.DOPunchScale(Vector3.one * punchScale, punchDuration, 10, 1);
                                });
                            });
                        });
                    });
                });
            }
        }
        else
        {

        }
    }
    #endregion

    void ChangeImageStatus(Sprite actionSprite, Sprite choice1Sprite, Sprite choice2Sprite, Sprite choice3Sprite)
    {
        actionImageHUD.sprite = actionSprite;
        choicesImage[1].sprite = choice1Sprite;
        choicesImage[2].sprite = choice2Sprite;
        choicesImage[3].sprite = choice3Sprite;
    }
}

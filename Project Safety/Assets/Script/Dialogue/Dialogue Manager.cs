using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue")]
    bool actionInput;
    bool option1Input;
    bool option2Input;
    bool option3Input;
    float typingSpeed = 0.03f;
    List<DialogueProperties> dialogueList;
    int currentDialogueIndex;

    [Header("Dialogue HUD")]
    [SerializeField] RectTransform dialogueBackground;
    [SerializeField] TMP_Text dialogueText;

    [Space(10)]
    [SerializeField] GameObject[] dialogueChoices;
    [SerializeField] GameObject actionOption;
    [SerializeField] GameObject leftOption;
    [SerializeField] GameObject upOption;
    [SerializeField] GameObject rightOption;

    [Space(10)]
    [SerializeField] Image[] choicesImage;
    [SerializeField] Image actionImageHUD;
    // [SerializeField] Image choice1ImageHUD;
    // [SerializeField] Image choice2ImageHUD;
    // [SerializeField] Image choice3ImageHUD;

    [Space(10)]
    [SerializeField] Sprite[] keyboardSprite;
    // [SerializeField] Sprite spaceSprite;
    // [SerializeField] Sprite oneSprite;
    // [SerializeField] Sprite twoSprite;
    // [SerializeField] Sprite threeSprite;

    [Space(5)]
    [SerializeField] Sprite[] gamepadSprite;
    // [SerializeField] Sprite XSprite;
    // [SerializeField] Sprite squareSprite;
    // [SerializeField] Sprite triangleSprite;
    // [SerializeField] Sprite circleSprite;

    [Space(10)]
    
    [SerializeField] AudioSource dialogueSFX;
    bool isInDialogue;
    bool isSpecialEvent;

    [Header("DoTween")]
    [SerializeField] float punchDuration = 0.3f;
    [SerializeField] float punchScale = 0.2f;


    void Awake()
    {
        
        PlayerManager.instance.playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        PlayerManager.instance.playerControls.SpeechDialogue.Action.performed += ctx => actionInput = true;
        PlayerManager.instance.playerControls.SpeechDialogue.Action.canceled += ctx => actionInput = false;

        PlayerManager.instance.playerControls.SpeechDialogue.Option1.performed += ctx => option1Input = true;
        PlayerManager.instance.playerControls.SpeechDialogue.Option1.canceled += ctx => option1Input = false;

        PlayerManager.instance.playerControls.SpeechDialogue.Option2.performed += ctx => option2Input = true;
        PlayerManager.instance.playerControls.SpeechDialogue.Option2.canceled += ctx => option2Input = false;

        PlayerManager.instance.playerControls.SpeechDialogue.Option3.performed += ctx => option3Input = true;
        PlayerManager.instance.playerControls.SpeechDialogue.Option3.canceled += ctx => option3Input = false;

        PlayerManager.instance.playerControls.SpeechDialogue.Enable();
    }

    void OnDisable()
    {
        PlayerManager.instance.playerControls.SpeechDialogue.Disable();
    }
    
    void Update()
    {
        if(isInDialogue)
        {
            if(DeviceManager.instance.keyboardDevice)
            {
                // ChangeImageStatus(spaceSprite, oneSprite, twoSprite, threeSprite);
                ChangeImageStatus(keyboardSprite[0], keyboardSprite[1], keyboardSprite[2], keyboardSprite[3]);
            }
            else if (DeviceManager.instance.gamepadDevice)
            {
                ChangeImageStatus(gamepadSprite[0], gamepadSprite[1], gamepadSprite[2], gamepadSprite[3]);
            }
        }
    }

    public void DialogueStart(List<DialogueProperties> textToPrint)
    {
        isInDialogue = true;

        // DISABLE OTHER SCRIPTS
        PlayerManager.instance.playerMovement.enabled = false;
        // playerMovement.playerAnim.enabled = false;
        PlayerManager.instance.interact.enabled = false;
        PlayerManager.instance.stamina.enabled = false;
        PlayerManager.instance.cinemachineInputProvider.enabled = false;

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
            isSpecialEvent =  dialogueList[currentDialogueIndex].isOtherEvent;
            dialogueSFX.clip = dialogueList[currentDialogueIndex].dialogouAudio;
            dialogueSFX.Play();
            Debug.Log(dialogueSFX.clip);

            // npcName.text = line.npcName

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
        // isTyping = true;
        actionOption.SetActive(false);
        dialogueBackground.gameObject.SetActive(true);

        foreach(char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        if(dialogueList[currentDialogueIndex].isOtherEvent)
        {
            yield return new WaitUntil(() => !dialogueSFX.isPlaying);

            actionOption.SetActive(true);
            yield return new WaitUntil(() => actionInput == true);
            dialogueBackground.gameObject.SetActive(false);
        }
        else
        {
            // IF STATEMENT ACTION INPUT IF AUDIO IS STOP PLAYING
            // if()
            yield return new WaitUntil(() => !dialogueSFX.isPlaying);
            
            actionOption.SetActive(true);
            yield return new WaitUntil(() => actionInput == true);
        }

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

        PlayerManager.instance.playerMovement.enabled = true;
        // playerMovement.playerAnim.enabled = true;
        // playerMovement.playerAnim.enabled = true;
        PlayerManager.instance.interact.enabled = true;
        PlayerManager.instance.stamina.enabled = true;
        PlayerManager.instance.cinemachineInputProvider.enabled = true;

        // DOTWEENING
        DialogueHUDHide();

        if (PlayerManager.instance.interact.dialogueTrigger != null)
        {
            PlayerManager.instance.interact.dialogueTrigger.isSpeaking = false;
        }
        else
        {
            Debug.Log("Interact.DialogueTrigger is Null");
        }
    }   

    #region - TWEENING -

    void DialogueHUDShow()
    {
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
        // TODO - FIX CODE MAKE IT SHORTER

        RectTransform leftOptionRectTransform = leftOption.GetComponent<RectTransform>();
        RectTransform upOptionRectTransform = upOption.GetComponent<RectTransform>();
        RectTransform rightOptionRectTransform = rightOption.GetComponent<RectTransform>();

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
        // choice1ImageHUD.sprite = choice1Sprite;
        // choice2ImageHUD.sprite = choice2Sprite;
        // choice3ImageHUD.sprite = choice3Sprite;

        choicesImage[0].sprite = choice1Sprite;
        choicesImage[1].sprite = choice2Sprite;
        choicesImage[2].sprite = choice3Sprite;
    }
}

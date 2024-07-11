using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    PlayerControls playerControls;
    [Header("Scripts")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Interact interact;
    [SerializeField] Stamina stamina;
    [SerializeField] CinemachineInputProvider cinemachineInputProvider;

    [Header("Dialogue")]
    bool actionInput;
    bool option1Input;
    bool option2Input;
    bool option3Input;
    float typingSpeed = 0.03f;
    List<DialogueProperties> dialogueList;
    int currentDialogueIndex;

    [Header("Dialogue HUD")]
    [SerializeField] GameObject playerHUD;
    [SerializeField] GameObject dialogueHUD;
    [SerializeField] RectTransform dialogueBackground;
    [SerializeField] TMP_Text dialogueText;
    // [SerializeField] bool isTyping;


    [Space(10)]
    [SerializeField] GameObject actionOption;
    [SerializeField] GameObject leftOption;
    [SerializeField] GameObject upOption;
    [SerializeField] GameObject rightOption;

    [Space(10)]
    [SerializeField] Image actionImageHUD;
    [SerializeField] Image choice1ImageHUD;
    [SerializeField] Image choice2ImageHUD;
    [SerializeField] Image choice3ImageHUD;

    [Space(10)]
    [SerializeField] Sprite spaceSprite;
    [SerializeField] Sprite oneSprite;
    [SerializeField] Sprite twoSprite;
    [SerializeField] Sprite threeSprite;
    [Space(5)]
    [SerializeField] Sprite XSprite;
    [SerializeField] Sprite squareSprite;
    [SerializeField] Sprite triangleSprite;
    [SerializeField] Sprite circleSprite;

    [Space(10)]
 
    bool isInDialogue;
    bool isSpecialEvent;

    [Header("DoTween")]
    [SerializeField] float punchDuration = 0.3f;
    [SerializeField] float punchScale = 0.2f;


    void Awake()
    {
        
        playerControls = new PlayerControls();
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
                ChangeImageStatus(spaceSprite, oneSprite, twoSprite, threeSprite);
            }
            else if (DeviceManager.instance.gamepadDevice)
            {
                ChangeImageStatus(XSprite, squareSprite, triangleSprite, circleSprite);
            }
        }
    }

    public void DialogueStart(List<DialogueProperties> textToPrint)
    {
        isInDialogue = true;

        // DISABLE OTHER SCRIPTS
        playerMovement.enabled = false;    
        interact.enabled = false;
        stamina.enabled = false;
        cinemachineInputProvider.enabled = false;

        playerMovement.playerAnim.SetBool("Idle", true);

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
            isSpecialEvent = dialogueList[currentDialogueIndex].otherEvent;
            
            // npcName.text = line.npcName;

            line.startDialogueEvent?.Invoke();

            if(line.isQuestion)
            {
                yield return StartCoroutine(TypeText(line.dialogue));

                leftOption.GetComponentInChildren<TMP_Text>().text = line.answerOption1;
                rightOption.GetComponentInChildren<TMP_Text>().text = line.answerOption3;


                if(line.is3Question)
                {
                    ChoiceHUDStatus(true, true);

                    upOption.GetComponentInChildren<TMP_Text>().text = line.answerOption2;
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
        
        // DialogueStop();

        if(!isSpecialEvent)
        {
            DialogueStop();
        }
        else
        {
            dialogueHUD.SetActive(false);
            StopAllCoroutines();
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

        if(dialogueList[currentDialogueIndex].isQuestion)
        {
            actionOption.SetActive(true);
            
            // TODO - MAKE DIALOGUE BACKGROUND TWEENING WHEN DISAPPEARING AND APPEARING DURING QUESTIONS
            
            yield return new WaitUntil(() => actionInput == true);
            dialogueBackground.gameObject.SetActive(false);
        }
        else
        {
            actionOption.SetActive(true);
            yield return new WaitUntil(() => actionInput == true);
        }

        if(dialogueList[currentDialogueIndex].isEnd)
        {
            dialogueList[currentDialogueIndex].endDialogueEvent?.Invoke();
            DialogueStop();
        }

        if(dialogueList[currentDialogueIndex].otherEvent)
        {
            dialogueList[currentDialogueIndex].endDialogueEvent?.Invoke();
        }

        currentDialogueIndex++;
    }

    bool CheckForInput(DialogueProperties line)
    {
        if (option1Input)
        {
            HandleOptionSelected(line.option1IndexJump);
            return true;
        }
        else if (option3Input)
        {
            HandleOptionSelected(line.option3IndexJump);
            return true;
        }
        else if (line.is3Question && option2Input)
        {
            HandleOptionSelected(line.option2IndexJump);
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

        // ChoiceHUDHide(false);

        currentDialogueIndex = indexJump;
    }

    void DialogueStop()
    {
        StopAllCoroutines();

        dialogueText.text = string.Empty;
        isInDialogue = false;

        playerMovement.enabled = true;
        interact.enabled = true;
        stamina.enabled = true;
        cinemachineInputProvider.enabled = true;

        playerMovement.playerAnim.SetBool("Idle", false);

        // DOTWEENING
        DialogueHUDHide();

        if (interact.dialogueTrigger != null)
        {
            interact.dialogueTrigger.isSpeaking = false;
        }
        else
        {
            Debug.Log("Interact.DialogueTrigger is Null");
        }
    }   

    #region - TWEENING -

    void DialogueHUDShow()
    {
        dialogueHUD.SetActive(true);
        playerHUD.SetActive(false);
    
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
            dialogueHUD.SetActive(false);
            playerHUD.SetActive(true);
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
        choice1ImageHUD.sprite = choice1Sprite;
        choice2ImageHUD.sprite = choice2Sprite;
        choice3ImageHUD.sprite = choice3Sprite;
    }
}

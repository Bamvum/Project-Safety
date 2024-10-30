using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class PostAssessmentSceneManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] CanvasGroup sceneNameText;

    [Header("TPASS")]
    [SerializeField] Button[] tpassButtons;
    [SerializeField] RectTransform[] tpassButtonsRectTransform;

    [Header("Swapper")]
    [SerializeField] Button swapperButton1;
    [SerializeField] Vector2 swapperAnchorPosition1;
    
    [Space(5)]
    [SerializeField] Button swapperButton2;
    [SerializeField] Vector2 swapperAnchorPosition2;

    [Space(5)]
    [SerializeField] bool stopSwapperInput;

    [Header("Audio")]
    [SerializeField] AudioSource sceneBGM;
    [SerializeField] AudioSource correctSFX;
    [SerializeField] AudioSource wrongSFX;

    [Header("HUD")]
    [SerializeField] RectTransform tpassExtinguisherTestRectTransform;
    [SerializeField] CanvasGroup tpassExtinguisherTestCG;

    [Space(10)]
    [SerializeField] RectTransform classFireTestRectTransform;
    [SerializeField] CanvasGroup classFireTestCG;
    
    [Space(5)]
    [SerializeField] List<ClassFireQuestionandAnswer> classFireQnA;
    [SerializeField] GameObject[] classFireChoices;
    [SerializeField] int classFireCurrentQuestion;
    [SerializeField] Image questionImage;

    [Header("Set Selected Game Object")]
    [SerializeField] GameObject lastSelectedButton; // FOR GAMEPAD

    [Space(5)]
    [SerializeField] GameObject tpassExtinguisherTestSelectedButton;
    [SerializeField] GameObject classFireTestSelectedButton;

    [Header("Flags")]
    [SerializeField] int tpassScore;
    [SerializeField] int classFireScore;

    bool isGamepad;

    void Start()
    {
        PlayerPrefs.SetInt("Post-Assessment", 1);

        Cursor.lockState = CursorLockMode.Locked;

        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                 LoadingSceneManager.instance.fadeImage.color.g,
                                                 LoadingSceneManager.instance.fadeImage.color.b,
                                                 1);

        StartCoroutine(FadeOutEffect());

        RandomTPASSPosition();
    }

    IEnumerator FadeOutEffect()
    {
        yield return new WaitForSeconds(1);
        // sceneNameText.DOFade(1, 1).OnComplete(() =>
        // {
        //     sceneNameText.DOFade(1,1).OnComplete(() =>
        //     {
        //         sceneNameText.DOFade(0, 1);
        //     });
        // });

        sceneNameText.DOFade(1, 1);
        
        yield return new WaitForSeconds(1);

        sceneNameText.DOFade(0, 1);

        yield return new WaitForSeconds(5);

        LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
                // HomeworkManager.instance.enabled = true;
                ShowTPASSExtinguisherText();
                sceneBGM.Play();
                sceneBGM.DOFade(1, 1);
            });
    }

    void Update()
    {
        DeviceInputChecker();
    }

    void DeviceInputChecker()
    {
        DeviceInputCheckerUI();

        // GAMEPAD VIBRATION ON NAVIGATION 
        if (Gamepad.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            if (Gamepad.current.leftStick.ReadValue() != Vector2.zero || Gamepad.current.dpad.ReadValue() != Vector2.zero)
            {
                GameObject currentSelectedButton = EventSystem.current.currentSelectedGameObject;

                // Check if the selected UI element has changed (button navigation)
                if (currentSelectedButton != lastSelectedButton)
                {
                    // Trigger vibration when navigating to a new button
                    VibrateGamepad();
                    lastSelectedButton = currentSelectedButton; // Update the last selected button
                }
            }
        }
    }

    private void VibrateGamepad()
    {
        // Set a short vibration
        Gamepad.current.SetMotorSpeeds(0.3f, 0.3f);
        Invoke("StopVibration", 0.1f);
    }


    private void StopVibration()
    {
        Gamepad.current.SetMotorSpeeds(0, 0);
    }

    #region - DEVICE INPUT CHECKER -

    void DeviceInputCheckerUI()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            if(tpassExtinguisherTestRectTransform.gameObject.activeSelf || classFireTestRectTransform.gameObject.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                EventSystem.current.SetSelectedGameObject(null);
                isGamepad = false;
            }
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            Cursor.lockState = CursorLockMode.Locked;

            if(!isGamepad)
            {
                
                if(tpassExtinguisherTestRectTransform.gameObject.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(tpassExtinguisherTestSelectedButton);
                }

                if(classFireTestRectTransform.gameObject.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(classFireTestSelectedButton);
                }

                isGamepad = true;
            }
        }

    }

    #endregion

    // RANDOMIZE BETWEEN TPASS AND HOMEWORK TO GO FIRST

    #region - ARRANGE FIRE EXINTGUISHER - 
    void ShowTPASSExtinguisherText()
    {
        // INITIALIZE
        tpassExtinguisherTestRectTransform.localScale = Vector3.zero;
        tpassExtinguisherTestCG.interactable = false;

        tpassExtinguisherTestRectTransform.gameObject.SetActive(true);
        tpassExtinguisherTestRectTransform.DOScale(Vector3.one, .75f).OnComplete(() =>
        {
            tpassExtinguisherTestRectTransform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1)
                .SetEase(Ease.InFlash)
                .OnComplete(() =>
                {
                    tpassExtinguisherTestCG.interactable = true;
                });

        });
    }

    void HideTPASSExtinguisherText()
    {
        // Disable interactability before starting the hide animation
        tpassExtinguisherTestCG.interactable = false;
        tpassExtinguisherTestRectTransform.DOScale(Vector3.zero, .75f).OnComplete(() =>
        {
            tpassExtinguisherTestRectTransform.gameObject.SetActive(false);
            // HomeworkManager.instance.enabled = true;
            // HomeworkManager.instance.homeworkHUD.SetActive(true);
            ShowClassFireTest();
            GenerateFireClassQuestions();
            // DISPLAY HOMEWORK
        });

        // // Punch scale effect before hiding
        // tpassExtinguisherTestRectTransform.DOPunchScale(Vector3.one * -0.2f, 0.3f, 10, 1)
        //     .SetEase(Ease.OutFlash)
        //     .OnComplete(() =>
        //     {
        //         // Scale down to zero
        //         tpassExtinguisherTestRectTransform.DOScale(Vector3.zero, 1f).OnComplete(() =>
        //         {
                   
        //         });
        //     });
    }

    public void SubmitTPASSAnswer()
    {
        if (tpassButtonsRectTransform[0].anchoredPosition == new Vector2(-575.5f, 0))
        {
            tpassScore++;
        }
        if (tpassButtonsRectTransform[1].anchoredPosition == new Vector2(-250.5f, 0))
        {
            tpassScore++;
        }
        if (tpassButtonsRectTransform[2].anchoredPosition == new Vector2(74.5f, 0))
        {
            tpassScore++;
        }
        if (tpassButtonsRectTransform[3].anchoredPosition == new Vector2(399.5f, 0))
        {
            tpassScore++;
        }
        if (tpassButtonsRectTransform[4].anchoredPosition == new Vector2(724.5f, 0))
        {
            tpassScore++;
        }

        if (tpassScore == 5)
        {
            Debug.Log("Correct!");
            correctSFX.Play();
        }
        else
        {
            Debug.Log("Wrong!");
            wrongSFX.Play();
        }

        HideTPASSExtinguisherText();
    }
    
    public void RandomTPASSPosition()
    {
        // Define the possible positions
        Vector2[] positions = new Vector2[]
        {
            new Vector2(-575.5f, 0),
            new Vector2(-250.5f, 0),
            new Vector2(74.5f, 0),
            new Vector2(399.5f, 0),
            new Vector2(724.5f, 0)
        };

        // Shuffle the positions
        List<Vector2> shuffledPositions = new List<Vector2>(positions);
        Shuffle(shuffledPositions);

        // Assign each button a unique position
        for (int i = 0; i < tpassButtons.Length && i < shuffledPositions.Count; i++)
        {
            tpassButtons[i].transform.localPosition = shuffledPositions[i];
        }
    }
    
    private void Shuffle<T>(List<T> list)
    {
        System.Random rand = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = rand.Next(n--);
            T temp = list[n];
            list[n] = list[k];
            list[k] = temp;
        }
    }

    public void GetRectTransform(Button buttonObject)
    {
        if (!stopSwapperInput)
        {
            // Check if the first button has been assigned
            if (swapperButton1 == null)
            {
                swapperButton1 = buttonObject;

                // Get the RectTransform of this button
                RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();

                // Print the anchored position
                Debug.Log("Button RectTransform: " + rectTransform.anchoredPosition);

                // Store the anchored position
                swapperAnchorPosition1 = rectTransform.anchoredPosition;
            }
            else if (swapperButton2 == null)
            {
                // Assign the second button
                swapperButton2 = buttonObject;

                // Get the RectTransform of this button
                RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();

                // Print the anchored position
                Debug.Log("Button RectTransform: " + rectTransform.anchoredPosition);

                // Store the anchored position
                swapperAnchorPosition2 = rectTransform.anchoredPosition;

                // Swap the positions of the two buttons
                SwapButtons();
            }
            else
            {
                // Reset if both buttons are already assigned (optional)
                Debug.Log("Both buttons have been selected. Resetting...");
                ResetButtons();
            }
        }
    }

    private void SwapButtons()
    {
        // Swap the anchored positions
        if (swapperButton1 != null && swapperButton2 != null)
        {
            RectTransform rectTransform1 = swapperButton1.GetComponent<RectTransform>();
            RectTransform rectTransform2 = swapperButton2.GetComponent<RectTransform>();

            // Swap their anchored positions
            Vector2 tempPosition = rectTransform1.anchoredPosition;
            rectTransform1.anchoredPosition = swapperAnchorPosition2;
            rectTransform2.anchoredPosition = swapperAnchorPosition1;

            // Reset the buttons for the next swap (optional)
            ResetButtons();
        }
    }

    private void ResetButtons()
    {
        // Reset the buttons and positions for future swaps
        swapperButton1 = null;
        swapperButton2 = null;
        swapperAnchorPosition1 = Vector2.zero;
        swapperAnchorPosition2 = Vector2.zero;
    }

    #endregion

    #region - CLASSIFICATION OF FIRE -

    void ShowClassFireTest()
    {
        //GenerateFireClassQuestions();

        classFireTestRectTransform.gameObject.SetActive(true);
        classFireTestRectTransform.DOScale(Vector3.one, .75f).OnComplete(() =>
        {
            classFireTestRectTransform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1)
                .SetEase(Ease.InFlash)
                .OnComplete(() =>
                {
                    classFireTestCG.interactable = true;
                    
                });

        });
    }

    void HideClassFireTest() 
    {
        // Disable interactability before starting the hide animation
        classFireTestCG.interactable = false;
        classFireTestRectTransform.DOScale(Vector3.zero, .75f).OnComplete(() =>
        {
            classFireTestRectTransform.gameObject.SetActive(false);
            HomeworkManager.instance.enabled = true;
            HomeworkManager.instance.homeworkHUD.SetActive(true);
            // ShowClassFireTest();
            // GenerateFireClassQuestions();
            // DISPLAY HOMEWORK
        });
    }

    void SetAnswer()
    {
        for (int i = 0; i < classFireChoices.Length; i++)
        {
            classFireChoices[i].GetComponent<ClassFireAnswerKey>().isCorrect = false;
            // homeworkChoices[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].answer[i];
            
            if (classFireQnA[classFireCurrentQuestion].correctAnswer == i+1)
            {
                classFireChoices[i].GetComponent<ClassFireAnswerKey>().isCorrect = true;
            }
        }
    }

    void GenerateFireClassQuestions()
    {
        if(classFireQnA.Count > 0)
        {
            classFireCurrentQuestion = Random.Range(0, classFireQnA.Count);
            questionImage.sprite = classFireQnA[classFireCurrentQuestion].questionImage;

            SetAnswer();
        }
        else
        {
            Debug.Log("Out of Questions");
        
            HideClassFireTest();
        }
    }

    public void classFireCorrect()
    {
        classFireQnA.RemoveAt(classFireCurrentQuestion);
        classFireScore++;
        correctSFX.Play();
        GenerateFireClassQuestions();
    }

    public void classFireWrong()
    {
        classFireQnA.RemoveAt(classFireCurrentQuestion);
        wrongSFX.Play();
        GenerateFireClassQuestions();
    }

    #endregion
}

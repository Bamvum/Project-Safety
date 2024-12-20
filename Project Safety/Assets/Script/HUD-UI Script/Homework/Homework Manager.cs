using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;



public class HomeworkManager : MonoBehaviour
{
    public static HomeworkManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }

    [SerializeField] DialogueTrigger dialogueTrigger;

    [SerializeField] List<QuestionandAnswer> QnA;

    [SerializeField] GameObject choiceSelected;
    [SerializeField] int currentQuestion;

    [Header("Homework HUD")]
    public GameObject homeworkHUD;
    [SerializeField] GameObject homeworkQnA;
    [SerializeField] GameObject homeworkScore;
    [SerializeField] RectTransform homeworkScoreRectTransform;
    [SerializeField] TMP_Text homeworkScoreText;
    [SerializeField] TMP_Text questionText;
    [SerializeField] GameObject[] homeworkChoices;

    [Header("Set Selected Game Object")]
    [SerializeField] GameObject lastSelectedButton; // FOR GAMEPAD

    [Header("Flag")]
    int score = 0;
    int totalOfQuestions;
    bool isGamepad;

    void Start()
    {
        totalOfQuestions = QnA.Count;
        GenerateQuestions();
    }
    
    void Update()
    {
        Debug.Log(QnA.Count);

        if(DeviceManager.instance.keyboardDevice)
        {
            // Cursor.lockState = CursorLockMode.None;
            EventSystem.current.SetSelectedGameObject(null);
            isGamepad = false;
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            // Cursor.lockState = CursorLockMode.Locked;

            // Make this if Statement occur only once
            if(!isGamepad)
            {
                EventSystem.current.SetSelectedGameObject(choiceSelected);
                isGamepad = true;
            }
        }

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
        Gamepad.current.SetMotorSpeeds(0.3f, 0.3f); // Adjust the intensity here
        Invoke("StopVibration", 0.1f); // Stops vibration after 0.1 seconds
    }


    private void StopVibration()
    {
        Gamepad.current.SetMotorSpeeds(0, 0);
    }

    void OnDisable()
    {
        // Cursor.lockState = CursorLockMode.Locked;
    }

    
    void SetAnswer()
    {
        for(int i = 0; i < homeworkChoices.Length; i++)
        {
            homeworkChoices[i].GetComponent<AnswerKey>().isCorrect = false;
            homeworkChoices[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].answer[i];

            if(QnA[currentQuestion].correctAnswer == i+1)
            {
                homeworkChoices[i].GetComponent<AnswerKey>().isCorrect = true;
            }
        }
    }

    void GenerateQuestions()
    {
        if(QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);
            questionText.text = QnA[currentQuestion].question;
            
            SetAnswer();
        }
        else 
        {
            Debug.Log("Out of Questions");
            // TODO - METHOD TO END HOMEWORK SCRIPT/HUD
            //      - ENABLE PLAYER SCRIPTS

            PreEndOfHomework();
        }

    }

    public void Correct()
    {
        QnA.RemoveAt(currentQuestion);
        score++;
        GenerateQuestions();
    }

    public void Wrong()
    {
        QnA.RemoveAt(currentQuestion);
        GenerateQuestions();
    }

    void PreEndOfHomework()
    {
        homeworkScore.SetActive(true);
        homeworkScoreRectTransform.DOScale(Vector3.one, .1f).OnComplete(() =>
        {
            homeworkScoreRectTransform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1).SetEase(Ease.InFlash);
        });
        homeworkQnA.SetActive(false);
        homeworkScoreText.text = score + " / "  + totalOfQuestions;
        

        Invoke("EndOfHomework", 3);
    }

    void EndOfHomework()
    {
        if(SceneManager.GetActiveScene().name == "Prologue")
        {
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

            LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                 .OnComplete(() =>
            {
                homeworkHUD.SetActive(false);

                PrologueSceneManager.instance.monitor.layer = 0;

                LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
                     .OnComplete(() =>
                 {
                     LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
                     this.enabled = false;
                     dialogueTrigger.StartDialogue();
                 });
            });
        }
        else
        {
           EndOfScene();
        }
    }

    void EndOfScene()
    {
         LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

            LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                 .OnComplete(() =>
            {
                homeworkHUD.SetActive(false);

                PrologueSceneManager.instance.monitor.layer = 0;

                LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
                     .OnComplete(() =>
                 {
                     LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
                     
                     this.enabled = false;
                 });
            });

        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            LoadingSceneManager.instance.loadingScreen.SetActive(true);
            LoadingSceneManager.instance.enabled = true;
            // LoadingSceneManager.instance.sceneName = "C";
            // DISPLAY CREDITS
        });
    }
}


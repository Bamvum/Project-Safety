using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Unity.VisualScripting;


public class HomeworkManager : MonoBehaviour
{
    [Header("Script")]
    // [SerializeField] PrologueSceneManager prologueSceneManager;
    // [SerializeField] TransitionManager transitionManager;
    // [SerializeField] PlayerMovement playerMovement;
    // [SerializeField] Interact interact;
    [SerializeField] DialogueTrigger dialogueTrigger;

    [Header("Homework HUDs")]
    // [SerializeField] GameObject homeworkHUD;
    [SerializeField] GameObject homeworkQnAHUD;
    [SerializeField] GameObject homeworkScoreHUD;
    [SerializeField] TMP_Text homeworkScoreText;
    [SerializeField] List<QuestionandAnswer> QnA;
    
    [SerializeField] GameObject[] choices;
    [SerializeField] GameObject choiceSelected;
    [SerializeField] int currentQuestion;

    [SerializeField] TMP_Text questionText;

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
            Cursor.lockState = CursorLockMode.None;
            EventSystem.current.SetSelectedGameObject(null);
            isGamepad = false;
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            Cursor.lockState = CursorLockMode.Locked;

            // Make this if Statement occur only once
            if(!isGamepad)
            {
                EventSystem.current.SetSelectedGameObject(choiceSelected);
                isGamepad = true;
            }
        }
    }
    
    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void SetAnswer()
    {
        for(int i = 0; i < choices.Length; i++)
        {
            choices[i].GetComponent<AnswerKey>().isCorrect = false;
            choices[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].answer[i];

            if(QnA[currentQuestion].correctAnswer == i+1)
            {
                choices[i].GetComponent<AnswerKey>().isCorrect = true;
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
        homeworkScoreHUD.SetActive(true);
        homeworkQnAHUD.SetActive(false);
        homeworkScoreText.text = score + " / "  + totalOfQuestions;
        

        Invoke("EndOfHomework", 3);
    }

    void EndOfHomework()
    {
        // ScriptManager.instance.transitionManager.transitionImage.DOFade(1, 2).OnComplete(() =>
        // {
        //     HUDManager.instance.homeworkHUD.SetActive(false);
        //     ScriptManager.instance.transitionManager.transitionImage.DOFade(0,2).OnComplete(() =>
        //     {
        //         // Debug.Log("Display Dialogou!");
        //         this.enabled = false;
        //         dialogueTrigger.StartDialogue();
        //     });
        // });

       
    }
}


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Unity.VisualScripting;


public class HomeworkManager : MonoBehaviour
{
    [SerializeField] DialogueTrigger dialogueTrigger;

    [SerializeField] List<QuestionandAnswer> QnA;

    [SerializeField] GameObject choiceSelected;
    [SerializeField] int currentQuestion;

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
    }
    
    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void SetAnswer()
    {
        for(int i = 0; i < HUDManager.instance.homeworkChoices.Length; i++)
        {
            HUDManager.instance.homeworkChoices[i].GetComponent<AnswerKey>().isCorrect = false;
            HUDManager.instance.homeworkChoices[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].answer[i];

            if(QnA[currentQuestion].correctAnswer == i+1)
            {
                HUDManager.instance.homeworkChoices[i].GetComponent<AnswerKey>().isCorrect = true;
            }
        }
    }

    void GenerateQuestions()
    {
        if(QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);
            HUDManager.instance.questionText.text = QnA[currentQuestion].question;
            
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
        HUDManager.instance.homeworkScore.SetActive(true);
        HUDManager.instance.homeworkQnA.SetActive(false);
        HUDManager.instance.homeworkScoreText.text = score + " / "  + totalOfQuestions;
        

        Invoke("EndOfHomework", 3);
    }

    void EndOfHomework()
    {
       LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

       LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .OnComplete(() =>
       {
           HUDManager.instance.homeworkHUD.SetActive(false);

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
}


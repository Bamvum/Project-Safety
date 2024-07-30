using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HomeworkManager : MonoBehaviour
{
    [SerializeField] List<QuestionandAnswer> QnA;
    [SerializeField] GameObject[] choices;
    [SerializeField] int currentQuestion;

    [SerializeField] TMP_Text questionText;

    void Start()
    {
        GenerateQuestions();
    }
    
    // void Update()
    // {
    //     Debug.Log("Choi");
    // }
    
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
        }

    }

    public void Correct()
    {
        QnA.RemoveAt(currentQuestion);
        GenerateQuestions();
    }

    public void Wrong()
    {
        QnA.RemoveAt(currentQuestion);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerKey : MonoBehaviour
{
    [SerializeField] HomeworkManager homeworkManager;
    public  bool isCorrect;

    public void Answer()
    {
        if(isCorrect)
        {
            Debug.Log("Answer is Correct!");
            homeworkManager.Correct();
        }
        else
        {        
            homeworkManager.Correct();
            Debug.Log("Answer is Wrong!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassFireAnswerKey : MonoBehaviour
{
    [SerializeField] PostAssessmentSceneManager postAssessmentSceneManager;
    public  bool isCorrect;

    public void Answer()
    {
        if(isCorrect)
        {
            Debug.Log("Answer is Correct!");
            postAssessmentSceneManager.classFireCorrect();
        }
        else
        {        
            postAssessmentSceneManager.classFireWrong();
            Debug.Log("Answer is Wrong!");
        }
    }
}

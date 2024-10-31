using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisherAnswerKey : MonoBehaviour
{
    [SerializeField] PostAssessmentSceneManager postAssessmentSceneManager;

    public bool isCorrect;

    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Answer is Correct!");
            postAssessmentSceneManager.FireExtinguisherCorrect();
        }
        else
        {
            postAssessmentSceneManager.FireExtinguisherWrong();
            Debug.Log("Answer is Wrong!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }

    [Header("Buttons")]

    [SerializeField] Button nightmareButton;
    [SerializeField] Button houseButton;
    [SerializeField] Button neighborhoodButton;
    [SerializeField] Button fireStationButton;
    [SerializeField] Button trainingGroundsButton;
    [SerializeField] Button schoolStartButton;
    [SerializeField] Button schoolEscapeButton;
    [SerializeField] Button postAssessmentButton;



    void Start()
    {
        
        if(PlayerPrefs.GetInt("House Scene", 0) == 1)
        {
            TMP_Text houseText = houseButton.GetComponentInChildren<TMP_Text>();
            houseText.text = "HOUSE";
            houseButton.interactable = true;
        }
        else
        {
            houseButton.interactable = false;
        }

        if(PlayerPrefs.GetInt("Neighborhood Scene", 0) == 1)
        {
            TMP_Text neighborHoodText = neighborhoodButton.GetComponentInChildren<TMP_Text>();
            neighborHoodText.text = "NEIGHBORHOOD";

            neighborhoodButton.interactable = true;
        }
        else
        {
            neighborhoodButton.interactable = false;
        }

        if (PlayerPrefs.GetInt("Fire Station Scene", 0) == 1)
        {
            TMP_Text fireStationText = fireStationButton.GetComponentInChildren<TMP_Text>();
            fireStationText.text = "FIRE STATION";
            fireStationButton.interactable = true;
        }
        else
        {
            fireStationButton.interactable = false;
        }

        if (PlayerPrefs.GetInt("Training Grounds Scene", 0) == 1)
        {
            TMP_Text trainingGroundsText = trainingGroundsButton.GetComponentInChildren<TMP_Text>();
            trainingGroundsText.text = "TRAINING GROUNDS";

            trainingGroundsButton.interactable = true;
        }
        else
        {
            trainingGroundsButton.interactable = false;
        }

        if (PlayerPrefs.GetInt("School: Start", 0) == 1)
        {
            TMP_Text schoolStartText = schoolStartButton.GetComponentInChildren<TMP_Text>();
            schoolStartText.text = "SCHOOL START";

            schoolStartButton.interactable = true;
        }
        else
        {
            schoolStartButton.interactable = false;
        }

        if (PlayerPrefs.GetInt("School: Escape", 0) == 1)
        {
            TMP_Text schoolEscapeText = schoolEscapeButton.GetComponentInChildren<TMP_Text>();
            schoolEscapeText.text = "SCHOOL ESCAPE";

            schoolEscapeButton.interactable = true;
        }
        else
        {
            schoolEscapeButton.interactable = false;
        }

        if(PlayerPrefs.GetInt("Post-Assessment", 0) == 1)
        {
            TMP_Text postAssessmentText = postAssessmentButton.GetComponentInChildren<TMP_Text>();
            postAssessmentText.text = "POST-ASSESSMENT";

            postAssessmentButton.interactable = true;
        }
        else
        {
            postAssessmentButton.interactable = false;
        }

    }


}

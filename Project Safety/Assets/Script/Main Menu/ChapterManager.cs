using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager instance { get; private set; }

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

        FirebaseManager.Instance.GetChapterUnlockStatusFromFirebase("House Scene", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                // Update PlayerPrefs if Firebase shows it's unlocked
                PlayerPrefs.SetInt("House Scene", 1);
            }

            // Check local PlayerPrefs status
            if (PlayerPrefs.GetInt("House Scene", 0) == 1)
            {
                TMP_Text houseText = houseButton.GetComponentInChildren<TMP_Text>();
                houseText.text = "HOUSE";
                houseButton.interactable = true;

                // Save to Firebase if unlocked locally
                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveChapterUnlockToFirebase("House Scene", true);
                }
            }
            else
            {
                houseButton.interactable = false;
            }
        });

        FirebaseManager.Instance.GetChapterUnlockStatusFromFirebase("Neighborhood Scene", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("Neighborhood Scene", 1);
            }

            if (PlayerPrefs.GetInt("Neighborhood Scene", 0) == 1)
            {
                TMP_Text neighborHoodText = neighborhoodButton.GetComponentInChildren<TMP_Text>();
                neighborHoodText.text = "NEIGHBORHOOD";
                neighborhoodButton.interactable = true;

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveChapterUnlockToFirebase("Neighborhood Scene", true);
                }
            }
            else
            {
                neighborhoodButton.interactable = false;
            }
        });

        FirebaseManager.Instance.GetChapterUnlockStatusFromFirebase("Fire Station Scene", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("Fire Station Scene", 1);
            }

            if (PlayerPrefs.GetInt("Fire Station Scene", 0) == 1)
            {
                TMP_Text fireStationText = fireStationButton.GetComponentInChildren<TMP_Text>();
                fireStationText.text = "FIRE STATION";
                fireStationButton.interactable = true;

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveChapterUnlockToFirebase("Fire Station Scene", true);
                }
            }
            else
            {
                fireStationButton.interactable = false;
            }
        });

        FirebaseManager.Instance.GetChapterUnlockStatusFromFirebase("Training Grounds Scene", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("Training Grounds Scene", 1);
            }

            if (PlayerPrefs.GetInt("Training Grounds Scene", 0) == 1)
            {
                TMP_Text trainingGroundsText = trainingGroundsButton.GetComponentInChildren<TMP_Text>();
                trainingGroundsText.text = "TRAINING GROUNDS";
                trainingGroundsButton.interactable = true;

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveChapterUnlockToFirebase("Training Grounds Scene", true);
                }
            }
            else
            {
                trainingGroundsButton.interactable = false;
            }
        });

        FirebaseManager.Instance.GetChapterUnlockStatusFromFirebase("School: Start", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("School: Start", 1);
            }

            if (PlayerPrefs.GetInt("School: Start", 0) == 1)
            {
                TMP_Text schoolStartText = schoolStartButton.GetComponentInChildren<TMP_Text>();
                schoolStartText.text = "SCHOOL START";
                schoolStartButton.interactable = true;

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveChapterUnlockToFirebase("School: Start", true);
                }
            }
            else
            {
                schoolStartButton.interactable = false;
            }
        });

        FirebaseManager.Instance.GetChapterUnlockStatusFromFirebase("School: Escape", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("School: Escape", 1);
            }
            if (PlayerPrefs.GetInt("School: Escape", 0) == 1)
            {
                TMP_Text schoolEscapeText = schoolEscapeButton.GetComponentInChildren<TMP_Text>();
                schoolEscapeText.text = "SCHOOL ESCAPE";
                schoolEscapeButton.interactable = true;

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveChapterUnlockToFirebase("School: Escape", true);
                }
            }
            else
            {
                schoolEscapeButton.interactable = false;
            }
        });

        FirebaseManager.Instance.GetChapterUnlockStatusFromFirebase("Post-Assessment", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("Post-Assessment", 1);
            }
            if (PlayerPrefs.GetInt("Post-Assessment", 0) == 1)
            {
                TMP_Text postAssessmentText = postAssessmentButton.GetComponentInChildren<TMP_Text>();
                postAssessmentText.text = "POST-ASSESSMENT";
                postAssessmentButton.interactable = true;

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveChapterUnlockToFirebase("Post-Assessment", true);
                }
            }
            else
            {
                postAssessmentButton.interactable = false;
            }
        });

    }


}

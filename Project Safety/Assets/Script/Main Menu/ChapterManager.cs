using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;

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
        // Check if user is authenticated
        if (FirebaseManager.auth.CurrentUser != null)
        {
            // User is online, fetch data from Firebase
            FetchChapterUnlocksFromFirebase();
        }
        else
        {
            // No user is authenticated, fallback to PlayerPrefs
            SetupChaptersFromPlayerPrefs();
        }
    }

    private void FetchChapterUnlocksFromFirebase()
    {
        string userId = FirebaseManager.auth.CurrentUser.UserId;

        // Fetch each chapter's unlock status from Firebase
        FirebaseManager.databaseReference
            .Child("users")
            .Child(userId)
            .Child("chapters")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && !task.IsFaulted && task.Result.Exists)
                {
                    var chaptersData = task.Result;

                    // Check and set button states based on Firebase data
                    SetButtonState(chaptersData, "House Scene", houseButton);
                    SetButtonState(chaptersData, "Neighborhood Scene", neighborhoodButton);
                    SetButtonState(chaptersData, "Fire Station Scene", fireStationButton);
                    SetButtonState(chaptersData, "Training Grounds Scene", trainingGroundsButton);
                    SetButtonState(chaptersData, "School: Start", schoolStartButton);
                    SetButtonState(chaptersData, "School: Escape", schoolEscapeButton);
                    SetButtonState(chaptersData, "Post-Assessment", postAssessmentButton);
                }
                else
                {
                    Debug.LogError("Failed to fetch chapter unlocks from Firebase.");
                    SetupChaptersFromPlayerPrefs(); // Fall back to PlayerPrefs if fetch fails
                }
            });
    }

    private void SetButtonState(DataSnapshot chaptersData, string chapterKey, Button button)
    {
        if (chaptersData.Child(chapterKey).Value != null && (int)chaptersData.Child(chapterKey).Value == 1)
        {
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            buttonText.text = chapterKey.ToUpper();
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    private void SetupChaptersFromPlayerPrefs()
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

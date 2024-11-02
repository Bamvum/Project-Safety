using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }
    public static FirebaseAuth auth;
    private FirebaseApp app;
    public static FirebaseDatabase database;
    public static DatabaseReference databaseReference;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist across scenes
        }
        else
        {
            Destroy(gameObject);  // Ensure only one instance of FirebaseManager exists
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            if (task.IsCompleted)
            {
                app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                database = FirebaseDatabase.DefaultInstance;
                FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(true);
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase initialized.");
            }
            else
            {
                Debug.LogError("Could not initialize Firebase: " + task.Exception);
            }
        });
    }

    public static void DeleteGuestData()
    {
        if (auth != null && auth.CurrentUser != null && auth.CurrentUser.IsAnonymous && databaseReference != null)
        {
            // Delete guest user's data from the database
            databaseReference
                .Child("users")
                .Child(auth.CurrentUser.UserId)
                .RemoveValueAsync()
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        Debug.Log("Guest user data deleted successfully.");
                    }
                    else
                    {
                        Debug.LogError("Failed to delete guest data: " + task.Exception);
                    }
                });
        }
        else
        {
            Debug.LogWarning("No guest data to delete or Firebase references are null.");
        }
    }


    public static void DeleteGuestAccount()
    {
        if (auth.CurrentUser != null && auth.CurrentUser.IsAnonymous && databaseReference != null)
        {
            auth.CurrentUser.DeleteAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    Debug.Log("Guest user account deleted successfully.");
                }
                else
                {
                    Debug.LogError("Failed to delete guest account: " + task.Exception);
                }
            });
        }
        else
        {
            Debug.LogWarning("No guest data to delete or Firebase references are null.");
        }
    }

    public static void SignOut()
    {
        auth.SignOut();
        Debug.Log("User has been signed out.");
    }

    // This method is called when the application is about to quit
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        FirebaseManager.DeleteGuestData();
        FirebaseManager.DeleteGuestAccount();
        SignOut();  // Log the user out when the game quits
    }

    #region SettingsDB

    public void SaveSettingsToFirebase(float masterVolume, float musicVolume, float sfxVolume, bool isFullScreen, int qualityIndex, int resolutionIndex, float xMouseSens, float yMouseSens, float xGamepadSens, float yGamepadSens, float dialogueSpeed, int languageIndex)
    {
        if (auth.CurrentUser != null)
        {
            string userId = auth.CurrentUser.UserId;

            DatabaseReference userSettingsRef = databaseReference.Child("users").Child(userId).Child("settings");

            userSettingsRef.Child("masterVolume").SetValueAsync(masterVolume);
            userSettingsRef.Child("musicVolume").SetValueAsync(musicVolume);
            userSettingsRef.Child("sfxVolume").SetValueAsync(sfxVolume);
            userSettingsRef.Child("isFullScreen").SetValueAsync(isFullScreen ? 1 : 0);
            userSettingsRef.Child("qualityIndex").SetValueAsync(qualityIndex);
            userSettingsRef.Child("resolutionIndex").SetValueAsync(resolutionIndex);
            userSettingsRef.Child("xMouseSensitivity").SetValueAsync(xMouseSens);
            userSettingsRef.Child("yMouseSensitivity").SetValueAsync(yMouseSens);
            userSettingsRef.Child("xGamepadSensitivity").SetValueAsync(xGamepadSens);
            userSettingsRef.Child("yGamepadSensitivity").SetValueAsync(yGamepadSens);
            userSettingsRef.Child("dialogueSpeed").SetValueAsync(dialogueSpeed);
            userSettingsRef.Child("languageIndex").SetValueAsync(languageIndex);

            Debug.Log("User settings saved to Firebase.");
        }
        else
        {
            Debug.LogWarning("No user is signed in, cannot save settings to Firebase.");
        }
    }

    public void FetchSettingsFromFirebase(Action<float, float, float, bool, int, int, float, float, float, float, float, int> onSettingsFetched)
    {
        if (auth.CurrentUser != null)
        {
            string userId = auth.CurrentUser.UserId;

            databaseReference.Child("users").Child(userId).Child("settings").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result.Exists)
                    {
                        var settingsSnapshot = task.Result;

                        float masterVolume = float.Parse(settingsSnapshot.Child("masterVolume").Value.ToString());
                        float musicVolume = float.Parse(settingsSnapshot.Child("musicVolume").Value.ToString());
                        float sfxVolume = float.Parse(settingsSnapshot.Child("sfxVolume").Value.ToString());
                        bool isFullScreen = settingsSnapshot.Child("isFullScreen").Value.ToString() == "1";
                        int qualityIndex = int.Parse(settingsSnapshot.Child("qualityIndex").Value.ToString());
                        int resolutionIndex = int.Parse(settingsSnapshot.Child("resolutionIndex").Value.ToString());
                        float xMouseSens = float.Parse(settingsSnapshot.Child("xMouseSensitivity").Value.ToString());
                        float yMouseSens = float.Parse(settingsSnapshot.Child("yMouseSensitivity").Value.ToString());
                        float xGamepadSens = float.Parse(settingsSnapshot.Child("xGamepadSensitivity").Value.ToString());
                        float yGamepadSens = float.Parse(settingsSnapshot.Child("yGamepadSensitivity").Value.ToString());
                        float dialogueSpeed = float.Parse(settingsSnapshot.Child("dialogueSpeed").Value.ToString());
                        int languageIndex = int.Parse(settingsSnapshot.Child("languageIndex").Value.ToString());

                        onSettingsFetched(masterVolume, musicVolume, sfxVolume, isFullScreen, qualityIndex, resolutionIndex, xMouseSens, yMouseSens, xGamepadSens, yGamepadSens, dialogueSpeed, languageIndex);
                    }
                    else
                    {
                        Debug.LogWarning("No settings found for user, using default values.");
                        onSettingsFetched(1f, 1f, 1f, true, 0, 0, 1f, 1f, 1f, 1f, 1f, 0); // Provide default values
                    }
                }
                else
                {
                    Debug.LogError("Failed to fetch settings from Firebase: " + task.Exception);
                }
            });
        }
        else
        {
            Debug.LogWarning("No user is signed in, cannot fetch settings from Firebase.");
            onSettingsFetched(1f, 1f, 1f, true, 0, 0, 1f, 1f, 1f, 1f, 1f, 0); // Provide default values
        }
    }

    #endregion

    #region ChoicesDB

    public void SaveChoiceToFirebase(string choiceKey, int choiceValue)
    {
        if (auth.CurrentUser != null)
        {
            string userId = auth.CurrentUser.UserId;
            databaseReference
                .Child("users")
                .Child(userId)
                .Child("choices")
                .Child(choiceKey)
                .SetValueAsync(choiceValue)
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        Debug.Log($"Choice {choiceKey} saved to Firebase with value {choiceValue}.");
                    }
                    else
                    {
                        Debug.LogError($"Failed to save choice {choiceKey} to Firebase: {task.Exception}");
                    }
                });
        }
        else
        {
            Debug.LogWarning("No user is signed in, cannot save choice to Firebase.");
        }
    }

    public void FetchUserChoice(string choiceKey, Action<int?> onChoiceFetched)
    {
        if (auth.CurrentUser != null)
        {
            string userId = auth.CurrentUser.UserId;
            databaseReference
                .Child("users")
                .Child(userId)
                .Child("choices")
                .Child(choiceKey)
                .GetValueAsync()
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted && task.Result.Exists)
                    {
                        int choiceValue = int.Parse(task.Result.Value.ToString());
                        onChoiceFetched.Invoke(choiceValue);
                    }
                    else
                    {
                        onChoiceFetched.Invoke(null); // No choice data found
                    }
                });
        }
        else
        {
            onChoiceFetched.Invoke(null); // User not logged in
        }
    }

    public void FetchChoiceStatistics(string choiceKey, Action<int, int> onPercentagesFetched)
    {
        databaseReference
            .Child("users")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    int choice1Count = 0;
                    int choice2Count = 0;
                    int totalCount = 0;

                    DataSnapshot snapshot = task.Result;
                    foreach (DataSnapshot userSnapshot in snapshot.Children)
                    {
                        if (userSnapshot.Child("choices").HasChild(choiceKey))
                        {
                            int choiceValue = int.Parse(userSnapshot.Child("choices").Child(choiceKey).Value.ToString());
                            switch (choiceValue)
                            {
                                case 1: choice1Count++; break;
                                case 2: choice2Count++; break;
                            }
                            totalCount++;
                        }
                    }

                    // Debug log total number of users and their choices
                    Debug.Log($"Total users: {totalCount}");
                    Debug.Log($"Choice 1 Count: {choice1Count}, Choice 2 Count: {choice2Count}");

                    if (totalCount > 0)
                    {
                        int percentage1 = Mathf.RoundToInt((choice1Count * 100f) / totalCount);
                        int percentage2 = Mathf.RoundToInt((choice2Count * 100f) / totalCount);
                        onPercentagesFetched.Invoke(percentage1, percentage2);
                    }
                    else
                    {
                        onPercentagesFetched.Invoke(0, 0);
                    }
                }
                else
                {
                    Debug.LogError("Failed to fetch choice statistics: " + task.Exception);
                    onPercentagesFetched.Invoke(0, 0);
                }
            });
    }

    public void FetchChoiceStatisticsThreeOptions(string choiceKey, Action<int, int, int> onPercentagesFetched)
    {
        databaseReference
            .Child("users")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    int choice1Count = 0;
                    int choice2Count = 0;
                    int choice3Count = 0;
                    int totalCount = 0;

                    DataSnapshot snapshot = task.Result;
                    foreach (DataSnapshot userSnapshot in snapshot.Children)
                    {
                        if (userSnapshot.Child("choices").HasChild(choiceKey))
                        {
                            int choiceValue = int.Parse(userSnapshot.Child("choices").Child(choiceKey).Value.ToString());
                            switch (choiceValue)
                            {
                                case 1: choice1Count++; break;
                                case 2: choice2Count++; break;
                                case 3: choice3Count++; break;
                            }
                            totalCount++;
                        }
                    }

                    // Debug log total number of users and their choices
                    Debug.Log($"Total users: {totalCount}");
                    Debug.Log($"Choice 1 Count: {choice1Count}, Choice 2 Count: {choice2Count}, Choice 3 Count: {choice3Count}");

                    if (totalCount > 0)
                    {
                        int percentage1 = Mathf.RoundToInt((choice1Count * 100f) / totalCount);
                        int percentage2 = Mathf.RoundToInt((choice2Count * 100f) / totalCount);
                        int percentage3 = Mathf.RoundToInt((choice3Count * 100f) / totalCount);
                        onPercentagesFetched.Invoke(percentage1, percentage2, percentage3);
                    }
                    else
                    {
                        onPercentagesFetched.Invoke(0, 0, 0);
                    }
                }
                else
                {
                    Debug.LogError("Failed to fetch choice statistics: " + task.Exception);
                    onPercentagesFetched.Invoke(0, 0, 0);
                }
            });
    }

    #endregion

    #region ChaptersDB

    public void SaveChapterUnlockToFirebase(string chapterKey, bool isUnlocked)
    {
        if (auth.CurrentUser != null)
        {
            string userId = auth.CurrentUser.UserId;
            databaseReference
                .Child("users")
                .Child(userId)
                .Child("chapters")
                .Child(chapterKey)
                .SetValueAsync(isUnlocked ? 1 : 0)
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        Debug.Log($"Chapter {chapterKey} unlock status saved to Firebase: {isUnlocked}");
                    }
                    else
                    {
                        Debug.LogError($"Failed to save chapter {chapterKey} unlock status to Firebase: {task.Exception}");
                    }
                });
        }
        else
        {
            Debug.LogWarning("No user is signed in, cannot save chapter unlock status to Firebase.");
        }
    }

    public void GetChapterUnlockStatusFromFirebase(string chapterKey, System.Action<bool> callback)
    {
        if (auth.CurrentUser != null)
        {
            string userId = auth.CurrentUser.UserId;
            databaseReference
                .Child("users")
                .Child(userId)
                .Child("chapters")
                .Child(chapterKey)
                .GetValueAsync()
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        if (task.Result.Exists)
                        {
                            bool isUnlocked = task.Result.Value.ToString() == "1";
                            callback(isUnlocked);
                        }
                        else
                        {
                            // Log specific message if chapter data is missing
                            Debug.LogWarning($"Chapter {chapterKey} not found in Firebase, defaulting to locked.");
                            callback(false); // Default to locked if chapter is missing
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to retrieve chapter unlock status from Firebase due to: " + task.Exception);
                        callback(false); // Default to locked if fetch fails
                    }
                });
        }
        else
        {
            Debug.LogWarning("No user signed in. Unable to fetch chapter unlock status from Firebase.");
            callback(false); // Default to locked if no user is signed in
        }
    }

    #endregion

    #region AchievementDB

    public void SaveAchievementToFirebase(string achievementKey, bool isUnlocked)
    {
        if (auth.CurrentUser != null)
        {
            string userId = auth.CurrentUser.UserId;
            databaseReference
                .Child("users")
                .Child(userId)
                .Child("achievements")
                .Child(achievementKey)
                .SetValueAsync(isUnlocked ? 1 : 0)
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        Debug.Log($"Achievement {achievementKey} unlock status saved to Firebase: {isUnlocked}");
                    }
                    else
                    {
                        Debug.LogError($"Failed to save achievement {achievementKey} unlock status to Firebase: {task.Exception}");
                    }
                });
        }
        else
        {
            Debug.LogWarning("No user is signed in, cannot save achievement unlock status to Firebase.");
        }
    }

    public void GetAchievementStatusFromFirebase(string achievementKey, System.Action<bool> callback)
    {
        if (auth.CurrentUser != null)
        {
            string userId = auth.CurrentUser.UserId;
            databaseReference
                .Child("users")
                .Child(userId)
                .Child("achievements")
                .Child(achievementKey)
                .GetValueAsync()
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        if (task.Result.Exists)
                        {
                            bool isUnlocked = task.Result.Value.ToString() == "1";
                            callback(isUnlocked);
                        }
                        else
                        {
                            Debug.LogWarning($"Achievement {achievementKey} not found in Firebase, defaulting to locked.");
                            callback(false); // Default to locked if achievement data is missing
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to retrieve achievement status from Firebase due to: " + task.Exception);
                        callback(false); // Default to locked if fetch fails
                    }
                });
        }
        else
        {
            Debug.LogWarning("No user signed in. Unable to fetch achievement status from Firebase.");
            callback(false); // Default to locked if no user is signed in
        }
    }

    #endregion


}
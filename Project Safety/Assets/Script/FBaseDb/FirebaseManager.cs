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
        if (auth.CurrentUser != null && auth.CurrentUser.IsAnonymous)
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
    }

    public static void SignOut()
    {
        auth.SignOut();
        Debug.Log("User has been signed out.");
    }

    // This method is called when the application is about to quit
    private void OnApplicationQuit()
    {
        FirebaseManager.DeleteGuestData();
        FirebaseManager.DeleteGuestAccount();
        SignOut();  // Log the user out when the game quits
    }


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


}
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
        if (auth.CurrentUser != null && auth.CurrentUser.IsAnonymous)
        {
            // Delete guest user's data from the database
            databaseReference
                .Child("users")
                .Child(auth.CurrentUser.UserId)
                .RemoveValueAsync()
                .ContinueWithOnMainThread(task => {
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

}
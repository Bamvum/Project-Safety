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
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase initialized.");
            }
            else
            {
                Debug.LogError("Could not initialize Firebase: " + task.Exception);
            }
        });
    }

    // Save the player's decision in the database
    public void SaveDecision(string userId, string act, string decisionKey, bool decisionValue)
    {
        string decisionPath = $"users/{userId}/decisions/{act}/{decisionKey}";
        databaseReference.Child(decisionPath).SetValueAsync(decisionValue).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Failed to save decision {decisionKey} for act {act}: {task.Exception.Message}");
            }
            else
            {
                Debug.Log($"Decision {decisionKey} for act {act} saved successfully.");
            }
        });
    }

    // Update overall statistics based on player decisions
    public void UpdateOverallStatistics(string act, string decisionKey, bool decisionValue)
    {
        string overallPath = $"statistics/{act}/{decisionKey}/totalPlayers";
        string truePath = $"statistics/{act}/{decisionKey}/trueCount";

        databaseReference.RunTransaction(mutableData =>
        {
            Dictionary<string, object> stats = mutableData.Value as Dictionary<string, object>;
            if (stats == null)
            {
                stats = new Dictionary<string, object> { { "totalPlayers", 0 }, { "trueCount", 0 } };
            }

            stats["totalPlayers"] = (long)stats["totalPlayers"] + 1;

            if (decisionValue)
            {
                stats["trueCount"] = (long)stats["trueCount"] + 1;
            }

            mutableData.Value = stats;
            return TransactionResult.Success(mutableData);
        }).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to update overall statistics: " + task.Exception.Message);
            }
            else
            {
                Debug.Log("Overall statistics updated successfully.");
            }
        });
    }

    // Compare the player's decisions with overall statistics
    public void ComparePlayerToOverall(string userId)
    {
        string userPath = $"users/{userId}/decisions";
        string statsPath = "statistics";

        // Fetch the user's decisions
        databaseReference.Child(userPath).GetValueAsync().ContinueWith(userTask =>
        {
            if (userTask.IsFaulted || userTask.Result == null)
            {
                Debug.LogError("Failed to fetch player decisions: " + userTask.Exception.Message);
                return;
            }

            Dictionary<string, object> playerDecisions = userTask.Result.Value as Dictionary<string, object>;

            // Fetch the overall statistics
            databaseReference.Child(statsPath).GetValueAsync().ContinueWith(statsTask =>
            {
                if (statsTask.IsFaulted || statsTask.Result == null)
                {
                    Debug.LogError("Failed to fetch overall statistics: " + statsTask.Exception.Message);
                    return;
                }

                Dictionary<string, object> overallStats = statsTask.Result.Value as Dictionary<string, object>; 
                // Lagay dito playerref comparison logic
                Debug.Log("Comparison completed.");
            });
        });
    }
}
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance;

    private string userId;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetUserId(string userId)
    {
        this.userId = userId;
        Debug.Log("User ID set: " + userId);
    }

    public void SavePlayerDecision(string act, string decisionKey, bool decisionValue)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            FirebaseManager.Instance.SaveDecision(userId, act, decisionKey, decisionValue);
            FirebaseManager.Instance.UpdateOverallStatistics(act, decisionKey, decisionValue);
        }
        else
        {
            Debug.LogError("User ID is not set. Using PlayerPrefs to save.");
            PlayerPrefs.SetInt($"{act}_{decisionKey}", decisionValue ? 1 : 0);
        }
    }

    public void CompareDecisionsWithOverall()
    {
        if (!string.IsNullOrEmpty(userId))
        {
            FirebaseManager.Instance.ComparePlayerToOverall(userId);
        }
        else
        {
            Debug.LogError("User ID is not set. Cannot compare decisions.");
        }
    }
}

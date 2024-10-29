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
}

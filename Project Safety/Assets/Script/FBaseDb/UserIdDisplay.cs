using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TMPro;

public class UserIdDisplay : MonoBehaviour
{
    public TMP_Text userIdText;
    private FirebaseAuth auth;
    private FirebaseUser user;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;

        if (user != null)
        {
            userIdText.text = "User ID: " + user.UserId;
        }
        else
        {
            userIdText.text = "User not logged in.";
        }
    }

    void Update()
    {
        if (auth.CurrentUser != user)
        {
            user = auth.CurrentUser;
            userIdText.text = "User ID: " + (user != null ? user.UserId : "User not logged in.");
        }
    }
}
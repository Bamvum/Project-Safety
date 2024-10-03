using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TMPro;

public class LoginRegisterManager : MonoBehaviour
{
    [Header("Login UI")]
    public TMP_InputField emailLoginInput;
    public TMP_InputField passwordLoginInput;
    public TMP_Text loginErrorText;
    public GameObject LoginPanel;

    [Header("Register UI")]
    public TMP_InputField usernameRegInput;
    public TMP_InputField emailRegInput;
    public TMP_InputField passwordRegInput;
    public TMP_Text registerErrorText;
    public GameObject RegisterPanel;

    // Switch to Register Panel
    public void ShowRegisterPanel()
    {
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }

    // Switch to Login Panel
    public void ShowLoginPanel()
    {
        RegisterPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    // Login Functionality
    public void LoginUser()
    {
        Debug.Log("Login button pressed"); // Log when login is initiated
        string email = emailLoginInput.text;
        string password = passwordLoginInput.text;

        FirebaseManager.auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                ShowLoginError("Login canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                var firebaseEx = task.Exception?.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                if (firebaseEx != null)
                {
                    ShowLoginError($"Login failed: {firebaseEx.Message} (Error Code: {firebaseEx.ErrorCode})");
                    Debug.LogError($"Error Code: {firebaseEx.ErrorCode} - {firebaseEx.Message}");
                }
                else
                {
                    ShowLoginError("Login failed: An internal error has occurred.");
                }
                return;
            }

            Firebase.Auth.AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;

            Debug.Log("Login successful: " + user.Email);

            UserManager.Instance.SetUserId(user.UserId);

            // Load Main Menu scene on the main thread
            UnitySynchronizationContext.ExecuteOnMainThread(() =>
            {
                SceneManager.LoadScene("Main Menu"); // or use index
            });
        });
    }

    // Register Functionality
    public void RegisterUser()
    {
        Debug.Log("Register button pressed"); // Log when register is initiated
        string username = usernameRegInput.text;
        string email = emailRegInput.text;
        string password = passwordRegInput.text;

        FirebaseManager.auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                ShowRegisterError("Registration canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                ShowRegisterError("Registration failed: " + task.Exception.Flatten().InnerExceptions[0].Message);
                return;
            }

            Firebase.Auth.AuthResult authResult = task.Result;
            FirebaseUser newUser = authResult.User;

            Debug.Log("Registration successful: " + newUser.Email);
            SaveUsername(newUser.UserId, username);

            UserManager.Instance.SetUserId(newUser.UserId);

            // Load Main Menu scene on the main thread
            UnitySynchronizationContext.ExecuteOnMainThread(() =>
            {
                SceneManager.LoadScene("Main Menu"); // or use index
            });
        });
    }

    // Save Username (optional)
    private void SaveUsername(string userId, string username)
    {
        Dictionary<string, object> userData = new Dictionary<string, object>();
        userData["username"] = username;

        FirebaseManager.databaseReference.Child("users").Child(userId).UpdateChildrenAsync(userData).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to save username: " + task.Exception.Message);
            }
            else
            {
                Debug.Log("Username successfully saved.");
            }
        });
    }

    // Play as Guest
    public void PlayAsGuest()
    {
        FirebaseManager.auth.SignInAnonymouslyAsync().ContinueWith(task => {
        if (task.IsCanceled)
        {
            Debug.LogError("Guest sign-in canceled.");
            return;
        }
        if (task.IsFaulted)
        {
            Debug.LogError("Guest sign-in failed: " + task.Exception.Flatten().InnerExceptions[0].Message);
            return;
        }

        FirebaseUser user = task.Result.User;
        Debug.Log("Logged in as guest: " + user.UserId); // User ID of the guest

        UnitySynchronizationContext.ExecuteOnMainThread(() =>
            {
                SceneManager.LoadScene("Main Menu"); // or use index
            });
    });
    }

    // Error Handling
    private void ShowLoginError(string message)
    {      
            loginErrorText.text = message;
            Debug.LogError(message);    
    }

    private void ShowRegisterError(string message)
    {
            registerErrorText.text = message;
            Debug.LogError(message);      
    }

// Start is called before the first frame update
void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

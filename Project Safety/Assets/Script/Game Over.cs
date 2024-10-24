using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.iOS;

public class GameOver : MonoBehaviour
{
    public static GameOver instance { get; private set; }

    void Awake()
    {
        instance = this;
    }

    [Header("HUD")] 
    [SerializeField] RectTransform gameOverHUDRectTransform;
    [SerializeField] CanvasGroup gameOverCG;

    [Header("Selected Button")] 
    [SerializeField] GameObject lastSelectedButton; // FOR GAMEPAD
    
    [Space(5)]
    [SerializeField] GameObject retrySelectedButton;

    [Header("Flag")]
    [SerializeField] bool isGamepad;

    void Update()
    {
        DeviceInputChecher();
        
        // GAMEPAD VIBRATION ON NAVIGATION 
        if (Gamepad.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            if (Gamepad.current.leftStick.ReadValue() != Vector2.zero || Gamepad.current.dpad.ReadValue() != Vector2.zero)
            {
                GameObject currentSelectedButton = EventSystem.current.currentSelectedGameObject;

                Debug.Log("Inputed Leftstick and Dpad");
                // Check if the selected UI element has changed (button navigation)
                if (currentSelectedButton != lastSelectedButton)
                {
                    // Trigger vibration when navigating to a new button
                    VibrateGamepad();
                    lastSelectedButton = currentSelectedButton; // Update the last selected button
                }
            }
        }
    }

    private void VibrateGamepad()
    {
        // Set a short vibration
        Gamepad.current.SetMotorSpeeds(0.3f, 0.3f); // Adjust the intensity here
        Invoke("StopVibration", 0.1f); // Stops vibration after 0.1 seconds
        StartCoroutine(StopVibration(.1f));
    }


    IEnumerator StopVibration(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Gamepad.current.SetMotorSpeeds(0, 0);
    }

    public void GameIsOver()
    {

        if(SceneManager.GetActiveScene().name == "Act 2 Scene 1")
        {
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
            
            LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    PlayerScript.instance.DisablePlayerScripts();

                    LoadingSceneManager.instance.loadingScreen.SetActive(true);
                    LoadingSceneManager.instance.enabled = true;

                    LoadingSceneManager.instance.sceneName = "Act 2 Scene 1";
                });
        }

        if(SceneManager.GetActiveScene().name == "Act 2 Scene 2")
        {
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
            
            LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    PlayerScript.instance.DisablePlayerScripts();

                    LoadingSceneManager.instance.loadingScreen.SetActive(true);
                    LoadingSceneManager.instance.enabled = true;

                    LoadingSceneManager.instance.sceneName = "Act 2 Scene 2";
                });
        }
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0;

        gameOverHUDRectTransform.sizeDelta = new Vector2(0, 700);
        gameOverCG.interactable = false;
        
        gameOverHUDRectTransform.gameObject.SetActive(true);
        gameOverHUDRectTransform.DOSizeDelta(new Vector2(1250, gameOverHUDRectTransform.sizeDelta.y), .25f)
            .SetEase(Ease.InFlash)
            .SetUpdate(true)
            .OnComplete(() =>
            {
               gameOverCG.gameObject.SetActive(true); 
               gameOverCG.DOFade(1, .25f)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    gameOverCG.interactable = true;
                }); 
            });
    }

    public void GoToMainMenu()
    {
        gameOverCG.interactable = false;

        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                PlayerScript.instance.DisablePlayerScripts();

                LoadingSceneManager.instance.loadingScreen.SetActive(true);
                LoadingSceneManager.instance.enabled = true;

                LoadingSceneManager.instance.sceneName = "Main Menu";
            });
    }

    #region - DEVICE INPUT CHECKER UI -

    void DeviceInputChecher()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            if(gameOverHUDRectTransform.gameObject.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                EventSystem.current.SetSelectedGameObject(null);
                isGamepad = false;
            }
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            Cursor.lockState = CursorLockMode.Locked;

            if(!isGamepad)
            {
                if(gameOverHUDRectTransform.gameObject.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(retrySelectedButton);
                    isGamepad = true;
                }
            }
        }
    }

    #endregion
}

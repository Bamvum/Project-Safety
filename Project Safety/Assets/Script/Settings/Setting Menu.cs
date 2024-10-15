using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;

public class SettingMenu : MonoBehaviour
{
    public static SettingMenu instance { get; private set; }
    private DatabaseReference dbReference;
    private FirebaseAuth auth;
    private string userId;

    void Awake()
    {
        instance = this;
    }

    [SerializeField] AudioMixer audioMixer;
    Resolution[] resolutions;

    [Space(5)]
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;

    [Space(5)]
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] Toggle fullScreenToggle;

    [Space(5)]
    public Slider xMouseSensSlider;
    public Slider yMouseSensSlider;
    public Slider xGamepadSensSlider;
    public Slider yGamepadSensSlider;

    [Space(5)]
    public Slider dialogueSpeedSlider;
    public TMP_Dropdown languageDropdown;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        if (auth.CurrentUser != null)
        {
            userId = auth.CurrentUser.UserId;
            LoadSettingsFromFirebase(userId); // Load from Firebase first
        }
        else
        {
            LoadSettingsFromPlayerPrefs(); // Only load PlayerPrefs if no user is logged in
        }
    }

    #region - LOAD SETTINGS FROM PLAYER PREFS -

    public void LoadSettingsFromPlayerPrefs()
    {
        // VOLUME PLAYER PREFS
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0f);

        // GRAPHICS PLAYER PREFS
        fullScreenToggle.isOn = PlayerPrefs.GetInt("IsFullScreen", 1) == 1;
        qualityDropdown.value = PlayerPrefs.GetInt("QualityGraphics", 2);

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // CONTROLS PLAYER PREFS    
        xMouseSensSlider.value = PlayerPrefs.GetFloat("XMouseSensitivity", 1f);
        yMouseSensSlider.value = PlayerPrefs.GetFloat("YMouseSensitivity", 1f);
        xGamepadSensSlider.value = PlayerPrefs.GetFloat("XGamepadSensitivity", 1f);
        yGamepadSensSlider.value = PlayerPrefs.GetFloat("YGamepadSensitivity", 1f);

        // LANGUAGE PLAYER PREFS  
        dialogueSpeedSlider.value = PlayerPrefs.GetFloat("DialogueSpeed", 0.5f);
        languageDropdown.value = PlayerPrefs.GetInt("Language", 0);
    }

    #endregion

    #region - SAVE SETTINGS TO FIREBASE -

    public void SaveSettingsToFirebase()
    {
        if (auth.CurrentUser != null)
        {
            userId = auth.CurrentUser.UserId;

            Dictionary<string, object> settingsData = new Dictionary<string, object>
            {
                { "MasterVolume", masterVolumeSlider.value },
                { "MusicVolume", musicVolumeSlider.value },
                { "SFXVolume", sfxVolumeSlider.value },
                { "ResolutionIndex", resolutionDropdown.value },
                { "IsFullScreen", fullScreenToggle.isOn },
                { "QualityGraphics", qualityDropdown.value },
                { "XMouseSensitivity", xMouseSensSlider.value },
                { "YMouseSensitivity", yMouseSensSlider.value },
                { "XGamepadSensitivity", xGamepadSensSlider.value },
                { "YGamepadSensitivity", yGamepadSensSlider.value },
                { "DialogueSpeed", dialogueSpeedSlider.value },
                { "Language", languageDropdown.value }
            };

            string userSettingsPath = $"users/{userId}/settings";
            dbReference.Child(userSettingsPath).SetValueAsync(settingsData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError($"Failed to save user settings: {task.Exception}");
                }
                else
                {
                    Debug.Log("User settings saved successfully.");
                }
            });
        }
    }

    #endregion

    #region - LOAD SETTINGS FROM FIREBASE -

    public void LoadSettingsFromFirebase(string userId)
    {
        string userSettingsPath = $"users/{userId}/settings";
        dbReference.Child(userSettingsPath).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Failed to load user settings: {task.Exception}");
            }
            else if (task.IsCompleted && task.Result.Value != null)
            {
                Dictionary<string, object> settingsData = task.Result.Value as Dictionary<string, object>;
                ApplySettings(settingsData);
                Debug.Log("Settings loaded from Firebase.");
            }
            else
            {
                Debug.Log("No settings found for the user in Firebase.");
            }
        });
    }

    void ApplySettings(Dictionary<string, object> settingsData)
    {
        if (settingsData == null) return;

        // VOLUME SETTINGS
        if (settingsData.ContainsKey("MasterVolume"))
        {
            masterVolumeSlider.value = float.Parse(settingsData["MasterVolume"].ToString());
            SetMasterVolume(masterVolumeSlider.value);
        }
        if (settingsData.ContainsKey("MusicVolume"))
        {
            musicVolumeSlider.value = float.Parse(settingsData["MusicVolume"].ToString());
            SetMusicVolume(musicVolumeSlider.value);
        }
        if (settingsData.ContainsKey("SFXVolume"))
        {
            sfxVolumeSlider.value = float.Parse(settingsData["SFXVolume"].ToString());
            SetSFXVolume(sfxVolumeSlider.value);
        }

        // GRAPHICS SETTINGS
        if (settingsData.ContainsKey("IsFullScreen"))
        {
            fullScreenToggle.isOn = bool.Parse(settingsData["IsFullScreen"].ToString());
            SetFullScreen(fullScreenToggle.isOn);
        }
        if (settingsData.ContainsKey("QualityGraphics"))
        {
            int qualityIndex = int.Parse(settingsData["QualityGraphics"].ToString());
            qualityDropdown.value = qualityIndex; // Load quality index directly
            SetQuality(qualityIndex);
        }

        // Resolution Handling
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        if (settingsData.ContainsKey("ResolutionIndex"))
        {
            int savedResolutionIndex = int.Parse(settingsData["ResolutionIndex"].ToString());
            if (savedResolutionIndex >= 0 && savedResolutionIndex < options.Count)
            {
                resolutionDropdown.value = savedResolutionIndex; // Load resolution index
            }
            else
            {
                resolutionDropdown.value = currentResolutionIndex; // Fallback to current resolution
            }
        }
        resolutionDropdown.RefreshShownValue();
        SetResolution(resolutionDropdown.value); // Set the screen resolution

        // CONTROLS SETTINGS
        if (settingsData.ContainsKey("XMouseSensitivity"))
        {
            xMouseSensSlider.value = float.Parse(settingsData["XMouseSensitivity"].ToString());
            SetXMouseSensitivity(xMouseSensSlider.value);
        }
        if (settingsData.ContainsKey("YMouseSensitivity"))
        {
            yMouseSensSlider.value = float.Parse(settingsData["YMouseSensitivity"].ToString());
            SetYMouseSensitivity(yMouseSensSlider.value);
        }
        if (settingsData.ContainsKey("XGamepadSensitivity"))
        {
            xGamepadSensSlider.value = float.Parse(settingsData["XGamepadSensitivity"].ToString());
            SetXGamepadSensitivity(xGamepadSensSlider.value);
        }
        if (settingsData.ContainsKey("YGamepadSensitivity"))
        {
            yGamepadSensSlider.value = float.Parse(settingsData["YGamepadSensitivity"].ToString());
            SetYGamepadSensitivity(yGamepadSensSlider.value);
        }

        // LANGUAGE SETTINGS
        if (settingsData.ContainsKey("DialogueSpeed"))
        {
            dialogueSpeedSlider.value = float.Parse(settingsData["DialogueSpeed"].ToString());
            SetDialogueSpeed(dialogueSpeedSlider.value);
        }
        if (settingsData.ContainsKey("Language"))
        {
            languageDropdown.value = int.Parse(settingsData["Language"].ToString());
            SetLanguage(languageDropdown.value);
        }
    }

    #endregion

    #region - VOLUME -

    public void SetMasterVolume(float masterVolume)
    {
        audioMixer.SetFloat("Master", masterVolume);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        SaveSettingsToFirebase();
    }

    public void SetMusicVolume(float musicVolume)
    {
        audioMixer.SetFloat("Music", musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        SaveSettingsToFirebase();
    }

    public void SetSFXVolume(float sfxVolume)
    {
        audioMixer.SetFloat("SFX", sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        SaveSettingsToFirebase();
    }

    #endregion

    #region - QUALITY -

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityGraphics", qualityIndex);
        SaveSettingsToFirebase();
    }

    #endregion

    #region - FULL SCREEN -

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("IsFullScreen", isFullScreen ? 1 : 0);
        SaveSettingsToFirebase();
    }

    #endregion

    #region - RESOLUTION -

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        SaveSettingsToFirebase();
    }

    #endregion

    #region - MOUSE SENSITIVITY -

    public void SetXMouseSensitivity(float xMouseSens)
    {
        // PlayerScript.instance.playerMovement.xMouseSensitivity = xMouseSens;
        xMouseSensSlider.value = xMouseSens;
        PlayerPrefs.SetFloat("XMouseSensitivity", xMouseSens);
        SaveSettingsToFirebase();
    }

    public void SetYMouseSensitivity(float yMouseSens)
    {
        // PlayerScript.instance.playerMovement.yMouseSensitivity = yMouseSens;
        yMouseSensSlider.value = yMouseSens;
        PlayerPrefs.SetFloat("YMouseSensitivity", yMouseSens);
        SaveSettingsToFirebase();
    }

    #endregion

    #region - GAMEPAD SENSITIVITY -

    public void SetXGamepadSensitivity(float xGamepadSens)
    {
        // PlayerScript.instance.playerMovement.xGamepadSensitivity = xGamepadSens;
        xGamepadSensSlider.value = xGamepadSens;
        PlayerPrefs.SetFloat("XGamepadSensitivity", xGamepadSens);
        SaveSettingsToFirebase();
    }

    public void SetYGamepadSensitivity(float yGamepadSens)
    {
        // PlayerScript.instance.playerMovement.yGamepadSensitivity = yGamepadSens;
        yGamepadSensSlider.value = yGamepadSens;
        PlayerPrefs.SetFloat("YGamepadSensitivity", yGamepadSens);
        SaveSettingsToFirebase();
    }

    #endregion

    #region - DIALOGUE SPEED -

    public void SetDialogueSpeed(float dialogueSpeed)
    {
        // DialogueManager.instance.typingSpeed = dialogueSpeed;
        dialogueSpeedSlider.value = dialogueSpeed;
        PlayerPrefs.SetFloat("DialogueSpeed", dialogueSpeed);
        SaveSettingsToFirebase();
    }

    #endregion

    #region - LANGUAGE PREFERENCE -

    public void SetLanguage(int languageIndex)
    {
        languageDropdown.value = languageIndex;
        PlayerPrefs.SetInt("Language", languageIndex);
        SaveSettingsToFirebase();
    }

    #endregion
}
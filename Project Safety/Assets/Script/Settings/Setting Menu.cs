using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Firebase.Auth;
using System.Linq;

public class SettingMenu : MonoBehaviour
{
private FirebaseAuth auth;

public static SettingMenu instance { get; private set; }
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
        FetchSettings();
        // VOLUME PLAYER PREFS
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        // GRAPHICS PLAYER PREFS
        fullScreenToggle.isOn = PlayerPrefs.GetInt("IsFullScreen") != 0;
        qualityDropdown.value = PlayerPrefs.GetInt("QualityGraphics");

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        // Sort resolutions by width, height, and refresh rate in descending order
        resolutions = resolutions.OrderByDescending(r => r.width)
                                 .ThenByDescending(r => r.height)
                                 .ThenByDescending(r => r.refreshRateRatio.numerator / r.refreshRateRatio.denominator)
                                 .ToArray();

        int currentResolutionIndex = 0; // Start with the highest resolution after sorting

        for (int i = 0; i < resolutions.Length; i++)
        {
            int refreshRateHz = (int)(resolutions[i].refreshRateRatio.numerator / resolutions[i].refreshRateRatio.denominator);
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + refreshRateHz + "Hz";
            options.Add(option);
        }

        // Add sorted options to the dropdown
        resolutionDropdown.AddOptions(options);
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // CONTROLS PLAYER PREFS    
        xMouseSensSlider.value = PlayerPrefs.GetFloat("XMouseSensitivity");
        yMouseSensSlider.value = PlayerPrefs.GetFloat("YMouseSensitivity");
        xGamepadSensSlider.value = PlayerPrefs.GetFloat("XGamepadSensitivity");
        yGamepadSensSlider.value = PlayerPrefs.GetFloat("YGamepadSensitivity");

        // LANGUAGE PLAYER PREFS  
        dialogueSpeedSlider.value = PlayerPrefs.GetFloat("DialogueSpeed");
        languageDropdown.value = PlayerPrefs.GetInt("Language");

        Debug.Log("Dialogue Slider: " + dialogueSpeedSlider.value);

    }

    public void FetchSettings()
    {
        FirebaseManager.Instance.FetchSettingsFromFirebase((masterVolume, musicVolume, sfxVolume, isFullScreen, qualityIndex, resolutionIndex, xMouseSens, yMouseSens, xGamepadSens, yGamepadSens, dialogueSpeed, languageIndex) =>
        {
            // Set the slider values for audio settings
            masterVolumeSlider.value = masterVolume;
            musicVolumeSlider.value = musicVolume;
            sfxVolumeSlider.value = sfxVolume;

            // Set the toggle for full-screen mode
            fullScreenToggle.isOn = isFullScreen;

            // Set the quality dropdown value
            qualityDropdown.value = qualityIndex;

            // Set the resolution dropdown value
            resolutionDropdown.value = resolutionIndex;

            // Set mouse sensitivity sliders
            xMouseSensSlider.value = xMouseSens;
            yMouseSensSlider.value = yMouseSens;

            // Set gamepad sensitivity sliders
            xGamepadSensSlider.value = xGamepadSens;
            yGamepadSensSlider.value = yGamepadSens;

            // Set dialogue speed
            dialogueSpeedSlider.value = dialogueSpeed;

            // Set the language dropdown value
            languageDropdown.value = languageIndex;

            // Finally log the settings fetched
            Debug.Log("Settings fetched from Firebase.");
        });
    }

    public void SaveSettings()
    {
        FirebaseManager.Instance.SaveSettingsToFirebase(
            masterVolumeSlider.value,
            musicVolumeSlider.value,
            sfxVolumeSlider.value,
            fullScreenToggle.isOn,
            qualityDropdown.value,
            resolutionDropdown.value,
            xMouseSensSlider.value,
            yMouseSensSlider.value,
            xGamepadSensSlider.value,
            yGamepadSensSlider.value,
            dialogueSpeedSlider.value,
            languageDropdown.value
        );
    }

    #region - VOLUME -

    public void SetMasterVolume(float masterVolume)
    {
        audioMixer.SetFloat("Master", masterVolume);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        SaveSettings();
    }

    public void SetMusicVolume(float musicVolume)
    {
        audioMixer.SetFloat("Music", musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        SaveSettings();
    }

    public void SetSFXVolume(float sfxVolume)
    {
        audioMixer.SetFloat("SFX", sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        SaveSettings();
    }

    #endregion

    #region - QUALITY -

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityGraphics", qualityIndex);
        SaveSettings();
    }

    #endregion

    #region - FULL SCREEN - 

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("IsFullscreen", isFullScreen ? 1 : 0);
        SaveSettings();
    }

    #endregion

    #region - RESOLUTION -

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        uint refreshRateHz = (uint)(resolution.refreshRateRatio.numerator / resolution.refreshRateRatio.denominator);
        
        Screen.SetResolution(
            resolution.width,
            resolution.height,
            fullScreenToggle.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed, // Toggle fullscreen 
            new RefreshRate { numerator = refreshRateHz, denominator = 1 }
        );

        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        SaveSettings();
    }

    #endregion

    #region - MOUSE SENSITIVITY - 

    public void SetXMouseSensitivity(float xMouseSens)
    {
        // PlayerScript.instance.playerMovement.xMouseSensitivity = xMouseSens;
        xMouseSensSlider.value = xMouseSens;
        PlayerPrefs.SetFloat("XMouseSensitivity", xMouseSens);
        SaveSettings();
    }


    public void SetYMouseSensitivity(float yMouseSens)
    {
        // PlayerScript.instance.playerMovement.yMouseSensitivity = yMouseSens;
        yMouseSensSlider.value = yMouseSens;
        PlayerPrefs.SetFloat("YMouseSensitivity", yMouseSens);
        SaveSettings();
    }

    #endregion

    #region - GAMEPAD SENSITIVITY - 

    public void SetXGamepadSensitivity(float xGamepadSens)
    {
        // PlayerScript.instance.playerMovement.xGamepadSensitivity = xGamepadSens;
        xGamepadSensSlider.value = xGamepadSens;
        PlayerPrefs.SetFloat("XGamepadSensitivity", xGamepadSens);
        SaveSettings();
    }

    public void SetYGamepadSensitivity(float yGamepadSens)
    {
        // PlayerScript.instance.playerMovement.yGamepadSensitivity = yGamepadSens;
        yGamepadSensSlider.value = yGamepadSens;
        PlayerPrefs.SetFloat("YGamepadSensitivity", yGamepadSens);
        SaveSettings();
    }

    #endregion

    #region - DIALOGUE SPEED - 

    public void SetDialogueSpeed(float dialogueSpeed)
    {
        // DialogueManager.instance.typingSpeed = dialogueSpeed;
        dialogueSpeedSlider.value = dialogueSpeed;
        PlayerPrefs.SetFloat("DialogueSpeed", dialogueSpeed);
        SaveSettings();
    }

    #endregion

    #region - LANGUAGE PREFERENCE - 

    public void SetLanguage(int languageIndex)
    {
        PlayerPrefs.SetInt("Language", languageIndex);
        SaveSettings();
    }

    #endregion
}
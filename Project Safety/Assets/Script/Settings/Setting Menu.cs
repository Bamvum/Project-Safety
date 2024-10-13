using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public static SettingMenu instance {get; private set;}
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
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
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
        xMouseSensSlider.value = PlayerPrefs.GetFloat("XMouseSensitivity");
        yMouseSensSlider.value = PlayerPrefs.GetFloat("YMouseSensitivity");
        xGamepadSensSlider.value = PlayerPrefs.GetFloat("XGamepadSensitivity");
        yGamepadSensSlider.value = PlayerPrefs.GetFloat("YGamepadSensitivity");
        
        // LANGUAGE PLAYER PREFS  
        dialogueSpeedSlider.value = PlayerPrefs.GetFloat("DialogueSpeed");
        languageDropdown.value = PlayerPrefs.GetInt("Language");

        Debug.Log("Dialogue Slider: " + dialogueSpeedSlider.value);

    }

    #region - VOLUME -

    public void SetMasterVolume(float masterVolume)
    {
        audioMixer.SetFloat("Master", masterVolume);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
    }

    public void SetMusicVolume(float musicVolume)
    {
        audioMixer.SetFloat("Music", musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void SetSFXVolume(float sfxVolume)
    {
        audioMixer.SetFloat("SFX", sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    #endregion

    #region - QUALITY -
    
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityGraphics", qualityIndex);
    }
    
    #endregion

    #region - FULL SCREEN - 

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("IsFullscreen", isFullScreen ? 1 : 0);
    }

    #endregion

    #region - RESOLUTION -
    
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
    }

    #endregion

    #region - MOUSE SENSITIVITY - 

    public void SetXMouseSensitivity(float xMouseSens)
    {
        // PlayerScript.instance.playerMovement.xMouseSensitivity = xMouseSens;
        xMouseSensSlider.value = xMouseSens;
        PlayerPrefs.SetFloat("XMouseSensitivity", xMouseSens);
    }

    
    public void SetYMouseSensitivity(float yMouseSens)
    {
        // PlayerScript.instance.playerMovement.yMouseSensitivity = yMouseSens;
        yMouseSensSlider.value = yMouseSens;
        PlayerPrefs.SetFloat("YMouseSensitivity", yMouseSens);
    }

    #endregion

    #region - GAMEPAD SENSITIVITY - 

    public void SetXGamepadSensitivity(float xGamepadSens)
    {
        // PlayerScript.instance.playerMovement.xGamepadSensitivity = xGamepadSens;
        xGamepadSensSlider.value = xGamepadSens;
        PlayerPrefs.SetFloat("XGamepadSensitivity", xGamepadSens);
    }

    public void SetYGamepadSensitivity(float yGamepadSens)
    {
        // PlayerScript.instance.playerMovement.yGamepadSensitivity = yGamepadSens;
        yGamepadSensSlider.value = yGamepadSens;
        PlayerPrefs.SetFloat("YGamepadSensitivity", yGamepadSens);
    }

    #endregion

    #region - DIALOGUE SPEED - 

    public void SetDialogueSpeed(float dialogueSpeed)
    {
        // DialogueManager.instance.typingSpeed = dialogueSpeed;
        dialogueSpeedSlider.value = dialogueSpeed;
        PlayerPrefs.SetFloat("DialogueSpeed", dialogueSpeed);
    }

    #endregion

    #region - LANGUAGE PREFERENCE - 

    public void SetLanguage(int languageIndex)
    {
        PlayerPrefs.SetInt("Language", languageIndex);
    }

    #endregion
}

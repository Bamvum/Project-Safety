using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{

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
    [SerializeField] Slider mouseSensSlider;
    [SerializeField] Slider gamepadSensSlider;

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
        mouseSensSlider.value = PlayerPrefs.GetFloat("MouseSensitivity");
        gamepadSensSlider.value = PlayerPrefs.GetFloat("GamepadSensitivity");
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
        PlayerPrefs.SetFloat("MusicVolume", sfxVolume);
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

    public void SetMouseSensitivity(float mouseSens)
    {
        PlayerScript.instance.playerMovement.mouseSensitivity = mouseSens;
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSens);
    }

    #endregion

    #region - GAMEPAD SENSITIVITY - 

    public void SetGamepadSensitivity(float gamepadSens)
    {
        PlayerScript.instance.playerMovement.gamepadSensitivity = gamepadSens;
        PlayerPrefs.SetFloat("GamepadSensitivity", gamepadSens);
    }

    #endregion
}

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
    [SerializeField] Toggle fullScreenToggle;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown qualityDropdown;

    void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");

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
        // resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();
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
}

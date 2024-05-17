using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioMixer musicAudioMixer;
    public AudioMixer SFXAudioMixer;

    public TMP_Dropdown resolutionDropdown;


    public static float musicVolume;
    public static float SFXVolume;

    public static bool firstStart = true;

    public Slider music;
    public Slider SFX;

    public CanvasGroup menuButtons;
    public CanvasGroup configButtons;

    public Resolution[] resolutions;

    public void Start()
    {
        if (firstStart)
        {
            musicVolume = music.value;
            SFXVolume = SFX.value;

        }
        firstStart = false;

        resolutions  = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if ((resolutions[i].width == Screen.currentResolution.width) && (resolutions[i].height == Screen.currentResolution.height))
            {
                currentResolutionIndex = i;
            }
        
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }
    public void SetMusicVolume(float volume)
    {

        musicVolume = volume;

    
    }



    public void SetResolution(int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetSFXVolume(float volume)
    {

        SFXVolume = volume; 

    }

    public void Update()
    {
        musicAudioMixer.SetFloat("Music Volume", Mathf.Log10(musicVolume) * 20);
        SFXAudioMixer.SetFloat("SFX Volume", Mathf.Log10(SFXVolume) * 20);

        music.value = musicVolume;
        SFX.value = SFXVolume;

    }

    public void EnableSettings()
    {
        menuButtons.alpha = 0;
        menuButtons.interactable = false;
        menuButtons.blocksRaycasts = false;
        configButtons.alpha = 1;
        configButtons.interactable = true;
        configButtons.blocksRaycasts = true;

    }

    public void DisableSettings()
    {

        menuButtons.alpha = 1;
        menuButtons.interactable = true;
        menuButtons.blocksRaycasts = true;
        configButtons.alpha = 0;
        configButtons.interactable = false;
        configButtons.blocksRaycasts = false;

    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    
    }
}

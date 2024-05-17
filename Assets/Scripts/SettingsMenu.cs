using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioMixer musicAudioMixer;
    public AudioMixer SFXAudioMixer;

    public static float musicVolume;
    public static float SFXVolume;

    public static bool firstStart = true;

    public Slider music;
    public Slider SFX;

    public void Start()
    {
        if (firstStart)
        {
            musicVolume = music.value;
            SFXVolume = SFX.value;

        }
        firstStart = false;
    }
    public void SetMusicVolume(float volume)
    {



        musicVolume = volume;

            
    
    
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
}

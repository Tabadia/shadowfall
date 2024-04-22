using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioMixer musicAudioMixer;
    public AudioMixer SFXAudioMixer;
    public void SetMusicVolume(float volume)
    {

        musicAudioMixer.SetFloat("Music Volume", Mathf.Log10(volume) * 20);
            
    
    
    }

    public void SetSFXVolume(float volume)
    {

        SFXAudioMixer.SetFloat("SFX Volume", Mathf.Log10(volume) * 20);



    }
}

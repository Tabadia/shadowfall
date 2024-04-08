using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    public AudioSource[] audioSources;
    public int currentIndex = 0;

    void Start()
    {
        ShuffleAudioSources();
        PlayNextAudioSource();
    }

    void Update()
    {
        if (!audioSources[currentIndex].isPlaying)
        {
            PlayNextAudioSource();
        }
    }

    void ShuffleAudioSources()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            int randomIndex = Random.Range(i, audioSources.Length);
            AudioSource temp = audioSources[i];
            audioSources[i] = audioSources[randomIndex];
            audioSources[randomIndex] = temp;
        }
    }

    void PlayNextAudioSource()
    {
        audioSources[currentIndex].Play();
        currentIndex = (currentIndex + 1) % audioSources.Length;
        print("Playing audio source " + currentIndex);
    }
}

using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] musicTracks;
    
    private int currentTrackIndex = 0;

    void Start()
    {
        ShuffleTracks();
        PlayNextTrack();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    void ShuffleTracks()
    {
        for (int i = 0; i < musicTracks.Length; i++)
        {
            AudioClip temp = musicTracks[i];
            int randomIndex = Random.Range(i, musicTracks.Length);
            musicTracks[i] = musicTracks[randomIndex];
            musicTracks[randomIndex] = temp;
        }
    }

    void PlayNextTrack()
    {
        audioSource.clip = musicTracks[currentTrackIndex];
        audioSource.Play();
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAmbiance : MonoBehaviour
{
    public AudioClip[] audioFiles;
    public float minInterval = 5f;
    public float maxInterval = 10f;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomAudio());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayRandomAudio()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            int randomIndex = Random.Range(0, audioFiles.Length);
            audioSource.clip = audioFiles[randomIndex];
            audioSource.Play();
        }
    }
}

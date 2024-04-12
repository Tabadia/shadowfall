using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private float hoverVolume = 1f;
    
    [SerializeField] private AudioClip pressClip;
    [SerializeField] private float pressVolume = 1f;
    
    [SerializeField] private float nextSceneDelay = 0.3f;

    public void Hover()
    {
        audioSource.PlayOneShot(hoverClip, hoverVolume);
    }
    
    public void Pressed(string sceneName)
    {
        StartCoroutine(ChangeSceneDelay(sceneName));
        audioSource.PlayOneShot(pressClip, pressVolume);
    }
    
    IEnumerator ChangeSceneDelay(string sceneName)
    {
        yield return new WaitForSeconds(nextSceneDelay);
        SceneManager.LoadScene(sceneName);
    }
}

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

    public Animator transition;

    public void Hover()
    {
        audioSource.PlayOneShot(hoverClip, hoverVolume);
    }
    
    public void Pressed(string sceneName)
    {
        StartCoroutine(ChangeSceneDelay(sceneName));
        audioSource.PlayOneShot(pressClip, pressVolume);

    }
    public void Quit()
    {
        audioSource.PlayOneShot(pressClip, pressVolume);
        Application.Quit();

    }
    IEnumerator ChangeSceneDelay(string sceneName)
    {
        transition.SetTrigger("Start");



        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }    
    }

   
}

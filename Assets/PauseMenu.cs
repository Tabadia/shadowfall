using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update


    public static bool IsPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))

        {

            if (IsPaused)
            {
                Resume();


            }
            else
            {
                Pause();
            
            
            }
            
        
        }
        
    }


    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

        IsPaused = false;
            
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        IsPaused = true;

    }


}

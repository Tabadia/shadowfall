using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public bool isGamePaused = false;
    public void PauseGame()
    {
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
        IsGamePaused = true;
   
    }
    public void ContinueGame()
    {
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
        IsGamePaused = false;
    }
    public void Update()
    {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        if (IsGamePaused)
        {
            ContinueGame();
            Debug.Log("Game should NOT be paused rn");
        } else
        {
            PauseGame();
            Debug.Log("Game should be paused rn");
        }
    }
}
}
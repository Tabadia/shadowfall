using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update


    public static bool IsPaused = false;
    public GameObject pauseMenuUI;
    public static bool IsInventory = false;
    public GameObject inventoryMenuUI;
    public GameObject crosshair;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused || IsInventory)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (IsInventory)
            {
                Resume();
            }
            else if (!IsPaused)
            {
                Inventory();
            }
        }
    }


    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.canMove = true;
        pauseMenuUI.SetActive(false);
        inventoryMenuUI.SetActive(false); 
        crosshair.SetActive(true);

        Time.timeScale = 1f;

        IsPaused = false;
        IsInventory = false;
            
    }
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        PlayerController.canMove = false;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        crosshair.SetActive(false);
        Time.timeScale = 0f;

        IsPaused = true;

    }
    public void Inventory()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inventoryMenuUI.SetActive(true);
        crosshair.SetActive(false);

        Time.timeScale = 1f;

        IsInventory = true;
    }
}

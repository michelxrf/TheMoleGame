using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public Canvas pauseMenuUI;

    private void Start()
    {
        pauseMenuUI.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if(gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.enabled = false;
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.enabled = true;
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Quit()
    {
        // TODO: save game or put the save system call at OnApplicationQuit()
        Application.Quit();
        Debug.Log("I quit!");
    }
}

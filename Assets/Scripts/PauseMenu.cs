using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Canvas pauseMenuUI;
    public Button resumeButton;
    public Button quitButton;

    private void Start()
    {
        pauseMenuUI.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if(GameData.gameIsPaused)
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
        Cursor.visible = false;
        pauseMenuUI.enabled = false;
        Time.timeScale = 1f;
        GameData.gameIsPaused = false;
    }

    public void Pause()
    {
        Cursor.visible = true;
        pauseMenuUI.enabled = true;
        Time.timeScale = 0f;
        GameData.gameIsPaused = true;
        resumeButton.Select();
    }

    public void Quit()
    {
        Resume();
        SceneManager.LoadScene("Title Screen");
    }
}

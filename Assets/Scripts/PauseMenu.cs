using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Canvas pauseMenuUI;
    public Button resumeButton;
    public Text leaveMinesButton;
    public Text leaveMinesHint;

    private void Start()
    {
        pauseMenuUI.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel") && GameData.health > 0)
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

    public void enableRetreat()
    {
        GameData.leaveMines = !GameData.leaveMines;

        if(GameData.leaveMines)
        {
            leaveMinesHint.enabled = true;
            leaveMinesButton.GetComponent<Text>().text = "Keep Mining";
        }
        else
        {
            leaveMinesHint.enabled = false;
            leaveMinesButton.GetComponent<Text>().text = "Leave Mines";
        }
    }
}

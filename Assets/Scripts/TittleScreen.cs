using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TittleScreen : MonoBehaviour
{
    public Text currentLevel;
    public Text highestLevel;
    public GameObject resumeButton;
    public GameObject newGameButton;

    private void Start()
    {
         Application.targetFrameRate = 300;
        Cursor.visible = true;
        resumeButton.GetComponent<Button>().Select();

        if(!SaveSystem.LoadGame())
        {
            resumeButton.SetActive(false);
            newGameButton.GetComponent<Button>().Select();
        }

        currentLevel.text = "current level: " + GameData.level.ToString();
        highestLevel.text = "highest level: " + GameData.highestLevel.ToString();
    }
    public void Resume()
    {
        SceneManager.LoadScene("Play");
    }

    public void NewGame()
    {
        GameData.level = 1;
        GameData.silver = 0;
        GameData.gold = 0;
        GameData.emerald = 0;
        SaveSystem.SaveGame();
        
        SceneManager.LoadScene("Play");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("I quit!");
    }
}

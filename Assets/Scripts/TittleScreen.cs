using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TittleScreen : MonoBehaviour
{
    public Text currentLevel;
    public Text highestLevel;

    private void Start()
    {
        if(!SaveSystem.LoadGame())
        {
            GameData.level = 0;
            GameData.highestLevel = 0;
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
        GameData.level = 0;
        SceneManager.LoadScene("Play");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("I quit!");
    }
}

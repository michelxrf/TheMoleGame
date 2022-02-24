using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TittleScreen : MonoBehaviour
{
    public void Resume()
    {
        //TODO: load game
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Play");
    }

    public void Quit()
    {
        // TODO: save game or put the save system call at OnApplicationQuit()
        Application.Quit();
        Debug.Log("I quit!");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TittleScreen : MonoBehaviour
{
    public Text highestLevel;
    public Text resumeText;
    public Button resumeButton;
    public GameObject resetProgressButton;
    public GameObject confirmationWarning;
    public GameObject confirmationMark;
    public Button confirmationButton;

    private void Start()
    {
        SaveSystem.LoadGame();

        highestLevel.text = "highest level: " + GameData.highestLevel.ToString();

        Cursor.visible = true;

        if(GameData.level > 0)
        {
            resumeText.text = "Resume - Level " + GameData.level.ToString();
        }
        else
        {
            resumeText.text = "Resume - Start New Run";
        }
    }

    public void Resume()
    {
        if(GameData.level == 0)
        {
            SceneManager.LoadScene("Shop Screen");
        }
        else
        {
            SceneManager.LoadScene("Play");
        }
    }

    public void ResetGame()
    {
        GameData.level = 0;
        GameData.highestLevel = 0;
        GameData.boxChance = -50f;

        GameData.silver = 0;
        GameData.gold = 0;
        GameData.emerald = 0;

        GameData.storedSilver = 0;
        GameData.storedGold = 0;
        GameData.storedEmerald = 0;

        GameData.gameOverScoreSilver = 0;
        GameData.gameOverScoreGold = 0;
        GameData.gameOverScoreEmerald = 0;
        GameData.killedMonsters = 0;

        GameData.health = 3;
        GameData.maxHealth = 3;
        GameData.armor = 0;
        GameData.speed = 3;
        GameData.damage = 1;
        GameData.pickaxe = 1;
        GameData.lamp = 1;

        for(int i = 0; i < GameData.consumables_bought.Length; i++)
        {
            GameData.consumables_bought[i] = false;
        }
        for(int i = 0; i < GameData.upgrades_bought.Length; i++)
        {
            GameData.upgrades_bought[i] = false;
        }
        for(int i = 0; i < GameData.skins_bought.Length; i++)
        {
            GameData.skins_bought[i] = false;
        }

        SaveSystem.SaveGame();

        confirmationMark.SetActive(true);
        confirmationButton.interactable = false;

        highestLevel.text = "highest level: " + GameData.highestLevel.ToString();
        resumeText.text = "Resume - Start New Run";
    }

    public void ShowConfirmation()
    {
        confirmationWarning.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

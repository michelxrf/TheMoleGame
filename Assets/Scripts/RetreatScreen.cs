using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RetreatScreen : MonoBehaviour
{
    public Text silverScore;
    public Text goldScore;
    public Text emeraldScore;
    public Text killedMonsters;
    public Text totalScoreText;

    private int totalScore;

    public Leaderboard leaderboard;

    private void Start()
    {
        Cursor.visible = true;

        GameData.gameOverScoreSilver += GameData.silver;
        GameData.gameOverScoreGold += GameData.gold;
        GameData.gameOverScoreEmerald += GameData.emerald;

        totalScore = GameData.gameOverScoreSilver + 2 * GameData.gameOverScoreGold + 3 * GameData.gameOverScoreEmerald;        
        totalScoreText.text = "Your Score: " + totalScore.ToString();
        StartCoroutine(leaderboard.SubmitScoreRoutine(totalScore));

        silverScore.text = GameData.gameOverScoreSilver.ToString() + " x";
        goldScore.text = GameData.gameOverScoreGold.ToString() + " x";
        emeraldScore.text = GameData.gameOverScoreEmerald.ToString() + " x";
        killedMonsters.text = GameData.killedMonsters.ToString() + " x";

        GameData.level = 0;

        GameData.storedSilver += GameData.silver;
        GameData.storedGold += GameData.gold;
        GameData.storedEmerald += GameData.emerald;

        GameData.silver = 0;
        GameData.gold = 0;
        GameData.emerald = 0;

        GameData.gameOverScoreSilver = 0;
        GameData.gameOverScoreGold = 0;
        GameData.gameOverScoreEmerald = 0;
        GameData.killedMonsters = 0;

        SaveSystem.SaveGame();
    }

    public void Continue()
    {
        SceneManager.LoadScene("Title Screen");
    }
}

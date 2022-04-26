using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    int leaderboard_id = 2549;
    public Text playerRank;

    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;

    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");

        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboard_id, (response) => 
        {
            if(response.success)
            {
                done = true;
            }
            else
            {
                Debug.LogError("Score failed to upload: " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchTopHighscoresRoutine()
    {
        bool done = false;
        int playerID = int.Parse(PlayerPrefs.GetString("PlayerID"));

        LootLockerSDKManager.GetScoreListMain(leaderboard_id, 10, 0, (response) =>
        {
            if(response.success)
            {
                string tempPlayerNames = "";
                string tempPlayerScores = "";

                LootLockerLeaderboardMember[] members = response.items;

                for(int i = 0; i < members.Length; i++)
                {
                    tempPlayerNames += members[i].rank + ". ";

                    if(members[i].player.name != "")
                    {
                        if(playerID == members[i].player.id)
                        {
                            tempPlayerNames += "<b>" + members[i].player.name + "</b>";
                        }
                        else
                        {
                            tempPlayerNames += members[i].player.name;
                        }
                    }
                    else
                    {
                        if(playerID == members[i].player.id)
                        {
                            tempPlayerNames += "<b>" + "Guest" + members[i].player.id + "</b>";
                        }
                        else
                        {
                            tempPlayerNames += "Guest" + members[i].player.id;
                        }
                    }

                    if(playerID == members[i].player.id)
                    {
                        tempPlayerScores += "<b>" + members[i].score + "</b>" + "\n";
                    }
                    else
                    {
                        tempPlayerScores += members[i].score + "\n";
                    }
                    
                    tempPlayerNames += "\n";
                }

                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;
            }
            else
            {
                Debug.LogError("Failed to fetch highscores: " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator GetThisPlayerRank()
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");

        LootLockerSDKManager.GetMemberRank(leaderboard_id.ToString(), playerID, (response) =>
        {
            if (response.success)
            {
                playerRank.text = "Your Highest Score: " + response.score.ToString();
                done = true;
            } 
            else
            {
                playerRank.text = "Your Highest Score: ???";
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
    }
}

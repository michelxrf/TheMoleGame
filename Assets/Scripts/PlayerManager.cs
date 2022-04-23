using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public Leaderboard leaderboard;
    public TMP_InputField playerNameInputField;

    void Start()
    {
        StartCoroutine(SetupLeaderboard());
    }

    IEnumerator SetupLeaderboard()
    {
        yield return LoginRoutine();
        yield return leaderboard.FetchTopHighscoresRoutine();
    }

    public void SetPlayerName()
    {
        if(playerNameInputField.text != "")
        {
            LootLockerSDKManager.SetPlayerName(playerNameInputField.text, (response) => {
            if(response.success)
            {
                Debug.Log("player name set");
            }
            else
            {
                Debug.Log("failed to set player name: " + response.Error);
            }
        });
        }
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) => 
        {
            if(response.success)
            {
                Debug.Log("Player has logged in.");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("failed to log in");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}

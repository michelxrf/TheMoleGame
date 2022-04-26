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
        yield return leaderboard.GetThisPlayerRank();
        yield return leaderboard.FetchTopHighscoresRoutine();
    }

    public void SetPlayerName()
    {
        Debug.Log("Entered Method");
        if(playerNameInputField.text != "")
        {
            Debug.Log("Entered If");
            LootLockerSDKManager.SetPlayerName(playerNameInputField.text, (response) => {
            if(!response.success)
            {
                Debug.Log("response not success");
                Debug.LogError("failed to set player name: " + response.Error);
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
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.LogError("failed to log in");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class BoxController : MonoBehaviour
{
    public Text tip;
    public Animator lidAnimation;
    public Animator boxAnimation;
    public GameObject lights;
    public AudioSource audioPlayer;

    private bool isInteractable = false;
    private bool isConsumed = false;

    public int boxedSilver = 0;
    public int boxedGold = 0;
    public int boxedEmerdald = 0;

    private void Start()
    {
        tip = GameObject.Find("TipText").GetComponent<Text>();
    }

    void Update()
    {
        tip.enabled = isInteractable;
        if(Input.GetKeyDown(KeyCode.E) && isInteractable)
        {
            StoreLoot();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "Player" && !isConsumed)
        {
            isInteractable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            isInteractable = false;
        }    
    }

    private void StoreLoot()
    {
        isConsumed = true;
        isInteractable = false;

        boxedSilver = GameData.silver;
        GameData.silver = 0;

        boxedGold = GameData.gold;
        GameData.gold = 0;

        boxedEmerdald = GameData.emerald;
        GameData.emerald = 0;

        lidAnimation.SetTrigger("close_trigger");
        audioPlayer.Play();
        Destroy(lights, 1f);
    }

    private void OnDestroy()
    {
        GameData.storedSilver += boxedSilver;
        GameData.gameOverScoreSilver += boxedSilver;

        GameData.storedGold += boxedGold;
        GameData.gameOverScoreGold += boxedGold;

        GameData.storedEmerald += boxedEmerdald;
        GameData.gameOverScoreEmerald += boxedEmerdald;

        if(isConsumed)
        {
            GameData.boxChance = 0f;
        }
    }
}

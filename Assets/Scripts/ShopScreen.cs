using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopScreen : MonoBehaviour
{
    public Text silver;
    public Text gold;
    public Text emerald;

    public Text health;
    public Text armor;
    public Text damage;
    public Text pickaxe;
    public Text speed;
    public Text lamp;

    private void Start()
    {
        Cursor.visible = true;
    }

    private void Awake()
    {
        ResetPlayerStats();
    }

    void Update()
    {
        silver.text = GameData.storedSilver.ToString() + "x ";
        gold.text = GameData.storedGold.ToString() + "x ";
        emerald.text = GameData.storedEmerald.ToString() + "x ";

        health.text = "x " + GameData.maxHealth.ToString();
        armor.text = "x " + GameData.armor.ToString();
        damage.text = "x " + GameData.damage.ToString();
        pickaxe.text = "x " + GameData.pickaxe.ToString();
        speed.text = "x " + GameData.speed.ToString();
        lamp.text = "x " + GameData.lamp.ToString();
    }

    public void StartGame()
    {
        GameData.level = 1;
        GameData.health = GameData.maxHealth;

        SaveSystem.SaveGame();
        SceneManager.LoadScene("Play");
    }

    private void ResetPlayerStats()
    {   
        GameData.health = 3;
        GameData.maxHealth = 3;
        GameData.armor = 0;
        GameData.speed = 3;
        GameData.damage = 1;
        GameData.pickaxe = 1;
        GameData.lamp = 1;
    }
}

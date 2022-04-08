using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    public Text level;
    public Text silver;
    public Text gold;
    public Text emerald;
    public Text health;
    public Text timer;
    public Text population;
    
    // Start is called before the first frame update
    void Update()
    {
       UpdateValues(); 
    }

    public void UpdateValues()
    {
        level.text = "Level: " + GameData.level.ToString();
        health.text = "Health: " + GameData.health.ToString() + "/" + GameData.maxHealth.ToString();
        silver.text = "Silver: " + GameData.silver.ToString();
        gold.text = "Gold: " + GameData.gold.ToString();
        emerald.text = "Emerald: " + GameData.emerald.ToString();
        timer.text = "spawn every: " + GameData.spawnTimer.ToString("F2") + " seconds";
        population.text = "Spiders: " + GameData.monsterPopulation.ToString() + "/" + GameData.maxMonsterPopulation.ToString();
    }
}

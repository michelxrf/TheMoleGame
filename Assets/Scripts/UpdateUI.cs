using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    public GameObject emptyHeartPrefab;
    public GameObject fullHeartPrefab;
    public GameObject shieldPrefab;
    public GameObject hud;

    private List<GameObject> heartList = new List<GameObject>();
    private List<GameObject> shieldList = new List<GameObject>();

    public Text level;
    public Text silver;
    public Text gold;
    public Text emerald;
    
    private void Start()
    {
        fillEmptyHearts();
        fillFullHearts();
        fillShields();
    }

    void Update()
    {
       UpdateValues(); 
    }

    public void UpdateValues()
    {
        level.text = "Level: " + GameData.level.ToString();
        silver.text = GameData.silver.ToString() + " x";
        gold.text = GameData.gold.ToString() + " x";
        emerald.text = GameData.emerald.ToString() + " x";

        for(int i = 0; i < heartList.Count; i++)
        {
            heartList[i].SetActive(i < GameData.health);
        }
        for(int i = 0; i < shieldList.Count; i++)
        {
            shieldList[i].SetActive(i < GameData.armor);
        }
    }

    private void fillEmptyHearts()
    {
        for(int i = 0; i < GameData.maxHealth; i++)
        {
            var thisHeart = Instantiate(emptyHeartPrefab, new Vector3(20 + 30 * i, 20, 0), Quaternion.identity, hud.transform);
        }
    }

    private void fillFullHearts()
    {
        for(int i = 0; i < GameData.maxHealth; i++)
        {
            var thisHeart = Instantiate(fullHeartPrefab, new Vector3(20 + 30 * i, 20, 0), Quaternion.identity, hud.transform);
            heartList.Add(thisHeart);
        }
    }

    private void fillShields()
    {
        for(int i = 0; i < GameData.armor; i++)
        {
            var thisShield = Instantiate(shieldPrefab, new Vector3(20 + 30 * (GameData.maxHealth + i), 20, 0), Quaternion.identity, hud.transform);
            shieldList.Add(thisShield);
        }
    }

}

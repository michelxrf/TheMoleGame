using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    public Text level;
    public Text money;
    
    // Start is called before the first frame update
    void Update()
    {
       UpdateValues(); 
    }

    public void UpdateValues()
    {
        level.text = "Level: " + GameData.level.ToString();
        money.text = "Money: " + GameData.money.ToString("F2");
    }
}

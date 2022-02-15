using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    public Text level;
    
    // Start is called before the first frame update
    void Start()
    {
       UpdateValues(); 
    }

    public void UpdateValues()
    {
        level.text = "Level: " + GameData.level.ToString();
    }
}

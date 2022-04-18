using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
    public int price;
    public string moneyType;
    public string stat;
    public float stat_increase;
    public string itemType;

    public Text priceTag;

    public Image soldMark;
    public Button button;
    public int id = -1;

    private bool paid = false;

    private void Start()
    {
        priceTag.text = price.ToString() + " x";
        
        ResetConsumbables();
    }

    private void Update()
    {
        CheckItemAvailability();
    }
    
    public void Buy()
    {
           switch (moneyType)
            {
                case "silver":
                    if((GameData.storedSilver - price) >= 0)
                    {
                        paid = true;
                        GameData.storedSilver -= price;
                    }
                    break;
                
                case "gold":
                    if((GameData.storedGold - price) >= 0)
                    {
                        paid = true;
                        GameData.storedGold -= price;
                    }
                    break;
                
                case "emerald":
                    if((GameData.storedEmerald - price) >= 0)
                    {
                        paid = true;
                        GameData.storedEmerald -= price;
                    }
                    break;

                default:
                    Debug.LogError("Valuable type not recognized.");
                    break;
            }

            if(paid)
            {
                switch (stat)
                {
                    case "health":
                        GameData.maxHealth += Mathf.FloorToInt(stat_increase);
                        break;

                    case "armor":
                        GameData.armor += Mathf.FloorToInt(stat_increase);
                        break;

                    case "speed":
                        GameData.speed += stat_increase;
                        break;

                    case "damage":
                        GameData.damage += Mathf.FloorToInt(stat_increase);
                        break;

                    case "pickaxe":
                        GameData.pickaxe += Mathf.FloorToInt(stat_increase);
                        break;

                    case "lamp":
                        GameData.pickaxe += Mathf.FloorToInt(stat_increase);
                        break;

                    case "skin":
                        //TODO: unlock skin
                        break;

                    default:
                        Debug.LogError("Stat type not recognized.");
                        break;
                }

                switch (itemType)
                {
                    case "consumable":
                        GameData.consumables_bought[id] = true;
                        break;
                    
                    case "upgrade":
                        GameData.upgrades_bought[id] = true;
                        break;
                    
                    case "skin":
                        GameData.skins_bought[id] = true;
                        break;
                    
                    default:
                        Debug.LogError("Item type not recognized.");
                        break;
                }

                button.interactable = false;
            }
            else
            {
                Debug.Log("not enough money");
                //TODO: not enough money
            }
    }

    public void CheckItemAvailability()
    {
        switch (itemType)
                {
                    case "consumable":
                        button.interactable = !GameData.consumables_bought[id];
                        soldMark.enabled = GameData.consumables_bought[id];
                        break;
                    
                    case "upgrade":
                        button.interactable = !GameData.upgrades_bought[id];
                        soldMark.enabled = GameData.upgrades_bought[id];
                        break;
                    
                    case "skin":
                        button.interactable = !GameData.skins_bought[id];
                        soldMark.enabled = GameData.skins_bought[id];
                        break;
                    
                    default:
                        Debug.LogError("Couldn't check item availabilty. Check if id is correctly set.");
                        break;
                }
    }

    public void ResetConsumbables()
    {
        if(itemType == "consumable")
        {
            GameData.consumables_bought[id] = false;
        }
        else if(GameData.upgrades_bought[id])
        {
            switch (stat)
                {
                    case "health":
                        GameData.maxHealth += Mathf.FloorToInt(stat_increase);
                        break;

                    case "armor":
                        GameData.armor += Mathf.FloorToInt(stat_increase);
                        break;

                    case "speed":
                        GameData.speed += stat_increase;
                        break;

                    case "damage":
                        GameData.damage += Mathf.FloorToInt(stat_increase);
                        break;

                    case "pickaxe":
                        GameData.pickaxe += Mathf.FloorToInt(stat_increase);
                        break;

                    case "lamp":
                        GameData.lamp += Mathf.FloorToInt(stat_increase);
                        break;

                    case "skin":
                        //TODO: unlock skin
                        break;

                    default:
                        Debug.LogError("Stat type not recognized.");
                        break;
                }
        }
    }
}

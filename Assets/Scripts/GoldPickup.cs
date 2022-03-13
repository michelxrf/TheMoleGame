using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    // Start is called before the first frame update
    public string type;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.name == "Player")
        {
            switch (type)
            {
                case "silver":
                    GameData.silver += 1;
                    break;
                
                case "gold":
                    GameData.gold += 1;
                    break;
                
                case "emerald":
                    GameData.emerald += 1;
                    break;

                default:
                    Debug.LogError("Valuable type not recognized.");
                    break;
            }
            Destroy(gameObject);
        }
    }
}

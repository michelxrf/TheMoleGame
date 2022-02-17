using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    // Start is called before the first frame update
    public float value;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.name == "Player")
        {
            GameData.money += value;
            Destroy(gameObject);
        }
    }
}

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
            other.GetComponent<PlayerController>().PickupCoin(type);
            Destroy(gameObject);
        }
    }
}

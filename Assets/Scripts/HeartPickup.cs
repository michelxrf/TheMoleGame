using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    // Start is called before the first frame update
    public int amount;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.name == "Player")
        {
            other.GetComponent<PlayerController>().PickupHeart(amount);            
            Destroy(gameObject);
        }
    }
}

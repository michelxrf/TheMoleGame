using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGold : MonoBehaviour
{
    public GameObject spawnThis;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    public void Hit() 
    {
        int quantity = Random.Range(1, 5);
        for(int i = 0; i < quantity; i++)
        {
            float randomOffsetX = Random.Range(-.4f, .4f);
            float randomOffsetY = Random.Range(-.4f, .4f);
            float randomOffsetZ = Random.Range(-.4f, .4f);

            var myPrefab = Instantiate(spawnThis, new Vector3(transform.position.x + randomOffsetX, transform.position.y + randomOffsetY, transform.position.z + randomOffsetZ), Quaternion.identity);
            myPrefab.transform.LookAt(new Vector3(0, 50, 0));

            myPrefab.GetComponent<Rigidbody>().AddForce(new Vector3(20*randomOffsetX, 20*randomOffsetY, 20*randomOffsetZ));
        }
        
    }
}

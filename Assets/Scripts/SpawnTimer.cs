using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTimer : MonoBehaviour
{
    private float timerTimeRemaining;
    public bool timerActive = true;
    private float timerSet = 30f;
    private float timerDecay = 0.9f;
    private int mapLevel;
    private int monsterTickets;
    
    // Start is called before the first frame update
    void Start()
    {
        timerTimeRemaining = timerSet;

        mapLevel = GameData.level;
        monsterTickets = Random.Range(Mathf.FloorToInt(mapLevel/10), Mathf.FloorToInt(mapLevel/5));
        Debug.Log("map tickets: " + monsterTickets);
    }

    // Update is called once per frame
    void Update()
    {
        TimedSpawn();
        GameData.spawnTimer = timerTimeRemaining;
    }

    void TimedSpawn()
    {
        if(timerActive)
        timerTimeRemaining -= Time.deltaTime;

        if(timerTimeRemaining < 0)
        {
            DecayTimer();
            timerTimeRemaining = timerSet;
            
            CallSpawners();
        }
    }

    void DecayTimer()
    {
        if(timerSet > 15f)
        {
            timerSet = timerSet * timerDecay;
        }
    }

    void CallSpawners()
    {
        int spawnedMonsters = 0;

        int spawnerCount = transform.childCount;
        for(int i = 0; i < spawnerCount; i++)
        {
            if(spawnedMonsters >= monsterTickets)
            {
                Debug.Log("timed Spawn: " + spawnedMonsters);
                break;
            }
            if(transform.GetChild(i).GetComponent<MonsterSpawner>().SpawnIfHidden())
            {
                spawnedMonsters++;
            } 
        }     
    }

    public void PopulateMap(Transform playerTransform)
    {
        int spawnerCount = transform.childCount;
        for(int i = 0; i < spawnerCount; i++)
        {
            float distance = Vector3.Distance(playerTransform.position, transform.GetChild(i).position);
            Debug.Log("player to spawner distance: " + distance);

            if(distance > 8f)
            {
                transform.GetChild(i).GetComponent<MonsterSpawner>().ForcedSpawn();
            } 
        }     
    }
}

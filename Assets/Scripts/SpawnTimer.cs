using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTimer : MonoBehaviour
{
    private float timerTimeRemaining;
    public bool timerActive = true;
    public float timerSet = 5f;
    public float timerDecay = 0.9f;
    private int monsterTickets = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        timerTimeRemaining = timerSet;
    }

    // Update is called once per frame
    void Update()
    {
        TimedSpawn();
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
        if(timerSet > 60f)
        {
            timerSet = timerSet * timerDecay;
        }
    }

    void CallSpawners()
    {
        // TODO: goes through the list of spawners and call Spawn() on each
        // every time they answer true decrease one monster ticket
        // do this until all spawners have been called or run out of tickets
        int spawnerCount = transform.childCount;
        if(monsterTickets > 0)
        {
            for(int i = 0; i < spawnerCount; i++)
            {
                if(transform.GetChild(i).GetComponent<MonsterSpawner>().Spawn())
                {
                    monsterTickets--;
                }
                if(monsterTickets < 1)
                {
                    break;
                }
            }
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTimer : MonoBehaviour
{
    private float spawnRate;
    private Coroutine timer;
    private int spawnerCount;
    private int spawnerToken = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        spawnRate = Mathf.Clamp(-GameData.level + 20, 1, 15);

        timer = StartCoroutine(TimedSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        GameData.spawnTimer = spawnRate;
    }

    IEnumerator TimedSpawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnRate);
            CallSpawners();
        }
    }

    void CallSpawners()
    {
        if(GameData.monsterPopulation < GameData.maxMonsterPopulation)
        {
            for(int i = 0; i < spawnerCount; i++)
            {
                if(transform.GetChild(spawnerToken).GetComponent<MonsterSpawner>().SpawnIfHidden())
                {
                    break;
                }
                else
                {
                    spawnerToken++;
                }
            }   
        }
    }

    public void PopulateMap(Transform playerTransform)
    {
        GameData.monsterPopulation = 0;
        spawnerCount = transform.childCount;

        for(int i = 0; i < spawnerCount; i++)
        {
            float distance = Vector3.Distance(playerTransform.position, transform.GetChild(i).position);

            if(GameData.monsterPopulation >= GameData.maxMonsterPopulation)
            {
                break;
            }
            else if(distance > 8f)
            {
                transform.GetChild(i).GetComponent<MonsterSpawner>().ForcedSpawn();
            }             
        }     
    }
}

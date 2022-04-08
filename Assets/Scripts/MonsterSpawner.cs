using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    Renderer m_Renderer;

    public GameObject monster;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    public bool SpawnIfHidden()
    {
        if(!m_Renderer.isVisible)
        {
            var newMonster = Instantiate(monster, new Vector3(transform.position.x, .5f, transform.position.z), Quaternion.identity);
            GameData.monsterPopulation++;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ForcedSpawn()
    {
        var newMonster = Instantiate(monster, new Vector3(transform.position.x, .5f, transform.position.z), Quaternion.identity);
        GameData.monsterPopulation++;
    }
}

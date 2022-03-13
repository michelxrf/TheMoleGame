using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
	public static int level = 0;
    public static int highestLevel = 0;

    public static int killedMonsters = 0;
    
    public static int silver = 0;
    public static int gold = 0;
    public static int emerald = 0;

    public static int health = 3;
    public static int maxHealth = 3;

    public static bool gameIsPaused = false;

    // DEBUG
    public static float spawnTimer = 0;
}

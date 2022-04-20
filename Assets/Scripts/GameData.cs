using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static int level = 0;
    public static int highestLevel = 0;
    public static float boxChance = -50f;
    
    public static int silver = 0;
    public static int gold = 0;
    public static int emerald = 0;

    public static int gameOverScoreSilver = 0;
    public static int gameOverScoreGold = 0;
    public static int gameOverScoreEmerald = 0;
    public static int killedMonsters = 0;

    public static int killedMonstersThisLevel = 0;

    public static int storedSilver = 0;
    public static int storedGold = 0;
    public static int storedEmerald = 0;

    public static int health = 3;
    public static int maxHealth = 3;
    public static int armor = 0;
    public static float speed = 3;
    public static int damage = 1;
    public static int pickaxe = 1;
    public static int lamp = 1;

    // Shop arrays
    public static bool[] consumables_bought = new bool[8];
            /*
            - [ ]  Health 1
            - [ ]  Health 2
            - [ ]  Armor 1
            - [ ]  Armor 2
            - [ ]  Damage 1
            - [ ]  Lamp 1
            - [ ]  Speed 1
            - [ ]  Speed 2
            */
    public static bool[] upgrades_bought = new bool[8];
            /*
            - [ ]  Pickaxe 1
            - [ ]  Pickaxe 2
            - [ ]  Armor
            - [ ]  Health 1
            - [ ]  Health 2
            - [ ]  Speed 1
            - [ ]  Speed 2
            - [ ]  Lamp
            */

    public static bool gameIsPaused = false;
    public static int monsterPopulation;
    public static int maxMonsterPopulation;

    public static bool leaveMines = false;

    public static float ambienceSoundTime = 0f;
}

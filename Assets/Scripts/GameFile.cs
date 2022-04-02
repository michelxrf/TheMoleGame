using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class GameFile
{
    public int highestLevel;
    public int lastLevel;
    public float boxChance;
    
    public int silver;
    public int gold;
    public int emerald;

    public int gameOverScoreSilver;
    public int gameOverScoreGold;
    public int gameOverScoreEmerald;
    public int killedMonsters;

    public int storedSilver;
    public int storedGold;
    public int storedEmerald;

    public int health;
    public int maxHealth;
    public int armor;
    public float speed;
    public int damage;
    public int pickaxe;
    public int lamp;

    // Shop arrays
    public bool[] consumables_bought = new bool[8];
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
    public bool[] upgrades_bought = new bool[8];
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
    public bool[] skins_bought = new bool[2];
            /*
            - [ ]  Skin 1
            - [ ]  Skin 2
            */
}

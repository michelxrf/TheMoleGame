using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// path
// https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html

public static class SaveSystem
{
    public static void SaveGame()
    {
        GameFile saveFile = new GameFile();

        // variables getting stored //
        saveFile.highestLevel = GameData.highestLevel;
        saveFile.lastLevel = GameData.level;
        saveFile.boxChance = GameData.boxChance;
        
        saveFile.silver = GameData.silver;
        saveFile.gold = GameData.gold;
        saveFile.emerald = GameData.emerald;

        saveFile.gameOverScoreSilver = GameData.gameOverScoreSilver;
        saveFile.gameOverScoreGold = GameData.gameOverScoreGold;
        saveFile.gameOverScoreEmerald = GameData.gameOverScoreEmerald;

        saveFile.storedSilver = GameData.storedSilver;
        saveFile.storedGold = GameData.storedGold;
        saveFile.storedEmerald = GameData.storedEmerald;
        saveFile.killedMonsters = GameData.killedMonsters;

        saveFile.health = GameData.health;
        saveFile.maxHealth = GameData.maxHealth;
        saveFile.armor = GameData.armor;
        saveFile.speed = GameData.speed;
        saveFile.damage = GameData.damage;
        saveFile.pickaxe = GameData.pickaxe;
        saveFile.lamp = GameData.lamp;

        for (int i = 0; i < GameData.consumables_bought.Length; i++)
        {
            saveFile.consumables_bought[i] = GameData.consumables_bought[i];
        }
        for (int i = 0; i < GameData.upgrades_bought.Length; i++)
        {
            saveFile.upgrades_bought[i] = GameData.upgrades_bought[i];
        }
        for (int i = 0; i < GameData.skins_bought.Length; i++)
        {
            saveFile.skins_bought[i] = GameData.skins_bought[i];
        }
        //////////////////////////////

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savegame.bin";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, saveFile);
        stream.Close();
    }

    public static bool LoadGame()
    {
        string path = Application.persistentDataPath + "/savegame.bin";

        if(File.Exists(path))
        {
            FileStream stream = null;

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                stream = new FileStream(path, FileMode.Open);

                GameFile saveFile = formatter.Deserialize(stream) as GameFile;
                stream.Close();

                // variables getting recovered //
                GameData.level = saveFile.lastLevel;
                GameData.highestLevel = saveFile.highestLevel;
                GameData.boxChance = saveFile.boxChance;
                
                GameData.silver = saveFile.silver;
                GameData.gold = saveFile.gold;
                GameData.emerald = saveFile.emerald;

                GameData.gameOverScoreSilver = saveFile.gameOverScoreSilver;
                GameData.gameOverScoreGold = saveFile.gameOverScoreGold;
                GameData.gameOverScoreEmerald = saveFile.gameOverScoreEmerald;
                GameData.killedMonsters = saveFile.killedMonsters;

                GameData.storedSilver = saveFile.storedSilver;
                GameData.storedGold = saveFile.storedGold;
                GameData.storedEmerald = saveFile.storedEmerald;

                GameData.health = saveFile.health;
                GameData.maxHealth = saveFile.maxHealth;
                GameData.armor = saveFile.armor;
                GameData.speed = saveFile.speed;
                GameData.damage = saveFile.damage;
                GameData.pickaxe = saveFile.pickaxe;
                GameData.lamp = saveFile.lamp;

                for (int i = 0; i < saveFile.consumables_bought.Length; i++)
                {
                    GameData.consumables_bought[i] = saveFile.consumables_bought[i];
                }
                for (int i = 0; i < saveFile.upgrades_bought.Length; i++)
                {
                    GameData.upgrades_bought[i] = saveFile.upgrades_bought[i];
                }
                for (int i = 0; i < saveFile.skins_bought.Length; i++)
                {
                    GameData.skins_bought[i] = saveFile.skins_bought[i];
                }
                /////////////////////////////////

                return true;
            }
            catch (System.Exception)
            {
                Debug.Log("Save file corrupted.");
                if(stream != null)
                stream.Close();

                File.Delete(path);
                Debug.Log("Save file deleted.");
                SaveGame();
                Debug.Log("new save file created.");
            }
            return false;
            
        }
        else
        {
            Debug.Log("There's no savefile. Path: " + path);
            SaveGame();
            Debug.Log("new save file created.");
            return false;
        }
        
    }
}


using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGame()
    {
        GameFile saveFile = new GameFile();

        saveFile.highestLevel = GameData.highestLevel;
        saveFile.lastLevel = GameData.level;

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
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameFile saveFile = formatter.Deserialize(stream) as GameFile;
            stream.Close();

            GameData.level = saveFile.lastLevel;
            GameData.highestLevel = saveFile.highestLevel;

            return true;
        }
        else
        {
            Debug.Log("There's no savefile. Path: " + path);
            return false;
        }
    }
}


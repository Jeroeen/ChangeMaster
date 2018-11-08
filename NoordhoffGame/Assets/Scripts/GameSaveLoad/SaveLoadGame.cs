using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using Assets.Scripts.GameSaveLoad.Player;

public static class SaveLoadGame
{
    public static Game SavedGame;

    public static void Save()
    {
        SavedGame = Game.GetGame();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/SavedGame.gd");
        bf.Serialize(file, Game.GetGame());
        file.Close();
    }

    public static bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/SavedGame.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SavedGame.gd", FileMode.Open);
            Game.setGame((Game)bf.Deserialize(file));
            file.Close();
            return true;
        }
        else
        {
            return false;
        }
    }
}



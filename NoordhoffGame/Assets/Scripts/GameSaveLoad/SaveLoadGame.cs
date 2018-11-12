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
            
            Player.GetPlayer().Enthousiasm = Game.GetGame().player.Enthousiasm;
            Player.GetPlayer().Analytic = Game.GetGame().player.Analytic;
            Player.GetPlayer().ChangeKnowledge = Game.GetGame().player.ChangeKnowledge;
            Player.GetPlayer().Convincing = Game.GetGame().player.Convincing;
            Player.GetPlayer().Creative = Game.GetGame().player.Creative;
            Player.GetPlayer().Empathic = Game.GetGame().player.Empathic;
            Player.GetPlayer().Decisive = Game.GetGame().player.Decisive;
            Player.GetPlayer().Coins = Game.GetGame().player.Coins;
            Player.GetPlayer().Name = PlayerPrefs.GetString("PlayerName");
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(Application.persistentDataPath + "/SavedGame.gd"))
        {
            File.Delete(Application.persistentDataPath + "/SavedGame.gd");
        }
    }
}



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
            Game.SetGame((Game)bf.Deserialize(file));
            file.Close();
            Player player = Player.GetPlayer();
            Game game = Game.GetGame();

            player.Enthousiasm = game.Player.Enthousiasm;
            player.Analytic = game.Player.Analytic;
            player.ChangeKnowledge = game.Player.ChangeKnowledge;
            player.Convincing = game.Player.Convincing;
            player.Creative = game.Player.Creative;
            player.Empathic = game.Player.Empathic;
            player.Decisive = game.Player.Decisive;
            player.Coins = game.Player.Coins;
            player.Name = PlayerPrefs.GetString("PlayerName");
            return true;
        }
            return false;
    }

    public static void DeleteSave()
    {
        if (File.Exists(Application.persistentDataPath + "/SavedGame.gd"))
        {
            File.Delete(Application.persistentDataPath + "/SavedGame.gd");
        }
    }
}



using Assets.Scripts.GameSaveLoad.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Game
{
    public static Game CurrentGame {  set; private get; }
    public Player Player { get; set; }
    public InfoList Information { get; set; }
    public int LastLevelFinished { get; set; }
    public Dictionary<string, bool> DialogueRead { get; set; }

    private Game()
    {
        Player = Player.GetPlayer();
        LastLevelFinished = -1;
        DialogueRead = new Dictionary<string, bool>();
    }

    public void AddLevel()
    {
        LastLevelFinished++;
        Information = null;
        DialogueRead = new Dictionary<string, bool>();
    }

    public static void SetGame(Game game)
    {
        CurrentGame = game;
    }

    public static Game GetGame()
    {
        // if currentGame is null, make a new Game and return it. Otherwise return the existing Game
        return CurrentGame ?? (CurrentGame = new Game());
    }

    public static void ClearGame()
    {
        CurrentGame = new Game();
    }
}
using Assets.Scripts.GameSaveLoad.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Game
{
    public static Game currentGame {  set; private get; }
    public Player Player { get; set; }
    public InfoList Information { get; set; }
    public int lastLevelfinished { get; set; }
    public Dictionary<string, bool> DialogueRead { get; set; }

    private Game()
    {
        Player = Player.GetPlayer();
        lastLevelfinished = -1;
        DialogueRead = new Dictionary<string, bool>();
    }

    public void AddLevel()
    {
        lastLevelfinished++;
        Information = null;
        DialogueRead = new Dictionary<string, bool>();
    }

    public static void SetGame(Game game)
    {
        currentGame = game;
    }

    public static Game GetGame()
    {
        // if currentGame is null, make a new Game and return it. Otherwise return the existing Game
        return currentGame ?? (currentGame = new Game());
    }

    public static void ClearGame()
    {
        currentGame = new Game();
    }
}
using Assets.Scripts.GameSaveLoad.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Game
{
    public static Game currentGame {  set; private get; }

    public Player player; 
    public InfoList information;
    public int levelfinishedlast;
    public Dictionary<string, bool> dialogueRead;

    public void addlevel()
    {
        levelfinishedlast++;
        information = null;
        dialogueRead = new Dictionary<string, bool>();
    }

    private Game()
    {
        player = Player.GetPlayer();
        levelfinishedlast = -1;
        dialogueRead = new Dictionary<string, bool>();
    }
    public static void setGame(Game game)
    {
        currentGame = game;
    }
    public static Game GetGame()
    {
        // if currentGame is null, make a new Game and return it. Otherwise return the existing Game
        return currentGame ?? (currentGame = new Game());
    }
    public static void clearGame()
    {
        currentGame = new Game();
    }
}
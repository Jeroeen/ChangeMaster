using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Game
{
    private static Game currentGame;

    public static int test = 0;
    public InfoList information;
    public int levelfinishedlast;


    private Game()
    {
        RetrieveJson json = new RetrieveJson();
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
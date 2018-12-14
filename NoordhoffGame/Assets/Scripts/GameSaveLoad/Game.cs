﻿using System.Collections.Generic;
using Assets.Scripts.UI;
using Assets.Scripts.Utility;

namespace Assets.Scripts.GameSaveLoad
{
	[System.Serializable]
	public class Game
	{
		public static Game CurrentGame {  set; private get; }
		public Player Player { get; set; }
		public InfoList Information { get; set; }
		public int LastFinishedLevel { get; set; }
		public int CurrentLevelIndex => InLevel ? LastFinishedLevel : -1;
		public int CurrentLevelNumber => CurrentLevelIndex - GlobalVariablesHelper.BASE_LEVEL_INDEX;
		public Dictionary<string, bool> DialogueRead { get; set; }
		public string CurrentDestination { get; set; }
		public string CurrentLocation { get; set; }
		public bool InLevel { get; set; }

		private Game()
		{
			Player = Player.GetPlayer();
			// This must be changed to 2 once level 0 is implemented
			// Base is 3 because of the StageChooser-Opening Cutscene-Character Creation-Bridge
			LastFinishedLevel = GlobalVariablesHelper.BASE_LEVEL_INDEX;
			DialogueRead = new Dictionary<string, bool>();
		}

		public void AddLevel()
		{
			InLevel = false;
			CurrentLocation = CurrentDestination;
			CurrentDestination = null;
			LastFinishedLevel++;
			Information = null;
			DialogueRead = new Dictionary<string, bool>();
			SaveLoadGame.Save();
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
}
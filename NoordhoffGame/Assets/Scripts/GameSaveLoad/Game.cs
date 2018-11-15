using System.Collections.Generic;

namespace Assets.Scripts.GameSaveLoad
{
	[System.Serializable]
	public class Game
	{
		public static Game CurrentGame {  set; private get; }
		public Player.Player Player { get; set; }
		public InfoList Information { get; set; }
		public int LastFinishedLevel { get; set; }
		public Dictionary<string, bool> DialogueRead { get; set; }
		public string CurrentDestination { get; set; }
		public string CurrentLocation { get; set; }

		private Game()
		{
			Player = GameSaveLoad.Player.Player.GetPlayer();
			// This must be changed to 2 once level 0 is implemented
			// Base is 4 because of the StageChooser-Opening Cutscene-Character Creation-Bridge
			LastFinishedLevel = 3;
			DialogueRead = new Dictionary<string, bool>();
		}

		public void AddLevel()
		{
			CurrentLocation = CurrentDestination;
			CurrentDestination = null;
			LastFinishedLevel++;
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
}
using System.Collections.Generic;

namespace Assets.Scripts.GameSaveLoad
{
	[System.Serializable]
	public class Game
	{
		public static Game CurrentGame {  set; private get; }
		public Player.Player Player { get; set; }
		public InfoList Information { get; set; }
		public int LastLevelFinished { get; set; }
		public Dictionary<string, bool> DialogueRead { get; set; }
		public string CurrentDestination { get; set; }
		public string CurrentLocation { get; set; }

		private Game()
		{
			Player = GameSaveLoad.Player.Player.GetPlayer();
			LastLevelFinished = -1;
			DialogueRead = new Dictionary<string, bool>();
		}

		public void AddLevel()
		{
			CurrentLocation = CurrentDestination;
			CurrentDestination = null;
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
}
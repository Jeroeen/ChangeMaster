using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.Progress
{
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

				player.Approach = game.Player.Approach;
				player.Analytic = game.Player.Analytic;
				player.Communication = game.Player.Communication;
				player.Facilitating = game.Player.Facilitating;
				player.Ownership = game.Player.Ownership;
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
}



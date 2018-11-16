using Assets.Scripts.GameSaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class PlayerScript : MonoBehaviour
	{
		public Text CoinAmount;
		private Player player;
	
		// Start is called before the first frame update
		void Start()
		{
			player = Player.GetPlayer();
			CoinAmount.text = player.Coins.ToString();
		}

		public void AddCoin()
		{
			CoinAmount.text = player.AddCoin().ToString();
			Game.GetGame().Player.AddCoin();
		}

		public void AddCoins(int amount)
		{
			CoinAmount.text = player.AddCoins(amount).ToString();
		}
	}
}

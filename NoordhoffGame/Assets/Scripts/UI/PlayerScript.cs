using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameSaveLoad.Player;
using UnityEngine;
using UnityEngine.UI;

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
	}

	public void AddCoins(int amount)
	{
		CoinAmount.text = player.AddCoins(amount).ToString();
	}
}

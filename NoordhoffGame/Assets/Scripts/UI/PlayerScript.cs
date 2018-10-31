using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameSaveLoad.Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
	public Text CoinAmount;
	private Player _player;
	
	// Start is called before the first frame update
	void Start()
	{
		_player = Player.GetPlayer();
		CoinAmount.text = _player.Coins.ToString();
	}

	public void AddCoin()
	{
		CoinAmount.text = _player.AddCoin().ToString();
	}

	public void AddCoins(int amount)
	{
		CoinAmount.text = _player.AddCoins(amount).ToString();
	}
}

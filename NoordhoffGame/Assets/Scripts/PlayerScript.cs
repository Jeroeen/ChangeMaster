using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Text CoinAmount;
    public int Coins = 0;

    // Start is called before the first frame update
    void Start()
    {
        CoinAmount.text = Coins.ToString();     
    }
    public void AddCoin()
    {
        Coins++;
        CoinAmount.text = Coins.ToString();
    }
    public void AddCoins(int amount)
    {
        Coins += amount;
        CoinAmount.text = Coins.ToString();
    }
}

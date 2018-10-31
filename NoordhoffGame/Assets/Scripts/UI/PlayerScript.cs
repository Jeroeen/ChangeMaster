using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Text CoinAmount;
    public static int Coins = 0;
    public static int Analytic = 0;
    public static int Enthusiasm = 0;
    public static int Decisive = 0;
    public static int Empatic = 0;
    public static int Convincing = 0;
    public static int Creative = 0;
    public static int ChangeKnowledge = 0;

    public string naam = "I.J.S. Beer";

    public void AddSkill()
    {
        ChangeKnowledge++;
    }

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
    public string GetPlayerTitle()
    {
        if(ChangeKnowledge >= 1)
        {
            return "veranderkundige";
        }
        else
        {
            return "junior veranderkundige";
        }
    }
}

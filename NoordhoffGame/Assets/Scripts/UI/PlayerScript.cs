using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Text CoinAmount;
    public static int Coins = 0;
    public static int Analytisch = 0;
    public static int Enthousiasmerend = 0;
    public static int Besluitvaardig = 0;
    public static int Empathisch = 0;
    public static int Overtuigend = 0;
    public static int Creatief = 0;
    public static int Veranderkunde_Kennis = 0;

    public string naam = "I.J.S. Beer";

    public void AddSkill()
    {
        Veranderkunde_Kennis++;
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
        if(Veranderkunde_Kennis >= 1)
        {
            return "veranderkundige";
        }
        else
        {
            return "junior veranderkundige";
        }
    }
}

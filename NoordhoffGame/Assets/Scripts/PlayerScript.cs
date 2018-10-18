using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Text coinAmount;
    public int coins = 0;

    // Start is called before the first frame update
    void Start()
    {
        coinAmount.text = coins.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addCoin()
    {
        coins++;
        coinAmount.text = coins.ToString();
    }

    public void addCoins(int amount)
    {
        coins += amount;
        coinAmount.text = coins.ToString();
    }
}

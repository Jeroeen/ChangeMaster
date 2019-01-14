using Assets.Scripts.Dialogue;
using Assets.Scripts.Progress;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class PlayerScript : MonoBehaviour
	{
	    private Player player;
	    [SerializeField] private SpriteRenderer playerSprite = null;
	    [SerializeField] private Image image = null;

        public Text CoinAmount;
	
		// Start is called before the first frame update
		void Start()
		{
			player = Player.GetPlayer();
		    RetrieveAsset.RetrieveAssets();
            playerSprite.sprite = RetrieveAsset.GetSpriteByName(PlayerPrefs.GetString(GlobalVariablesHelper.CHARACTER_NAME_PLAYERPREFS));
		    if (image != null)
		    {
		        image.sprite = playerSprite.sprite;
		    }

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

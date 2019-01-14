using UnityEngine;

namespace Assets.Scripts.Progress
{
    [System.Serializable]
	public class Player
	{
		private static Player player;

		private string name;


		#region Getter and setter of name. Every time a different value is set, the property is saved.
		public string Name 
		{
			get => name;
			set
			{
				name = value;
                PlayerPrefs.SetString("PlayerName", value);
            }
		}
        #endregion
        public int Coins { get; set; }

        public int Analytic { get; set; }
        public int Approach { get; set; }
        public int Ownership { get; set; }
        public int Facilitating { get; set; }
        public int Communication { get; set; }
        


        void Start()
        {
            name = PlayerPrefs.GetString("PlayerName");

        }

		private Player()
		{
			
		}

		public static Player GetPlayer()
		{
			// if player is null, make a new player and return it. Otherwise return the existing player
			return player ?? (player = new Player());
		}

		public int AddCoin()
		{
			return ++Coins;
		}

		public int AddCoins(int amount)
		{
            Coins += amount;
			return Coins;
		}

		public string GetPlayerTitle()
		{
			switch (Analytic)
			{
				case 0: return "Junior veranderkundige";
				case 1: return "Adviseur";
				case 2: return "Mentor";
				case 3: return "Coach";
				case 4: return "Teamcoach";
				case 5: return "Herstructureerder";
				case 6: return "Herorganisator";
				case 7: return "Organisatiecoach";
				case 8: return "Cultuurveranderaar";

				default: return "Changemaster";
			}
		}
	}
}


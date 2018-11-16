using UnityEngine;

namespace Assets.Scripts.GameSaveLoad
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
        public int Enthousiasm { get; set; }
        public int Decisive { get; set; }
        public int Empathic { get; set; }
        public int Convincing { get; set; }
        public int Creative { get; set; }
        public int ChangeKnowledge { get; set; }


        void start()
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
			switch (ChangeKnowledge)
			{
				case 0: return "Rookie";
				case 1: return "Junior veranderkundige";
				case 2: return "Adviseur";
				case 3: return "Mentor";
				case 4: return "Coach";
				case 5: return "Teamcoach";
				case 6: return "Herstructureerder";
				case 7: return "Herorganisator";
				case 8: return "Organisatiecoach";
				case 9: return "Cultuurveranderaar";

				default: return "Changemaster";
			}
		}
	}
}


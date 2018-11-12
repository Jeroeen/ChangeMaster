using UnityEngine;

namespace Assets.Scripts.GameSaveLoad.Player
{
    [System.Serializable]
	public class Player
	{
		private static Player _player;

		private string _name;
		private int _coins;
		private int _analytic;
		private int _enthusiasm;
		private int _decisive;
		private int _empathic;
		private int _convincing;
		private int _creative;
		private int _changeKnowledge;

		#region Getters and setters of skills. Every time a different value is set, the property is saved.
		public string Name
		{
			get => _name;
			set
			{
				_name = value;
			}
		}

		public int Coins
		{
			get => _coins;
			set
			{
				_coins = value;
			}
		}

		public int Analytic
		{
			get => _analytic;
			set
			{
				_analytic = value;
            }
        }

		public int Enthousiasm
		{
			get => _enthusiasm;
			set
			{
				_enthusiasm = value;

            }
        }

		public int Decisive
		{
			get => _decisive;
			set
			{
				_decisive = value;

            }
        }

		public int Empathic
		{
			get => _empathic;
			set
			{
				_empathic = value;

            }
        }

		public int Convincing
		{
			get => _convincing;
			set
			{
				_convincing = value;

            }
        }

		public int Creative
		{
			get => _creative;
			set
			{
				_creative = value;

            }
        }

		public int ChangeKnowledge
		{
			get => _changeKnowledge;
			set
			{
				_changeKnowledge = value;

            }
        }
		#endregion

        void start()
        {
            _name = PlayerPrefs.GetString("PlayerName");

        }

		private Player()
		{
			
		}

		public static Player GetPlayer()
		{
			// if player is null, make a new player and return it. Otherwise return the existing player
			return _player ?? (_player = new Player());
		}

		public int AddCoin()
		{
			return ++_coins;
		}

		public int AddCoins(int amount)
		{
			_coins += amount;
			return _coins;
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


using UnityEngine;

namespace Assets.Scripts.GameSaveLoad.Player
{
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
				PlayerPrefs.SetString("PlayerName", value);
			}
		}

		public int Coins
		{
			get => _coins;
			set
			{
				_coins = value;
				PlayerPrefs.SetInt("PlayerCoins", value);
			}
		}

		public int Analytic
		{
			get => _analytic;
			set
			{
				_analytic = value;
				PlayerPrefs.SetInt("SkillAnalytic", value);
			}
		}

		public int Enthousiasm
		{
			get => _enthusiasm;
			set
			{
				_enthusiasm = value;
				PlayerPrefs.SetInt("SkillEnthusiasm", value);
			}
		}

		public int Decisive
		{
			get => _decisive;
			set
			{
				_decisive = value;
				PlayerPrefs.SetInt("SkillDecisive", value);
			}
		}

		public int Empathic
		{
			get => _empathic;
			set
			{
				_empathic = value;
				PlayerPrefs.SetInt("SkillEmpathic", value);
			}
		}

		public int Convincing
		{
			get => _convincing;
			set
			{
				_convincing = value;
				PlayerPrefs.SetInt("SkillConvincing", value);
			}
		}

		public int Creative
		{
			get => _creative;
			set
			{
				_creative = value;
				PlayerPrefs.SetInt("SkillCreative", value);
			}
		}

		public int ChangeKnowledge
		{
			get => _changeKnowledge;
			set
			{
				_changeKnowledge = value;
				PlayerPrefs.SetInt("SkillChangeKnowledge", value);
			}
		}
		#endregion

		private Player()
		{
			_name = PlayerPrefs.GetString("PlayerName");

			_coins = PlayerPrefs.GetInt("PlayerCoins");
			_analytic = PlayerPrefs.GetInt("SkillAnalytic");
			_enthusiasm = PlayerPrefs.GetInt("SkillEnthusiasm");
			_decisive = PlayerPrefs.GetInt("SkillDecisive");
			_empathic = PlayerPrefs.GetInt("SkillEmpathic");
			_convincing = PlayerPrefs.GetInt("SkillConvincing");
			_creative = PlayerPrefs.GetInt("SkillCreative");
			_changeKnowledge = PlayerPrefs.GetInt("SkillChangeKnowledge");
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


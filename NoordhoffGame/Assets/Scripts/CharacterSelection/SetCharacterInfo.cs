using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.GameSaveLoad
{
	public class SetCharacterInfo : MonoBehaviour
	{
		[SerializeField] private InputField field = null;
		[SerializeField] private Text errorMessage = null;
		private string selectedCharacter;

		public void CreateCharacter()
		{
			if (field.text.Length > GlobalVariablesHelper.MAX_NAME_LENGTH)
			{
				errorMessage.text = "De ingevulde naam is te lang. Maximaal " + GlobalVariablesHelper.MAX_NAME_LENGTH +
									" tekens is toegestaan.";
				return;
			}

			if (string.IsNullOrWhiteSpace(field.text))
			{
				errorMessage.text = "De ingevulde naam mag niet leeg zijn.";
				return;
			}

			errorMessage.text = "";
			PlayerPrefs.SetString("PlayerName", field.text);
			PlayerPrefs.SetString(GlobalVariablesHelper.CHARACTER_NAME_PLAYERPREFS, selectedCharacter);

            PlayerPrefs.SetString("LastLevel", GlobalVariablesHelper.LEVEL_0_SCENE_NAME);

		    Game.GetGame().InLevel = true;
            SaveLoadGame.Save();
            
            SceneManager.LoadScene(GlobalVariablesHelper.BASE_LEVEL_INDEX);
		}

		public void SetSelectedCharacter(Transform character)
		{
			selectedCharacter = character.name;
		}
	}
}

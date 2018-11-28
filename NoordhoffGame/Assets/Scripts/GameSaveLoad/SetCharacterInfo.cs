using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.GameSaveLoad
{
	public class SetCharacterInfo : MonoBehaviour
	{
		[SerializeField] private InputField field;
		[SerializeField] private Text errorMessage;
	    private string selectedCharacter;
        
		public void CreateCharacter()
		{
			if (field.text.Length <= GlobalVariablesHelper.MAX_NAME_LENGTH)
			{
				errorMessage.text = "";
				PlayerPrefs.SetString("PlayerName", field.text);
			    PlayerPrefs.SetString(GlobalVariablesHelper.CHARACTER_NAME_PLAYERPREFS, selectedCharacter);
			}
			else
			{
				errorMessage.text = "De ingevulde naam is te lang, maximaal " + GlobalVariablesHelper.MAX_NAME_LENGTH + " tekens is toegestaan.";
			    return;
			}
            // Going to the next scene (so current scene index + 1)
		    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

	    public void SetSelectedCharacter(Transform character)
	    {
	        selectedCharacter = character.name;
	    }
	}
}

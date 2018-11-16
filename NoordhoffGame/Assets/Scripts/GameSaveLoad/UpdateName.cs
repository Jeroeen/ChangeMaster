using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.GameSaveLoad
{
	public class UpdateName : MonoBehaviour
	{
		[SerializeField] private InputField field;
		[SerializeField] private Text errorMessage;

		public void CreateName()
		{
			if (field.text.Length <= 15)
			{
				errorMessage.text = "";
				PlayerPrefs.SetString("PlayerName", field.text);
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			}
			else
			{
				errorMessage.text = "De ingevulde naam is te lang, maximaal 15 tekens is toegestaan.";
			}
		}
	}
}

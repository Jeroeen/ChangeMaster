using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class Settingscript : MonoBehaviour
	{
		[SerializeField] private Button interventionButton = null;
		[SerializeField] private Button infoButton = null;
		[SerializeField] private Button settingsButton = null;
		[SerializeField] private GameObject settingsScreen = null;
		[SerializeField] private CanvasGroup blockingPanel = null;
		//a function that will enable or disable the menu
		public void ShowMenu()
		{
			settingsScreen.SetActive(!settingsScreen.activeSelf);
			settingsButton.interactable = !settingsButton.IsInteractable();

			if (blockingPanel)
			{
				blockingPanel.blocksRaycasts = !blockingPanel.blocksRaycasts;
			}

			if (infoButton)
			{
				infoButton.interactable = !infoButton.IsInteractable();
			}

			if (interventionButton)
			{
				interventionButton.interactable = !interventionButton.IsInteractable();
			}

		}
	}
}

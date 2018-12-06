using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSaveLoad
{
	public class CharacterSelector : MonoBehaviour
	{
		private Button currentSelected;
		private Color resetColor;

		[SerializeField] private Button confirmButton = null;

		// Shows the current selected button in the color set with "pressedColor"
		// Saves this state even while clicking back on the panel or another button
		public void SetCurrentSelected(Button button)
		{
			// Can't do "currentSelected.colors.normalColor = x" directly, so a small detour has to be used
			ColorBlock newColors = button.colors;

			if (currentSelected == null)
			{
				resetColor = newColors.normalColor;
				confirmButton.interactable = true;
			}
			else
			{
				ColorBlock currentColors = currentSelected.colors;
				currentColors.normalColor = resetColor;
				currentColors.highlightedColor = resetColor;
				currentSelected.colors = currentColors;
			}

			newColors.normalColor = newColors.pressedColor;
			newColors.highlightedColor = newColors.pressedColor;

			button.colors = newColors;
			currentSelected = button;
		}
	}
}

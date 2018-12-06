using Assets.Scripts.Dialogue.Models;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Dialogue
{
	public class ObjectInfoHandler : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer spriteRenderer = null;
		[SerializeField] private Button settingButton = null;
		[SerializeField] private Button infoButton = null;
		[SerializeField] private Button interventionButton = null;
		[SerializeField] private Infoscreen infoscreen = null;

		public void Initialize(ObjectModel objectModel)
		{
			spriteRenderer.sprite = objectModel.Sprite;
			if (infoscreen != null)
			{
				infoscreen.ShowStakeholder(objectModel.Sprite.name);
				infoscreen.SaveInformation();
			}
		}

		public void CloseInfo()
		{
			gameObject.SetActive(false);
			OpenPopUp.IsActive = false;
			if (infoButton != null && settingButton != null)
			{
				infoButton.interactable = true;
				settingButton.interactable = true;
				interventionButton.interactable = true;
			}
		}
	}
}

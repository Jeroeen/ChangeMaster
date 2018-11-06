using Assets.Scripts.Dialogue.Models;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Dialogue
{
	public class ObjectInfoHandler : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;
		[SerializeField]
		private Button settingButton;
		[SerializeField]
		private Button infoButton;

		public void Initialize(ObjectModel objectModel)
		{
			RetrieveAsset.RetrieveAssets();
			spriteRenderer.sprite = RetrieveAsset.GetSpriteByName(objectModel.Sprite);
			Debug.Log(objectModel.Sprite);
			Debug.Log(spriteRenderer.sprite.name);
		}

		public void CloseInfo()
		{
			gameObject.SetActive(false);
			OpenPopUp.IsActive = false;
			if (infoButton != null && settingButton != null)
			{
				infoButton.interactable = true;
				settingButton.interactable = true;
			}
		}
	}
}

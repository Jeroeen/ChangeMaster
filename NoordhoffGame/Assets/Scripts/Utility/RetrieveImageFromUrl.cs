using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts.Utility
{
	public class RetrieveImageFromUrl : MonoBehaviour
	{
		[SerializeField] private string url;
		[SerializeField] private Image thumbnail;

		void Start()
		{
			StartCoroutine(GetTexture());
		}

		IEnumerator GetTexture()
		{
			UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url);

			yield return unityWebRequest.SendWebRequest();

			if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
			{
				Debug.Log(unityWebRequest.error);
			}
			else
			{
				Texture2D texture = ((DownloadHandlerTexture)unityWebRequest.downloadHandler).texture;
				thumbnail.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
			}
		}
	}
}

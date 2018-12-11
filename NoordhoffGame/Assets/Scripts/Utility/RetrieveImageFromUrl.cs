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

		// Start is called before the first frame update
		IEnumerator GetTexture()
		{
			UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
				thumbnail.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
			}
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}

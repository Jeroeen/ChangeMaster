using System;
using System.Collections.Generic;
using Assets.Scripts.Dialogue;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Splashscreen
{
	public class SplashAnimation : MonoBehaviour
	{
		private List<Sprite> sprites;
		private int currentIndex;

		[SerializeField] private Image renderedImage = null;

		// Start is called before the first frame update
		void Start()
		{
			currentIndex = 0;
			sprites = new List<Sprite>();

			RetrieveAsset.RetrieveAssets();

			for (int i = 1; i < Int32.MaxValue; i++)
			{
				Sprite sprite = RetrieveAsset.GetSpriteByName("Splashscreen_frame" + i);

				if (sprite == null)
				{
					break;
				}

				sprites.Add(sprite);
			}

			InvokeRepeating("ChangeAnimationFrame", 0.0f, 0.08f);
		}

		void ChangeAnimationFrame()
		{
			renderedImage.sprite = sprites[currentIndex];
			currentIndex++;
			if (currentIndex > sprites.Count - 1)
			{
				currentIndex = 0;
			}
		}
	}
}

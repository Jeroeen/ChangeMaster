using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Dialogue;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Splashscreen
{
	public class SplashAnimation : MonoBehaviour
	{
		private List<Sprite> sprites;
		private int currentIndex;
		private int pauseTimer;

		[SerializeField] private Image renderedImage = null;

		// Start is called before the first frame update
		void Start()
		{
			currentIndex = 0;
			sprites = new List<Sprite>();

			RetrieveAsset.RetrieveAssets();

			int index = 1;
			Sprite sprite = RetrieveAsset.GetSpriteByName("Splashscreen_frame" + index);

			while (sprite != null)
			{
				sprites.Add(sprite);

				index++;
				sprite = RetrieveAsset.GetSpriteByName("Splashscreen_frame" + index);
			}

			Action changeFrame = ChangeAnimationFrame;

			// Invokes the method ChangeAnimationFrame in 0.0f seconds every 0.1f seconds.
			InvokeRepeating(changeFrame.Method.Name, 0.0f, 0.1f);
		}

		private void ChangeAnimationFrame()
		{
			// Animation is called every 0.1 second, so multiply the pause seconds by 10
			if (pauseTimer < GlobalVariablesHelper.PAUSE_SECONDS_BETWEEN_SPLASHSCREEN_ANIMATION * 10)
			{
				pauseTimer++;
				return;
			}

			renderedImage.sprite = sprites[currentIndex];
			currentIndex++;
			if (currentIndex > sprites.Count - 1)
			{
				currentIndex = 0;
				pauseTimer = 0;
				renderedImage.sprite = sprites[currentIndex];
			}
		}
	}
}

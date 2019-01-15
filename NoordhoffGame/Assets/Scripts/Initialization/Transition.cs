using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Initialization
{
	public class Transition : MonoBehaviour
	{
		private float alpha;
		private float fadeSpeed = 0.02f;

		[SerializeField] private Image image = null;
		
		void Start()
		{
			alpha = image.color.a;
		}

		public bool FadeIn()
		{
			if (alpha > 0)
			{
				alpha -= fadeSpeed;
				image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
				return false;
			}

			return true;
		}

		public bool FadeOut()
		{
			if (alpha < 1)
			{
				alpha += fadeSpeed;
				image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
				return false;
			}

			return true;
		}

	}
}

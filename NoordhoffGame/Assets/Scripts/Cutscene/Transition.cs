using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Cutscene
{
	public class Transition : MonoBehaviour
	{
		private float _alpha;
		private float _fadeSpeed = 0.02f;

		[SerializeField] private Image _image = null;
		
		void Start()
		{
			_alpha = _image.color.a;
		}

		public bool FadeIn()
		{
			if (_alpha > 0)
			{
				_alpha -= _fadeSpeed;
				_image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _alpha);
				return false;
			}

			return true;
		}

		public bool FadeOut()
		{
			if (_alpha < 1)
			{
				_alpha += _fadeSpeed;
				_image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _alpha);
				return false;
			}

			return true;
		}

	}
}

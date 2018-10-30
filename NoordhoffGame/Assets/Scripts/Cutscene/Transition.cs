using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
	[SerializeField] private Image _image;
	private float _alpha;
	private float _fadeSpeed = 0.02f;


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

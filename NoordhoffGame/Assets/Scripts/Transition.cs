using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
	[SerializeField] private Image image;
	private float _alpha;
	private float _fadeSpeed = 0.02f;


	void Start()
	{
		_alpha = image.color.a;
	}

	public bool FadeIn()
	{
		if (_alpha > 0)
		{
			_alpha -= _fadeSpeed;
			image.color = new Color(image.color.r, image.color.g, image.color.b, _alpha);
			return false;
		}

		return true;
	}

	public bool FadeOut()
	{
		if (_alpha < 1)
		{
			_alpha += _fadeSpeed;
			image.color = new Color(image.color.r, image.color.g, image.color.b, _alpha);
			return false;
		}

		return true;
	}

}

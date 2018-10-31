using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeScene : MonoBehaviour
{
	[SerializeField]
	private Transition _transition;

	[SerializeField]
	private CameraController _controller;

	private bool _faded;

	void Start()
	{
		PlayerPrefs.SetString("LastLevel", SceneManager.GetActiveScene().name);
	}

    // Update is called once per frame
    void Update()
    {
	    if (_faded)
	    {
		    return;
	    }

	    if (_transition.FadeIn())
	    {
		    _controller.CanUse = true;
		    Destroy(_transition.gameObject, 0.01f);
		    _faded = true;
	    }
    }
}

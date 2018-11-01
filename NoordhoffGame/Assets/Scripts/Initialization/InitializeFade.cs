using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeFade : MonoBehaviour
{
	[SerializeField]
	private Transition transition;

	[SerializeField]
	private CameraController controller;

	private bool isFaded;

	void Start()
	{
		PlayerPrefs.SetString("LastLevel", SceneManager.GetActiveScene().name);
	}

    // Update is called once per frame
    void Update()
    {
	    if (isFaded)
	    {
		    return;
	    }

	    if (transition.FadeIn())
	    {
		    controller.CanUse = true;
            
		    Destroy(transition.gameObject, 0.01f);
		    isFaded = true;
	    }
    }
}

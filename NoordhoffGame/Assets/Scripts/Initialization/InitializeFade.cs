﻿using Assets.Scripts.CameraBehaviour;
using Assets.Scripts.Cutscene;
using Assets.Scripts.GameSaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Initialization
{
	public class InitializeFade : MonoBehaviour
	{
		[SerializeField] private Transition transition = null;
		[SerializeField] private CameraController controller = null;

		private bool isFaded;

		void Start()
		{
			PlayerPrefs.SetString("LastLevel", SceneManager.GetActiveScene().name);
			SaveLoadGame.Load();
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
}

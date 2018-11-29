﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Initialization
{
	public class LevelChooser : MonoBehaviour
	{
		public void LoadScene()
		{
			string levelName = PlayerPrefs.GetString("LastLevel"); //this assumes you save a string in PlayerPrefs at some point that's the name of the scene with that level
			if (levelName == null || levelName == "")
			{
				levelName = "Opening Cutscene"; //the default scene that should be loaded when you play for the first time
			}
			SceneManager.LoadScene(levelName);
		}
	}
}
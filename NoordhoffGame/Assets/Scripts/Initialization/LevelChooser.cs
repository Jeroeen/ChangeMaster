using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class LevelChooser : MonoBehaviour
	{
		// Start is called before the first frame update
		void Start()
		{
			string levelName = PlayerPrefs.GetString("LastLevel"); //this assumes you save a string in PlayerPrefs at some point that's the name of the scene with that level
			if (levelName == null || levelName == "")
			{
				levelName = "Opening Cutscene"; //the default scene that should be loaded when you play for the first time
			}
			Application.LoadLevel(levelName);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateName : MonoBehaviour
{
	[SerializeField] private InputField _field;

	public void CreateName()
	{
		PlayerPrefs.SetString("PlayerName", _field.text);
		SceneManager.LoadScene(3);
	}
}

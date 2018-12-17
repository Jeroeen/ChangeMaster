using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSound : MonoBehaviour
{
	[SerializeField] private AudioSource source;

	public void ToggleAudio()
	{
		source.enabled = !source.enabled;
	}
}

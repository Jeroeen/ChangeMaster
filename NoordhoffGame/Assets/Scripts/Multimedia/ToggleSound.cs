using UnityEngine;

namespace Assets.Scripts.Multimedia
{
	public class ToggleSound : MonoBehaviour
	{
		[SerializeField] private AudioSource source;

		public void ToggleAudio()
		{
			source.enabled = !source.enabled;
		}

		public void PlayAudio()
		{
			if (!source.isPlaying)
			{
				source.Play();
			}
		}
	}
}

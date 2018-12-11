using Assets.Scripts.GameSaveLoad;
using UnityEngine;
using UnityEngine.Video;

namespace Assets.Scripts
{
	public class VideoHandler : MonoBehaviour
	{
		[SerializeField] private GameObject level0Movies = null;
		[SerializeField] private GameObject level1Movies = null;
		[SerializeField] private GameObject level2Movies = null;

		[SerializeField] private GameObject movie = null;
		[SerializeField] private YoutubePlayer youtubePlayer = null;
		[SerializeField] private VideoPlayer videoPlayer = null;

		void Start()
		{
			int currentLevel = Game.GetGame().CurrentLevelNumber - 1;
			
			if (currentLevel >= -7)
			{
				level0Movies.SetActive(true);
			}
			if (currentLevel >= 1)
			{
				level1Movies.SetActive(true);
			}
			if (currentLevel >= 2)
			{
				level2Movies.SetActive(true);
			}
		}

		public void PlayMovie(string youTubeUrl)
		{
			movie.SetActive(true);
			youtubePlayer.enabled = true;
			youtubePlayer.youtubeUrl = youTubeUrl;
			gameObject.SetActive(false);

			youtubePlayer.StartVideo();
			videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;
		}

		public void StopMovie()
		{
			youtubePlayer.enabled = false;
			movie.SetActive(false);
			gameObject.SetActive(true);
		}
	}
}

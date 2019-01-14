﻿using System.Collections;
using Assets.LightShaft.Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Assets.LightShaft.YoutubeAPI.Scripts.Demos
{
    public class YoutubeVideoUi : MonoBehaviour {

        public Text videoName;
        public string videoId,thumbUrl;
        public Image videoThumb;
        private GameObject mainUI;
        public void PlayYoutubeVideo()
        {
            //search for the low quality if not find search for highquality
            if(GameObject.FindObjectOfType<YoutubePlayer>() != null)
            {
                GameObject.FindObjectOfType<YoutubePlayer>().LoadYoutubeVideo(videoId);
                GameObject.FindObjectOfType<YoutubePlayer>().videoPlayer.loopPointReached += VideoFinished;
            }

            if (GameObject.FindObjectOfType<VideoSearchDemo>() != null)
                mainUI = GameObject.FindObjectOfType<VideoSearchDemo>().mainUI;
            else if (GameObject.FindObjectOfType<ChannelSearchDemo>() != null)
                mainUI = GameObject.FindObjectOfType<ChannelSearchDemo>().mainUI;
            else if (GameObject.FindObjectOfType<PlaylistDemo>() != null)
                mainUI = GameObject.FindObjectOfType<PlaylistDemo>().mainUI;
            mainUI.SetActive(false);
        }

        private void VideoFinished(VideoPlayer vPlayer)
        {
            if (GameObject.FindObjectOfType<YoutubePlayer>() != null)
            {
                GameObject.FindObjectOfType<YoutubePlayer>().videoPlayer.loopPointReached -= VideoFinished;
            }
        
            Debug.Log("Video Finished");
            mainUI.SetActive(true);
        }

        public IEnumerator PlayVideo(string url)
        {
#if UNITY_ANDROID || UNITY_IOS
            yield return Handheld.PlayFullScreenMovie(url, Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.Fill);
#else
        yield return true;
#endif
            Debug.Log("below this line will run when the video is finished");
            VideoFinished();
        }

        public void LoadThumbnail()
        {
            StartCoroutine(DownloadThumb());
        }

        IEnumerator DownloadThumb()
        {
            WWW www = new WWW(thumbUrl);
            yield return www;
            Texture2D thumb = new Texture2D(100, 100);
            www.LoadImageIntoTexture(thumb);
            videoThumb.sprite = Sprite.Create(thumb, new Rect(0, 0, thumb.width, thumb.height), new Vector2(0.5f, 0.5f), 100);
        }

        public void VideoFinished()
        {
            Debug.Log("Finished!");
        }
    }
}

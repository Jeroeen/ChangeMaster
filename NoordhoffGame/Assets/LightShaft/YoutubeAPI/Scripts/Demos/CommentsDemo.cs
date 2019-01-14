using Assets.LightShaft.YoutubeAPI.Scripts.Src;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LightShaft.YoutubeAPI.Scripts.Demos
{
    public class CommentsDemo : MonoBehaviour {
        YoutubeAPIManager youtubeapi;

        public Text videoIdInput;
        public Text commentsTextArea;

        void Start()
        {
            //Get the api component
            youtubeapi = GameObject.FindObjectOfType<YoutubeAPIManager>();
            if (youtubeapi == null)
            {
                youtubeapi = gameObject.AddComponent<YoutubeAPIManager>();
            }
        }

        public void GetComments()
        {
            youtubeapi.GetComments(videoIdInput.text, OnFinishLoadingComments);
        }

        void OnFinishLoadingComments(YoutubeComments[] comments)
        {
            string allComments = "";
            for(int index = 0; index < comments.Length; index++)
            {
                allComments += "<color=red>"+comments[index].authorDisplayName + "</color>: " + comments[index].textDisplay + "\n";
            }
            commentsTextArea.text = allComments;
        }
    }
}

using System;
using Assets.Scripts.Dialogue.JsonItems;
using Assets.Scripts.Progress;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Info;
using Assets.Scripts.UI.Info.Theory;
using Assets.Scripts.UI.InterventionScreen;
using Assets.UnityLitJson;
using UnityEngine;

namespace Assets.Scripts.Dialogue
{
	public class RetrieveJson
	{
		public DialogueItem LoadJsonDialogue(string nameOfPartner, string stage, int dialogueCount)
		{
			string path = "DialogueFiles/" + stage + "/" + nameOfPartner;

			if (dialogueCount >= 0)
			{
				path += "-" + dialogueCount;
			}

            SaveLoadGame.Save();
            if (!Game.GetGame().DialogueRead.ContainsKey(nameOfPartner + stage + dialogueCount))
            {
                Game.GetGame().DialogueRead.Add(nameOfPartner + stage + dialogueCount, false);
            }
			string jsonString = GetJsonString(path);

			DialogueItem item = JsonMapper.ToObject<DialogueItem>(jsonString);
			item.ReplaceTags();

			return item;
		}
	

		public InterventionList LoadJsonInterventions(string level)
		{
			string path = "InterventionFiles/Interventions" + level;

			string jsonString = GetJsonString(path);

            return JsonMapper.ToObject<InterventionList>(jsonString);
        }

		public InfoList LoadJsonInformation(string level)
		{
			string path = "InformationFiles/Information" + level;

			string jsonString = GetJsonString(path);

			InfoList item = JsonMapper.ToObject<InfoList>(jsonString);
			for (int i = 0; i < item.InformationList.Length; i++)
			{
				item.InformationList[i].Found = false;
			}
			return item;
		}

        public PriorityList LoadJsonPriorities(string level)
        {
            string path = "PriorityFiles/Priority" + level;

            string jsonString = GetJsonString(path);

            return JsonMapper.ToObject<PriorityList>(jsonString);
        }

	    public TheoryList LoadJsonTheory(string level)
	    {
	        string path = "TheoryFiles/Theory" + level;

	        string jsonString = GetJsonString(path);


            return JsonMapper.ToObject<TheoryList>(jsonString);
        }

        private string GetJsonString(string path)
		{
			TextAsset asset = Resources.Load(path) as TextAsset;

			if (asset == null)
			{
				throw new NullReferenceException("Path kan niets vinden: " + path);
			}

			return asset.ToString();
		}
	}
}

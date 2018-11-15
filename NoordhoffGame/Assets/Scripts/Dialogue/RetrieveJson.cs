using System;
using LitJson;
using UnityEngine;

namespace Assets.Scripts.Dialogue
{
	public class RetrieveJson
	{
		public DialogueItem LoadJsonDialogue(string nameOfPartner, string stage, int dialogueCount)
		{
			string path = "DialogueFiles/";
			if (dialogueCount < 0)
			{
				path += stage + "/" + nameOfPartner;
			}
			else
			{
				path += stage + "/" + nameOfPartner + "-" + dialogueCount;
			}

			string jsonString = GetJsonString(path);

			DialogueItem item = JsonMapper.ToObject<DialogueItem>(jsonString);
			item.ReplaceName();

			return item;
		}
	

		public InterventionList LoadJsonInterventions(int level)
		{
			string path = "InterventionFiles/InterventionsLevel_" + level;

			string jsonString = GetJsonString(path);

			InterventionList item = JsonMapper.ToObject<InterventionList>(jsonString);

			return item;
		}

		public InfoList LoadJsonInformation(int level)
		{
			string path = "InformationFiles/InformationLevel_" + level;

			string jsonString = GetJsonString(path);

			InfoList item = JsonMapper.ToObject<InfoList>(jsonString);
			for (int i = 0; i < item.InformationList.Length; i++)
			{
				item.InformationList[i].Found = false;
			}
			return item;
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

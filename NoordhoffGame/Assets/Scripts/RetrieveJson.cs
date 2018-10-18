using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using UnityEngine;

namespace Assets.Scripts
{
	public class RetrieveJson
	{
		public DialogueItem LoadJson(string nameOfPartner, int level, int dialogueCount)
		{
			string path;
			if (dialogueCount < 0)
			{
				path = "DialogueFiles/" + nameOfPartner + "-" + level;
			}
			else
			{
				path = "DialogueFiles/" + nameOfPartner + "-" + level + "-" + dialogueCount;
			}

			TextAsset asset = Resources.Load(path) as TextAsset;
			string jsonString = asset.ToString();

			DialogueItem item = JsonMapper.ToObject<DialogueItem>(jsonString);

			return item;
		}
	}
}


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
				path = "Assets/Scripts/DialogueFiles/" + nameOfPartner + "-" + level + ".json";
			}
			else
			{
				path = "Assets/Scripts/DialogueFiles/" + nameOfPartner + "-" + level + "-" + dialogueCount + ".json";
			}

			using (StreamReader r = new StreamReader(path))
			{
				string json = r.ReadToEnd();
				DialogueItem item = JsonConvert.DeserializeObject<DialogueItem>(json);

				return item;
			}
		}
	}
}


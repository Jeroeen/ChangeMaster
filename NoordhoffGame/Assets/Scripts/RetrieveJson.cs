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
		public DialogueItem LoadJson(string nameOfPartner, int numberOfConversation)
		{
			using (StreamReader r = new StreamReader("Assets/Scripts/DialogueFiles/" + nameOfPartner + "-" + numberOfConversation + ".json"))
			{
				string json = r.ReadToEnd();
				DialogueItem item = JsonConvert.DeserializeObject<DialogueItem>(json);
				
				return item;
			}
		}
	}
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using UnityEngine;

namespace Assets.Scripts
{
	public class DialogueItem
	{
		public int CurrentPage { get; set; }

		[JsonAlias("speaker")]
		public string NameOfSpeaker { get; private set; }

		[JsonAlias("previousButtonText")]
		public string PreviousButtonText { get; private set; }

		[JsonAlias("nextButtonText")]
		public string NextButtonText { get; private set; }

		[JsonAlias("confirmButtonText")]
		public string ConfirmButtonText { get; private set; }

		[JsonAlias("lines")]
		public List<string> DialogueLines { get; private set; }

		public DialogueItem()
		{
			CurrentPage = 0;
		}

		public string PreviousLine()
		{
			CurrentPage--;
			return DialogueLines[CurrentPage];
		}

		public string NextLine()
		{
			CurrentPage++;
			return DialogueLines[CurrentPage];
		}

		public bool IsEndOfDialogue()
		{
			return CurrentPage >= DialogueLines.Count - 1;
		}

		public bool IsBeginningOfDialogue()
		{
			return CurrentPage <= 0;
		}

	    public void ReplaceName()
	    {
	        for (int index = 0; index < DialogueLines.Count; index++)
	        {
	            string s = DialogueLines[index];
	            if (s.Contains("[Naam]"))
	            {
                    DialogueLines[index] = s.Replace("[Naam]", PlayerPrefs.GetString("PlayerName"));
	            }
	        }
	    }
	}
}

using System.Collections.Generic;
using Assets.Scripts.Utility;
using Assets.UnityLitJson;
using UnityEngine;

namespace Assets.Scripts.Dialogue.JsonItems
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

		public void ReplaceTags()
		{
			for (int i = 0; i < DialogueLines.Count; i++)
			{
				string s = DialogueLines[i];
				if (s.Contains("[Naam]"))
				{
					DialogueLines[i] = s.Replace("[Naam]", PlayerPrefs.GetString("PlayerName"));
				}

				if (s.Contains("[Greeting]"))
				{
					DialogueLines[i] = DialogueLines[i].Replace("[Greeting]", TimeHelper.GetGreetingTimeOfDay());
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Assets.Scripts
{
	public class DialogueItem
	{
		public int currentPage { get; set; }

		[JsonProperty("speaker")]
		public string Speaker { get; private set; }

		[JsonProperty("previousButtonText")]
		public string PreviousButtonText { get; private set; }

		[JsonProperty("nextButtonText")]
		public string NextButtonText { get; private set; }

		[JsonProperty("confirmButtonText")]
		public string ConfirmButtonText { get; private set; }

		[JsonProperty("lines")]
		public List<string> DialogueLines { get; private set; }

		public DialogueItem()
		{
			currentPage = 0;
		}

		public string PreviousLine()
		{
			currentPage--;
			return DialogueLines[currentPage];
		}

		public string NextLine()
		{
			currentPage++;
			return DialogueLines[currentPage];
		}

		public bool IsEndOfDialogue()
		{
			return currentPage >= DialogueLines.Count - 1;
		}

		public bool IsBeginningOfDialogue()
		{
			return currentPage <= 0;
		}
	}
}

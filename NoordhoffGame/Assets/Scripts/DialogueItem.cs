using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;

namespace Assets.Scripts
{
	public class DialogueItem
	{
		public int currentPage { get; set; }

		public string speaker { get; private set; }

		public string previousButtonText { get; private set; }

		public string nextButtonText { get; private set; }

		public string confirmButtonText { get; private set; }

		public List<string> lines { get; private set; }

		public DialogueItem()
		{
			currentPage = 0;
		}

		public string PreviousLine()
		{
			currentPage--;
			return lines[currentPage];
		}

		public string NextLine()
		{
			currentPage++;
			return lines[currentPage];
		}

		public bool IsEndOfDialogue()
		{
			return currentPage >= lines.Count - 1;
		}

		public bool IsBeginningOfDialogue()
		{
			return currentPage <= 0;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utility
{
	public class TimeHelper
	{
		public static string GetGreetingTimeOfDay()
		{
			// Standard is UK time, so add one hour.
			int time = DateTime.UtcNow.Hour + 1;
			Debug.Log(time);


			if (time < 6)
			{
				return "Goedenacht";
			}

			if (time < 12)
			{
				return "Goedemorgen";
			}

			if (time < 18)
			{
				return "Goedemiddag";
			}

			return time <= 24 ? "Goedenavond" : "";
		}
	}
}

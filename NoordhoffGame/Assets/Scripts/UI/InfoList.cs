using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
	[System.Serializable]
	public struct Info
	{
		public string Image;
		public string Text;
		public bool Found;
	}

	[System.Serializable]
	public class InfoList
	{
		public Info[] InformationList;

		public InfoList()
		{

		}

		public InfoList(Info[] Information)
		{
			InformationList = Information;
		}
	}
}
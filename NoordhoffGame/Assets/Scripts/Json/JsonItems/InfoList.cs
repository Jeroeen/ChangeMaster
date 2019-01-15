namespace Assets.Scripts.Json.JsonItems
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
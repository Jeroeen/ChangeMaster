using Assets.Scripts.GameSaveLoad;
using UnityEngine;

namespace Assets.Scripts.Progress
{
	public class DeletePlayerPrefs : MonoBehaviour
	{
		public void DeletePrefs()
		{
			SaveLoadGame.DeleteSave();
			PlayerPrefs.DeleteAll();
		}
	}
}

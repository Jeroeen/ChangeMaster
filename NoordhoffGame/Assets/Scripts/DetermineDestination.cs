using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class DetermineDestination : MonoBehaviour
	{
		[SerializeField] private Text text;

		public void DestinationClick()
		{
			Debug.Log(gameObject.name);
		}
	}
}

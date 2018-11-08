using UnityEngine;

namespace Assets.Scripts
{
	public class DetermineDestination : MonoBehaviour
	{
		public void DestinationClick(GameObject destination)
		{
			Debug.Log("x");
			Debug.Log(destination.name);
		}
	}
}

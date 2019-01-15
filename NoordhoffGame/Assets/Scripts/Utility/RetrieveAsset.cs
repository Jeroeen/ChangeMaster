using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Utility
{
	public class RetrieveAsset
	{
		private static Dictionary<string, Sprite> _assets;

		private RetrieveAsset() { }

		public static Dictionary<string, Sprite> RetrieveAssets()
		{
			if (_assets != null)
			{
				return _assets;
			}

			_assets = new Dictionary<string, Sprite>();

			Object[] assetObjects = Resources.LoadAll("Art/Sprites", typeof(Sprite));
			

			foreach (Object obj in assetObjects.Distinct())
			{
				if (obj != null)
				{
					_assets.Add(obj.name, (Sprite)obj);
				}
			}

			return _assets;
		}

		public static Sprite GetSpriteByName(string name)
		{
			return _assets.FirstOrDefault(x => x.Key == name).Value;
		}
	}
}

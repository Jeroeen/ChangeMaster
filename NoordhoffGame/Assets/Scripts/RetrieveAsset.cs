using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts
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

			string[] assetGuids = AssetDatabase.FindAssets("", new[] { "Assets/Sprites" });

			List<String> assetPaths = assetGuids.Select(AssetDatabase.GUIDToAssetPath).ToList();

			foreach (string path in assetPaths.Distinct())
			{
				Sprite sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
				if (sprite != null)
				{
					_assets.Add(sprite.name, sprite);
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

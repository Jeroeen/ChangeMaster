using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
	class RetrieveAsset
	{
		private Dictionary<string, Sprite> assets;

		private RetrieveAsset() { }

		public Dictionary<string, Sprite> retrieveAssets()
		{
			if (assets != null)
			{
				string[] AssetGuids = AssetDatabase.FindAssets("", new[] {"Assets"});
				List<String> AssetPaths = new List<String>();

				foreach (string guid in AssetGuids)
				{
					AssetPaths.Add(AssetDatabase.GUIDToAssetPath(guid));
					Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
				}
			}

			return assets;
		}
	}
}

using UnityEngine;

namespace Assets.Scripts.Tilemap
{
	public class TilemapHandler : MonoBehaviour
	{
		public UnityEngine.Tilemaps.Tilemap Tilemap;

		[HideInInspector]
		public Vector2 MinBounds { get; private set; }

		[HideInInspector]
		public Vector2 MaxBounds { get; private set; }

		void Start()
		{
			Tilemap.CompressBounds();
			CalculateWorldSpaceBounds();
		}

		public void DrawBounds()
		{
			Debug.DrawLine(MinBounds, MaxBounds, Color.cyan);

			// Down to up boundary
			Debug.DrawLine(Tilemap.CellToWorld(Tilemap.cellBounds.min), Tilemap.CellToWorld(Tilemap.cellBounds.max), Color.magenta);

			// Left and Right boundary
			Debug.DrawLine(Tilemap.CellToWorld(new Vector3Int(Tilemap.cellBounds.xMax, Tilemap.cellBounds.yMin, 0)), Tilemap.CellToWorld(new Vector3Int(Tilemap.cellBounds.xMin, Tilemap.cellBounds.yMax, 0)), Color.magenta);

		}

		private void CalculateWorldSpaceBounds()
		{
			float xMin = Tilemap.CellToWorld(new Vector3Int(Tilemap.cellBounds.xMin, Tilemap.cellBounds.yMax, 0)).x;
			float yMin = Tilemap.CellToWorld(Tilemap.cellBounds.min).y;
			float xMax = Tilemap.CellToWorld(new Vector3Int(Tilemap.cellBounds.xMax, Tilemap.cellBounds.yMin, 0)).x;
			float yMax = Tilemap.CellToWorld(Tilemap.cellBounds.max).y;

			MinBounds = new Vector2(xMin, yMin);
			MaxBounds = new Vector2(xMax, yMax);
		}
	}
}

// Attach this script on your main ortohgraphic Camera:

/* The MIT License (MIT)

Copyright (c) 2014, Marcel Căşvan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using UnityEngine;

namespace Assets.Scripts.CameraBehaviour
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class ViewportHandler : MonoBehaviour
	{
		#region FIELDS
		public Color wireColor = Color.white;
		public float UnitsSize = 1; // size of your scene in unity units
		public Constraint constraint = Constraint.Portrait;
		public static ViewportHandler Instance;
		public Camera camera;

		private float _width;
		private float _height;
		//*** bottom screen
		private Vector3 bottomLeft;
		private Vector3 bottomCamera;
		private Vector3 bottomRight;
		//*** middle screen
		private Vector3 middleLeft;
		private Vector3 middleCamera;
		private Vector3 middleRight;
		//*** top screen
		private Vector3 topLeft;
		private Vector3 topCamera;
		private Vector3 topRight;
		#endregion

		#region PROPERTIES
		public float Width
		{
			get
			{
				return _width;
			}
		}
		public float Height
		{
			get
			{
				return _height;
			}
		}

		// helper points:
		public Vector3 BottomLeft
		{
			get
			{
				return bottomLeft;
			}
		}
		public Vector3 BottomCenter
		{
			get
			{
				return bottomCamera;
			}
		}
		public Vector3 BottomRight
		{
			get
			{
				return bottomRight;
			}
		}
		public Vector3 MiddleLeft
		{
			get
			{
				return middleLeft;
			}
		}
		public Vector3 MiddleCenter
		{
			get
			{
				return middleCamera;
			}
		}
		public Vector3 MiddleRight
		{
			get
			{
				return middleRight;
			}
		}
		public Vector3 TopLeft
		{
			get
			{
				return topLeft;
			}
		}
		public Vector3 TopCenter
		{
			get
			{
				return topCamera;
			}
		}
		public Vector3 TopRight
		{
			get
			{
				return topRight;
			}
		}
		#endregion

		#region METHODS
		private void Awake()
		{
			camera = GetComponent<Camera>();
			Instance = this;
			ComputeResolution();
		}

		private void ComputeResolution()
		{
			float leftX, rightX, topY, bottomY;

			if (constraint == Constraint.Landscape)
			{
				camera.orthographicSize = 1f / camera.aspect * UnitsSize / 2f;
			}
			else
			{
				camera.orthographicSize = UnitsSize / 2f;
			}

			_height = 2f * camera.orthographicSize;
			_width = _height * camera.aspect;

			float cameraX, cameraY;
			cameraX = camera.transform.position.x;
			cameraY = camera.transform.position.y;

			leftX = cameraX - _width / 2;
			rightX = cameraX + _width / 2;
			topY = cameraY + _height / 2;
			bottomY = cameraY - _height / 2;

			//*** bottom
			bottomLeft = new Vector3(leftX, bottomY, 0);
			bottomCamera = new Vector3(cameraX, bottomY, 0);
			bottomRight = new Vector3(rightX, bottomY, 0);
			//*** middle
			middleLeft = new Vector3(leftX, cameraY, 0);
			middleCamera = new Vector3(cameraX, cameraY, 0);
			middleRight = new Vector3(rightX, cameraY, 0);
			//*** top
			topLeft = new Vector3(leftX, topY, 0);
			topCamera = new Vector3(cameraX, topY, 0);
			topRight = new Vector3(rightX, topY, 0);
		}

		private void Update()
		{
//#if UNITY_EDITOR
			ComputeResolution();
//#endif
		}

		void OnDrawGizmos()
		{
			Gizmos.color = wireColor;

			Matrix4x4 temp = Gizmos.matrix;
			Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
			if (camera.orthographic)
			{
				float spread = camera.farClipPlane - camera.nearClipPlane;
				float center = (camera.farClipPlane + camera.nearClipPlane) * 0.5f;
				Gizmos.DrawWireCube(new Vector3(0, 0, center), new Vector3(camera.orthographicSize * 2 * camera.aspect, camera.orthographicSize * 2, spread));
			}
			else
			{
				Gizmos.DrawFrustum(Vector3.zero, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect);
			}
			Gizmos.matrix = temp;
		}
		#endregion

		public enum Constraint { Landscape, Portrait }

	}
}

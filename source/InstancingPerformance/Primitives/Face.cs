using InstancingPerformance.Core;
using SharpDX;

namespace InstancingPerformance.Primitives
{
	public struct Face
	{
		public Vector3 BottomLeft;
		public Vector3 BottomRight;
		public Vector3 TopLeft;
		public Vector3 TopRight;

		public Face(Vector3 bottomLeft, Vector3 topLeft, Vector3 topRight, Vector3 bottomRight)
		{
			BottomLeft = bottomLeft;
			TopLeft = topLeft;
			TopRight = topRight;
			BottomRight = bottomRight;
		}

		public static Face operator +(Face face, Vector3 v)
		{
			return new Face(
				v + face.BottomLeft,
				v + face.TopLeft,
				v + face.TopRight,
				v + face.BottomRight);
		}

		public static Face operator *(Face face, Quaternion quaternion)
		{
			return new Face(
				Vector3.Transform(face.BottomLeft, quaternion),
				Vector3.Transform(face.TopLeft, quaternion),
				Vector3.Transform(face.TopRight, quaternion),
				Vector3.Transform(face.BottomRight, quaternion));
		}
	}
}

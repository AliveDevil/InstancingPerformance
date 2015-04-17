using System.Runtime.InteropServices;
using SharpDX;

namespace InstancingPerformance.Primitives
{
	[StructLayout(LayoutKind.Explicit, Size = 20)]
	public struct VertexPositionNormalColor
	{
		[FieldOffset(0)]
		public Vector4 Position; // 16

		[FieldOffset(16)]
		public Vector3 Normal; // 12

		[FieldOffset(28)]
		public Color Color; // 4

		public VertexPositionNormalColor(Vector3 position, Vector3 normal, Color color)
			: this(new Vector4(position, 1), normal, color)
		{
		}

		public VertexPositionNormalColor(Vector4 position, Vector3 normal, Color color)
		{
			Position = position;
			Normal = normal;
			Color = color;
		}
	}
}

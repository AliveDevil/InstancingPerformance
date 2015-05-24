using System.Runtime.InteropServices;
using SharpDX;

namespace InstancingPerformance.Primitives
{
	[StructLayout(LayoutKind.Explicit, Size = 28)]
	public struct VertexPositionNormal
	{
		[FieldOffset(0)]
		public Vector4 Position; // 16

		[FieldOffset(16)]
		public Vector3 Normal; // 12

		public VertexPositionNormal(Vector3 position, Vector3 normal)
			: this(new Vector4(position, 1), normal)
		{
		}

		public VertexPositionNormal(Vector4 position, Vector3 normal)
		{
			Position = position;
			Normal = normal;
		}
	}
}

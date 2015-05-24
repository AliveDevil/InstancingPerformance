using System.Runtime.InteropServices;
using SharpDX;

namespace InstancingPerformance.Primitives
{
	[StructLayout(LayoutKind.Explicit, Size = 80)]
	public struct VertexWorldColor
	{
		[FieldOffset(0)]
		public Matrix World; // 64
		[FieldOffset(64)]
		public Vector4 Color; // 80

		public VertexWorldColor(Matrix world, Color color)
		{
			World = Matrix.Transpose(world);
			Color = color.ToVector4();
		}
	}
}

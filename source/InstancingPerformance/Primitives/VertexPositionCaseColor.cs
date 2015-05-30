using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace InstancingPerformance.Primitives
{
	[StructLayout(LayoutKind.Explicit, Size = 36)]
	public struct VertexPositionCaseColor
	{
		[FieldOffset(0)]
		public Vector4 Position; // 16

		[FieldOffset(16)]
		public uint Case; // 4

		[FieldOffset(20)]
		public Vector4 Color; // 16

		public VertexPositionCaseColor(Vector3 position, uint @case, Color color)
			: this(new Vector4(position, 1), @case, color)
		{
		}

		public VertexPositionCaseColor(Vector4 position, uint @case, Color color)
		{
			Position = position;
			Case = @case;
			Color = color.ToVector4();
		}
	}
}

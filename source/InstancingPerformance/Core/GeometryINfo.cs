using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace InstancingPerformance.Core
{
	public struct GeometryInfo
	{
		private static int[] vertexCount = { 0, 6, 6, 12, 6, 12, 12, 18, 6, 12, 12, 18, 12, 18, 18, 24, 6, 12, 12, 18, 12, 18, 18, 24, 12, 18, 18, 24, 18, 24, 24, 30, 6, 12, 12, 18, 12, 18, 18, 24, 12, 18, 18, 24, 18, 24, 24, 30, 12, 18, 18, 24, 18, 24, 24, 30, 18, 24, 24, 30, 24, 30, 30, 36 };

		public Vector3 Position;
		public uint Case;
		public Color Color;

		public int VertexCount => vertexCount[Case];

		public GeometryInfo(Vector3 position, Color color, uint @case)
		{
			Position = position;
			Case = @case;
			Color = color;
		}
	}
}

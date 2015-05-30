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
		public Vector3 Position;
		public uint Case;
		public Color Color;

		public GeometryInfo(Vector3 position, Color color, uint @case)
		{
			Position = position;
			Case = @case;
			Color = color;
		}
	}
}

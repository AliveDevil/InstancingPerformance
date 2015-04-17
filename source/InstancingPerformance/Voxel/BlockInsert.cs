using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstancingPerformance.Primitives;
using SharpDX;

namespace InstancingPerformance.Voxel
{
	public struct BlockInsert
	{
		public Vector3 Position;
		public Block Block;

		public BlockInsert(Vector3 position, Block block)
		{
			Position = position;
			Block = block;
		}

		public override string ToString()
		{
			return string.Format("{0}", Position);
		}
	}
}

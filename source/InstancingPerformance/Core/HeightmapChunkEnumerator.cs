using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstancingPerformance.Primitives;
using InstancingPerformance.Voxel;
using SharpDX;

namespace InstancingPerformance.Core
{
	public class MapGenerator : IEnumerable<BlockInsert[]>
	{
		private Heightmap heightmap;
		private ColorMap colormap;
		private int chunkSize;
		private int chunkWidthCount;
		private int chunkDepthCount;
		private int chunkCount;
		private int batchYield;

		public MapGenerator(Heightmap heightmap, ColorMap colormap, int chunkSize, int batchYield)
		{
			this.heightmap = heightmap;
			this.colormap = colormap;
			this.chunkSize = chunkSize;
			this.chunkWidthCount = heightmap.Width / chunkSize;
			this.chunkDepthCount = heightmap.Height / chunkSize;
			this.chunkCount = this.chunkWidthCount * this.chunkDepthCount;
			this.batchYield = batchYield;
		}

		public IEnumerator<BlockInsert[]> GetEnumerator()
		{
			int batch = 0;
			List<BlockInsert> inserts = new List<BlockInsert>();
			for (int i = 0; i < chunkCount; i++)
			{
				int cX = i % chunkWidthCount;
				int cZ = i / chunkWidthCount % chunkDepthCount;

				for (int iX = 0; iX < chunkSize; iX++)
				{
					for (int iZ = 0; iZ < chunkSize; iZ++)
					{
						int x = cX * chunkSize + iX;
						int z = cZ * chunkSize + iZ;
						float h = heightmap[x, z];
						System.Drawing.Color c = colormap[x, z];
						inserts.Add(new BlockInsert(new Vector3(x, h, z).Floor(), new Block(true, c.R / 255f, c.G / 255f, c.B / 255f)));
					}
				}

				if (batch++ >= batchYield)
				{
					yield return inserts.ToArray();
					inserts.Clear();
				}
			}
			if (inserts.Count != 0)
				yield return inserts.ToArray();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}

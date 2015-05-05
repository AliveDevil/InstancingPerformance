using System;
using System.Collections;
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
		private Heightmap heightmap { get; }
		private ColorMap colormap { get; }
		private int chunkSize { get; }
		private int batchYield { get; }
		private int chunkWidthCount => heightmap.Width / chunkSize;
		private int chunkDepthCount => heightmap.Height / chunkSize;
		private int chunkCount => chunkWidthCount * chunkDepthCount;

		public MapGenerator(Heightmap heightmap, ColorMap colormap, int chunkSize, int batchYield)
		{
			this.heightmap = heightmap;
			this.colormap = colormap;
			this.chunkSize = chunkSize;
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

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}

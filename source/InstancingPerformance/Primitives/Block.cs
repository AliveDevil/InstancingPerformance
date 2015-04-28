using InstancingPerformance.Core;
using SharpDX;

namespace InstancingPerformance.Primitives
{
	public struct Block
	{
		public static Block Empty = new Block(false, 0, 0, 0);
		public static Block Green = new Block(true, 0, 1, 0);

		public Color Color;
		public bool Solid;

		public Block(bool solid, float r, float g, float b)
		{
			Solid = solid;
			Color = new Color(r, g, b, 1);
		}

		public static bool operator ==(Block l, Block r)
		{
			return l.Solid == r.Solid && l.Color == r.Color;
		}

		public static bool operator !=(Block l, Block r)
		{
			return l.Solid != r.Solid || l.Color != r.Color;
		}

		public void MeshData(Primitives.Chunk chunk, int x, int y, int z, MeshData meshData)
		{
			int c = 0;

			if (Solid)
			{
				if (!chunk.GetBlock(new Vector3(x, y + 1, z)).Solid)
				{
					FaceDataUp(x, y, z, meshData);
					c |= 0x01;
				}
				if (!chunk.GetBlock(new Vector3(x, y - 1, z)).Solid)
				{
					FaceDataDown(x, y, z, meshData);
					c |= 0x02;
				}
				if (!chunk.GetBlock(new Vector3(x, y, z + 1)).Solid)
				{
					FaceDataNorth(x, y, z, meshData);
					c |= 0x04;
				}
				if (!chunk.GetBlock(new Vector3(x, y, z - 1)).Solid)
				{
					FaceDataSouth(x, y, z, meshData);
					c |= 0x08;
				}
				if (!chunk.GetBlock(new Vector3(x + 1, y, z)).Solid)
				{
					FaceDataEast(x, y, z, meshData);
					c |= 0x10;
				}
				if (!chunk.GetBlock(new Vector3(x - 1, y, z)).Solid)
				{
					FaceDataWest(x, y, z, meshData);
					c |= 0x20;
				}
			}

			if (c > 0 && c < 0x3F)
			{
				meshData.AddGeometryInfo(new Vector3(x, y, z), Color, c);
			}
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", Solid, Color);
		}

		private void FaceDataDown(int x, int y, int z, MeshData meshData)
		{
			meshData.AddFace(new Vector3(x, y, z), new Rotation(0, 90, 0), Color, Vector3.Down);
		}

		private void FaceDataEast(int x, int y, int z, MeshData meshData)
		{
			meshData.AddFace(new Vector3(x, y, z), new Rotation(90, 0, 0), Color, Vector3.Right);
		}

		private void FaceDataNorth(int x, int y, int z, MeshData meshData)
		{
			meshData.AddFace(new Vector3(x, y, z), new Rotation(0, 0, 0), Color, Vector3.ForwardLH);
		}

		private void FaceDataSouth(int x, int y, int z, MeshData meshData)
		{
			meshData.AddFace(new Vector3(x, y, z), new Rotation(180, 0, 0), Color, Vector3.BackwardLH);
		}

		private void FaceDataUp(int x, int y, int z, MeshData meshData)
		{
			meshData.AddFace(new Vector3(x, y, z), new Rotation(0, -90, 0), Color, Vector3.Up);
		}

		private void FaceDataWest(int x, int y, int z, MeshData meshData)
		{
			meshData.AddFace(new Vector3(x, y, z), new Rotation(-90, 0, 0), Color, Vector3.Left);
		}
	}
}

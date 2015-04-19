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
			if (Solid)
			{
				if (!chunk.GetBlock(new Vector3(x, y + 1, z)).Solid)
					FaceDataUp(x, y, z, meshData);
				if (!chunk.GetBlock(new Vector3(x, y - 1, z)).Solid)
					FaceDataDown(x, y, z, meshData);
				if (!chunk.GetBlock(new Vector3(x, y, z + 1)).Solid)
					FaceDataNorth(x, y, z, meshData);
				if (!chunk.GetBlock(new Vector3(x, y, z - 1)).Solid)
					FaceDataSouth(x, y, z, meshData);
				if (!chunk.GetBlock(new Vector3(x + 1, y, z)).Solid)
					FaceDataEast(x, y, z, meshData);
				if (!chunk.GetBlock(new Vector3(x - 1, y, z)).Solid)
					FaceDataWest(x, y, z, meshData);
			}
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", Solid, Color);
		}

		private void FaceDataDown(int x, int y, int z, MeshData meshData)
		{
			meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
			meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
			meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
			meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

			meshData.AddQuadTriangles();
			meshData.AddNormal(Vector3.Down);
			meshData.AddColor(Color);
		}

		private void FaceDataEast(int x, int y, int z, MeshData meshData)
		{
			meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
			meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
			meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
			meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

			meshData.AddQuadTriangles();
			meshData.AddNormal(Vector3.Right);
			meshData.AddColor(Color);
		}

		private void FaceDataNorth(int x, int y, int z, MeshData meshData)
		{
			meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
			meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
			meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
			meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

			meshData.AddQuadTriangles();
			meshData.AddNormal(Vector3.ForwardLH);
			meshData.AddColor(Color);
		}

		private void FaceDataSouth(int x, int y, int z, MeshData meshData)
		{
			meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
			meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
			meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
			meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

			meshData.AddQuadTriangles();
			meshData.AddNormal(Vector3.BackwardLH);
			meshData.AddColor(Color);
		}

		private void FaceDataUp(int x, int y, int z, MeshData meshData)
		{
			meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
			meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
			meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
			meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));

			meshData.AddQuadTriangles();
			meshData.AddNormal(Vector3.Up);
			meshData.AddColor(Color);
		}

		private void FaceDataWest(int x, int y, int z, MeshData meshData)
		{
			meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
			meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
			meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
			meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

			meshData.AddQuadTriangles();
			meshData.AddNormal(Vector3.Left);
			meshData.AddColor(Color);
		}
	}
}

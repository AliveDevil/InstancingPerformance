using InstancingPerformance.Core;
using SharpDX;

namespace InstancingPerformance.Primitives
{
	public class Chunk
	{
		private Block[] blocks;

		public Block this[int index]
		{
			get { return blocks[index]; }
		}

		public bool Altered { get; set; }

		public int ChunkSize { get; private set; }

		public int Length { get; private set; }

		public MeshData MeshData { get; private set; }

		public Vector3 Position { get; private set; }

		public Vector3 WorldPosition { get { return Position * ChunkSize; } }

		public Chunk(Vector3 position, int chunksize)
		{
			Position = position;
			ChunkSize = chunksize;
			blocks = new Block[Length = chunksize * chunksize * chunksize];
			Altered = true;
			MeshData = new MeshData();
		}

		public void BuildMesh()
		{
			if (Altered)
			{
				Altered = false;
				MeshData.Clear();
				for (int i = 0; i < Length; i++)
				{
					Vector3 v = i.ResolveIndex(ChunkSize, ChunkSize, ChunkSize);
					blocks[i].MeshData(this, (int)v.X, (int)v.Y, (int)v.Z, MeshData);
				}
			}
		}

		public Block GetBlock(Vector3 position)
		{
			if (position.X < 0 || position.X >= ChunkSize || position.Y < 0 || position.Y >= ChunkSize || position.Z < 0 || position.Z >= ChunkSize)
				return Block.Empty;
			int i = position.IndiceVector(ChunkSize, ChunkSize, ChunkSize);
			return blocks[i];
		}

		public bool SetBlock(Vector3 position, Block block)
		{
			if (position.X < 0 || position.X >= ChunkSize || position.Y < 0 || position.Y >= ChunkSize || position.Z < 0 || position.Z >= ChunkSize)
				return false;
			int i = position.IndiceVector(ChunkSize, ChunkSize, ChunkSize);
			if (blocks[i] == block)
				return false;
			blocks[i] = block;
			Altered = true;
			return true;
		}
	}
}

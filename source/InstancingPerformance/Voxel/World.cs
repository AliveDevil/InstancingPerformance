using System.Collections.Generic;
using InstancingPerformance.Core;
using InstancingPerformance.Primitives;
using SharpDX;
using System.Linq;

namespace InstancingPerformance.Voxel
{
	public class World : AppObject, IDraw, IUpdate
	{
		private ObjectPool<Voxel.Chunk, Vector3> chunkPool;
		private Dictionary<Vector3, Primitives.Chunk> map;

		public Vector3 LoadReference { get; set; }

		public int ChunkSize { get; private set; }

		public int ViewDistance { get; private set; }

		private int FullViewDistance { get { return ViewDistance * 2 + 1; } }

		private int FullViewLength { get { return FullViewDistance * FullViewDistance * FullViewDistance; } }

		public Vector3 ViewModifier { get { return new Vector3(ViewDistance, ViewDistance, ViewDistance); } }

		public World(App app, int chunkSize, int viewDistance)
			: base(app)
		{
			ChunkSize = chunkSize;
			ViewDistance = viewDistance;
			map = new Dictionary<Vector3, Primitives.Chunk>();
			chunkPool = new ObjectPool<Voxel.Chunk, Vector3>(() => new Chunk(this));
		}

		public void Draw(double time)
		{
			foreach (var item in chunkPool.ActiveObjects)
			{
				if (item.CanDraw())
				{
					App.ActiveShader.GetMatrix("World").SetMatrix(Matrix.Translation(item.WorldPosition));
					App.ApplyShader();
					item.Draw(time);
				}
			}
		}

		public Block GetBlock(Vector3 position)
		{
			Vector3 chunkIndex = position.FloorDiv(ChunkSize);
			Primitives.Chunk chunk;
			if (!map.TryGetValue(chunkIndex, out chunk))
				return Block.Empty;
			return chunk.GetBlock(position.Mod(ChunkSize));
		}

		public void SetBlock(Vector3 position, Block block)
		{
			Vector3 chunkIndex = position.FloorDiv(ChunkSize);
			Primitives.Chunk chunk;
			if (!map.TryGetValue(chunkIndex, out chunk))
			{
				chunk = new Primitives.Chunk(chunkIndex, ChunkSize);
				map.Add(chunkIndex, chunk);
			}
			chunk.SetBlock(position.Mod(ChunkSize), Block.Green);
		}

		public void SetBlocks(IEnumerable<BlockInsert> inserts)
		{
			foreach (var insert in inserts)
			{
				SetBlock(insert.Position, insert.Block);
			}
		}

		public void Update(double time)
		{
			var alteredChunk = map.FirstOrDefault(kvp => kvp.Value.Altered).Value;
			if (alteredChunk != null)
				alteredChunk.BuildMesh();

			chunkPool.UpdateObjects();

			for (int i = 0; i < FullViewLength; i++)
			{
				Vector3 chunkPosition = i.ResolveIndex(FullViewDistance, FullViewDistance, FullViewDistance) - ViewModifier + LoadReference;
				Primitives.Chunk chunk;
				map.TryGetValue(chunkPosition, out chunk);

				if (!chunkPool.ContainsKey(chunkPosition))
				{
					Voxel.Chunk voxelChunk = chunkPool.GetObject(chunkPosition);
					voxelChunk.SetChunk(chunk);
				}
			}

			foreach (var chunk in chunkPool.ActiveObjects)
			{
				chunk.Update(time);
			}
		}
	}
}

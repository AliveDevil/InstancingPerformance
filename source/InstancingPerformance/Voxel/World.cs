using System;
using System.Collections.Generic;
using System.Linq;
using InstancingPerformance.Core;
using InstancingPerformance.Primitives;
using SharpDX;

namespace InstancingPerformance.Voxel
{
	public class World : AppObject, IDraw, IUpdate
	{
		private HashSet<Action> bufferedActions;
		private ObjectPool<Voxel.Chunk, Vector3> chunkPool;
		private Dictionary<Vector3, Primitives.Chunk> map;

		public int ChunkSize { get; private set; }

		public int DrawChunkCount { get { return chunkPool.ActiveObjects.Count(chunk => chunk.CanDraw()); } }

		public Vector3 LoadReference { get; set; }

		public int MapChunkCount { get { return map.Count; } }

		public int TriangleCount { get { return chunkPool.ActiveObjects.Select(c => c.TriangleCount).Sum(); } }

		public int ViewDistance { get; private set; }

		public Vector3 ViewModifier { get { return new Vector3(ViewDistance, ViewDistance, ViewDistance); } }

		private int FullViewDistance { get { return ViewDistance * 2 + 1; } }

		private int FullViewLength { get { return FullViewDistance * FullViewDistance * FullViewDistance; } }

		public World(App app, int chunkSize, int viewDistance)
			: base(app)
		{
			ChunkSize = chunkSize;
			ViewDistance = viewDistance;
			map = new Dictionary<Vector3, Primitives.Chunk>();
			chunkPool = new ObjectPool<Voxel.Chunk, Vector3>(() => new Chunk(this));
			bufferedActions = new HashSet<Action>();
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
			chunk.SetBlock(position.Mod(ChunkSize), block);
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
			using (Watch watch = new Watch())
			{
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

				IEnumerator<Chunk> chunkEnumerator = chunkPool.ActiveObjects.GetEnumerator();
				while (watch.Elapsed < TimeSpan.FromSeconds(0.033) && chunkEnumerator.MoveNext())
				{
					chunkEnumerator.Current.Update(time);
				}
			}
		}

		public void UseBasicMode()
		{
		}

		public void UseHardwareMode()
		{
		}
	}
}

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
		private HashSet<Action> bufferedActions => new HashSet<Action>();
		private ObjectPool<Chunk, Vector3> chunkPool => new ObjectPool<Chunk, Vector3>(() => new Chunk(this));
		private Dictionary<Vector3, Primitives.Chunk> map => new Dictionary<Vector3, Primitives.Chunk>();

		public int ViewDistance { get; }
		public int ChunkSize { get; }
		public Vector3 LoadReference { get; set; }
		public int DrawChunkCount => chunkPool.ActiveObjects.Count(chunk => chunk.CanDraw);
		public int MapChunkCount => map?.Count ?? 0;
		public int TriangleCount => chunkPool.ActiveObjects.Select(c => c.TriangleCount).Sum();
		public Vector3 ViewModifier => new Vector3(ViewDistance, ViewDistance, ViewDistance);
		public int FullViewDistance => ViewDistance * 2 + 1;
		public int FullViewLength => FullViewDistance * FullViewDistance * FullViewDistance;

		public World(App app, int chunkSize, int viewDistance)
			: base(app)
		{
			ChunkSize = chunkSize;
			ViewDistance = viewDistance;
		}

		public void Draw(double time)
		{
			foreach (var item in chunkPool.ActiveObjects)
			{
				if (item.CanDraw)
				{
					App.ActiveShader.Matrix("World").SetMatrix(Matrix.Translation(item.WorldPosition));
					App.ApplyShader("Basic");
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

		public void SetBlocks(IEnumerable<BlockInsert> inserts) => inserts.Loop(insert => SetBlock(insert.Position, insert.Block));

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
						Chunk voxelChunk = chunkPool.GetObject(chunkPosition);
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

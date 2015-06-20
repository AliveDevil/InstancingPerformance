using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InstancingPerformance.Core;
using InstancingPerformance.Primitives;
using SharpDX;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace InstancingPerformance.Voxel
{
	public class World : AppObject, IDraw, IUpdate
	{
		private DrawMode drawMode;

		private ObjectPool<Chunk, Vector3> chunkPool { get; }
		private Dictionary<Vector3, Primitives.Chunk> map { get; }
		private Buffer worldBuffer { get; }

		public int ViewDistance { get; }
		public int ChunkSize { get; }
		public Vector3 LoadReference { get; set; }
		public int DrawChunkCount => chunkPool.ActiveObjects.Count(chunk => chunk.CanDraw);
		public int MapChunkCount => map?.Count ?? 0;
		public int TriangleCount => chunkPool.ActiveObjects.Select(c => c.TriangleCount).Sum();
		public Vector3 ViewModifier => new Vector3(ViewDistance, ViewDistance, ViewDistance);
		public int FullViewDistance => ViewDistance * 2 + 1;
		public int FullViewLength => FullViewDistance * FullViewDistance * FullViewDistance;
		public WorldBuffer WorldSetup;

		public World(App app, Buffer worldBuffer, int chunkSize, int viewDistance)
		: base(app)
		{
			ChunkSize = chunkSize;
			ViewDistance = viewDistance;

			chunkPool = new ObjectPool<Chunk, Vector3>(() => new Chunk(this));
			map = new Dictionary<Vector3, Primitives.Chunk>();
			this.worldBuffer = worldBuffer;
		}

		public void Draw(TimeSpan totalTime, TimeSpan frameTime, double time)
		{
			int totalVertexCount = 0, drawnVertexCount = 0, totalTriangleCount = 0, chunkCount = 0;

			foreach (var chunk in chunkPool.ActiveObjects)
			{
				if (chunk.CanDraw)
				{
					WorldSetup.World = Matrix.Transpose(Matrix.Translation(chunk.WorldPosition));
					DataStream data;
					Context.MapSubresource(worldBuffer, 0, MapMode.WriteDiscard, MapFlags.None, out data);
					using (data) data.Write(WorldSetup);
					Context.UnmapSubresource(worldBuffer, 0);
					chunk.Draw(totalTime, frameTime, time);
					totalVertexCount += chunk.VertexCount;
					drawnVertexCount += chunk.GraphicsVertexCount;
					totalTriangleCount += chunk.TriangleCount;
					chunkCount++;
				}
			}
			App.Statistics.AddRecord(frameTime, totalVertexCount, drawnVertexCount, totalTriangleCount, chunkCount);
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
				while (watch.Elapsed < TimeSpan.FromSeconds(0.016) && chunkEnumerator.MoveNext())
				{
					chunkEnumerator.Current.Update(time);
				}
				chunkEnumerator.Dispose();
			}
		}
	}
}

using InstancingPerformance.Core;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace InstancingPerformance.Voxel
{
	public class Chunk : AppObject, IDraw, IUpdate, IPooled<Vector3>
	{
		public Vector3 Position { get; set; }
		public Vector3 WorldPosition { get { return Position * world.ChunkSize; } }
		public Primitives.Chunk ActiveChunk { get; private set; }
		private Buffer indexBuffer;
		private Buffer instanceBuffer;
		private MeshData meshData;
		private bool updateRequired = true;
		private VertexBufferBinding vertexBinding;
		private Buffer vertexBuffer;
		private int vertexStride;
		private World world;

		public bool IsActive
		{
			get { return Helper.Distance(Position, world.LoadReference) <= world.ViewDistance; }
		}

		public Vector3 Key
		{
			get { return Position; }
			set { Position = value; }
		}

		public Chunk(World world)
			: base(world.App)
		{
			this.world = world;
		}

		public bool CanDraw()
		{
			return ActiveChunk != null && vertexBuffer != null;
		}

		public void Draw(double time)
		{
			if (CanDraw())
			{
				Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
				Context.InputAssembler.SetVertexBuffers(0, vertexBinding);
				Context.InputAssembler.SetIndexBuffer(indexBuffer, Format.R32_SInt, 0);
				Context.DrawIndexed(ActiveChunk.MeshData.Triangles.Count, 0, 0);
			}
		}

		public void SetChunk(Primitives.Chunk chunk)
		{
			if (this.ActiveChunk != chunk)
			{
				this.ActiveChunk = chunk;
				updateRequired = true;
				if (chunk != null)
					meshData = this.ActiveChunk.MeshData;
				else
					meshData = null;
			}
		}

		public void Update(double time)
		{
			if ((updateRequired || (ActiveChunk != null && ActiveChunk.Altered)))
			{
				updateRequired = false;
				UpdateChunk();
				RenderMesh();
			}
		}

		private void RenderMesh()
		{
			Utilities.Dispose(ref vertexBuffer);
			Utilities.Dispose(ref instanceBuffer);
			Utilities.Dispose(ref indexBuffer);

			if (ActiveChunk != null && ActiveChunk.MeshData.VertexCount > 0)
			{
				ActiveChunk.MeshData.BasicBuffer(Device, out vertexStride, out vertexBuffer, out indexBuffer);
				vertexBinding = new VertexBufferBinding(vertexBuffer, vertexStride, 0);
			}
		}

		private void UpdateChunk()
		{
			if (ActiveChunk != null)
				ActiveChunk.BuildMesh();
		}

		public override int GetHashCode()
		{
			return Position.GetHashCode();
		}
	}
}

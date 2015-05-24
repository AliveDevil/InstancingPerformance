using InstancingPerformance.Core;
using InstancingPerformance.Primitives;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace InstancingPerformance.Voxel
{
	public class Chunk : AppObject, IDraw, IUpdate, IPooled<Vector3>
	{
		public static readonly Face Face = new Face(
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f));

		private Buffer indexBuffer;
		private int indexCount;
		private VertexBufferBinding instanceBinding;
		private Buffer instanceBuffer;
		private int instanceCount;
		private int instanceStride;
		private MeshData meshData;
		private bool updateRequired = true;
		private VertexBufferBinding vertexBinding;
		private Buffer vertexBuffer;
		private int vertexCount;
		private int vertexPerInstance;
		private int vertexStride;
		private World world;

		public Primitives.Chunk ActiveChunk { get; private set; }

		public bool CanDraw => vertexBuffer != null;

		public bool IsActive => Helper.Distance(Position, world.LoadReference) <= world.ViewDistance;

		public Vector3 Key
		{
			get { return Position; }
			set { Position = value; }
		}

		public Vector3 Position { get; set; }

		public int TriangleCount => meshData.TriangleCount;

		public Vector3 WorldPosition => Position * world.ChunkSize;

		public Chunk(World world)
			: base(world.App)
		{
			this.world = world;
			meshData = new MeshData();
			meshData.UseFace(Face, Vector3.ForwardLH);
		}

		public void Draw(double time)
		{
			if (CanDraw)
			{
				Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
				switch (world.DrawMode)
				{
					case DrawMode.Basic:
						Context.InputAssembler.SetIndexBuffer(indexBuffer, Format.R32_UInt, 0);
						Context.InputAssembler.SetVertexBuffers(0, vertexBinding);
						Context.DrawIndexed(indexCount, 0, 0);
						break;

					case DrawMode.Instance:
						Context.InputAssembler.SetIndexBuffer(indexBuffer, Format.R32_UInt, 0);
						Context.InputAssembler.SetVertexBuffers(0, vertexBinding, instanceBinding);
						Context.DrawIndexedInstanced(6, instanceCount, 0, 0, 0);
						break;

					case DrawMode.Geometry:
						break;
				}
			}
		}

		public override int GetHashCode()
		{
			return Position.GetHashCode();
		}

		public void Invalidate()
		{
			updateRequired = true;
		}

		public void Reset()
		{
			ActiveChunk = null;
			ResetBuffer();
		}

		public void SetChunk(Primitives.Chunk chunk)
		{
			if (this.ActiveChunk != chunk)
			{
				this.ActiveChunk = chunk;
				updateRequired = true;
			}
		}

		public void Update(double time)
		{
			if (updateRequired || (ActiveChunk != null && ActiveChunk.Updated))
			{
				updateRequired = false;
				ResetBuffer();
				if (ActiveChunk != null)
				{
					ActiveChunk.Updated = false;
					UpdateChunk();
					RenderMesh();
				}
			}
		}

		private void RenderMesh()
		{
			meshData.DrawMode = world.DrawMode;
			meshData.Create(Device, out vertexStride, out vertexCount, out vertexBuffer, out instanceStride, out instanceCount, out vertexPerInstance, out instanceBuffer, out indexCount, out indexBuffer);
			vertexBinding = new VertexBufferBinding(vertexBuffer, vertexStride, 0);
			instanceBinding = new VertexBufferBinding(instanceBuffer, instanceStride, 0);
		}

		private void ResetBuffer()
		{
			meshData.Clear();
			Utilities.Dispose(ref vertexBuffer);
			Utilities.Dispose(ref instanceBuffer);
			Utilities.Dispose(ref indexBuffer);
		}

		private void UpdateChunk()
		{
			ActiveChunk.BuildMesh(meshData);
		}
	}
}

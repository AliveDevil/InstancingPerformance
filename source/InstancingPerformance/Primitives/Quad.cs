using System.Runtime.InteropServices;
using InstancingPerformance.Core;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace InstancingPerformance.Primitives
{
	public class Quad : AppObject
	{
		private VertexBufferBinding binding;
		private Buffer vertexBuffer;

		public Quad(App app, float left, float bottom, float width, float height)
			: base(app)
		{
			vertexBuffer = Buffer.Create(Device, BindFlags.VertexBuffer, new VertexPositionTexture[]
			{
				new VertexPositionTexture(left, bottom, 0, 0),
				new VertexPositionTexture(left, bottom + height, 0, 1),
				new VertexPositionTexture(left + width, bottom, 1, 0),
				new VertexPositionTexture(left + width, bottom + height, 1, 1)
			}, 64, ResourceUsage.Immutable, CpuAccessFlags.None, ResourceOptionFlags.None, 16);
			binding = new VertexBufferBinding(vertexBuffer, 16, 0);
		}

		public void Draw()
		{
			Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleStrip;
			Context.InputAssembler.SetVertexBuffers(0, binding);
			Context.Draw(4, 0);
		}

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				vertexBuffer.Dispose();
			}
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct VertexPositionTexture
		{
			[FieldOffset(0)]
			public Vector2 Position;

			[FieldOffset(8)]
			public Vector2 Texture;

			public VertexPositionTexture(float x, float y, float u, float v)
			{
				Position.X = x;
				Position.Y = y;
				Texture.X = u;
				Texture.Y = v;
			}
		}
	}
}

using System.Runtime.InteropServices;
using SharpDX;

namespace InstancingPerformance.Primitives
{
	[StructLayout(LayoutKind.Explicit, Size = 192)]
	public struct WorldBuffer
	{
		[FieldOffset(0)]
		private Matrix world;
		[FieldOffset(64)]
		private Matrix view;
		[FieldOffset(128)]
		private Matrix projection;

		public Matrix World { get { return world; } set { world = value; } }

		public Matrix View { get { return view; } set { view = value; } }

		public Matrix Projection { get { return projection; } set { projection = value; } }

		public void SetWorld(Matrix matrix) => world = matrix;

		public void SetView(Matrix matrix) => view = matrix;

		public void SetProjection(Matrix matrix) => projection = matrix;
	}
}

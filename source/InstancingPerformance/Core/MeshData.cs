using System.Collections.Generic;
using System.Threading.Tasks;
using InstancingPerformance.Primitives;
using SharpDX;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace InstancingPerformance.Core
{
	public class MeshData
	{
		public List<Color> Colors = new List<Color>();
		public List<Vector3> Normals = new List<Vector3>();
		public List<int> Triangles = new List<int>();
		public List<Vector3> Vertices = new List<Vector3>();

		private VertexPositionNormalColor[] vertexPositionColor;

		public int TriangleCount { get { return Triangles.Count; } }

		public int VertexCount { get { return Vertices.Count; } }

		public void AddColor(Color color)
		{
			Colors.Add(color);
		}

		public void AddNormal(Vector3 n)
		{
			Normals.Add(n);
		}

		public void AddQuadTriangles()
		{
			AddTriangle(Vertices.Count - 4); // 0
			AddTriangle(Vertices.Count - 3); // 1
			AddTriangle(Vertices.Count - 2); // 2

			AddTriangle(Vertices.Count - 4); // 0
			AddTriangle(Vertices.Count - 2); // 2
			AddTriangle(Vertices.Count - 1); // 3
		}

		public void AddTriangle(int tri)
		{
			Triangles.Add(tri);
		}

		public void AddVertex(Vector3 v)
		{
			Vertices.Add(v);
		}

		public void BasicBuffer(Device device, out int stride, out Buffer vertexBuffer, out Buffer indexBuffer)
		{
			stride = (3 * 4) + (4 * 4) + 4;

			vertexBuffer = Buffer.Create(device, BindFlags.VertexBuffer, ToVertexPositionColor(), Vertices.Count * stride, ResourceUsage.Immutable, CpuAccessFlags.None, ResourceOptionFlags.None, stride);

			indexBuffer = Buffer.Create(device, BindFlags.IndexBuffer, Triangles.ToArray(), Triangles.Count * 4, ResourceUsage.Immutable, CpuAccessFlags.None, ResourceOptionFlags.None, 4);
		}

		public void Clear()
		{
			Vertices.Clear();
			Normals.Clear();
			Colors.Clear();
			Triangles.Clear();
			vertexPositionColor = null;
		}

		public VertexPositionNormalColor[] ToVertexPositionColor()
		{
			if (vertexPositionColor == null)
			{
				vertexPositionColor = new VertexPositionNormalColor[Vertices.Count];
				Parallel.For(0, vertexPositionColor.Length, i =>
				{
					vertexPositionColor[i] = new VertexPositionNormalColor(Vertices[i], Normals[i / 4], Colors[i / 4]);
				});
			}
			return vertexPositionColor;
		}
	}
}

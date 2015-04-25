using System.Collections.Generic;
using InstancingPerformance.Primitives;
using SharpDX;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace InstancingPerformance.Core
{
	public class MeshData
	{
		public List<Color> Colors = new List<Color>();
		public DrawMode DrawMode = DrawMode.Basic;
		public Face Face;
		public List<Vector3> Normals = new List<Vector3>();
		public List<Vector3> Positions = new List<Vector3>();
		public List<Rotation> Rotations = new List<Rotation>();
		public List<GeometryInfo> GeometryInfos = new List<GeometryInfo>();
		private int count = 0;

		public int TriangleCount { get; private set; }

		public int VertexCount { get; private set; }

		public void AddGeometryInfo(Vector3 position, Color color, int @case)
		{
			GeometryInfos.Add(new GeometryInfo(position, color, @case));
		}

		public void AddFace(Vector3 position, Rotation rotation, Color color, Vector3 normal)
		{
			Positions.Add(position);
			Rotations.Add(rotation);
			Colors.Add(color);
			Normals.Add(normal);
			count++;
		}

		public void Clear()
		{
			count = 0;
			Positions.Clear();
			Normals.Clear();
			Rotations.Clear();
			Colors.Clear();
			GeometryInfos.Clear();
		}

		public void Create(
			Device device,
			out int vertexStride,
			out int vertexCount,
			out Buffer vertexBuffer,
			out int instanceStride,
			out int instanceCount,
			out int vertexPerInstance,
			out Buffer instanceBuffer,
			out int indexCount,
			out Buffer indexBuffer)
		{
			//switch (DrawMode)
			//{
			//	case DrawMode.Basic:
			instanceStride = 0;
			instanceCount = 0;
			vertexPerInstance = 0;
			instanceBuffer = null;
			vertexStride = (3 * 4) + (4 * 4) + 4;
			List<VertexPositionNormalColor> vertices = new List<VertexPositionNormalColor>();
			List<int> indices = new List<int>();
			for (int i = 0; i < count; i++)
			{
				Vector3 basePosition = Positions[i];
				Rotation rotation = Rotations[i];
				Vector3 normal = Normals[i];
				Color color = Colors[i];
				Face face = Face * rotation + basePosition;
				vertices.Add(new VertexPositionNormalColor(face.BottomLeft, normal, color));
				vertices.Add(new VertexPositionNormalColor(face.TopLeft, normal, color));
				vertices.Add(new VertexPositionNormalColor(face.TopRight, normal, color));
				vertices.Add(new VertexPositionNormalColor(face.BottomRight, normal, color));
				int index = i * 4;
				indices.AddRange(new[]
				{
					index + 0, index + 1, index + 2,
					index + 0, index + 2, index + 3
				});
			}
			vertexCount = vertices.Count;
			indexCount = indices.Count;
			if (vertexCount > 0)
				vertexBuffer = Buffer.Create(device, BindFlags.VertexBuffer, vertices.ToArray(), vertexStride * vertexCount, ResourceUsage.Immutable, CpuAccessFlags.None, ResourceOptionFlags.None, vertexStride);
			else
				vertexBuffer = null;
			if (indexCount > 0)
				indexBuffer = Buffer.Create(device, BindFlags.IndexBuffer, indices.ToArray(), 4 * indexCount, ResourceUsage.Immutable, CpuAccessFlags.None, ResourceOptionFlags.None, 4);
			else
				indexBuffer = null;
			TriangleCount = indexCount;
			VertexCount = vertexCount;
			//break;

			//case DrawMode.Hardware:
			//	break;

			//case DrawMode.Geometry:
			//	break;
			//}
		}

		public void UseFace(Face face)
		{
			Face = face;
		}
	}
}

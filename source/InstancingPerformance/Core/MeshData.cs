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
		public Vector3 FaceNormal;
		public List<GeometryInfo> GeometryInfos = new List<GeometryInfo>();
		public List<Vector3> Normals = new List<Vector3>();
		public List<Vector3> Positions = new List<Vector3>();
		public List<Rotation> Rotations = new List<Rotation>();
		private int count = 0;

		public int GraphicsVertexCount { get; private set; }
		public int TriangleCount { get; private set; }
		public int VertexCount { get; private set; }

		public void AddFace(Vector3 position, Rotation rotation, Color color, Vector3 normal)
		{
			Positions.Add(position);
			Rotations.Add(rotation);
			Colors.Add(color);
			Normals.Add(normal);
			count++;
		}

		public void AddGeometryInfo(Vector3 position, Color color, uint @case)
		{
			GeometryInfos.Add(new GeometryInfo(position, color, @case));
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
			vertexStride = 0;
			vertexCount = 0;
			vertexBuffer = null;
			instanceStride = 0;
			instanceCount = 0;
			vertexPerInstance = 0;
			instanceBuffer = null;
			indexCount = 0;
			indexBuffer = null;

			switch (DrawMode)
			{
				case DrawMode.Basic:
					{
						vertexStride = (3 * 4) + (4 * 4) + (4 * 4);
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
						if (indexCount > 0)
							indexBuffer = Buffer.Create(device, BindFlags.IndexBuffer, indices.ToArray(), 4 * indexCount, ResourceUsage.Immutable, CpuAccessFlags.None, ResourceOptionFlags.None, 4);
						VertexCount = vertexCount;
						GraphicsVertexCount = indexCount;
						TriangleCount = indexCount / 3;
						break;
					}

				case DrawMode.Instance:
					{
						vertexStride = (4 * 4) + (3 * 4);
						instanceStride = (4 * 4 * 4) + (4 * 4);
						List<VertexWorldColor> instances = new List<VertexWorldColor>();
						VertexPositionNormal[] vertices =
						{
							new VertexPositionNormal(Face.BottomLeft, FaceNormal),
							new VertexPositionNormal(Face.TopLeft, FaceNormal),
							new VertexPositionNormal(Face.TopRight, FaceNormal),
							new VertexPositionNormal(Face.BottomRight, FaceNormal)
						};
						int[] indices = { 0, 1, 2, 0, 2, 3 };
						for (int i = 0; i < count; i++)
						{
							instances.Add(new VertexWorldColor(Rotations[i].Matrix * Matrix.Translation(Positions[i]), Colors[i]));
						}
						vertexCount = 4;
						indexCount = 6;
						instanceCount = instances.Count;

						if (vertexCount > 0)
							vertexBuffer = Buffer.Create(device, BindFlags.VertexBuffer, vertices, vertexStride * vertexCount, ResourceUsage.Immutable, CpuAccessFlags.None, ResourceOptionFlags.None, vertexStride);
						if (instanceCount > 0)
							instanceBuffer = Buffer.Create(device, BindFlags.VertexBuffer, instances.ToArray(), instanceStride * instanceCount, ResourceUsage.Immutable, CpuAccessFlags.None, ResourceOptionFlags.None, instanceStride);
						if (indexCount > 0)
							indexBuffer = Buffer.Create(device, BindFlags.IndexBuffer, indices, 4 * indexCount, ResourceUsage.Default, CpuAccessFlags.None, ResourceOptionFlags.None, 4);
						VertexCount = vertexCount + instanceCount;
						GraphicsVertexCount = instanceCount * indexCount;
						TriangleCount = (instanceCount * indexCount) / 3;
						break;
					}

				case DrawMode.Geometry:
					{
						int tempVertexCount = 0;
						vertexStride = (4 * 4) + 4 + (4 * 4);
						List<VertexPositionCaseColor> vertices = new List<VertexPositionCaseColor>();
						for (int i = 0; i < GeometryInfos.Count; i++)
						{
							GeometryInfo info = GeometryInfos[i];
							tempVertexCount += info.VertexCount;
							vertices.Add(new VertexPositionCaseColor(info.Position, info.Case, info.Color));
						}
						vertexCount = vertices.Count;
						if (vertexCount > 0)
							vertexBuffer = Buffer.Create(device, BindFlags.VertexBuffer, vertices.ToArray(), vertexStride * vertexCount, ResourceUsage.Immutable, CpuAccessFlags.None, ResourceOptionFlags.None, vertexStride);
						VertexCount = vertexCount;
						GraphicsVertexCount = tempVertexCount;
						TriangleCount = tempVertexCount / 3;
						break;
					}
			}
		}

		public void UseFace(Face face, Vector3 normal)
		{
			Face = face;
			FaceNormal = normal;
		}
	}
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InstancingPerformance.Content;
using InstancingPerformance.Core;
using InstancingPerformance.Primitives;
using InstancingPerformance.Voxel;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Linq;
using SharpDX.DirectInput;
using System.Dynamic;
using System;
using System.Text;
using System.Diagnostics;

namespace InstancingPerformance.Screens
{
	public class Scene : Screen
	{
		private Shader basicShader;
		private PathCamera camera;
		private World world;
		private double lastFrameTime;

		public Scene(App app)
			: base(app)
		{
			camera = new PathCamera(App);
			camera.NearPlane = 0.3f;
			camera.FarPlane = 1024;
			camera.Rotation.Yaw = 0;
			camera.Rotation.Pitch = 0;

			const int m = 20;
			camera.AddWayPoint(new WayPoint(0 * m, -232, 32, -232, 0.7853f, 1.571f, 0));
			camera.AddWayPoint(new WayPoint(1 * m, 180, 64, 180, 0.7853f, 0.5f, 0));
			camera.AddWayPoint(new WayPoint(2 * m, -240, 40, 240, 2.356f, 0.78f, 0));
			camera.AddWayPoint(new WayPoint(3 * m, 0, 40, 0, 2.356f, 0.25f, 0));
			camera.AddWayPoint(new WayPoint(4 * m, 230, 50, 180, 0, -0.1f, 0));
			camera.AddWayPoint(new WayPoint(5 * m, -232, 32, -232, 7.068f, 1.571f, 0));

			world = new World(App, 16, 8);

			basicShader = ResourceManager.Shader("Basic.fx",
				new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0),
				new InputElement("NORMAL", 0, Format.R32G32B32_Float, 0),
				new InputElement("COLOR", 0, Format.R8G8B8A8_UNorm, 0));

			using (Heightmap heightmap = new Heightmap(App, 64, "ip_heightmap.png"))
			using (ColorMap colormap = new ColorMap(App, "ip_color.png"))
			{
				MapGenerator chunkGenerator = new MapGenerator(heightmap, colormap, world.ChunkSize, 8);
				IEnumerator<BlockInsert[]> enumerator = chunkGenerator.GetEnumerator();

				while (enumerator.MoveNext())
				{
					world.SetBlocks(enumerator.Current.Select(insert =>
					{
						insert.Position -= new Vector3(heightmap.Width, 0, heightmap.Height) / 2;
						return insert;
					}));
				}
			}
		}

		public override void Update(double time)
		{
			camera.Update(time);
			world.LoadReference = camera.Position.FloorDiv(world.ChunkSize);
			world.Update(time);

			if (KeyState.IsPressed(Key.D))
			{
				var dump = new
				{
					FrameTime = lastFrameTime,
					MapChunkCount = world.MapChunkCount,
					DrawChunkCount = world.DrawChunkCount,
					TriangleCount = world.TriangleCount
				};
				StringBuilder builder = new StringBuilder();
				builder.AppendLine(string.Format("Dump at {0}", DateTime.Now));
				builder.AppendLine(string.Format("{0}: {1}", "FrameTime", dump.FrameTime));
				builder.AppendLine(string.Format("{0}: {1}", "MapChunkCount", dump.MapChunkCount));
				builder.AppendLine(string.Format("{0}: {1}", "DrawChunkCount", dump.DrawChunkCount));
				builder.AppendLine(string.Format("{0}: {1}", "TriangleCount", dump.TriangleCount));
				Trace.TraceInformation(builder.ToString());
			}
		}

		public override void Draw(double time)
		{
			lastFrameTime = time;
			App.UseShader(basicShader);
			basicShader.GetMatrix("View").SetMatrix(camera.View);
			basicShader.GetMatrix("Projection").SetMatrix(camera.Projection);
			basicShader.GetVector("LightColor").Set(Color.Black);
			basicShader.GetVector("LightDirection").Set(Vector3.Normalize(new Vector3(0.5f, -1, 2f)));
			world.Draw(time);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using InstancingPerformance.Content;
using InstancingPerformance.Core;
using InstancingPerformance.Voxel;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DirectInput;
using SharpDX.DXGI;

namespace InstancingPerformance.Screens
{
	public class Scene : Screen
	{
		private Shader basicShader;
		private PathCamera camera;
		private double lastFrameTime;
		private World world;

		public Scene(App app)
			: base(app)
		{
			camera = new PathCamera(App);
			camera.NearPlane = 0.3f;
			camera.FarPlane = 1024;
			camera.Rotation.Yaw = 0;
			camera.Rotation.Pitch = 0;

			const int m = 20;
			camera.AddWayPoint(new WayPoint(0 * m, -232, 32, -232, 45, 60, 0));
			camera.AddWayPoint(new WayPoint(1 * m, 180, 64, 180, 45, 25, 0));
			camera.AddWayPoint(new WayPoint(2 * m, -240, 40, 240, 135, 45, 0));
			camera.AddWayPoint(new WayPoint(3 * m, 0, 40, 0, 135, 30, 0));
			camera.AddWayPoint(new WayPoint(4 * m, 230, 50, 180, 0, -0.1f, 0));
			camera.AddWayPoint(new WayPoint(5 * m, -232, 32, -232, 440, 1.571f, 0));

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

		public override void Draw(double time)
		{
			lastFrameTime = time;
			App.UseShader(basicShader);
			basicShader.GetMatrix("View").SetMatrix(camera.View);
			basicShader.GetMatrix("Projection").SetMatrix(camera.Projection);
			basicShader.GetVector("LightColor").Set(Color.White);
			basicShader.GetVector("LightDirection").Set(Vector3.Normalize(new Vector3(1, -1, 1)));
			world.Draw(time);
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
	}
}

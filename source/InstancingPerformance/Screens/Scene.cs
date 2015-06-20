using System;
using System.Collections.Generic;
using System.Linq;
using InstancingPerformance.Content;
using InstancingPerformance.Core;
using InstancingPerformance.Primitives;
using InstancingPerformance.Voxel;
using SharpDX;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;
using MapFlags = SharpDX.Direct3D11.MapFlags;

namespace InstancingPerformance.Screens
{
	public class Scene : Screen
	{
		private const int maxLoops = 2;
		private Shader basicShader;
		private PathCamera camera;
		private Shader geometryShader;
		private Shader instanceShader;
		private double lastFrameTime;
		private Buffer lightBuffer;
		private LightBuffer lightSetup;
		private int loops = 0;
		private bool stopNextFrame;
		private World world;
		private Buffer worldBuffer;

		public Scene(App app)
			: base(app)
		{
			camera = new PathCamera(App);
			camera.NearPlane = 0.3f;
			camera.FarPlane = 1024;
			camera.Rotation.Yaw = 0;
			camera.Rotation.Pitch = 0;

			const int m = 25;
			camera.AddWayPoint(new WayPoint(0 * m, -232, 32, -232, 45, 45, 0));
			camera.AddWayPoint(new WayPoint(1 * m, 180, 64, 180, 45, 25, 0));
			camera.AddWayPoint(new WayPoint(2 * m, -240, 40, 240, 135, 45, 0));
			camera.AddWayPoint(new WayPoint(3 * m, 0, 40, 0, 135, 30, 0));
			camera.AddWayPoint(new WayPoint(4 * m, 230, 50, 180, 0, 0, 0));
			camera.AddWayPoint(new WayPoint(5 * m, -232, 32, -232, -675, 45, 0));

			lightBuffer = new Buffer(Device, 64, ResourceUsage.Dynamic, BindFlags.ConstantBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);
			worldBuffer = new Buffer(Device, 192, ResourceUsage.Dynamic, BindFlags.ConstantBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);

			world = new World(App, worldBuffer, 16, 9);

			basicShader = ResourceManager.BasicShader;
			instanceShader = ResourceManager.InstanceShader;
			geometryShader = ResourceManager.GeometryShader;

			lightSetup = new LightBuffer();
			lightSetup.AmbientColor = Color.White.ToVector4();
			lightSetup.AmbientIntensity = 0.1f;
			lightSetup.LightColor = Color.White.ToVector4();
			lightSetup.LightDirection = Vector3.Normalize(new Vector3(0.25f, -1, 2));

			using (Heightmap heightmap = new Heightmap(App, 64, "IP_HEIGHTMAP"))
			using (ColorMap colormap = new ColorMap(App, "IP_COLOR"))
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

		public override void Draw(TimeSpan totalTime, TimeSpan frameTime, double time)
		{
			lastFrameTime = time;
			switch (App.DrawMode)
			{
				case DrawMode.Basic:
					basicShader.Apply();
					break;

				case DrawMode.Instance:
					instanceShader.Apply();
					break;

				case DrawMode.Geometry:
					geometryShader.Apply();
					break;
			}
			Context.VertexShader?.SetConstantBuffer(0, worldBuffer);
			Context.GeometryShader?.SetConstantBuffer(0, worldBuffer);
			Context.PixelShader?.SetConstantBuffer(0, lightBuffer);
			world.Draw(totalTime, frameTime, time);

			if (stopNextFrame)
				App.Close();
		}

		public override void Update(double time)
		{
			camera.Update(time);
			world.LoadReference = camera.Position.FloorDiv(world.ChunkSize);
			world.Update(time);
			world.WorldSetup.View = Matrix.Transpose(camera.View);
			world.WorldSetup.Projection = Matrix.Transpose(camera.Projection);

			DataStream data;
			DataBox box = Context.MapSubresource(lightBuffer, 0, MapMode.WriteDiscard, MapFlags.None, out data);
			using (data)
				data.Write(lightSetup);
			Context.UnmapSubresource(lightBuffer, 0);

			if (camera.Loop)
			{
				loops++;
				if (loops >= maxLoops)
					stopNextFrame = true;
			}
		}
	}
}

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

namespace InstancingPerformance.Screens
{
	public class Scene : Screen
	{
		private Shader basicShader;
		private PathCamera camera;
		private World world;
		private Heightmap heightmap;
		private HeightmapChunkEnumerator chunkGenerator;
		private IEnumerator<BlockInsert[]> enumerator;

		public Scene(App app)
			: base(app)
		{
			camera = new PathCamera(App);
			camera.NearPlane = 0.3f;
			camera.FarPlane = 1024;
			camera.Rotation.Yaw = 0;
			camera.Rotation.Pitch = 0;

			//camera.AddWayPoint(new WayPoint(0, 0, 32, 0, 0, 0, 0));
			camera.AddWayPoint(new WayPoint(0, -232, 32, -232, 0.7853f, 0, 0));
			camera.AddWayPoint(new WayPoint(30, 232, 32, -232, -0.7853f, 0, 0));
			camera.AddWayPoint(new WayPoint(60, 232, 20, 232, -2.356f, 0, 0));
			camera.AddWayPoint(new WayPoint(90, -232, 20, 232, -3.926f, 0.7853f, 0));
			camera.AddWayPoint(new WayPoint(120, -232, 32, -232, -5.497f, 0, 0));

			Services.AddService((Camera)camera);
			world = new World(App, 16, 10);

			basicShader = ResourceManager.Shader("Basic.fx",
				new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0),
				new InputElement("NORMAL", 0, Format.R32G32B32_Float, 0),
				new InputElement("COLOR", 0, Format.R8G8B8A8_UNorm, 0));

			heightmap = new Heightmap(App, 32, "ip_heightmap.png");
			chunkGenerator = new HeightmapChunkEnumerator(heightmap, world.ChunkSize, 8);
			enumerator = chunkGenerator.GetEnumerator();

			while (enumerator.MoveNext())
			{
				world.SetBlocks(enumerator.Current.AsParallel().Select(insert =>
				{
					insert.Position -= new Vector3(heightmap.Width, 0, heightmap.Height) / 2;
					return insert;
				}));
			}
		}

		public override void Update(double time)
		{
			//TryEnumerate();
			camera.Update(time);
			world.LoadReference = camera.Position.FloorDiv(world.ChunkSize);
			world.Update(time);
		}

		private void TryEnumerate()
		{
			if (heightmap != null && chunkGenerator != null && enumerator != null)
			{
				if (enumerator.MoveNext())
				{
					world.SetBlocks(enumerator.Current.AsParallel().Select(insert =>
					{
						insert.Position -= new Vector3(heightmap.Width, 0, heightmap.Height) / 2;
						return insert;
					}));
				}
				else
				{
					enumerator.Dispose();
					enumerator = null;
					chunkGenerator = null;
					heightmap.Dispose();
					heightmap = null;
				}
			}
		}

		public override void Draw(double time)
		{
			App.UseShader(basicShader);
			basicShader.GetMatrix("View").SetMatrix(camera.View);
			basicShader.GetMatrix("Projection").SetMatrix(camera.Projection);
			basicShader.GetVector("LightColor").Set(Color.Black);
			basicShader.GetVector("LightDirection").Set(Vector3.Normalize(new Vector3(0.5f, -1, 2f)));
			world.Draw(time);
		}
	}
}

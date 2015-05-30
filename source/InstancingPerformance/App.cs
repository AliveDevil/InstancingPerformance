using InstancingPerformance.Content;
using InstancingPerformance.Core;
using InstancingPerformance.Primitives;
using InstancingPerformance.Screens;
using SharpDX;
using SharpDX.Direct3D11;

namespace InstancingPerformance
{
	public class App : AppWindow
	{
		private Resources resourceManager;
		private ScreenManager screenManager;

		public DrawMode DrawMode { get; }

		public Resources ResourceManager => resourceManager;

		public ScreenManager ScreenManager => screenManager;

		public App(DrawMode drawMode) : base("InstancingPerformance")
		{
			DrawMode = drawMode;
		}

		protected override void Draw(double time)
		{
			Context.ClearDepthStencilView(Depthbuffer, DepthStencilClearFlags.Depth, 1, 0);
			Context.ClearRenderTargetView(Backbuffer, Color.Black);
			screenManager.Draw(time);
		}

		protected override void LoadContent()
		{
			resourceManager = new Resources(this);
			screenManager = new ScreenManager(this);
			screenManager.SetScreen(new Scene(this));
		}

		protected override void Update(double time) => screenManager.Update(time);
	}
}

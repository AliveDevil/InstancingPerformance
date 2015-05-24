using InstancingPerformance.Content;
using InstancingPerformance.Core;
using InstancingPerformance.Screens;
using SharpDX;
using SharpDX.Direct3D11;

namespace InstancingPerformance
{
	public class App : AppWindow
	{
		private Resources resourceManager;
		private ScreenManager screenManager;

		public Resources ResourceManager => resourceManager;
		public ScreenManager ScreenManager => screenManager;

		public App() : base("InstancingPerformance") { }

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

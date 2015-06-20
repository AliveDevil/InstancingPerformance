using System;
using System.Diagnostics;
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
		private Statistics statistics;

		public DrawMode DrawMode { get; }

		public Resources ResourceManager => resourceManager;

		public Statistics Statistics => statistics;

		public ScreenManager ScreenManager => screenManager;

		public App(DrawMode drawMode) : base("InstancingPerformance")
		{
			Trace.TraceInformation("Starting App");
			DrawMode = drawMode;
		}

		protected override void Draw(TimeSpan totalTime, TimeSpan frameTime, double time)
		{
			Context.ClearDepthStencilView(Depthbuffer, DepthStencilClearFlags.Depth, 1, 0);
			Context.ClearRenderTargetView(Backbuffer, Color.Black);
			screenManager.Draw(totalTime, frameTime, time);
		}

		protected override void LoadContent()
		{
			Trace.TraceInformation("Loading contents.");
			statistics = new Statistics(DrawMode);
			resourceManager = new Resources(this);
			screenManager = new ScreenManager(this);
			screenManager.SetScreen(new Scene(this));
		}

		protected override void Update(double time) => screenManager.Update(time);

		protected override void UnloadContent()
		{
			statistics.Write();
		}
	}
}

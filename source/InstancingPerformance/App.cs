using InstancingPerformance.Content;
using InstancingPerformance.Core;
using InstancingPerformance.Screens;
using SharpDX;
using SharpDX.Direct3D11;

namespace InstancingPerformance
{
	public class App : AppWindow
	{
		private ResourceManager resourceManager;
		private ScreenManager screenManager;
		private Shader activeShader;

		public Shader ActiveShader => activeShader;
		public ResourceManager ResourceManager => resourceManager;
		public ScreenManager ScreenManager => screenManager;

		public App() : base("InstancingPerformance") { }

		public void ApplyShader(string passName)
		{
			if (activeShader != null)
			{
				activeShader.Effect.GetTechniqueByIndex(0).GetPassByIndex(0).Apply(Context);
				Context.InputAssembler.InputLayout = activeShader.Layout(passName);
			}
		}

		public void UseShader(Shader shader) => activeShader = shader;

		protected override void Draw(double time)
		{
			Context.ClearDepthStencilView(Depthbuffer, DepthStencilClearFlags.Depth, 1, 0);
			Context.ClearRenderTargetView(Backbuffer, Color.Black);
			screenManager.Draw(time);
		}

		protected override void LoadContent()
		{
			resourceManager = new ResourceManager(this);
			screenManager = new ScreenManager(this);
			screenManager.SetScreen(new Scene(this));
		}

		protected override void Update(double time) => screenManager.Update(time);
	}
}

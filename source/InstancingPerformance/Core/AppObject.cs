using System;
using System.Runtime.Remoting.Contexts;
using InstancingPerformance.Content;
using InstancingPerformance.Screens;
using SharpDX.Direct3D11;
using SharpDX.DirectInput;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;

namespace InstancingPerformance.Core
{
	public class AppObject : IDisposable
	{
		public App App { get; private set; }

		public DeviceContext Context { get { return App.Context; } }

		public Device Device { get { return Context.Device; } }

		public Keyboard Keyboard { get { return App.Keyboard; } }

		public KeyboardState KeyState { get { return App.KeyState; } }

		public Mouse Mouse { get { return App.Mouse; } }

		public MouseState MouseState { get { return App.MouseState; } }

		public ResourceManager ResourceManager { get { return App.ResourceManager; } }

		public ScreenManager ScreenManager { get { return App.ScreenManager; } }

		public SwapChain SwapChain { get { return App.SwapChain; } }

		public AppObject(App app)
		{
			App = app;
		}

		~AppObject()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool managed)
		{
		}
	}
}

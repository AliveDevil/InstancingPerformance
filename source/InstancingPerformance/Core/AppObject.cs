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
		public App App { get; }
		public DeviceContext Context => App?.Context;
		public Device Device => Context?.Device;
		public Keyboard Keyboard => App?.Keyboard;
		public KeyboardState KeyState => App?.KeyState;
		public Mouse Mouse => App?.Mouse;
		public MouseState MouseState => App?.MouseState;
		public ResourceManager ResourceManager => App?.ResourceManager;
		public ScreenManager ScreenManager => App?.ScreenManager;
		public SwapChain SwapChain => App?.SwapChain;

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

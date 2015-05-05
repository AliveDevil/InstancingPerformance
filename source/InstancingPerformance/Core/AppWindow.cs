using System;
using System.Diagnostics;
using System.Drawing;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DirectInput;
using SharpDX.DXGI;
using SharpDX.Windows;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;

namespace InstancingPerformance.Core
{
	public class AppWindow : IDisposable
	{
		private TimeSpan accumulator = TimeSpan.Zero, currentTime = TimeSpan.Zero, dt = TimeSpan.FromSeconds(0.025), slowCap = TimeSpan.FromSeconds(0.1), hardLock = TimeSpan.FromSeconds(0.25);
		private Texture2D backBuffer;
		private BlendState blendState;
		private Texture2D depthBuffer;
		private DepthStencilView depthView;
		private Factory factory;
		private DeviceContext graphicsContext;
		private Device graphicsDevice;
		private DirectInput input;
		private RasterizerState rasterizerState;
		private RenderTargetView renderView;
		private SwapChain swapChain;
		private SwapChainDescription swapChainDescription;
		private Stopwatch watch;
		private RenderForm window;

		public float AspectRatio => ScreenSize.X / ScreenSize.Y;
		public RenderTargetView Backbuffer => renderView;
		public DeviceContext Context => graphicsContext;
		public DepthStencilView Depthbuffer => depthView;
		public Vector2 ScreenSize => new Vector2(1280, 720);
		public SwapChain SwapChain => swapChain;

		public Keyboard Keyboard { get; }
		public Mouse Mouse { get; }
		public KeyboardState KeyState { get; private set; }
		public MouseState MouseState { get; private set; }
		public bool RunningSlow { get; private set; }

		public AppWindow(string title)
		{
			window = new RenderForm(title);
			window.ClientSize = new Size((int)ScreenSize.X, (int)ScreenSize.Y);

			bool isWindowed = true;

			swapChainDescription = new SwapChainDescription()
			{
				BufferCount = 1,
				Flags = SwapChainFlags.None,
				IsWindowed = isWindowed,
				ModeDescription = new ModeDescription((int)ScreenSize.X, (int)ScreenSize.Y, new Rational(1, 60), Format.R8G8B8A8_UNorm),
				OutputHandle = window.Handle,
				SampleDescription = new SampleDescription(1, 0),
				SwapEffect = SwapEffect.Discard,
				Usage = Usage.RenderTargetOutput
			};

			Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport | DeviceCreationFlags.Debug, swapChainDescription, out graphicsDevice, out swapChain);
			graphicsContext = graphicsDevice.ImmediateContext;

			factory = swapChain.GetParent<Factory>();
			factory.MakeWindowAssociation(window.Handle, WindowAssociationFlags.IgnoreAll);

			BuildBuffer();

			input = new DirectInput();
			Keyboard = new Keyboard(input);
			Keyboard.Acquire();
			Mouse = new Mouse(input);
			Mouse.Acquire();
			KeyState = new KeyboardState();
			MouseState = new MouseState();
		}

		public void Dispose()
		{
			Keyboard.Unacquire();
			Mouse.Unacquire();
			graphicsContext.Dispose();
			graphicsDevice.Dispose();
			swapChain.Dispose();
			window.Dispose();
			factory.Dispose();
		}

		public void Run()
		{
			LoadContent();
			watch = new Stopwatch();
			watch.Start();

			RenderLoop.Run(window, PerformLoop);

			UnloadContent();
		}

		protected virtual void Draw(double time)
		{ }

		protected virtual void LateUpdate()
		{ }

		protected virtual void LoadContent()
		{ }

		protected virtual void UnloadContent()
		{ }

		protected virtual void Update(double time)
		{ }

		private void BuildBuffer()
		{
			Utilities.Dispose(ref backBuffer);
			Utilities.Dispose(ref renderView);
			Utilities.Dispose(ref depthBuffer);
			Utilities.Dispose(ref depthView);

			swapChain.ResizeBuffers(swapChainDescription.BufferCount, (int)ScreenSize.X, (int)ScreenSize.Y, Format.Unknown, SwapChainFlags.None);
			backBuffer = Resource.FromSwapChain<Texture2D>(swapChain, 0);
			renderView = new RenderTargetView(graphicsDevice, backBuffer);
			depthBuffer = new Texture2D(graphicsDevice, new Texture2DDescription()
			{
				Format = Format.D32_Float_S8X24_UInt,
				ArraySize = 1,
				MipLevels = 1,
				Width = (int)ScreenSize.X,
				Height = (int)ScreenSize.Y,
				SampleDescription = new SampleDescription(1, 0),
				Usage = ResourceUsage.Default,
				BindFlags = BindFlags.DepthStencil,
				CpuAccessFlags = CpuAccessFlags.None,
				OptionFlags = ResourceOptionFlags.None
			});
			depthView = new DepthStencilView(graphicsDevice, depthBuffer);

			rasterizerState = new RasterizerState(graphicsDevice, new RasterizerStateDescription()
			{
				CullMode = CullMode.Back,
				FillMode = FillMode.Solid
			});

			graphicsContext.Rasterizer.State = rasterizerState;
			graphicsContext.Rasterizer.SetViewport(0, 0, (int)ScreenSize.X, (int)ScreenSize.Y);

			BlendStateDescription blending = new BlendStateDescription();
			blending.RenderTarget[0].IsBlendEnabled = true;
			blending.RenderTarget[0].BlendOperation = BlendOperation.Add;
			blending.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
			blending.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
			blending.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
			blending.RenderTarget[0].SourceAlphaBlend = BlendOption.Zero;
			blending.RenderTarget[0].DestinationAlphaBlend = BlendOption.Zero;
			blending.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;

			blendState = new BlendState(graphicsDevice, blending);

			graphicsContext.OutputMerger.SetBlendState(blendState);
			graphicsContext.OutputMerger.SetTargets(depthView, renderView);
		}

		private void PerformLoop()
		{
			TimeSpan newTime = watch.Elapsed;
			TimeSpan frameTime = newTime - currentTime;
			RunningSlow = frameTime >= slowCap;
			if (frameTime > hardLock) frameTime = hardLock;
			currentTime = newTime;
			accumulator += frameTime;

			while (accumulator >= dt)
			{
				KeyState = Keyboard.GetCurrentState();
				MouseState = Mouse.GetCurrentState();
				Update(dt.TotalSeconds);
				LateUpdate();
				accumulator -= dt;
			}

			double alpha = accumulator.TotalSeconds / dt.TotalSeconds;

			Draw(alpha);
			swapChain.Present(0, PresentFlags.None);
		}
	}
}

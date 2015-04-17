using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using InstancingPerformance.Content;

namespace InstancingPerformance.Core
{
	public class Heightmap : AppObject
	{
		private Bitmap map;

		private BitmapData mapData;
		
		public float this[int x, int y]
		{
			get
			{
				if (x < 0 || x >= Width) throw new ArgumentOutOfRangeException("x");
				if (y < 0 || y >= Height) throw new ArgumentOutOfRangeException("y");

				return map.GetPixel(x, y).GetBrightness() * Amplitude;
			}
		}

		public float Amplitude { get; private set; }

		public int Height { get; private set; }

		public int Width { get; private set; }

		public int Length { get; private set; }

		public Heightmap(App app, float amplitude, string resource)
			: base(app)
		{
			map = new Bitmap(ResourceManager.ResourceStream(resource));
			Width = map.Width;
			Height = map.Height;
			Length = Width * Height;
			Amplitude = amplitude;
			//mapData = map.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, map.PixelFormat);
		}

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				//map.UnlockBits(mapData);
				map.Dispose();
			}
			base.Dispose(managed);
		}
	}
}

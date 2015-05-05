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

		public float this[int x, int y]
		{
			get
			{
				if (x < 0 || x >= Width) throw new ArgumentOutOfRangeException("x");
				if (y < 0 || y >= Height) throw new ArgumentOutOfRangeException("y");

				return map.GetPixel(x, Height - (y + 1)).GetBrightness() * Amplitude;
			}
		}

		public float Amplitude { get; }
		public int Height { get; }
		public int Width { get; }
		public int Length { get; }

		public Heightmap(App app, float amplitude, string resource)
			: base(app)
		{
			map = new Bitmap(ResourceManager.ResourceStream(resource));
			Width = map.Width;
			Height = map.Height;
			Length = Width * Height;
			Amplitude = amplitude;
		}

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				map.Dispose();
			}
			base.Dispose(managed);
		}
	}
}

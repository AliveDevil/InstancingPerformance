using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstancingPerformance.Content;

namespace InstancingPerformance.Core
{
	public class ColorMap : AppObject
	{
		private Bitmap map;

		public Color this[int x, int y]
		{
			get
			{
				if (x < 0 || x >= Width) throw new ArgumentOutOfRangeException("x");
				if (y < 0 || y >= Height) throw new ArgumentOutOfRangeException("y");

				return map.GetPixel(x, Height - (y + 1));
			}
		}

		public int Height { get; }

		public int Width { get; }

		public ColorMap(App app, string resource)
			: base(app)
		{
			map = new Bitmap(ResourceManager.ResourceStream(resource));
			Width = map.Width;
			Height = map.Height;
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

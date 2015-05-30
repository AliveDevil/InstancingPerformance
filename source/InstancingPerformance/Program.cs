using System;
using InstancingPerformance.Primitives;

namespace InstancingPerformance
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			using (App app = new App((DrawMode)Enum.Parse(typeof(DrawMode), args[0])))
			{
				app.Run();
			}
		}
	}
}

using System;
using System.Diagnostics;
using System.Windows.Forms;
using InstancingPerformance.Primitives;
using DrawMode = InstancingPerformance.Primitives.DrawMode;

namespace InstancingPerformance
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			Trace.TraceInformation("Starting application");
			DrawMode drawMode = ((DrawMode)Enum.Parse(typeof(DrawMode), args[0]));
			Trace.TraceInformation($"Using drawmode {drawMode.ToString()}");
			using (App app = new App(drawMode))
			{
				app.Run();
			}
			Trace.TraceInformation("== Application exiting. ==");
			Trace.Close();
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Trace.TraceError(e.ExceptionObject.ToString());
		}
	}
}

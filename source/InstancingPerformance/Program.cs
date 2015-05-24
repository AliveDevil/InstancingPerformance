namespace InstancingPerformance
{
	internal static class Program
	{
		private static void Main()
		{
			Manager manager = new Manager();
			using (App app = new App())
			{
				app.Run();
			}
		}
	}
}

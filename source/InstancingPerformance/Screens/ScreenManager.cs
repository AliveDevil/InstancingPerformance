using InstancingPerformance.Core;

namespace InstancingPerformance.Screens
{
	public class ScreenManager : AppObject, IUpdate, IDraw
	{
		private Screen currentScreen;

		public Screen CurrentScreen { get { return currentScreen; } }

		public ScreenManager(App app)
			: base(app)
		{
		}

		public void Draw(double time)
		{
			if (currentScreen != null) currentScreen.Draw(time);
		}

		public void SetScreen(Screen screen)
		{
			if (currentScreen != null)
			{
				currentScreen.Dispose();
			}
			currentScreen = screen;
		}

		public void Update(double time)
		{
			if (currentScreen != null) currentScreen.Update(time);
		}
	}
}

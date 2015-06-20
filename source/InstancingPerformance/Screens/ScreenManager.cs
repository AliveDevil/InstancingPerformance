using System;
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

		public void Draw(TimeSpan totalTime, TimeSpan frameTime, double time)
		{
			currentScreen?.Draw(totalTime, frameTime, time);
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
			currentScreen?.Update(time);
		}
	}
}

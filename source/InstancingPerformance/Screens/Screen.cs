using System;
using InstancingPerformance.Core;

namespace InstancingPerformance.Screens
{
	public abstract class Screen : AppObject, IDraw, IUpdate
	{
		public Screen(App app)
			: base(app)
		{
		}

		public virtual void Draw(TimeSpan totalTime, TimeSpan frameTime, double time)
		{
		}

		public virtual void Update(double time)
		{
		}
	}
}

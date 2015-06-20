using System;

namespace InstancingPerformance.Core
{
	public interface IDraw
	{
		void Draw(TimeSpan totalTime, TimeSpan frameTime, double time);
	}
}

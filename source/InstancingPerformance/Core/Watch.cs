using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstancingPerformance.Core
{
	public class Watch : IDisposable
	{
		Stopwatch watch;

		public Watch()
		{
			(watch = new Stopwatch()).Start();
		}

		public TimeSpan Elapsed { get { return watch.Elapsed; } }

		public void Dispose()
		{
			watch.Reset();
			watch = null;
		}
	}
}

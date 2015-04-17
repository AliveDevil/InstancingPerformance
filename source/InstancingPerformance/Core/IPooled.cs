using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstancingPerformance.Core
{
	public interface IPooled<T>
	{
		T Key { get; set; }
		bool IsActive { get; }
	}
}

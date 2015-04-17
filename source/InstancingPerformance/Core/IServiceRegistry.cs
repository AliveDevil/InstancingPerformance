using System;
using SharpDX;

namespace InstancingPerformance.Core
{
	public interface IServiceRegistry : IServiceProvider
	{
		event EventHandler<ServiceEventArgs> ServiceAdded;

		event EventHandler<ServiceEventArgs> ServiceRemoved;

		void AddService(Type type, object provider);

		void AddService<T>(T provider);

		T GetService<T>();

		void RemoveService(Type type);
	}
}

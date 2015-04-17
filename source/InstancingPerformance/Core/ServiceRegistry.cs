using System;
using System.Collections.Generic;
using SharpDX;

namespace InstancingPerformance.Core
{
	public class ServiceRegistry : IServiceRegistry
	{
		public event EventHandler<ServiceEventArgs> ServiceAdded;

		public event EventHandler<ServiceEventArgs> ServiceRemoved;

		private readonly Dictionary<Type, object> registeredService = new Dictionary<Type, object>();

		public void AddService(Type type, object provider)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			if (provider == null)
				throw new ArgumentNullException("provider");

			if (!type.IsAssignableFrom(provider.GetType()))
				throw new ArgumentException(string.Format("Service [{0}] must be assignable to [{1}]", provider.GetType().FullName, type.GetType().FullName));

			lock (registeredService)
			{
				if (registeredService.ContainsKey(type))
					throw new ArgumentException("Service is already registered", "type");
				registeredService.Add(type, provider);
			}
			OnServiceAdded(new ServiceEventArgs(type, provider));
		}

		public void AddService<T>(T provider)
		{
			AddService(typeof(T), provider);
		}

		public object GetService(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			lock (registeredService)
			{
				if (registeredService.ContainsKey(type))
					return registeredService[type];
			}

			return null;
		}

		public T GetService<T>()
		{
			var service = GetService(typeof(T));

			if (service == null)
				throw new ArgumentException(string.Format("Service of type {0} is not registered.", typeof(T)));

			return (T)service;
		}

		public void RemoveService(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			object oldService = null;
			lock (registeredService)
			{
				if (registeredService.TryGetValue(type, out oldService))
					registeredService.Remove(type);
			}
			if (oldService != null)
				OnServiceRemoved(new ServiceEventArgs(type, oldService));
		}

		private void OnServiceAdded(ServiceEventArgs e)
		{
			EventHandler<ServiceEventArgs> handler = ServiceAdded;
			if (handler != null) handler(this, e);
		}

		private void OnServiceRemoved(ServiceEventArgs e)
		{
			EventHandler<ServiceEventArgs> handler = ServiceRemoved;
			if (handler != null) handler(this, e);
		}
	}
}

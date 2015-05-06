using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace InstancingPerformance.Core
{
	public class ObjectPool<T, TPool> where T : IPooled<TPool>
	{
		private Dictionary<TPool, T> active { get; } = new Dictionary<TPool, T>();
		private Func<T> objectFactory;
		private ConcurrentBag<T> objects { get; } = new ConcurrentBag<T>();

		public IEnumerable<T> ActiveObjects => active.Values;

		public ObjectPool(Func<T> objectGenerator)
		{
			if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
			objectFactory = objectGenerator;
		}

		public bool ContainsKey(TPool key) => active.ContainsKey(key);

		public T GetObject(TPool key)
		{
			T item;
			if (objects.TryTake(out item))
			{
				return AddObject(item, key);
			}
			return AddObject(objectFactory(), key);
		}

		public void PutObject(T item)
		{
			active.Remove(item.Key);
			objects.Add(item);
			item.Reset();
		}

		public void UpdateObjects()
		{
			var copy = new Dictionary<TPool, T>(active);
			foreach (var @object in copy)
			{
				if (!@object.Value.IsActive)
					PutObject(@object.Value);
			}
		}

		private T AddObject(T obj, TPool key)
		{
			active.Add(key, obj);
			obj.Key = key;
			return obj;
		}
	}
}

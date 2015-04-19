using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace InstancingPerformance.Core
{
	public class ObjectPool<T, TPool> where T : IPooled<TPool>
	{
		private Dictionary<TPool, T> _active;
		private Func<T> _objectGenerator;
		private ConcurrentBag<T> _objects;

		public IEnumerable<T> ActiveObjects { get { return _active.Values; } }

		public ObjectPool(Func<T> objectGenerator)
		{
			if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
			_objects = new ConcurrentBag<T>();
			_active = new Dictionary<TPool, T>();
			_objectGenerator = objectGenerator;
		}

		public bool ContainsKey(TPool key)
		{
			return _active.ContainsKey(key);
		}

		public T GetObject(TPool key)
		{
			T item;
			if (_objects.TryTake(out item))
			{
				return AddObject(item, key);
			}
			return AddObject(_objectGenerator(), key);
		}

		public void PutObject(T item)
		{
			_active.Remove(item.Key);
			_objects.Add(item);
			item.Reset();
		}

		public void UpdateObjects()
		{
			var copy = new Dictionary<TPool, T>(_active);
			foreach (var @object in copy)
			{
				if (!@object.Value.IsActive)
					PutObject(@object.Value);
			}
		}

		private T AddObject(T obj, TPool key)
		{
			_active.Add(key, obj);
			obj.Key = key;
			return obj;
		}
	}
}

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;

namespace InstancingPerformance.Tests
{
	[TestClass]
	public class VectorTests
	{
		[TestMethod]
		public void TestVector3Symmetrical()
		{
			for (int x = 0; x < 16; x++)
			{
				for (int y = 0; y < 16; y++)
				{
					for (int z = 0; z < 16; z++)
					{
						Vector3 v = new Vector3(x, y, z);
						int index = ((z * 16) + y) * 16 + x;
						Assert.AreEqual(index, v.IndiceVector(16, 16, 16));
						Assert.AreEqual(v, index.ResolveIndex(16, 16, 16));
					}
				}
			}
		}

		[TestMethod]
		public void TestVector2ResolveAsymmetrical()
		{
			Dictionary<int, Vector2> lookup = new Dictionary<int, Vector2>()
			{
				{ 0, new Vector2(0, 0) },
				{ 1, new Vector2(1, 0) },
				{ 2, new Vector2(2, 0) },
				{ 3, new Vector2(0, 1) },
				{ 4, new Vector2(1, 1) },
				{ 5, new Vector2(2, 1) },
			};

			foreach (var item in lookup)
			{
				Assert.AreEqual(item.Value, item.Key.ResolveIndex(3, 2));
			}
		}

		[TestMethod]
		public void TestVector2ResolveSymmetrical()
		{
			Dictionary<int, Vector2> lookup = new Dictionary<int, Vector2>()
			{
				{ 0, new Vector2(0, 0) },
				{ 1, new Vector2(1, 0) },
				{ 2, new Vector2(2, 0) },
				{ 3, new Vector2(0, 1) },
				{ 4, new Vector2(1, 1) },
				{ 5, new Vector2(2, 1) },
				{ 6, new Vector2(0, 2) },
				{ 7, new Vector2(1, 2) },
				{ 8, new Vector2(2, 2) },
			};

			foreach (var item in lookup)
			{
				Assert.AreEqual(item.Value, item.Key.ResolveIndex(3, 3));
			}
		}

		[TestMethod]
		public void TestIndiceAsymmetricalZ()
		{
			Dictionary<Vector3, int> lookup = new Dictionary<Vector3, int>()
			{
				{ new Vector3(0, 0, 0), 00 },
				{ new Vector3(1, 0, 0), 01 },
				{ new Vector3(2, 0, 0), 02 },
				{ new Vector3(0, 1, 0), 03 },
				{ new Vector3(1, 1, 0), 04 },
				{ new Vector3(2, 1, 0), 05 },
				{ new Vector3(0, 2, 0), 06 },
				{ new Vector3(1, 2, 0), 07 },
				{ new Vector3(2, 2, 0), 08 },
				{ new Vector3(0, 0, 1), 09 },
				{ new Vector3(1, 0, 1), 10 },
				{ new Vector3(2, 0, 1), 11 },
				{ new Vector3(0, 1, 1), 12 },
				{ new Vector3(1, 1, 1), 13 },
				{ new Vector3(2, 1, 1), 14 },
				{ new Vector3(0, 2, 1), 15 },
				{ new Vector3(1, 2, 1), 16 },
				{ new Vector3(2, 2, 1), 17 },
			};

			foreach (var item in lookup)
			{
				Assert.AreEqual(item.Value, item.Key.IndiceVector(3, 3, 2));
			}
		}

		[TestMethod]
		public void TestIndiceASymmetricalX()
		{
			Dictionary<Vector3, int> lookup = new Dictionary<Vector3, int>()
			{
				{ new Vector3(0, 0, 0), 00 },
				{ new Vector3(1, 0, 0), 01 },
				{ new Vector3(0, 1, 0), 02 },
				{ new Vector3(1, 1, 0), 03 },
				{ new Vector3(0, 2, 0), 04 },
				{ new Vector3(1, 2, 0), 05 },
				{ new Vector3(0, 0, 1), 06 },
				{ new Vector3(1, 0, 1), 07 },
				{ new Vector3(0, 1, 1), 08 },
				{ new Vector3(1, 1, 1), 09 },
				{ new Vector3(0, 2, 1), 10 },
				{ new Vector3(1, 2, 1), 11 },
				{ new Vector3(0, 0, 2), 12 },
				{ new Vector3(1, 0, 2), 13 },
				{ new Vector3(0, 1, 2), 14 },
				{ new Vector3(1, 1, 2), 15 },
				{ new Vector3(0, 2, 2), 16 },
				{ new Vector3(1, 2, 2), 17 },
			};

			foreach (var item in lookup)
			{
				Assert.AreEqual(item.Value, item.Key.IndiceVector(2, 3, 3));
			}
		}

		[TestMethod]
		public void TestIndiceSymmetrical()
		{
			Dictionary<Vector3, int> lookup = new Dictionary<Vector3, int>()
			{
				{ new Vector3(0, 0, 0), 00 },
				{ new Vector3(1, 0, 0), 01 },
				{ new Vector3(2, 0, 0), 02 },

				{ new Vector3(0, 1, 0), 03 },
				{ new Vector3(1, 1, 0), 04 },
				{ new Vector3(2, 1, 0), 05 },

				{ new Vector3(0, 2, 0), 06 },
				{ new Vector3(1, 2, 0), 07 },
				{ new Vector3(2, 2, 0), 08 },

				{ new Vector3(0, 0, 1), 09 },
				{ new Vector3(1, 0, 1), 10 },
				{ new Vector3(2, 0, 1), 11 },

				{ new Vector3(0, 1, 1), 12 },
				{ new Vector3(1, 1, 1), 13 },
				{ new Vector3(2, 1, 1), 14 },

				{ new Vector3(0, 2, 1), 15 },
				{ new Vector3(1, 2, 1), 16 },
				{ new Vector3(2, 2, 1), 17 },

				{ new Vector3(0, 0, 2), 18 },
				{ new Vector3(1, 0, 2), 19 },
				{ new Vector3(2, 0, 2), 20 },

				{ new Vector3(0, 1, 2), 21 },
				{ new Vector3(1, 1, 2), 22 },
				{ new Vector3(2, 1, 2), 23 },

				{ new Vector3(0, 2, 2), 24 },
				{ new Vector3(1, 2, 2), 25 },
				{ new Vector3(2, 2, 2), 26 },
			};

			foreach (var item in lookup)
			{
				Assert.AreEqual(item.Value, item.Key.IndiceVector(3, 3, 3));
			}
		}
	}
}

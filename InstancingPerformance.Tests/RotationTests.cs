using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstancingPerformance.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;

namespace InstancingPerformance.Tests
{
	[TestClass]
	public class RotationTests
	{
		[TestMethod]
		public void RotationTest()
		{
			Vector3 v = new Vector3(0, 0, 1);
			Rotation r = new Rotation(90, 0, 0);
			Vector3 vr = v * r;
		}
	}
}

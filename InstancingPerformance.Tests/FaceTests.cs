using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstancingPerformance.Core;
using InstancingPerformance.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;

namespace InstancingPerformance.Tests
{
	[TestClass]
	public class FaceTests
	{
		[TestMethod]
		public void TestFace()
		{
			Face face = new Face(new Vector3(-0.5f, -0.5f, 0), new Vector3(-0.5f, 0.5f, 0), new Vector3(0.5f, 0.5f, 0), new Vector3(0.5f, -0.5f, 0));
			Assert.AreEqual(new Vector3(-0.5f, -0.5f, 0), face.BottomLeft);
			Assert.AreEqual(new Vector3(-0.5f, 0.5f, 0), face.TopLeft);
			Assert.AreEqual(new Vector3(0.5f, 0.5f, 0), face.TopRight);
			Assert.AreEqual(new Vector3(0.5f, -0.5f, 0), face.BottomRight);
		}

		[TestMethod]
		public void TestFaceRotationHalf()
		{
			Face face = new Face(new Vector3(-1, -1, 0), new Vector3(-1, 1, 0), new Vector3(1, 1, 0), new Vector3(1, -1, 0));
			Rotation rotation = new Rotation(180, 0, 0);
			Face rotated = face * rotation;
			Assert.AreEqual(new Vector3(1, -1, 0), rotated.BottomLeft);
			Assert.AreEqual(new Vector3(1, 1, 0), rotated.TopLeft);
			Assert.AreEqual(new Vector3(-1, 1, 0), rotated.TopRight);
			Assert.AreEqual(new Vector3(-1, -1, 0), rotated.BottomRight);
		}
	}
}

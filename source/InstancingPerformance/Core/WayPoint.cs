using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;

namespace InstancingPerformance.Core
{
	public struct WayPoint
	{
		public static WayPoint Empty = new WayPoint();

		public float Time;
		public Vector3 Position;
		public Rotation Rotation;

		public WayPoint(float time, float x, float y, float z, float yaw, float pitch, float roll)
		{
			Time = time;
			Position = new Vector3(x, y, z);
			Rotation = new Rotation(yaw, pitch, roll);
		}
	}
}

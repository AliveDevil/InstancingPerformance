using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace InstancingPerformance.Core
{
	public class PathCamera : Camera, IUpdate
	{
		private List<WayPoint> waypoints;
		private float lastTime;
		private float currentTime;

		public bool Loop { get; private set; }

		public PathCamera(App app)
			: base(app)
		{
			waypoints = new List<WayPoint>();
		}

		public void AddWayPoint(WayPoint wayPoint)
		{
			if (wayPoint.Time > lastTime)
				lastTime = wayPoint.Time;
			waypoints.Add(wayPoint);
		}

		public void Update(double time)
		{
			Loop = false;
			if (currentTime >= lastTime)
			{
				currentTime = 0;
				Loop = true;
			}
			else
				currentTime += (float)time;

			WayPoint from = waypoints[0], to = from;

			for (int i = 1; i < waypoints.Count; i++)
			{
				var temp = waypoints[i];
				if (temp.Time >= currentTime)
				{
					from = waypoints[i - 1];
					to = waypoints[i];
					break;
				}
			}

			float currentModifiedTime = currentTime - from.Time;
			float divider = to.Time - from.Time;
			if (divider == 0)
				divider = 1;

			Position = Vector3.Lerp(from.Position, to.Position, currentModifiedTime / divider);
			Rotation.Yaw = MathUtil.Lerp(from.Rotation.Yaw, to.Rotation.Yaw, currentModifiedTime / divider);
			Rotation.Pitch = MathUtil.Lerp(from.Rotation.Pitch, to.Rotation.Pitch, currentModifiedTime / divider);
		}
	}
}

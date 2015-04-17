using SharpDX;

namespace InstancingPerformance.Core
{
	public struct Rotation
	{
		private bool changed;
		private float pitch;
		private Quaternion quaternion;
		private float roll;
		private float yaw;

		public float Pitch
		{
			get
			{
				return pitch;
			}
			set
			{
				pitch = value;
				changed = true;
			}
		}

		public Quaternion Quaternion
		{
			get
			{
				if (changed)
				{
					quaternion = Quaternion.RotationYawPitchRoll(Yaw, Pitch, Roll);
				}
				return quaternion;
			}
		}

		public float Roll
		{
			get
			{
				return roll;
			}
			set
			{
				roll = value;
				changed = true;
			}
		}

		public float Yaw
		{
			get
			{
				return yaw;
			}
			set
			{
				yaw = value;
				changed = true;
			}
		}

		public Rotation(float yaw, float pitch, float roll)
		{
			this.quaternion = Quaternion.Identity;
			this.yaw = yaw;
			this.pitch = pitch;
			this.roll = roll;
			this.changed = true;
		}
	}
}

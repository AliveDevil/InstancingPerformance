using System;
using System.Numerics;
using SharpDX;

namespace InstancingPerformance.Core
{
	public struct Rotation
	{
		const double DegRadD = 0.017453292519943296;
		const float DegRadF = (float)DegRadD;

		private bool changed;
		private float pitch;
		private float roll;
		private float yaw;
		private Matrix matrix;

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
			this.yaw = yaw;
			this.pitch = pitch;
			this.roll = roll;
			matrix = Matrix.RotationYawPitchRoll(MathUtil.DegreesToRadians(yaw), MathUtil.DegreesToRadians(pitch), MathUtil.DegreesToRadians(roll));
			changed = false;
		}

		public Matrix Matrix
		{
			get
			{
				if (changed)
				{
					changed = false;
					matrix = Matrix.RotationYawPitchRoll(MathUtil.DegreesToRadians(yaw), MathUtil.DegreesToRadians(pitch), MathUtil.DegreesToRadians(roll));
				}
				return matrix;
			}
		}

		public static Vector3 operator *(Vector3 v, Rotation r)
		{
			/*
			 * This is copied from here
			 * https://github.com/sharpdx/SharpDX/blob/master/Source/SharpDX.Mathematics/Quaternion.cs#L1079
			 * and here
			 * https://github.com/sharpdx/SharpDX/blob/master/Source/SharpDX.Mathematics/Vector3.cs#L1173
			 * because SharpDX internal methods do not provide enough precision (due to 32-Bit float)
			 * and Math.PI.
			 */

			double yaw = r.Yaw * DegRadD;
			double pitch = r.Pitch * DegRadD;
			double roll = r.Roll * DegRadD;

			double halfRoll = roll * 0.5f;
			double halfPitch = pitch * 0.5f;
			double halfYaw = yaw * 0.5f;

			double sinRoll = Math.Sin(halfRoll);
			double cosRoll = Math.Cos(halfRoll);
			double sinPitch = Math.Sin(halfPitch);
			double cosPitch = Math.Cos(halfPitch);
			double sinYaw = Math.Sin(halfYaw);
			double cosYaw = Math.Cos(halfYaw);

			double rotationX = (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll);
			double rotationY = (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll);
			double rotationZ = (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll);
			double rotationW = (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll);

			double x = rotationX + rotationX;
			double y = rotationY + rotationY;
			double z = rotationZ + rotationZ;
			double wx = rotationW * x;
			double wy = rotationW * y;
			double wz = rotationW * z;
			double xx = rotationX * x;
			double xy = rotationX * y;
			double xz = rotationX * z;
			double yy = rotationY * y;
			double yz = rotationY * z;
			double zz = rotationZ * z;

			return new Vector3(
				(float)Math.Round(((v.X * ((1.0 - yy) - zz)) + (v.Y * (xy - wz))) + (v.Z * (xz + wy)), 5),
				(float)Math.Round(((v.X * (xy + wz)) + (v.Y * ((1.0 - xx) - zz))) + (v.Z * (yz - wx)), 5),
				(float)Math.Round(((v.X * (xz - wy)) + (v.Y * (yz + wx))) + (v.Z * ((1.0 - xx) - yy)), 5));
		}
	}
}

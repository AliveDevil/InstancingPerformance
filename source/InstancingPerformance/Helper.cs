using System;
using SharpDX;

namespace InstancingPerformance
{
	public static class Helper
	{
		public static int Floor(this float f)
		{
			return (int)Math.Floor(f);
		}

		public static Vector3 Floor(this Vector3 v)
		{
			return new Vector3(v.X.Floor(), v.Y.Floor(), v.Z.Floor());
		}

		public static int IndiceVector(this Vector2 v, int x, int y)
		{
			return Floor((v.Y * x) + v.X);
		}

		public static Vector3 ToVector3(this Vector4 v)
		{
			return new Vector3(v.X, v.Y, v.Z);
		}

		public static int IndiceVector(this Vector3 v, int x, int y, int z)
		{
			return (int)(((v.Z * y) + v.Y) * x + v.X);
		}

		public static float Distance(Vector3 l, Vector3 r)
		{
			Vector3 v = l - r;
			float x = Math.Abs(v.X);
			float y = Math.Abs(v.Y);
			float z = Math.Abs(v.Z);
			return Math.Max(x, Math.Max(y, z));
		}

		public static Vector3 FloorDiv(this Vector3 v, int c)
		{
			return (v / c).Floor();
		}

		public static Vector3 Mod(this Vector3 v, int c)
		{
			return new Vector3(Math.Abs(v.X.Mod(c)), Math.Abs(v.Y.Mod(c)), Math.Abs(v.Z.Mod(c)));
		}

		public static float Mod(this float f, int c)
		{
			return f.Floor() & (c - 1);
		}

		public static Vector2 ResolveIndex(this int index, int x, int y)
		{
			return new Vector2(index % x, index / x % y);
		}

		public static Vector3 ResolveIndex(this int index, int x, int y, int z)
		{
			return new Vector3(index % x, index / x % y, index / x / y % z);
		}
	}
}

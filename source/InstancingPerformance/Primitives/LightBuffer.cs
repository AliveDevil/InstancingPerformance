using System.Runtime.InteropServices;
using SharpDX;

namespace InstancingPerformance.Primitives
{
	[StructLayout(LayoutKind.Explicit, Size = 64)]
	public struct LightBuffer
	{
		[FieldOffset(0)]
		public Vector4 AmbientColor;
		[FieldOffset(16)]
		public float AmbientIntensity;
		[FieldOffset(32)]
		public Vector4 LightColor;
		[FieldOffset(48)]
		public Vector3 LightDirection;
	}
}

using SharpDX;

namespace InstancingPerformance.Core
{
	public class Camera : AppObject
	{
		public float FarPlane;
		public float NearPlane;
		public Vector3 Position;
		public Rotation Rotation;
		public Matrix Projection => Matrix.PerspectiveFovLH(MathUtil.DegreesToRadians(60), App.AspectRatio, NearPlane, FarPlane);
		public Matrix View => Matrix.LookAtLH(Position, Position + Vector3.ForwardLH * Rotation, Vector3.Up);

		public Camera(App app)
			: base(app)
		{
		}
	}
}

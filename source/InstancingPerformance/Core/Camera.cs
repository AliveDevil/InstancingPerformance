using SharpDX;

namespace InstancingPerformance.Core
{
	public class Camera : AppObject
	{
		public float FarPlane;
		public float NearPlane;
		public Vector3 Position;
		public Rotation Rotation;

		public virtual Matrix Projection
		{
			get { return Matrix.PerspectiveFovLH(MathUtil.DegreesToRadians(60), App.AspectRatio, NearPlane, FarPlane); }
		}

		public virtual Matrix View
		{
			get { return Matrix.LookAtLH(Position, Position + Vector3.Transform(Vector3.ForwardLH, Rotation.Quaternion), Vector3.Up); }
		}

		public Camera(App app)
			: base(app)
		{
		}
	}
}

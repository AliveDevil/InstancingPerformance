using InstancingPerformance.Core;
using SharpDX;
using SharpDX.Direct3D11;

namespace InstancingPerformance.Content
{
	public class Shader : AppObject
	{
		public Effect Effect;
		public InputLayout Layout;

		public Shader(App app, Effect effect, InputLayout layout)
			: base(app)
		{
			Effect = effect;
			Layout = layout;
		}

		public void SetCamera(string name, Matrix world, Matrix view, Matrix projection)
		{
			GetMatrix(name).SetMatrix(world * view * projection);
		}

		public EffectMatrixVariable GetMatrix(string name)
		{
			return Effect.GetVariableByName(name).AsMatrix();
		}

		internal EffectVectorVariable GetVector(string name)
		{
			return Effect.GetVariableByName(name).AsVector();
		}
	}
}

using System.Collections.Generic;
using InstancingPerformance.Core;
using SharpDX;
using SharpDX.Direct3D11;

namespace InstancingPerformance.Content
{
	public class Shader : AppObject
	{
		public Effect Effect { get; }
		public IDictionary<string, InputLayout> Layouts { get; } = new Dictionary<string, InputLayout>();

		public Shader(App app, Effect effect, IDictionary<string, InputLayout> layouts)
			: base(app)
		{
			Effect = effect;
			foreach (var item in layouts)
			{
				Layouts.Add(item);
			}
		}

		public EffectMatrixVariable Matrix(string name) => Effect.GetVariableByName(name).AsMatrix();
		public EffectPass Pass(int index) => Effect.GetTechniqueByIndex(0).GetPassByIndex(index);
		public EffectVectorVariable Vector(string name) => Effect.GetVariableByName(name).AsVector();
		public void SetCamera(string name, Matrix world, Matrix view, Matrix projection) => Matrix(name).SetMatrix(world * view * projection);

		public InputLayout Layout(string pass)
		{
			InputLayout layout;
			Layouts.TryGetValue(pass, out layout);
			return layout;
		}
	}
}

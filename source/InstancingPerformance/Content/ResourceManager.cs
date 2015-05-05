using System.Collections.Generic;
using System.IO;
using System.Reflection;
using InstancingPerformance.Core;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace InstancingPerformance.Content
{
	public class ResourceManager : AppObject
	{
		private static Assembly assembly { get; } = Assembly.GetExecutingAssembly();
		private static string basePath { get; } = assembly.GetName().Name + ".Content";

		public ResourceManager(App app) : base(app) { }

		public static string ResourceContent(string filename)
		{
			using (Stream stream = ResourceStream(filename))
			using (StreamReader reader = new StreamReader(stream))
				return reader.ReadToEnd();
		}

		public static Stream ResourceStream(string filename) => assembly.GetManifestResourceStream($"{basePath}.{filename}");

		public Shader Shader(string filename, IDictionary<string, InputElement[]> input)
		{
			Effect effect = new Effect(Device, ShaderBytecode.Compile(ResourceContent(filename), "fx_5_0", ShaderFlags.None, EffectFlags.None));
			IDictionary<string, InputLayout> layouts = new Dictionary<string, InputLayout>();
			for (int i = 0; i < effect.GetTechniqueByIndex(0).Description.PassCount; i++)
			{
				EffectPass pass = effect.GetTechniqueByIndex(0).GetPassByIndex(0);
				var signature = pass.Description.Signature;
				string name = pass.Description.Name;
				layouts.Add(name, new InputLayout(Device, signature, input[name]));
			}
			return new Shader(App, effect, layouts);
		}
	}
}

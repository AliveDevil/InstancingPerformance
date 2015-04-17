using System.IO;
using System.Reflection;
using InstancingPerformance.Core;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace InstancingPerformance.Content
{
	public class ResourceManager : AppObject
	{
		private static Assembly assembly = Assembly.GetExecutingAssembly();
		private static string basePath = assembly.GetName().Name + ".Content";

		public ResourceManager(App app)
			: base(app)
		{
		}

		public static string ResourceContent(string filename)
		{
			using (Stream stream = ResourceStream(filename))
			using (StreamReader reader = new StreamReader(stream))
				return reader.ReadToEnd();
		}

		public static Stream ResourceStream(string filename)
		{
			return assembly.GetManifestResourceStream(string.Format("{0}.{1}", basePath, filename));
		}

		public Shader Shader(string filename, params InputElement[] input)
		{
			Effect effect = new Effect(Device, ShaderBytecode.Compile(ResourceContent(filename), "fx_5_0", ShaderFlags.None, EffectFlags.None));
			InputLayout layout = new InputLayout(Device, effect.GetTechniqueByIndex(0).GetPassByIndex(0).Description.Signature, input);
			return new Shader(App, effect, layout);
		}
	}
}

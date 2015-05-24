using System.Collections.Generic;
using System.IO;
using System.Reflection;
using InstancingPerformance.Core;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace InstancingPerformance.Content
{
	public class Resources : AppObject
	{
		private Shader basicShader;
		private Shader instanceShader;
		Manager manager = new Manager();

		public Resources(App app) : base(app) { }

		public Shader BasicShader
		{
			get
			{
				if (basicShader == null)
				{
					VertexShader vShader = new VertexShader(Device, manager.VertexShaderBasic);
					ShaderSignature vSignature = new ShaderSignature(manager.VertexShaderBasic);
					PixelShader pShader = new PixelShader(Device, manager.PixelShader);
					ShaderSignature pSignature = new ShaderSignature(manager.PixelShader);
					basicShader = new Shader(App, vShader, vSignature, pShader, pSignature, new InputLayout(Device, vSignature, new[]
					{
						new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0, InputClassification.PerVertexData, 0),
						new InputElement("NORMAL", 0, Format.R32G32B32_Float, 16, 0, InputClassification.PerVertexData, 0),
						new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 28, 0, InputClassification.PerVertexData, 0)
					}));
				}
				return basicShader;
			}
		}

		public Shader InstanceShader
		{
			get
			{
				if (instanceShader == null)
				{
					VertexShader vShader = new VertexShader(Device, manager.VertexShaderInstance);
					ShaderSignature vSignature = new ShaderSignature(manager.VertexShaderInstance);
					PixelShader pShader = new PixelShader(Device, manager.PixelShader);
					ShaderSignature pSignature = new ShaderSignature(manager.PixelShader);
					instanceShader = new Shader(App, vShader, vSignature, pShader, pSignature, new InputLayout(Device, vSignature, new[]
					{
						new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0, InputClassification.PerVertexData, 0),
						new InputElement("NORMAL", 0, Format.R32G32B32_Float, 16, 0, InputClassification.PerVertexData, 0),
						new InputElement("WORLD", 0, Format.R32G32B32A32_Float, 0, 1, InputClassification.PerInstanceData, 1),
						new InputElement("WORLD", 1, Format.R32G32B32A32_Float, 16, 1, InputClassification.PerInstanceData, 1),
						new InputElement("WORLD", 2, Format.R32G32B32A32_Float, 32, 1, InputClassification.PerInstanceData, 1),
						new InputElement("WORLD", 3, Format.R32G32B32A32_Float, 48, 1, InputClassification.PerInstanceData, 1),
						new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 64, 1, InputClassification.PerInstanceData, 1)
					}));
				}
				return instanceShader;
			}
		}

		public Stream ResourceStream(string filename)
		{
			return new MemoryStream(manager.Resource(filename));
		}
	}
}

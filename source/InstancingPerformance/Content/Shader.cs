using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstancingPerformance.Core;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace InstancingPerformance.Content
{
	public class Shader : AppObject
	{
		public VertexShader VertexShader { get; }
		public PixelShader PixelShader { get; }
		private ShaderSignature vertexShaderSignature, pixelShaderSignature;
		private InputLayout layout;

		public Shader(App app, VertexShader vertexShader, ShaderSignature vertexShaderSignature, PixelShader pixelShader, ShaderSignature pixelShaderSignature, InputLayout layout)
			: base(app)
		{
			VertexShader = vertexShader;
			PixelShader = pixelShader;
			this.vertexShaderSignature = vertexShaderSignature;
			this.pixelShaderSignature = pixelShaderSignature;
			this.layout = layout;
		}

		public void Apply()
		{
			Context.VertexShader.Set(VertexShader);
			Context.PixelShader.Set(PixelShader);
			Context.InputAssembler.InputLayout = layout;
		}
	}
}

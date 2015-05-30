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
		public GeometryShader GeometryShader { get; }
		public PixelShader PixelShader { get; }
		private ShaderSignature vertexShaderSignature, geometryShaderSignature, pixelShaderSignature;
		private InputLayout layout;

		public Shader(App app, VertexShader vertexShader, ShaderSignature vertexShaderSignature, GeometryShader geometryShader, ShaderSignature geometryShaderSignature, PixelShader pixelShader, ShaderSignature pixelShaderSignature, InputLayout layout)
			: base(app)
		{
			VertexShader = vertexShader;
			GeometryShader = geometryShader;
			PixelShader = pixelShader;
			this.vertexShaderSignature = vertexShaderSignature;
			this.geometryShaderSignature = geometryShaderSignature;
			this.pixelShaderSignature = pixelShaderSignature;
			this.layout = layout;
		}

		public void Apply()
		{
			Context.VertexShader.Set(VertexShader);
			Context.GeometryShader.Set(GeometryShader);
			Context.PixelShader.Set(PixelShader);
			Context.InputAssembler.InputLayout = layout;
		}
	}
}

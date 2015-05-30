#pragma once
using namespace System;
using namespace System::Reflection;

public ref class Manager
{
private:
	HMODULE currentModule;
	array<Byte>^ pixelShader;
	array<Byte>^ vertexShaderBasic;
	array<Byte>^ vertexShaderInstance;
	array<Byte>^ vertexShaderGeometry;
	array<Byte>^ geometryShader;
public:
	property array<Byte>^ PixelShader
	{
		array<Byte>^ get()
		{
			return pixelShader;
		}
	}
	property array<Byte>^ VertexShaderBasic
	{
		array<Byte>^ get()
		{
			return vertexShaderBasic;
		}
	}
	property array<Byte>^ VertexShaderInstance
	{
		array<Byte>^ get()
		{
			return vertexShaderInstance;
		}
	}
	property array<Byte>^ VertexShaderGeometry {
		array<Byte>^ get()
		{
			return vertexShaderGeometry;
		}
	}
	property array<Byte>^ GeometryShader {
		array<Byte>^ get()
		{
			return geometryShader;
		}
	}
	array<Byte>^ Resource(String^ name);
	Manager();
private:
	void LoadPixelShader();
	void LoadVertexShaderBasic();
	void LoadVertexShaderInstance();
	void LoadVertexShaderGeometry();
	void LoadGeometryShader();
};

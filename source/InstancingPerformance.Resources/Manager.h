#pragma once
using namespace System;
using namespace System::Reflection;

public ref class Manager
{
private:
	HMODULE currentModule;
	array<Byte>^ pixelShader;
	array<Byte>^ vertexShaderBasic;
	array<Byte>^ vertxShaderInstance;
public:
	property array<Byte>^ PixelShader
	{
		array<Byte>^ get() { return pixelShader; };
	}
	property array<Byte>^ VertexShaderBasic
	{
		array<Byte>^ get() { return vertexShaderBasic; };
	}
	property array<Byte>^ VertexShaderInstance
	{
		array<Byte>^ get() { return vertxShaderInstance; };
	}
	array<Byte>^ Resource(String^ name);
	Manager();
private:
	void LoadPixelShader();
	void LoadVertexShaderBasic();
	void LoadVertexShaderInstance();
};

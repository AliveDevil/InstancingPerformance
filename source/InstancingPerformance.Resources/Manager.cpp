#include "stdafx.h"
#include "Manager.h"
#include "PixelShader.h"
#include "VertexShaderBasic.h"
#include "VertexShaderInstance.h"
#include "VertexShaderGeometry.h"
#include "GeometryShader.h"

using namespace System::Runtime::InteropServices;

Manager::Manager()
{
	currentModule = GetCurrentModuleHandle();
	LoadPixelShader();
	LoadVertexShaderBasic();
	LoadVertexShaderInstance();
	LoadVertexShaderGeometry();
	LoadGeometryShader();
}

void Manager::LoadPixelShader()
{
	pixelShader = BuildBuffer<Byte, Byte>(g_PS, _countof(g_PS));
}
void Manager::LoadVertexShaderBasic()
{
	vertexShaderBasic = BuildBuffer<Byte, Byte>(g_VSBasic, _countof(g_VSBasic));
}
void Manager::LoadVertexShaderInstance()
{
	vertexShaderInstance = BuildBuffer<Byte, Byte>(g_VSInstance, _countof(g_VSInstance));
}
void Manager::LoadVertexShaderGeometry()
{
	vertexShaderGeometry = BuildBuffer<Byte, Byte>(g_VSGeometry, _countof(g_VSGeometry));
}
void Manager::LoadGeometryShader()
{
	geometryShader = BuildBuffer<Byte, Byte>(g_GS, _countof(g_GS));
}
array<Byte>^ Manager::Resource(String^ name)
{
	pin_ptr<const wchar_t> pName = PtrToStringChars(name);
	const wchar_t* wname = pName;
	HRSRC resource;
	if (!(resource = FindResourceW(currentModule, wname, RT_RCDATA)))
	{
		return nullptr;
	}
	HGLOBAL resourceInfo;
	if (!(resourceInfo = LoadResource(currentModule, resource)))
	{
		return nullptr;
	}
	int size = SizeofResource(currentModule, resource);
	void* dataAddress;
	if (!(dataAddress = LockResource(resourceInfo)))
	{
		return nullptr;
	}

	array<Byte>^ data = gcnew array<Byte>(size);
	pin_ptr<Byte> pinPtr = &data[0];
	memcpy(pinPtr, dataAddress, size);
	return data;
}
#include "stdafx.h"
#include "Manager.h"
#include "PixelShader.h"
#include "VertexShaderBasic.h"
#include "VertexShaderInstance.h"
using namespace System::Runtime::InteropServices;

Manager::Manager()
{
	currentModule = GetCurrentModuleHandle();
	LoadPixelShader();
	LoadVertexShaderBasic();
	LoadVertexShaderInstance();
}

void Manager::LoadPixelShader() {
	int count = _countof(g_PS);
	pixelShader = gcnew array<Byte>(count);
	pin_ptr<Byte> pinPtrArray = &pixelShader[0];
	memcpy_s(pinPtrArray, count, &g_PS[0], count);
}
void Manager::LoadVertexShaderBasic() {
	int count = _countof(g_VSBasic);
	vertexShaderBasic = gcnew array<Byte>(count);
	pin_ptr<Byte> pinPtrArray = &vertexShaderBasic[0];
	memcpy_s(pinPtrArray, count, &g_VSBasic[0], count);
}
void Manager::LoadVertexShaderInstance() {
	int count = _countof(g_VSInstance);
	vertxShaderInstance = gcnew array<Byte>(count);
	pin_ptr<Byte> pinPtrArray = &vertxShaderInstance[0];
	memcpy_s(pinPtrArray, count, &g_VSInstance[0], count);
}
array<Byte>^ Manager::Resource(String^ name) {
	pin_ptr<const wchar_t> pName = PtrToStringChars(name);
	const wchar_t* wname = pName;
	HRSRC resource;
	if (!(resource = FindResourceW(currentModule, wname, RT_RCDATA))) {
		return nullptr;
	}
	HGLOBAL resourceInfo;
	if (!(resourceInfo = LoadResource(currentModule, resource))) {
		return nullptr;
	}
	int size = SizeofResource(currentModule, resource);
	void* dataAddress;
	if (!(dataAddress = LockResource(resourceInfo))) {
		return nullptr;
	}

	array<Byte>^ data = gcnew array<Byte>(size);
	pin_ptr<Byte> pinPtr = &data[0];
	memcpy(pinPtr, dataAddress, size);
	return data;
}
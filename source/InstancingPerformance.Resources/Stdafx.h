// stdafx.h : Includedatei für Standardsystem-Includedateien
// oder häufig verwendete projektspezifische Includedateien,
// die nur in unregelmäßigen Abständen geändert werden.

#pragma once

#include <Windows.h>
#include <vcclr.h>
#include "resource.h"
#define BYTE Byte

HINSTANCE __stdcall GetInstanceFromAddress(PVOID pEip);
HINSTANCE __stdcall GetCurrentInstance();
HMODULE GetCurrentModuleHandle();

template<typename TSource, typename TDestination>
array<TDestination>^ BuildBuffer(const TSource* source, size_t count)
{
	array<TDestination>^ destination = gcnew array<TDestination>(count);
	pin_ptr<TDestination> pinPtrArray = &destination[0];
	memcpy_s(pinPtrArray, count, source, count);
	return destination;
}
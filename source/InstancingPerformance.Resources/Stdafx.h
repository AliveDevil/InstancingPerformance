// stdafx.h : Includedatei f�r Standardsystem-Includedateien
// oder h�ufig verwendete projektspezifische Includedateien,
// die nur in unregelm��igen Abst�nden ge�ndert werden.

#pragma once

#include <Windows.h>
#include <vcclr.h>
#include "resource.h"
#define BYTE Byte

HINSTANCE __stdcall GetInstanceFromAddress(PVOID pEip);
HINSTANCE __stdcall GetCurrentInstance();
HMODULE GetCurrentModuleHandle();
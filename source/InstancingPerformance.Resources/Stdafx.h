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
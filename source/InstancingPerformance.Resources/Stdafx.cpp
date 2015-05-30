// stdafx.cpp : Quelldatei, die nur die Standard-Includes einbindet.
// InstancingPerformance.Resources.pch ist der vorkompilierte Header.
// stdafx.obj enthält die vorkompilierten Typinformationen.

#include "stdafx.h"

HINSTANCE __stdcall GetInstanceFromAddress(PVOID pEip)
{
	MEMORY_BASIC_INFORMATION mem;
	if (VirtualQuery(pEip, &mem, sizeof(mem)))
	{
		return (HINSTANCE)mem.AllocationBase;
	}
	return NULL;
}

__declspec(naked)
HINSTANCE __stdcall GetCurrentInstance()
{
	__asm
	{
#ifdef _M_IX86
		mov eax, [esp]
		push eax
			jmp GetInstanceFromAddress
#else
# error This machine type is not supported.
#endif
	}
}

HMODULE GetCurrentModuleHandle()
{
	HMODULE hMod = NULL;
	GetModuleHandleExW(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS | GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT,
					   reinterpret_cast<LPCWSTR>(&GetCurrentModuleHandle),
					   &hMod);
	return hMod;
}

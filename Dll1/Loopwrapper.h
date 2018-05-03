#pragma once

#include "LoopCallbacksample.h"

namespace Loopwrapper
{
	typedef void(__stdcall *LoopCallbackWrapper)(void* pContext);

	extern "C" __declspec(dllexport) void __stdcall SetCallbackFunc(LoopCallbackWrapper callback);
	extern "C" __declspec(dllexport) void __stdcall SetCallbackContext(void* pContext);
	extern "C" __declspec(dllexport) void __stdcall Loop();
}
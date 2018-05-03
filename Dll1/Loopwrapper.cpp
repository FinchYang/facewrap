#include "stdafx.h"
#include "Loopwrapper.h"

namespace Loopwrapper
{
	LoopCallbackWrapper g_callbackWrapper;
	LoopCallbacksample* g_pObj = new LoopCallbacksample();

	void LoopCallbackFunc(void* pContext);

	void __stdcall SetCallbackFunc(LoopCallbackWrapper callback)
	{
		g_callbackWrapper = callback;
		g_pObj->SetCallbackFunc(LoopCallbackFunc);
	}

	void __stdcall SetCallbackContext(void* pContext)
	{
		g_pObj->SetCallbackContext(pContext);
	}

	void __stdcall Loop()
	{
		g_pObj->Loop();
	}

	void LoopCallbackFunc(void* pContext)
	{
		if (g_callbackWrapper != nullptr)
		{
			g_callbackWrapper(pContext);
		}
	}
}
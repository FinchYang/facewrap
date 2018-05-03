#include "stdafx.h"
#include "SampleCppWrapper.h"

namespace SampleCppWrapper
{
	SampleCppClass* g_pObj = new SampleCppClass();

	int __stdcall Add(int n1, int n2)
	{
		return g_pObj->Add(n1, n2);
	}

	int __stdcall Sub(int n1, int n2)
	{
		return g_pObj->Sub(n1, n2);
	}
}
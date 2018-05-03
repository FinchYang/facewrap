#pragma once

#include "SampleCppClass.h"

namespace SampleCppWrapper
{
	extern "C" __declspec(dllexport) int __stdcall Add(int n1, int n2);
	extern "C" __declspec(dllexport) int __stdcall Sub(int n1, int n2);
}
#pragma once
#include "stdafx.h"

class __declspec(dllexport) SampleCppClass
{
public:
	SampleCppClass(void);
	~SampleCppClass(void);

	int Add(int n1, int n2);
	int Sub(int n1, int n2);
};
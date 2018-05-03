#include "stdafx.h"
#include "LoopCallbacksample.h"

LoopCallbacksample::LoopCallbacksample(void)
{
}

LoopCallbacksample::~LoopCallbacksample(void)
{
}

void LoopCallbacksample::SetCallbackFunc(LoopCallback callback)
{
	m_callback = callback;
}

void LoopCallbacksample::SetCallbackContext(void* pContext)
{
	m_pContext = pContext;
}

void LoopCallbacksample::Loop()
{
	for (int i = 0; i<10; i++)
	{
		if (m_callback != nullptr)
		{
			m_callback(m_pContext);
		}
	}
}
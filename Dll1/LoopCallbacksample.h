#pragma once

typedef void(*LoopCallback)(void* pContext);

class __declspec(dllexport) LoopCallbacksample
{
public:
	LoopCallbacksample(void);
	~LoopCallbacksample(void);

	void SetCallbackFunc(LoopCallback callback);
	void SetCallbackContext(void* pContext);
	void Loop();
private:
	LoopCallback m_callback;
	void* m_pContext;
};
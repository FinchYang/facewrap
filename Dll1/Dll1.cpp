// Dll1.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"


#include<stdio.h>

__declspec(dllexport)
int haha()
{
	return 99;
}

long __stdcall TransProjPt(LPSTR psGridFile, long lDirection, double dEasting, double
	dNorthing, long lZone, double* pdEastNew, double* pdNorthNew, double* pdEastAcc,
	double* pdNorthAcc)
{
	return 99;
}
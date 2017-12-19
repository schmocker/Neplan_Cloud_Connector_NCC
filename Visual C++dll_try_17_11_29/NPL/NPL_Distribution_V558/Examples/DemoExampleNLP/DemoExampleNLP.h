// DemoExampleNLP.h : main header file for the DemoExampleNLP DLL
//

#pragma once

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols


#ifdef NPL_BATCH_EXPORTS
#define NPLMODULE_API __declspec(dllexport)
#else
#define NPLMODULE_API __declspec(dllimport)
#endif


NPLMODULE_API BOOL RunNeplanScript();

// CDemoExampleNLPApp
// See DemoExampleNLP.cpp for the implementation of this class
//

class CDemoExampleNLPApp : public CWinApp
{
public:
	CDemoExampleNLPApp();

// Overrides
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////
// Author: Giatgen Cott
// Date:   26. Januar 2007

// Copyright © 1988-2006 BCP Busarello + Cott + Partner Inc.  All Rights Reserved.
// 
// DemoExampleNPL.cpp : Examples for using the NEPLAN Programming Library
//
// This computer software is owned by BCP Busarello + Cott + Partner Inc. and is 
// protected by international copyright laws and other laws and by international 
// treaties. This computer software is furnished by BCP Busarello + Cott + Partner, Inc. 
// pursuant to a written license agreement and may be used, copied, transmitted, 
// and stored only in accordance with the terms of such license agreement and with
// the inclusion of the above copyright notice.  This computer software or 
// any other copies thereof may not be provided or otherwise made available 
// to any other person.
// This computer software is a trade secret of BCP Busarello + Cott + Partner, Inc. 
//
// This source code is only intended as a supplement to
// the NEPLAN Programming Library and related
// electronic documentation provided with the library.
//
/////////////////////////////////////////////////////////////////////////


#include "stdafx.h"
//#include <stdio.h>
//#include <tchar.h>
#include "Resource.h"
#include "NeplanProgrammingLibrary.h"
#include "NPLSelectFileNameDlg.h"
#include "DemoExampleNLP.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CDemoExampleNLPApp

BEGIN_MESSAGE_MAP(CDemoExampleNLPApp, CWinApp)
END_MESSAGE_MAP()


static void RunTestNPL(CString sProjectFileName);

// CDemoExampleNLPApp construction
CDemoExampleNLPApp::CDemoExampleNLPApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}


// The one and only CDemoExampleNLPApp object

CDemoExampleNLPApp theApp;


// CDemoExampleNLPApp initialization

BOOL CDemoExampleNLPApp::InitInstance()
{
	CWinApp::InitInstance();


	return TRUE;
}

//////////////////////////////////
// RunNeplanScript is the
// main entry to run dll
//////////////////////////////////
NPLMODULE_API BOOL RunNeplanScript()
{
  BOOL bRunOk = TRUE;
  CNPLSelectFileNameDlg dlg;
  if (dlg.DoModal() != IDOK)
    return FALSE;

  //Check file if "NPL-Demo-Ele.nepprj" is available
  CFileFind finder;
  BOOL bFound = finder.FindFile(dlg.m_FileName);
  if (!bFound) 
  {
    AfxMessageBox(_T("Import file not found!"));
    return FALSE;
  }
  //Run script
  RunTestNPL(dlg.m_FileName);

  //Show report
  ShowReport();

  return bRunOk;
}

/////////////////////////////////////////////////////////////////////////
// NPL Example shows how to:
// - run load flow, 
// - change a line length
/////////////////////////////////////////////////////////////////////////
void RunTestNPL(CString sProjectFileName)
{
  TCHAR *DataExampleDirectory = _T("");
  TCHAR PathFileName[400];
  swprintf_s(PathFileName, _T("%s"), sProjectFileName);
  //Open empty NEPLAN project file 'NPL-Demo-Ele.nepprj'
	BOOL bOpen = OpenNeplanProject(PathFileName);
  if (!bOpen)
  {
    AfxMessageBox(_T("Could not open empty NEPLAN project!"));
    return;
  }

  //Messages to log file
 	TCHAR cMessageText[300];

  //run initial load flow
  RunAnalysisLF();
  swprintf_s(cMessageText, _T("run initial load flow"));
  WriteMessageToLogFile(cMessageText);
  
  //change the line length of line 'LIN 2-4 2'
	unsigned long ElementID=0;
  GetElementByName(_T("LINE"),_T("LIN 2-4 2"), ElementID);  
  if (ElementID > 0)
	  SetParameterDouble(ElementID, _T("Length"), 0.5);

  //run load flow with changed line length
  RunAnalysisLF();
  swprintf_s(cMessageText, _T("run load flow with changed line length"));
  WriteMessageToLogFile(cMessageText);
}


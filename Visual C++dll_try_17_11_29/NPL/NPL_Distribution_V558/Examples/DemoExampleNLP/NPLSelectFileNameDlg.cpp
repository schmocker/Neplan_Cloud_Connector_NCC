// NPLSelectFileNameDlg.cpp : implementation file
//

#include "stdafx.h"
#include "DemoExampleNLP.h"
#include "NPLSelectFileNameDlg.h"
#include ".\nplselectfilenamedlg.h"


// CNPLSelectFileNameDlg dialog

IMPLEMENT_DYNAMIC(CNPLSelectFileNameDlg, CDialog)
CNPLSelectFileNameDlg::CNPLSelectFileNameDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CNPLSelectFileNameDlg::IDD, pParent)
  , m_FileName(_T(""))
{
}

CNPLSelectFileNameDlg::~CNPLSelectFileNameDlg()
{
}

void CNPLSelectFileNameDlg::DoDataExchange(CDataExchange* pDX)
{
  CDialog::DoDataExchange(pDX);
  DDX_Text(pDX, IDC_EDIT1, m_FileName);
}


BEGIN_MESSAGE_MAP(CNPLSelectFileNameDlg, CDialog)
  ON_BN_CLICKED(IDC_BUTTON1, OnBnClickedButton1)
  ON_BN_CLICKED(IDCANCEL, OnBnClickedCancel)
END_MESSAGE_MAP()


// CNPLSelectFileNameDlg message handlers

void CNPLSelectFileNameDlg::OnBnClickedButton1()
{
	CString strExt = _T("*.nepprj|*.*");
  CString strFilter = _T("NEPLAN Project File(*.nepprj)|*.nepprj|All Files (*.*)|*.*||");

  CString strInitFile = _T("");
	CFileDialog dlg(
		TRUE,		// open dialog
		strExt,		// default extention
		strInitFile,		// initial filename
		OFN_HIDEREADONLY,	// flags
		strFilter,	// filter
		this);	// parent window

	if (dlg.DoModal() == IDOK)
	{
    m_FileName = dlg.GetPathName();
    UpdateData(FALSE);
  }
}

void CNPLSelectFileNameDlg::OnBnClickedCancel()
{
  // TODO: Add your control notification handler code here
  OnCancel();
}

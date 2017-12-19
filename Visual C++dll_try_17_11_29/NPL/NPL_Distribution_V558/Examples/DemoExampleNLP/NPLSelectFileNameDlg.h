#pragma once


// CNPLSelectFileNameDlg dialog

class CNPLSelectFileNameDlg : public CDialog
{
	DECLARE_DYNAMIC(CNPLSelectFileNameDlg)

public:
	CNPLSelectFileNameDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CNPLSelectFileNameDlg();

// Dialog Data
	enum { IDD = IDD_DIALOG1 };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
  afx_msg void OnBnClickedButton1();
  CString m_FileName;
  afx_msg void OnBnClickedCancel();
};

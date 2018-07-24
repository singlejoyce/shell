// AutoBuilderDlg.cpp : implementation file
//

#include "stdafx.h"
#include "AutoBuilder.h"
#include "AutoBuilderDlg.h"
#include "shellapi.h"
#include "Md5.h"
#include <io.h>
#include <afxdb.h>
#include <odbcinst.h>
#include <MMSystem.h>
#include <string>
using namespace System;

using namespace std;

#pragma comment(lib, "winmm.lib")

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

// CAboutDlg dialog used for App About

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

	// Dialog Data
	enum { IDD = IDD_ABOUTBOX };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	// Implementation
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
END_MESSAGE_MAP()


// CAutoBuilderDlg dialog




CAutoBuilderDlg::CAutoBuilderDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CAutoBuilderDlg::IDD, pParent)
	, m_ParterName(_T(""))
	, m_Bundleversioncode(0)
	, m_bUI(FALSE)
	, m_batlas(FALSE)
	, m_bStage(FALSE)
	, m_strUnityPath(_T(""))
	, m_ProjectPath(_T(""))
	, m_thridPath(_T(""))
	, m_CurrntWorkDir(_T(""))
	, m_LogFile(NULL)
	, m_strbundleversion(_T(""))
	, m_SVNExePath(_T(""))
	, m_ResPath(_T(""))
	, m_ResourceVersion(0)
	, m_Rar(_T(""))
	, m_ProjectReVersion(0)
	, m_ThridReVersion(0)
	, m_ResReVersion(0)
	, m_strConfigPath(_T(""))
	, m_nConfigVersion(0)
	, m_strResPackName(_T(""))
	, m_SettingPath(_T(""))
	, m_SettingReVersion(0)
	, m_strMaterialsPath(_T(""))
	, m_strItemlistPath(_T(""))
	, m_bIsSmallPack(FALSE)
	, m_nMaterialsVersion(0)
	, m_strProductName(_T(""))
	, m_strMaterialsDiffPath(_T(""))
	, m_bSvnUp(FALSE)
	, m_strEffectPath(_T(""))
	, m_strEffectlistPath(_T(""))
	, m_nEffectVersion(0)
	, m_bGenDynamicPrefab(FALSE)
	, m_bDynDownlad(FALSE)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);

	m_DefaultSplashScreen = L"{fileID: 2800000, guid: 8d3cc0ac92f37ee4b9ba1f72017a81f7, type: 3}";
	m_DefaultAndroidKeyPath = L"D:\\smartphone\\DDLELW.keystore";
	m_DefaultAndroidAPILevel = 9;

	m_strMusiclistPath = _T("");
	m_strMusicPath = _T("");
	m_nMusicVersion = 0;
}

void CAutoBuilderDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_CBString(pDX, IDC_COMBOPARTER, m_ParterName);
	DDX_Text(pDX, IDC_EDIT2, m_Bundleversioncode);
	DDX_Check(pDX, IDC_UICOMPILE, m_bUI);
	DDX_Check(pDX, IDC_atlasCOMPILE2, m_batlas);
	DDX_Check(pDX, IDC_atlasCOMPILE3, m_bStage);
	DDX_Text(pDX, IDC_UNITYPATH, m_strUnityPath);
	DDX_Text(pDX, IDC_EDIT1, m_ProjectPath);
	DDX_Control(pDX, IDC_COMBOPARTER, m_ParterSelect);
	DDX_Text(pDX, IDC_THRIDPATH, m_thridPath);
	DDX_Text(pDX, IDC_EDIT3, m_strbundleversion);
	DDX_Text(pDX, IDC_SVNPATH, m_SVNExePath);
	DDX_Text(pDX, IDC_RESPATH, m_ResPath);
	DDX_Text(pDX, IDC_EDIT4, m_ResourceVersion);
	DDX_Text(pDX, IDC_RARPATH, m_Rar);
	DDX_Text(pDX, IDC_EDIT5, m_ProjectReVersion);
	DDX_Text(pDX, IDC_EDIT6, m_ThridReVersion);
	DDX_Text(pDX, IDC_EDIT7, m_ResReVersion);
	DDX_Text(pDX, IDC_CONFIGPATH, m_strConfigPath);
	DDX_Text(pDX, IDC_EDIT8, m_nConfigVersion);
	DDX_CBString(pDX, IDC_RESPACKNAME, m_strResPackName);
	DDX_Control(pDX, IDC_RESPACKNAME, m_ResPackNameSelect);
	DDX_Text(pDX, IDC_SETTINGPATH, m_SettingPath);
	DDX_Text(pDX, IDC_EDIT9, m_SettingReVersion);
	DDX_Text(pDX, IDC_MATERIALSPATH, m_strMaterialsPath);
	DDX_Text(pDX, IDC_ITEMLISTPATH, m_strItemlistPath);
	DDX_Check(pDX, IDC_CHECKSMALLPACK, m_bIsSmallPack);
	DDX_Text(pDX, IDC_EDIT11, m_nMaterialsVersion);
	DDX_Text(pDX, IDC_MUSICLISTPATH, m_strMusiclistPath);
	DDX_Text(pDX, IDC_MUSICPATH, m_strMusicPath);
	DDX_Text(pDX, IDC_EDIT12, m_nMusicVersion);
	DDX_Text(pDX, IDC_PRODUCTNAME, m_strProductName);
	DDX_Text(pDX, IDC_MATERIALSDIFFPATH, m_strMaterialsDiffPath);
	DDX_Check(pDX, IDC_SVNUP, m_bSvnUp);
	DDX_Text(pDX, IDC_EFFECTPATH, m_strEffectPath);
	DDX_Text(pDX, IDC_EFFECTLISTPATH, m_strEffectlistPath);
	DDX_Text(pDX, IDC_EDIT14, m_nEffectVersion);
	DDX_Check(pDX, IDC_CHECKGENPREFAB, m_bGenDynamicPrefab);
	DDX_Check(pDX, IDC_CHECKDYNDOWNLOAD, m_bDynDownlad);
}

BEGIN_MESSAGE_MAP(CAutoBuilderDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_BN_CLICKED(IDOK, &CAutoBuilderDlg::OnBnClickedOk)
	ON_BN_CLICKED(IDC_UICOMPILE, &CAutoBuilderDlg::OnBnClickedUicompile)
	ON_BN_CLICKED(IDC_atlasCOMPILE2, &CAutoBuilderDlg::OnBnClickedatlascompile2)
	ON_BN_CLICKED(IDC_atlasCOMPILE3, &CAutoBuilderDlg::OnBnClickedatlascompile3)
	ON_BN_CLICKED(IDC_CHECKSMALLPACK, &CAutoBuilderDlg::OnBnClickedChecksmallpack)
	ON_BN_CLICKED(IDC_SVNUP, &CAutoBuilderDlg::OnBnClickedSvnUp)
	ON_BN_CLICKED(IDC_CHECKGENPREFAB, &CAutoBuilderDlg::OnBnClickedCheckgenprefab)
	ON_BN_CLICKED(IDC_CHECKDYNDOWNLOAD, &CAutoBuilderDlg::OnBnClickedCheckdyndownload)
END_MESSAGE_MAP()


// CAutoBuilderDlg message handlers

BOOL CAutoBuilderDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here
	LoadData();
	m_ParterSelect.Clear();
	m_ParterSelect.AddString(L"ALL");
	int nCount = GetPrivateProfileInt(L"ALL", L"Count", 0, L".\\AutoBuild.ini");
	WCHAR cValue[MAX_PATH];
	for (int i = 0; i < nCount; i++)
	{
		CString s;
		s.Format(L"Parter%d", i + 1);
		GetPrivateProfileString(L"ALL", s.GetBuffer(), L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
		CString strParterName = cValue;
		m_ParterSelect.AddString(strParterName);
	}


	nCount = GetPrivateProfileInt(L"PackName", L"Count", 0, L".\\AutoBuild.ini");
	GetPrivateProfileString(L"PackName", L"Current", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_strResPackName = cValue;
	for (int i = 0; i < nCount; i++)
	{
		CString s;
		s.Format(L"Name%d", i + 1);
		GetPrivateProfileString(L"PackName", s.GetBuffer(), L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
		CString strPackName = cValue;
		m_ResPackNameSelect.AddString(strPackName);

	}
	//m_ResPackNameSelect.SetCurSel(nCur);
	UpdateData(FALSE);
	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CAutoBuilderDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CAutoBuilderDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CAutoBuilderDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

string Chinese2Unicode(wstring& strChinese)
{
	string strUnicodes;
	const wchar_t *szChinese = strChinese.c_str();
	int iSize = (int)strChinese.length();
	for (int i = 0; i < iSize; i++)
	{
		int item = (int)szChinese[i];

		if (item >= 0 && item <= 256)
		{
			char temp[10] = { 0 };
			sprintf_s(temp, "%c", item);
			strUnicodes += temp;
		}
		else
		{
			char temp[10] = { 0 };
			sprintf_s(temp, "%X", item);
			strUnicodes += "\\u";
			strUnicodes += temp;
		}
	}

	return strUnicodes;
}

string wstring2string(wstring sToMatch)
{
	string sResult;
	int iLen = WideCharToMultiByte(CP_ACP, NULL, sToMatch.c_str(), -1, NULL, 0, NULL, FALSE); // 计算转换后字符串的长度。（包含字符串结束符）  
	char *lpsz = new char[iLen];
	WideCharToMultiByte(CP_OEMCP, NULL, sToMatch.c_str(), -1, lpsz, iLen, NULL, FALSE); // 正式转换。  
	sResult.assign(lpsz, iLen - 1); // 对string对象进行赋值。  
	delete[]lpsz;

	return sResult;
}

//BOOL CAutoBuilderDlg::CheckDecryptContent()
//{
//	string workDir = wstring2string(m_CurrntWorkDir.GetBuffer());
//	workDir += "\\xuanqu\\lwts\\res\\UI\\Unity\\uiconfig.txt";
//
//	String^ dir = gcnew String(workDir.c_str());
//	if (ConvertTool::CheckDecryptContent(dir))
//		return TRUE;
//
//	return FALSE;
//}

BOOL CAutoBuilderDlg::DeletePath(CString strPath)
{
	CFileFind tempFind;
	CString strTempFileFind = strPath + L"\\*.*";
	BOOL IsFinded = tempFind.FindFile(strTempFileFind);
	while (IsFinded)
	{
		IsFinded = tempFind.FindNextFile();
		if (!tempFind.IsDots())
		{
			CString strFoundFileName = tempFind.GetFileName();
			if (tempFind.IsDirectory())
			{
				CString strTempDir = strPath + L"\\" + strFoundFileName;
				DeletePath(strTempDir);
			}
			else
			{
				CString strTempFileName = strPath + L"\\" + strFoundFileName;
				DeleteFile(strTempFileName);
			}
		}
	}

	tempFind.Close();
	if (!RemoveDirectory(strPath))
	{
		return FALSE;
	}
	return TRUE;
}

void CAutoBuilderDlg::GenerateAllPack()
{
	//copy res
	CString strCopyCmd = L"C:\\Windows\\System32\\ROBOCOPY.exe";
	CString strparam = L" \"" + m_ResPath + L"\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all\"" + L" /MIR   /xd Animations Animations_Basic Materials Materials_window Music Effect .svn /xf *.xlsx *.sog  *.sql";
	//Animations
	CString m_strAnimations = L" \"" + m_ResPath + L"\\Animations\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all\\Animations\"" + L" /MIR /e";;

	CFile f;
	f.Open(m_CurrntWorkDir + L"\\log\\ResCopy.log", CFile::modeCreate | CFile::modeWrite);
	ExceCommand(strCopyCmd, strparam, f.m_hFile);
	ExceCommand(strCopyCmd, m_strAnimations, f.m_hFile);
	f.Close();

	CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all\\UI", NULL);

	//Music
	if (!m_strMusicPath.IsEmpty())
	{
		bool hasError = false;
		CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all\\Music", NULL);

		CFile f;
		f.Open(m_CurrntWorkDir + L"\\log\\MusicCopy.log", CFile::modeCreate | CFile::modeWrite);

		//copy Music
		if (m_strMusiclistPath.IsEmpty())
		{
			CString strCopyCmd = L"C:\\Windows\\System32\\ROBOCOPY.exe";
			CString strparam = L" \"" + m_strMusicPath + L"\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all\\Music\"" + L" /MIR   /xd .svn /xf *.xlsx *.sog *.sql";
			ExceCommand(strCopyCmd, strparam, INVALID_HANDLE_VALUE);
		}
		else
		{
			hasError = !FilterMusic(&f, TRUE);
		}

		f.Close();

		if (hasError)
		{
			WriteLog(L"Build failed!");
			//OnOK();

			Closelog();

			//Beep(1600, 1600);
			PlaySound(L"note.wav", NULL, SND_ASYNC | SND_LOOP);

			MessageBox(L"Music错误,编译失败", L"编译结束");

			PlaySound(NULL, NULL, SND_PURGE);

			return;
		}
	}

	//Effect
	if (!m_strEffectPath.IsEmpty())
	{
		bool hasError = false;
		CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all\\Effect", NULL);

		CFile f;
		f.Open(m_CurrntWorkDir + L"\\log\\EffectCopy.log", CFile::modeCreate | CFile::modeWrite);

		//copy Effect
		CString strCopyCmd = L"C:\\Windows\\System32\\ROBOCOPY.exe";
		CString strparam = L" \"" + m_strEffectPath + L"\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all\\Effect\"" + L" /MIR   /xd .svn /xf *.xlsx *.sog *.sql";
		ExceCommand(strCopyCmd, strparam, INVALID_HANDLE_VALUE);

		f.Close();
	}

	//Icon
	if (FALSE)
	{
		CString m_CurrntIconPath = m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all\\Icon";
		DeletePath(m_CurrntIconPath);

		bool hasError = false;
		CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all\\Icon", NULL);

		CString iconPath = m_ResPath + L"\\Icon";

		CFile f;
		f.Open(m_CurrntWorkDir + L"\\log\\IconCopy.log", CFile::modeCreate | CFile::modeWrite);

		//copy icon
		if (!m_strItemlistPath.IsEmpty())
		{
			hasError = !FilterIcon(&f, TRUE);
		}

		f.Close();

		if (hasError)
		{
			WriteLog(L"Build failed!");
			//OnOK();

			Closelog();

			//Beep(1600, 1600);
			PlaySound(L"note.wav", NULL, SND_ASYNC | SND_LOOP);

			MessageBox(L"Icon错误,编译失败", L"编译结束");

			PlaySound(NULL, NULL, SND_PURGE);

			return;
		}
	}

	//Materials
	if (!m_strMaterialsPath.IsEmpty())
	{
		bool hasError = false;
		CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all\\Materials", NULL);

		CFile f;
		f.Open(m_CurrntWorkDir + L"\\log\\MaterialsCopy.log", CFile::modeCreate | CFile::modeWrite);

		//copy Materials
		if (m_strItemlistPath.IsEmpty())
		{
			CString strCopyCmd = L"C:\\Windows\\System32\\ROBOCOPY.exe";
			CString strparam = L" \"" + m_strMaterialsPath + L"\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all\\Materials\"" + L" /MIR   /xd .svn /xf *.xlsx *.sog *.sql";
			ExceCommand(strCopyCmd, strparam, INVALID_HANDLE_VALUE);
		}
		else
		{
			CString strCopyCmd = L"C:\\Windows\\System32\\ROBOCOPY.exe";
			CString strparam = L" \"" + m_strMaterialsPath + L"\\Skin\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all\\Materials\\Skin\"" + L" /MIR   /xd .svn /xf *.xlsx *.sog *.sql";
			ExceCommand(strCopyCmd, strparam, INVALID_HANDLE_VALUE);

			hasError = !FilterMaterials(&f, TRUE);
		}

		f.Close();

		if (hasError)
		{
			WriteLog(L"Build failed!");
			//OnOK();

			Closelog();

			//Beep(1600, 1600);
			PlaySound(L"note.wav", NULL, SND_ASYNC | SND_LOOP);

			MessageBox(L"Material错误,编译失败", L"编译结束");

			PlaySound(NULL, NULL, SND_PURGE);

			return;
		}
	}
}

void CAutoBuilderDlg::GeneratePack(BOOL isSmallPack, BOOL isDynDownlad)
{
	//copy res
	CString strCopyCmd = L"C:\\Windows\\System32\\ROBOCOPY.exe";
	CString strparam = L" \"" + m_ResPath + L"\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\"" + L" /MIR   /xd Animations Animations_Basic Materials Materials_window Music Effect .svn /xf *.xlsx *.sog  *.sql";
	//Animations
	CString m_strAnimations;
	if (isSmallPack)
	{
		m_strAnimations = L" \"" + m_ResPath + L"\\Animations_Basic\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Animations\"" + L" /MIR /e";
	}
	else
	{
		m_strAnimations = L" \"" + m_ResPath + L"\\Animations\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Animations\"" + L" /MIR /e";;
	}

	CFile f;
	f.Open(m_CurrntWorkDir + L"\\log\\ResCopy.log", CFile::modeCreate | CFile::modeWrite);
	ExceCommand(strCopyCmd, strparam, f.m_hFile);
	ExceCommand(strCopyCmd, m_strAnimations, f.m_hFile);
	f.Close();

	CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\UI", NULL);

	//Music
	if (!m_strMusicPath.IsEmpty())
	{
		bool hasError = false;
		CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Music", NULL);

		CFile f;
		f.Open(m_CurrntWorkDir + L"\\log\\MusicCopy.log", CFile::modeCreate | CFile::modeWrite);

		//copy Music
		if (m_strMusiclistPath.IsEmpty())
		{
			CString strCopyCmd = L"C:\\Windows\\System32\\ROBOCOPY.exe";
			CString strparam = L" \"" + m_strMusicPath + L"\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Music\"" + L" /MIR   /xd .svn /xf *.xlsx *.sog *.sql";
			ExceCommand(strCopyCmd, strparam, INVALID_HANDLE_VALUE);
		}
		else
		{
			hasError = !FilterMusic(&f, FALSE);
		}

		f.Close();

		if (hasError)
		{
			WriteLog(L"Build failed!");
			//OnOK();

			Closelog();

			//Beep(1600, 1600);
			PlaySound(L"note.wav", NULL, SND_ASYNC | SND_LOOP);

			MessageBox(L"Music错误,编译失败", L"编译结束");

			PlaySound(NULL, NULL, SND_PURGE);

			return;
		}
	}

	//Effect
	if (!m_strEffectPath.IsEmpty())
	{
		bool hasError = false;
		CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Effect", NULL);

		CFile f;
		f.Open(m_CurrntWorkDir + L"\\log\\EffectCopy.log", CFile::modeCreate | CFile::modeWrite);

		//copy Effect
		CString strCopyCmd = L"C:\\Windows\\System32\\ROBOCOPY.exe";
		CString strparam = L" \"" + m_strEffectPath + L"\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Effect\"" + L" /MIR   /xd .svn /xf *.xlsx *.sog *.sql";
		if (isDynDownlad)
		{
			strparam = L" \"" + m_strEffectPath + L"\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Effect\"" + L" /MIR   /xd .svn EnchantEffect /xf *.xlsx *.sog *.sql flotage*.* foot*.* hand*.* pr_medal*.* marriage*.*";
		}
		ExceCommand(strCopyCmd, strparam, INVALID_HANDLE_VALUE);

		f.Close();
	}

	//Icon
	if (FALSE)//(isDynDownlad)
	{
		CString m_CurrntIconPath = m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Icon";
		DeletePath(m_CurrntIconPath);

		if (!isDynDownlad)
		{
			bool hasError = false;
			CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Icon", NULL);

			CString iconPath = m_ResPath + L"\\Icon";

			CFile f;
			f.Open(m_CurrntWorkDir + L"\\log\\IconCopy.log", CFile::modeCreate | CFile::modeWrite);

			//copy icon
			if (!m_strItemlistPath.IsEmpty())
			{
				hasError = !FilterIcon(&f, FALSE);
			}

			f.Close();

			if (hasError)
			{
				WriteLog(L"Build failed!");
				//OnOK();

				Closelog();

				//Beep(1600, 1600);
				PlaySound(L"note.wav", NULL, SND_ASYNC | SND_LOOP);

				MessageBox(L"Icon错误,编译失败", L"编译结束");

				PlaySound(NULL, NULL, SND_PURGE);

				return;
			}
		}
	}

	//Materials
	if (!m_strMaterialsPath.IsEmpty())
	{
		bool hasError = false;
		CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Materials", NULL);

		CFile f;
		f.Open(m_CurrntWorkDir + L"\\log\\MaterialsCopy.log", CFile::modeCreate | CFile::modeWrite);

		//copy Materials
		if (m_strItemlistPath.IsEmpty())
		{
			CString strCopyCmd = L"C:\\Windows\\System32\\ROBOCOPY.exe";
			CString strparam = L" \"" + m_strMaterialsPath + L"\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Materials\"" + L" /MIR   /xd .svn /xf *.xlsx *.sog *.sql";
			ExceCommand(strCopyCmd, strparam, INVALID_HANDLE_VALUE);
		}
		else
		{
			CString strCopyCmd = L"C:\\Windows\\System32\\ROBOCOPY.exe";
			CString strparam = L" \"" + m_strMaterialsPath + L"\\Skin\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Materials\\Skin\"" + L" /MIR   /xd .svn /xf *.xlsx *.sog *.sql";
			ExceCommand(strCopyCmd, strparam, INVALID_HANDLE_VALUE);

			//strparam = L" \"" + m_strMaterialsPath + L"\\Pets\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Materials\\Pets\"" + L" /MIR   /xd .svn /xf *.xlsx *.sog *.sql";
			//ExceCommand(strCopyCmd, strparam, INVALID_HANDLE_VALUE);

			hasError = !FilterMaterials(&f, FALSE);
		}

		f.Close();

		if (hasError)
		{
			WriteLog(L"Build failed!");
			//OnOK();

			Closelog();

			//Beep(1600, 1600);
			PlaySound(L"note.wav", NULL, SND_ASYNC | SND_LOOP);

			MessageBox(L"Material错误,编译失败", L"编译结束");

			PlaySound(NULL, NULL, SND_PURGE);

			return;
		}
	}

}

void CAutoBuilderDlg::SvnUpdate()
{
	SVNClear(m_ProjectPath + L"\\Assets");
	Revert(m_ProjectPath + L"\\Assets");
	GetNew(m_ProjectPath + L"\\Assets", L"Project", m_ProjectReVersion);

	SVNClear(m_SettingPath);
	Revert(m_SettingPath);
	GetNew(m_SettingPath, L"Setting", m_SettingReVersion);

	SVNClear(m_ResPath);
	Revert(m_ResPath);
	GetNew(m_ResPath, L"Res", m_ResReVersion);

	SVNClear(m_thridPath);
	Revert(m_thridPath);
	GetNew(m_thridPath, L"Thrid", m_ThridReVersion);

	SVNClear(m_strConfigPath);
	Revert(m_strConfigPath);
	GetNew(m_strConfigPath, L"Config", m_nConfigVersion);

	if (!m_strMaterialsDiffPath.IsEmpty())
	{
		SVNClear(m_strMaterialsDiffPath);
		Revert(m_strMaterialsDiffPath);
		GetNew(m_strMaterialsDiffPath, L"Materials", m_nMaterialsVersion);
	}

	if (!m_strMaterialsPath.IsEmpty())
	{
		SVNClear(m_strMaterialsPath);
		Revert(m_strMaterialsPath);
		GetNew(m_strMaterialsPath, L"Materials", m_nMaterialsVersion);
	}

	if (!m_strMusicPath.IsEmpty())
	{
		SVNClear(m_strMusicPath);
		Revert(m_strMusicPath);
		GetNew(m_strMusicPath, L"Music", m_nMusicVersion);
	}

	if (!m_strEffectPath.IsEmpty())
	{
		SVNClear(m_strEffectPath);
		Revert(m_strEffectPath);
		GetNew(m_strEffectPath, L"Effect", m_nEffectVersion);
	}
}

void CAutoBuilderDlg::BuildRes()
{
	ClearScriptDefine();
	if (m_bIsSmallPack || m_bDynDownlad)
	{
		//DoChangeScriptDefine(m_SettingPath);
		if (m_bDynDownlad)
		{
			RebuildScriptDefine(L"PACKAGE_DYNAMICDOWNLOAD");
		}
		else
		{
			RebuildScriptDefine(L"PACKAGE_BASIC");
		}
	}

	if (m_bGenDynamicPrefab)
	{
		GenerateDynamicPrefab();
	}

	//build UI
	if (m_bUI)
	{
		BuildUI();
	}

	//build atlas
	if (m_batlas)
	{
		BuildAtlas();
	}

	//build stage
	if (m_bStage)
	{
		BuildStage();
	}
}

void CAutoBuilderDlg::CopyResExtend()
{
	CString strScr = m_ProjectPath + L"\\Res_Extend";
	CString strDes = m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all";

	CString strCopyCmd = L"C:\\Windows\\System32\\ROBOCOPY.exe";
	CString strparam = L" \"" + strScr + L"\" \"" + strDes + L"\" /E";
	ExceCommand(strCopyCmd, strparam, INVALID_HANDLE_VALUE);
}

void CAutoBuilderDlg::OnBnClickedOk()
{
	CString strLog;
	UpdateData(TRUE);
	SaveSelect();
	WritePrivateProfileString(L"PackName", L"Current", m_strResPackName.GetBuffer(), L".\\AutoBuild.ini");
	SetWorkDir();
	SetOpenLog();

	if (!m_strPreBuildScipt.IsEmpty())
	{
		strLog.Format(L"PrebuildScript: %s", m_strPreBuildScipt);
		WriteLog(strLog);

		CFile f;
		f.Open(m_CurrntWorkDir + L"\\log\\PreBuildScript.log", CFile::modeCreate | CFile::modeWrite);
		ExceCommand(m_strPreBuildScipt, L"", f.m_hFile);
		f.Close();
	}

	WriteLog(L"Build begin");

	strLog.Format(L"%s:%s", L"Parter", m_ParterName);
	WriteLog(strLog);

	strLog.Format(L"%s:%d", L"Bundleversioncod", m_Bundleversioncode);
	WriteLog(strLog);

	strLog.Format(L"%s:%s", L"ProjectPath", m_ProjectPath);
	WriteLog(strLog);

	strLog.Format(L"%s:%s", L"ThridPath", m_thridPath);
	WriteLog(strLog);

	strLog.Format(L"%s:%s", L"ConfigPath", m_strConfigPath);
	WriteLog(strLog);

	strLog.Format(L"%s:%s", L"ProductName", m_strProductName);
	WriteLog(strLog);

	strLog.Format(L"GenerateDynamicPrefab:%s", m_bGenDynamicPrefab ? L"TRUE" : L"FALSE");
	WriteLog(strLog);

	strLog.Format(L"编译UI:%s", m_bUI ? L"TRUE" : L"FALSE");
	WriteLog(strLog);

	strLog.Format(L"编译Atlas:%s", m_batlas ? L"TRUE" : L"FALSE");
	WriteLog(strLog);

	strLog.Format(L"编译Stage:%s", m_bStage ? L"TRUE" : L"FALSE");
	WriteLog(strLog);

	strLog.Format(L"编译小包:%s", m_bIsSmallPack ? L"TRUE" : L"FALSE");
	WriteLog(strLog);

	strLog.Format(L"编译动态下载基础包:%s", m_bDynDownlad ? L"TRUE" : L"FALSE");
	WriteLog(strLog);

	strLog.Format(L"svn执行更新:%s", m_bSvnUp ? L"TRUE" : L"FALSE");
	WriteLog(strLog);

	//CString strCopyCmd = L"C:\\Windows\\System32\\ROBOCOPY.exe";
	//OutputMd5ForFile(strCopyCmd);

	// TODO: Add your control notification handler code here
	//CString uiPath = m_ResPath + L"\\UI";
	//DeletePath(uiPath);
	if (m_bSvnUp)
	{
		SvnUpdate();
	}
	BuildRes();
	MakeConfigDir();

	if (!m_bIsSmallPack)
	{
		if (m_bDynDownlad)
		{
			GeneratePack(FALSE, TRUE);
			GenerateAllPack();
			CopyResExtend();
			GenerateJson();

			CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\temp", NULL);
			CString strCopyCmd = L"C:\\Windows\\System32\\ROBOCOPY.exe";
			CString strparam = L" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all" + L"\" \"" + m_CurrntWorkDir + L"\\xuanqu\\temp\"" + L" /MIR";
			ExceCommand(strCopyCmd, strparam, INVALID_HANDLE_VALUE);
			RemoveNoNullDir(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all");

			MakeAllZip(m_strResPackName);

			CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all", NULL);
			strparam = L" \"" + m_CurrntWorkDir + L"\\xuanqu\\temp" + L"\" \"" + m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all\"" + L" /MIR";
			ExceCommand(strCopyCmd, strparam, INVALID_HANDLE_VALUE);
			RemoveNoNullDir(m_CurrntWorkDir + L"\\xuanqu\\temp");
			MakeAllZipforDynDownlad(L"res_all");
		}
		else
		{
			GeneratePack(FALSE, FALSE);
			MakeAllZip(m_strResPackName);
		}
	}
	else
	{
		GeneratePack(TRUE, FALSE);
		MakeAllZip(m_strResPackName);
	}

	//if (m_bCopySmall)
	//{
	//	DeletePath(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Effect");
	//	DeletePath(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Materials");
	//	DeletePath(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Music");
	//	DeletePath(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Scenes");
	//	DeletePath(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\StaticData");
	//	DeletePath(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\UI");
	//	DeletePath(m_CurrntWorkDir + L"\\xuanqu\\lwts\\config");
	//}

	//build forParter
	if (m_ParterName == L"ALL")
	{
		BuildForALL();
	}
	else
	{
		BuildForParter(m_ParterName);
	}

	WriteLog(L"Build Complete");
	//OnOK();

	Closelog();

	if (!m_strAfterBuildScipt.IsEmpty())
	{
		strLog.Format(L"AfterbuildScript: %s", m_strAfterBuildScipt);
		WriteLog(strLog);

		CFile f;
		f.Open(m_CurrntWorkDir + L"\\log\\AfterBuildScript.log", CFile::modeCreate | CFile::modeWrite);
		ExceCommand(m_strAfterBuildScipt, L"", f.m_hFile);
		f.Close();
	}

	//Beep(1600, 1600);
	PlaySound(L"note.wav", NULL, SND_ASYNC | SND_LOOP);

	MessageBox(L"结束", L"编译结束");

	PlaySound(NULL, NULL, SND_PURGE);
}

void CAutoBuilderDlg::MakeAllZip(CString pack)
{
	USES_CONVERSION;
	CFile bat;
	bat.Open(m_CurrntWorkDir + L"\\MakZip.bat", CFile::modeCreate | CFile::modeWrite);
	CStringA cmd = "\"";
	cmd += W2A(m_Rar);
	cmd += "\" a -r -afzip -ep1 \"";
	cmd += W2A(m_CurrntWorkDir);
	cmd += "\\";
	cmd += pack;
	cmd += ".zip\" \"";
	cmd += W2A(m_CurrntWorkDir);
	cmd += "\\xuanqu\\lwts\\*\"";
	bat.Write(cmd.GetBuffer(), cmd.GetLength());

	bat.Close();

	CFile f;
	f.Open(m_CurrntWorkDir + L"\\log\\MakeAllZip.log", CFile::modeCreate | CFile::modeWrite);
	CString sarg;
	//sarg.Format(L"\"%s\" \"%s\\all.zip\" \"%s\\xuanqu\"",m_Rar,m_CurrntWorkDir,m_CurrntWorkDir);
	ExceCommand(m_CurrntWorkDir + L"\\MakZip.bat", L"", f.m_hFile);
	f.Close();
}

void CAutoBuilderDlg::MakeAllZipforDynDownlad(CString pack)
{
	USES_CONVERSION;
	CFile bat;
	bat.Open(m_CurrntWorkDir + L"\\MakZip.bat", CFile::modeCreate | CFile::modeWrite);
	CStringA cmd = "\"";
	cmd += W2A(m_Rar);
	cmd += "\" a -r -afzip -ep1 \"";
	cmd += W2A(m_CurrntWorkDir);
	cmd += "\\";
	cmd += pack;
	cmd += ".zip\" \"";
	cmd += W2A(m_CurrntWorkDir);
	cmd += "\\xuanqu\\lwts\\res_all\"";
	bat.Write(cmd.GetBuffer(), cmd.GetLength());

	bat.Close();

	CFile f;
	f.Open(m_CurrntWorkDir + L"\\log\\MakeAllZip.log", CFile::modeCreate | CFile::modeWrite);
	CString sarg;
	//sarg.Format(L"\"%s\" \"%s\\all.zip\" \"%s\\xuanqu\"",m_Rar,m_CurrntWorkDir,m_CurrntWorkDir);
	ExceCommand(m_CurrntWorkDir + L"\\MakZip.bat", L"", f.m_hFile);
	f.Close();
}


void CAutoBuilderDlg::CopyAllZipToThrid()
{
	CreateDirectory(m_ProjectPath + L"\\Assets\\Plugins\\Android\\assets", NULL);
	CreateDirectory(m_ProjectPath + L"\\Assets\\Plugins\\Android\\assets\\lwts", NULL);
	CString m_strdsc = m_ProjectPath;
	//m_strdsc += L"\\Assets\\Plugins\\Android\\assets\\lwts\\all.zip";
	m_strdsc += L"\\Assets\\Plugins\\Android\\assets\\lwts\\";
	m_strdsc += m_strResPackName;
	m_strdsc += ".zip";
	CString m_strsrc = m_CurrntWorkDir;
	//m_strsrc += L"\\all.zip";
	m_strsrc += "\\";
	m_strsrc += m_strResPackName;
	m_strsrc += ".zip";
	::CopyFile(m_strsrc, m_strdsc, false);
	CString strLog;
	strLog.Format(L"Copy %s to %s", m_strsrc, m_strdsc);
	WriteLog(strLog);
}

void CAutoBuilderDlg::MakeConfigDir()
{
	CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\lwts\\config", NULL);

	CopyFile(m_strConfigPath + L"\\SystemSetting.ini", m_CurrntWorkDir + L"\\xuanqu\\lwts\\config\\SystemSetting.ini", FALSE);

	CFile f;
	if (f.Open(m_CurrntWorkDir + L"\\xuanqu\\lwts\\config\\version.txt", CFile::modeCreate | CFile::modeWrite))
	{
		CStringA s;
		s.Format("%d", m_ResourceVersion);
		f.Write(s.GetBuffer(), s.GetLength());
		f.Close();
	}
	else
	{
		WriteLog(L"version.txt Create Filed !");
	}
}

void CAutoBuilderDlg::SVNClear(CString szDir)
{
	CString cmd;
	cmd.Format(L" cleanup   %s", szDir.GetString());
	ExceCommand(m_SVNExePath, cmd, NULL);
}
void CAutoBuilderDlg::GetNew(CString szDir, CString LogName, int nReversion)
{

	CFile f;
	CString strLogName = m_CurrntWorkDir + L"\\log\\" + LogName + L"_Update.log";
	f.Open(strLogName, CFile::modeCreate | CFile::modeWrite);

	CString cmd;
	if (nReversion > 0)
	{
		cmd.Format(L" info  %s -r%d", szDir.GetString(), nReversion);
	}
	else
	{
		cmd.Format(L" info  %s", szDir.GetString());
	}
	ExceCommand(m_SVNExePath, cmd, f.m_hFile);

	if (nReversion > 0)
	{
		cmd.Format(L" update  %s  --depth=infinity -r%d", szDir.GetString(), nReversion);
	}
	else
	{
		cmd.Format(L" update  %s  --depth=infinity", szDir.GetString());
	}

	ExceCommand(m_SVNExePath, cmd, f.m_hFile);

	f.Close();
}
void CAutoBuilderDlg::Revert(CString szDir)
{
	CString cmd = L" revert ";
	cmd += szDir;
	cmd += " --depth=infinity -R";

	CFile f;
	f.Open(m_CurrntWorkDir + L"\\log\\Revert.log", CFile::modeCreate | CFile::modeWrite);
	ExceCommand(m_SVNExePath, cmd, f.m_hFile);
	f.Close();
}
void CAutoBuilderDlg::SetWorkDir()
{
	WCHAR cValue[MAX_PATH];
	GetCurrentDirectory(MAX_PATH, cValue);
	m_CurrntWorkDir = cValue;
	m_CurrntWorkDir += L"\\";
	::GetDateFormat(LOCALE_SYSTEM_DEFAULT, 0, NULL, L"yyyy-MM-dd", cValue, MAX_PATH);
	m_CurrntWorkDir += cValue;
	::GetTimeFormat(LOCALE_SYSTEM_DEFAULT, 0, NULL, L"_HH_mm_ss", cValue, MAX_PATH);
	m_CurrntWorkDir += cValue;
	if (GetFileAttributes(m_CurrntWorkDir) & FILE_ATTRIBUTE_DIRECTORY)
	{
		RemoveDirectory(m_CurrntWorkDir);
	}
	CreateDirectory(m_CurrntWorkDir, NULL);
	CreateDirectory(m_CurrntWorkDir + L"\\xuanqu", NULL);
	CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\lwts", NULL);
	CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res", NULL);
	if (m_bDynDownlad)
	{
		CreateDirectory(m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all", NULL);
	}
}

void CAutoBuilderDlg::SetOpenLog()
{
	CString m_LogDir = m_CurrntWorkDir + L"\\log";

	CreateDirectory(m_LogDir, NULL);

	m_LogFile = new CFile;
	if (!m_LogFile->Open(m_LogDir + L"\\log.txt", CFile::modeCreate | CFile::modeReadWrite))
	{

		delete m_LogFile;
		m_LogFile = NULL;
	}
}

void CAutoBuilderDlg::WriteLog(CString strContent)
{
	if (m_LogFile != NULL)
	{
		USES_CONVERSION;
		CStringA log = "[";
		char cValue[MAX_PATH];
		::GetTimeFormatA(LOCALE_SYSTEM_DEFAULT, 0, NULL, "HH':'mm':'ss", cValue, MAX_PATH);
		log += cValue;
		log += "] ";
		log += W2A(strContent);
		log += "\r\n";
		m_LogFile->Write(log.GetBuffer(), log.GetLength());
	}
}
void CAutoBuilderDlg::BuildForALL()
{
	int nCount = GetPrivateProfileInt(L"ALL", L"Count", 0, L".\\AutoBuild.ini");
	WCHAR cValue[MAX_PATH];
	for (int i = 0; i < nCount; i++)
	{
		CString s;
		s.Format(L"Parter%d", i + 1);
		GetPrivateProfileString(L"ALL", s.GetBuffer(), L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
		CString strParterName = cValue;
		BuildForParter(strParterName);
	}
}

void CAutoBuilderDlg::CopyIcon(CString strthridr, CString strIcon)
{
	CString iconFile = strIcon;
	iconFile.Replace(L"72", L"144");
	iconFile = m_ProjectPath + L"\\Assets\\Art_new\\UI\\icon\\" + iconFile + L".png";
	CString strDestIconPath = m_thridPath + L"\\" + strthridr + L"\\res\\drawable-xxhdpi\\app_icon.png";
	CopyFile(iconFile, strDestIconPath, FALSE);
}

void CAutoBuilderDlg::BuildForParter(CString strParterName)
{
	CString log;
	log.Format(L"Begin Build APK for %s", strParterName);
	WriteLog(log);
	WCHAR cValue[MAX_PATH];
	GetPrivateProfileString(strParterName.GetBuffer(), L"PARTER_ID", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	CString strPARTER_ID = cValue;

	GetPrivateProfileString(strParterName.GetBuffer(), L"Bundle_identifier", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	CString strBundle_identifier = cValue;

	GetPrivateProfileString(strParterName.GetBuffer(), L"thrid", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	CString strthridr = cValue;

	GetPrivateProfileString(strParterName.GetBuffer(), L"Icon", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	CString strIcon = cValue;

	GetPrivateProfileString(strParterName.GetBuffer(), L"SplashScreen", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	CString strSplashScreen = cValue;

	GetPrivateProfileString(strParterName.GetBuffer(), L"KeyaliasName", L"lianwu", cValue, MAX_PATH, L".\\AutoBuild.ini");
	CString strKeyaliasName = cValue;

	GetPrivateProfileString(strParterName.GetBuffer(), L"KeyaliasPass", L"lianwu20130624", cValue, MAX_PATH, L".\\AutoBuild.ini");
	CString strKeyaliasPass = cValue;

	GetPrivateProfileString(strParterName.GetBuffer(), L"KeystorePass", L"DDLE_LW_20130624", cValue, MAX_PATH, L".\\AutoBuild.ini");
	CString strKeystorePass = cValue;

	GetPrivateProfileString(strParterName.GetBuffer(), L"AndroidKeyPath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	CString strAndroidKeyPath = cValue;

	GetPrivateProfileString(strParterName.GetBuffer(), L"Title", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	CString strTitle = cValue;

	if (strTitle.IsEmpty())
	{
		strTitle = m_strProductName;
	}
	if (!strTitle.IsEmpty())
	{
		string strRes = Chinese2Unicode((wstring)strTitle.GetBuffer());
		CString strProductName;
		strProductName = strRes.c_str();
		DoChangeProductName(m_SettingPath, strProductName);
	}

	int nAndroidAPILevel = GetPrivateProfileInt(strParterName.GetBuffer(), L"AndroidAPILevel", 0, L".\\AutoBuild.ini");

	//check whether use default
	if (strSplashScreen == "")
	{
		strSplashScreen = m_DefaultSplashScreen;
	}
	if (strAndroidKeyPath == "")
	{
		strAndroidKeyPath = m_DefaultAndroidKeyPath;
	}
	if (nAndroidAPILevel <= 0)
	{
		nAndroidAPILevel = m_DefaultAndroidAPILevel;
	}

	CopyIcon(strthridr, strIcon);

	//create ParterFolder
	CString _ParterDir = m_CurrntWorkDir + L"\\" + strParterName;
	CreateDirectory(_ParterDir, NULL);

	CString Oldpath = m_ProjectPath + L"\\AutoBuilder.apk";
	DeleteFile(Oldpath);
	RebuildScriptDefine(strPARTER_ID);
	RebuildProjectSetting(strPARTER_ID, strBundle_identifier, m_Bundleversioncode, m_strbundleversion, _ParterDir, strSplashScreen, nAndroidAPILevel);

	log.Format(L"Build Small APK for %s", strParterName);
	WriteLog(log);
	ReplaceThrid(strthridr, FALSE);
	//if( ReplaceThrid(strthridr,FALSE) )
	//{
	BuildAPK(strIcon, strAndroidKeyPath, strKeyaliasName, strKeyaliasPass, strKeystorePass);
	MoveApkToParterFolder(m_CurrntWorkDir, strParterName, FALSE);
	//}
	//else
	//{
	//	log.Format( L"thrid %s Is not found",strthridr );
	//	WriteLog(log);
	//}

	log.Format(L"Build Small APK for %s end ", strParterName);
	WriteLog(log);

	DeleteFile(Oldpath);

	//if( ReplaceThrid(strthridr,TRUE) )
	//{
	CopyAllZipToThrid();
	BuildAPK(strIcon, strAndroidKeyPath, strKeyaliasName, strKeyaliasPass, strKeystorePass);
	MoveApkToParterFolder(m_CurrntWorkDir, strParterName, TRUE);

	//}
	//else
	//{
	//	log.Format( L"thrid %s Is not found",strthridr );
	//	WriteLog(log);
	//}

	log.Format(L"Build Big APK for %s end ", strParterName);
	WriteLog(log);

	ClearScriptDefine();
	if (m_bIsSmallPack || m_bDynDownlad)
	{
		if (m_bDynDownlad)
		{
			RebuildScriptDefine(L"PACKAGE_DYNAMICDOWNLOAD");
		}
		else
		{
			RebuildScriptDefine(L"PACKAGE_BASIC");
		}
	}

	WriteLog(L"Build APK for strParterName end");

}
BOOL CAutoBuilderDlg::RemoveNoNullDir(CString path)
{

	SHFILEOPSTRUCT o;
	ZeroMemory((void*)&o, sizeof(SHFILEOPSTRUCT));
	CHAR pathname[MAX_PATH];
	ZeroMemory((void*)pathname, MAX_PATH);

	memcpy_s(pathname, MAX_PATH, path.GetBuffer(), path.GetLength() * 2);
	o.hwnd = this->GetSafeHwnd();
	o.wFunc = FO_DELETE;
	o.pFrom = (LPCWSTR)pathname;//L"F:\\workfornew\\hotdance\\Assets\\Plugins\\Android";//;
	o.lpszProgressTitle = NULL;//L"Delete Old Thrid";
	o.fFlags = FOF_NOCONFIRMATION;

	return SHFileOperation(&o) == 0;
}
BOOL CAutoBuilderDlg::CopyDir(CString pathScr, CString pathDes)
{
	CString log;
	log.Format(L"Copy %s to %s ", pathScr, pathDes);
	WriteLog(log);
	CHAR pathname[MAX_PATH];
	ZeroMemory((void*)pathname, MAX_PATH);
	memcpy_s(pathname, MAX_PATH, pathScr.GetBuffer(), pathScr.GetLength() * 2);
	SHFILEOPSTRUCT o;
	o.hwnd = this->GetSafeHwnd();
	o.wFunc = FO_COPY;
	o.pFrom = (LPCWSTR)pathname;
	o.pTo = pathDes;
	o.lpszProgressTitle = L"Copy Thrid";

	if (SHFileOperation(&o) == 0)
	{
		log.Format(L"Copy succeed");
		WriteLog(log);
		return TRUE;
	}
	else
	{
		log.Format(L"Copy Failed");
		WriteLog(log);
		return FALSE;
	}

}

BOOL CAutoBuilderDlg::ReplaceThrid(CString strThridName, BOOL bAll)
{
	CString m_strNewThrid = m_thridPath + L"\\" + strThridName;// + L"_All";
	if (bAll)
	{
		m_strNewThrid += L"_All";
	}
	if (_waccess(m_strNewThrid, 0) == 0)
	{

		CString log;
		CString m_strOldThrid = m_ProjectPath;
		m_strOldThrid += L"\\Assets\\Plugins\\Android";
		if (GetFileAttributes(m_strOldThrid) & FILE_ATTRIBUTE_DIRECTORY)
		{
			log.Format(L"%s Is exist", m_strOldThrid);
			if (RemoveNoNullDir(m_strOldThrid))
			{
				log.Format(L"%s delete succeed", m_strOldThrid);
				WriteLog(log);
			}
			else
			{
				log.Format(L"%s delete failed", m_strOldThrid);
				WriteLog(log);
			}
		}

		CopyDir(m_strNewThrid, m_strOldThrid);
		return TRUE;
	}
	return FALSE;
}
bool CAutoBuilderDlg::_GetLine(char * pContent, int& nlen, int nTotalLen, char* pResult)
{
	if (nlen >= nTotalLen)
		return false;
	int nCurrent = 0;
	while (nlen < nTotalLen)
	{
		pResult[nCurrent] = pContent[nlen];
		nlen++;
		if (pResult[nCurrent] == '\n')
		{
			break;
		}
		nCurrent++;
	}
	pResult[nCurrent + 1] = '\0';
	return true;
}

void CAutoBuilderDlg::DoChangeProductName(CString strPath, CString strProductName)
{
	CString strProjectFilePath = strPath + L"\\ProjectSettings.asset";
	CString strTemp = strPath + L"\\ProjectSettings.asset.tmp";
	CStdioFile fProject;
	CFile fTemp;
	if (fProject.Open(strProjectFilePath, CFile::modeReadWrite) && fTemp.Open(strTemp, CFile::modeCreate | CFile::modeReadWrite))
	{
		CString str;
		while (fProject.ReadString(str))
		{
			if (str.Find(L"productName:") != -1)
			{
				str = L"  productName: \"" + strProductName + L"\"";
			}

			str += "\n";
			string strLine = wstring2string(str.GetBuffer());
			fTemp.Write(strLine.c_str(), (int)strLine.length());
		}

		fProject.Close();
		fTemp.Close();

		DeleteFile(strProjectFilePath);
		CopyFile(strTemp, strProjectFilePath, TRUE);
		DeleteFile(strTemp);
	}
}

void CAutoBuilderDlg::GenerateJson()
{
	CString res = m_CurrntWorkDir + L"\\xuanqu\\lwts\\res";
	CString res_all = m_CurrntWorkDir + L"\\xuanqu\\lwts\\res_all";

	CString client = res + L"\\ClientVer";
	CString server = res + L"\\ServerVer";
	CString server_all = res_all + L"\\ServerVer";
	CreateDirectory(client, NULL);
	CreateDirectory(server, NULL);
	CreateDirectory(server_all, NULL);

	{
		WIN32_FIND_DATA findData;
		HANDLE hError;
		CString filePathName = res + L"\\*.*";
		hError = FindFirstFile(filePathName, &findData);
		if (hError == INVALID_HANDLE_VALUE)
			return;

		int count = 0;
		while (FindNextFile(hError, &findData))
		{
			CString fileName = findData.cFileName;
			if (fileName.Compare(L".") == 0 || fileName.Compare(L"..") == 0
				|| fileName.Compare(L"ClientVer") == 0 || fileName.Compare(L"ServerVer") == 0)
				continue;

			if (findData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
			{
				CString directoryName = res + L"\\" + fileName;
				CString jsonFileName = client + L"\\ResVer_" + fileName + L".json";
				ofstream jsonFile(jsonFileName, ofstream::out);
				GenerateJson(jsonFile, directoryName, TRUE);
				jsonFile.close();
			}
			else
			{
				count++;
			}
		}
		FindClose(hError);

		if (count > 0)
		{
			CString jsonFileName = client + L"\\ResVer_Other.json";
			ofstream jsonFile(jsonFileName, ofstream::out);
			GenerateJson(jsonFile, res, FALSE);
			jsonFile.close();
		}
	}

	{
		WIN32_FIND_DATA findData;
		HANDLE hError;
		CString filePathName = res_all + L"\\*.*";
		hError = FindFirstFile(filePathName, &findData);
		if (hError == INVALID_HANDLE_VALUE)
			return;

		int count = 0;
		while (FindNextFile(hError, &findData))
		{
			CString fileName = findData.cFileName;
			if (fileName.Compare(L".") == 0 || fileName.Compare(L"..") == 0
				|| fileName.Compare(L"ClientVer") == 0 || fileName.Compare(L"ServerVer") == 0)
				continue;

			if (findData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
			{
				CString directoryName = res_all + L"\\" + fileName;
				CString jsonFileName = server + L"\\ResVer_" + fileName + L".json";
				ofstream jsonFile(jsonFileName, ofstream::out);
				GenerateJson(jsonFile, directoryName, TRUE);
				jsonFile.close();
			}
			else
			{
				count++;
			}
		}
		FindClose(hError);

		if (count > 0)
		{
			CString jsonFileName = server + L"\\ResVer_Other.json";
			ofstream jsonFile(jsonFileName, ofstream::out);
			GenerateJson(jsonFile, res_all, FALSE);
			jsonFile.close();
		}

		CString configVer = client + L"\\ConfigVer.json";
		ofstream jsonFile(configVer, ofstream::out);
		GenerateJson(jsonFile, server, FALSE);
		jsonFile.close();
	}

	{
		WIN32_FIND_DATA findData;
		HANDLE hError;
		CString filePathName = res_all + L"\\*.*";
		hError = FindFirstFile(filePathName, &findData);
		if (hError == INVALID_HANDLE_VALUE)
			return;

		int count = 0;
		while (FindNextFile(hError, &findData))
		{
			CString fileName = findData.cFileName;
			if (fileName.Compare(L".") == 0 || fileName.Compare(L"..") == 0
				|| fileName.Compare(L"ServerVer") == 0)
				continue;

			if (findData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
			{
				CString directoryName = res_all + L"\\" + fileName;
				CString jsonFileName = server_all + L"\\ResVer_" + fileName + L".json";
				ofstream jsonFile(jsonFileName, ofstream::out);
				GenerateJson(jsonFile, directoryName, TRUE);
				jsonFile.close();
			}
			else
			{
				count++;
			}
		}
		FindClose(hError);

		if (count > 0)
		{
			CString jsonFileName = server_all + L"\\ResVer_Other.json";
			ofstream jsonFile(jsonFileName, ofstream::out);
			GenerateJson(jsonFile, res_all, FALSE);
			jsonFile.close();
		}

		CopyFile(client + L"\\ConfigVer.json", server_all + L"\\ConfigVer.json", FALSE);
	}

}

void CAutoBuilderDlg::GenerateJson(ofstream &json, CString path, BOOL includeSubdir)
{
	USES_CONVERSION;
	WIN32_FIND_DATA findData;
	CString filePathName = path + L"\\*.*";
	HANDLE hFind = FindFirstFile(filePathName, &findData);
	if (hFind == INVALID_HANDLE_VALUE)
		return;

	int count = 0;
	while (FindNextFile(hFind, &findData))
	{
		CString fileName = findData.cFileName;
		if (fileName.Compare(L".") == 0 || fileName.Compare(L"..") == 0)
			continue;

		if (findData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
		{
			if (includeSubdir)
			{
				CString subDir = path + L"\\" + fileName;
				GenerateJson(json, subDir, TRUE);
			}
		}
		else
		{
			CString tempFileName = fileName;
			if (path.Find(L"\\Animations\\Controller\\") != -1 && fileName.Find(L".txt") != -1)
			{
				int index = path.Find(L"\\Controller\\");
				int count = path.GetLength() - index - 12;
				CString temp = path.Right(count);
				tempFileName = temp + L"_" + tempFileName;
			}

			UINT64 fileSize = ((UINT64)findData.nFileSizeHigh << 32) + findData.nFileSizeLow;
			CString strFileName = path + L"\\" + fileName;
			unsigned char md5[17] = { 0 };
			MDFile(W2A(strFileName), md5);
			char buf[33] = { '\0' };
			char tmp[3] = { '\0' };
			for (int i = 0; i < 16; i++){
				sprintf_s(tmp, "%02x", md5[i]);
				strcat_s(buf, tmp);
			}
			json << W2A(tempFileName) << ":" << buf << "|" << fileSize << ",";
		}
	}
	FindClose(hFind);
}

void CAutoBuilderDlg::DoChangeScriptDefine(CString strPath)
{
	//CString strProjectFilePath = strPath + L"\\ProjectSettings.asset";
	//CString strTemp = strPath + L"\\ProjectSettings.asset.tmp";
	//CStdioFile fProject;
	//CFile fTemp;
	//if (fProject.Open(strProjectFilePath, CFile::modeReadWrite) && fTemp.Open(strTemp, CFile::modeCreate | CFile::modeReadWrite))
	//{
	//	CString str;
	//	while (fProject.ReadString(str))
	//	{
	//		if (str.Find(L"second:") != -1)
	//		{
	//			if (m_bDynDownlad)
	//			{
	//				if (str.Find(L"PACKAGE_DYNAMICDOWNLOAD") == -1)
	//				{
	//					str += L";PACKAGE_DYNAMICDOWNLOAD";
	//				}
	//			}
	//			else
	//			{
	//				if (str.Find(L"PACKAGE_BASIC") == -1)
	//				{
	//					str += L";PACKAGE_BASIC";
	//				}
	//			}
	//		}

	//		str += "\n";
	//		string strLine = wstring2string(str.GetBuffer());
	//		fTemp.Write(strLine.c_str(), (int)strLine.length());
	//	}

	//	fProject.Close();
	//	fTemp.Close();

	//	DeleteFile(strProjectFilePath);
	//	CopyFile(strTemp, strProjectFilePath, TRUE);
	//	DeleteFile(strTemp);
	//}
}

void CAutoBuilderDlg::RebuildProjectSetting(CString strPARTER_ID, CString strBundle_identifie, int nVersioncode, CString SubVersin, CString Parterpath, CString SplashScreen, int nAPILevel)
{
	USES_CONVERSION;
	CString strProjectFilePath = m_ProjectPath + L"\\ProjectSettings\\ProjectSettings.asset";
	CFile fProject;
	CList<CStringA> _List;
	if (fProject.Open(strProjectFilePath, CFile::modeReadWrite))
	{
		int len = (int)fProject.GetLength();

		char * content = new char[len];
		fProject.Read(content, len);

		char * pResult = new char[255];
		ZeroMemory(pResult, 255);
		int nlen = 0;
		BOOL bHeand = TRUE;
		while (_GetLine(content, nlen, len, pResult))
		{
			CStringA s = pResult;
			_List.AddTail(s);
		}
		fProject.Close();
	}
	//BundleIdentifier
	if (strBundle_identifie != "")
	{
		POSITION pos = _List.GetHeadPosition();
		while (pos != NULL)
		{
			CStringA& sa = _List.GetNext(pos);
			if (sa.Find("iPhoneBundleIdentifier") != -1)
			{
				sa = "  iPhoneBundleIdentifier: ";
				sa += W2A(strBundle_identifie);
				sa += '\n';
				break;
			}
		}
	}
	//BundleVersionCode
	if (nVersioncode > 0)
	{
		POSITION pos = _List.GetHeadPosition();
		while (pos != NULL)
		{
			CStringA& sa = _List.GetNext(pos);
			if (sa.Find("AndroidBundleVersionCode") != -1)
			{
				sa.Format("  AndroidBundleVersionCode: %d\n", nVersioncode);
				break;
			}
		}
	}
	//AndroidMinSdkVersion
	if (nAPILevel > 0)
	{
		POSITION pos = _List.GetHeadPosition();
		while (pos != NULL)
		{
			CStringA& sa = _List.GetNext(pos);
			if (sa.Find("AndroidMinSdkVersion") != -1)
			{
				sa.Format("  AndroidMinSdkVersion: %d\n", nAPILevel);
				break;
			}
		}
	}

	if (SplashScreen != "")
	{
		POSITION pos = _List.GetHeadPosition();
		while (pos != NULL)
		{
			CStringA& sa = _List.GetNext(pos);
			if (sa.Find("iPhoneSplashScreen") != -1)
			{
				sa = "  iPhoneSplashScreen: ";
				sa += W2A(SplashScreen);
				sa += "\n";
				//sa.Format("  iPhoneSplashScreen: %s\n",SplashScreen.GetBuffer());
				break;
			}
		}
	}

	//scriptingDefineSymbols
	//if (strPARTER_ID != "")
	//{
	//	POSITION pos = _List.GetHeadPosition();
	//	while (pos != NULL)
	//	{
	//		CStringA& sa = _List.GetNext(pos);
	//		if (sa.Find("scriptingDefineSymbols") != -1)
	//		{
	//			sa = "  scriptingDefineSymbols:\n";
	//			sa += "    data:\n";
	//			sa += "      first: 7\n";
	//			sa += "      second: ";
	//			sa += W2A(strPARTER_ID);

	//			if (m_bDynDownlad)
	//			{
	//				sa += ";PACKAGE_DYNAMICDOWNLOAD";
	//			}
	//			else if (m_bIsSmallPack)
	//			{
	//				sa += ";PACKAGE_BASIC";
	//			}

	//			sa += +"\n";

	//			while (pos != NULL)
	//			{
	//				CStringA& sa2 = _List.GetNext(pos);
	//				if (sa2.GetAt(2) != L' ')
	//				{
	//					break;
	//				}
	//				else
	//				{
	//					sa2 = "";
	//				}
	//			}
	//			break;
	//		}
	//	}
	//}

	//BundleVersion
	if (SubVersin != "")
	{
		POSITION pos = _List.GetHeadPosition();
		while (pos != NULL)
		{
			CStringA& sa = _List.GetNext(pos);
			if (sa.Find("iPhoneBundleVersion") != -1)
			{
				sa = "  iPhoneBundleVersion: ";
				sa += SubVersin;
				sa += '\n';
				break;
			}
		}
	}


	if (_List.GetCount() != 0)
	{
		if (fProject.Open(strProjectFilePath, CFile::modeCreate | CFile::modeWrite))
		{
			POSITION pos = _List.GetHeadPosition();
			while (pos != NULL)
			{
				CStringA& sa = _List.GetNext(pos);
				if (sa != "")
				{
					fProject.Write(sa.GetBuffer(), sa.GetLength());
				}
			}
			fProject.Flush();
			fProject.Close();

			CString newPath = Parterpath + L"\\ProjectSettings.asset";
			CopyFile(strProjectFilePath, newPath, TRUE);
		}

	}

}
void CAutoBuilderDlg::MoveApkToParterFolder(CString path, CString strParterName, BOOL IsBig)
{

	WCHAR cValue[MAX_PATH];
	GetPrivateProfileString(strParterName.GetBuffer(), L"APKNAME", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	CString strAPKName = cValue;

	CString Oldpath = m_ProjectPath + L"\\AutoBuilder.apk";
	CString newPath = path;
	newPath += L"\\";
	newPath += strAPKName;
	//newPath+= ".apk";
	if (IsBig)
	{
		newPath += L"_All.apk";
	}
	else
	{
		newPath += L".apk";
	}


	if (_waccess(Oldpath, 0) == 0)
	{
		CString log;
		log.Format(L"Move APK to %s", path);
		WriteLog(log);
		CopyFile(Oldpath, newPath, TRUE);
		OutputMd5ForFile(newPath);

		if (IsBig)
		{
			Oldpath = m_ProjectPath + L"\\ProjectSettings\\ProjectSettings.asset";
			newPath = path + L"\\ProjectSettings.asset";
			CopyFile(Oldpath, newPath, TRUE);
		}
	}
	else
	{
		WriteLog(L"Move APK Failed ,file not found");
	}

}
void CAutoBuilderDlg::OutputMd5ForFile(CString strFileName)
{
	USES_CONVERSION;
	CString Md5Path = strFileName + L".md5";
	unsigned char md5[17];
	MDFile(W2A(strFileName), md5);

	char buf[33] = { '\0' };
	char tmp[3] = { '\0' };
	for (int i = 0; i < 16; i++){
		sprintf_s(tmp, "%02X", md5[i]);
		strcat_s(buf, tmp);
	}

	CFile f;
	f.Open(Md5Path, CFile::modeCreate | CFile::modeWrite);
	f.Write(buf, 32);
	f.Close();
}
void CAutoBuilderDlg::BuildAPK(CString strIcon, CString strKeyFile, CString strKeyaliasName, CString strKeyaliasPass, CString strKeystorePass)
{
	WriteLog(L"Begin Build APK");

	CString strCommand = _T(" -quit -batchmode -executeMethod GenerateAPK.GenerateAPKFunction -projectPath \"");//
	strCommand += m_ProjectPath;
	strCommand += L"\" -logFile ";
	strCommand += L"\"";
	strCommand += m_CurrntWorkDir;
	strCommand += L"\\log\\BuildAPK.log\"";
	strCommand += L" -PARTERICON \"";
	strCommand += strIcon;
	strCommand += L"\" -KEYFILE \"";
	strCommand += strKeyFile;
	strCommand += L"\" -KEYALIASNAME \"";
	strCommand += strKeyaliasName;
	strCommand += L"\" -KEYALIASPASS \"";
	strCommand += strKeyaliasPass;
	strCommand += L"\" -KEYSTOREPASS \"";
	strCommand += strKeystorePass;
	strCommand += L"\"";
	ExceUnityCommand(strCommand);

	WriteLog(L"Build APK End ");
}

void CAutoBuilderDlg::RebuildScriptDefine(CString strDefine)
{
	WriteLog(L"Begin RebuildScriptDefine");

	CString strCommand = _T(" -quit -batchmode -executeMethod ProjectBuild.RebuildScriptDefine -projectPath \"");//
	strCommand += m_ProjectPath;
	strCommand += L"\" -logFile ";
	strCommand += L"\"";
	strCommand += m_CurrntWorkDir;
	strCommand += L"\\log\\RebuildScriptDefine.log\"";
	strCommand += L" -SCRIPTDEFINE \"";
	strCommand += strDefine;
	strCommand += L"\"";
	ExceUnityCommand(strCommand);

	WriteLog(L"Build RebuildScriptDefine");
}

void CAutoBuilderDlg::ClearScriptDefine()
{
	WriteLog(L"Begin ClearScriptDefine");

	CString strCommand = _T(" -quit -batchmode -executeMethod ProjectBuild.ClearScriptDefine -projectPath \"");//
	strCommand += m_ProjectPath;
	strCommand += L"\" -logFile ";
	strCommand += L"\"";
	strCommand += m_CurrntWorkDir;
	strCommand += L"\\log\\ClearScriptDefine.log\"";
	ExceUnityCommand(strCommand);

	WriteLog(L"Build ClearScriptDefine");
}

void CAutoBuilderDlg::GenerateDynamicPrefab()
{
	WriteLog(L"Begin Generate Prefab");

	CString strCommand = _T("-quit -batchmode -executeMethod BuildDynamicPrefab.GenerateDynamicPrefab -projectPath \"") + m_ProjectPath + L"\"";
	strCommand += " -logFile ";
	strCommand += "\"";
	strCommand += m_CurrntWorkDir;
	strCommand += "\\log\\GenerateDynamicPrefab.log\"";
	ExceUnityCommand(strCommand);

	WriteLog(L"Generate Prefab End ");

	CString strScr = m_ProjectPath;
	strScr += L"\\res\\Prefab";
	CString strDes = m_CurrntWorkDir;
	strDes += L"\\xuanqu\\lwts\\res\\Prefab";
	RemoveNoNullDir(strDes);

	CString strCopyCmd = L"C:\\Windows\\System32\\ROBOCOPY.exe";
	CString strparam = L" \"" + strScr + L"\" \"" + strDes + L"\" /E";
	ExceCommand(strCopyCmd, strparam, INVALID_HANDLE_VALUE);
}

void CAutoBuilderDlg::BuildUI()
{
	WriteLog(L"Begin Build UI");

	if (GetFileAttributes(m_ProjectPath + L"\\res\\UI") & FILE_ATTRIBUTE_DIRECTORY)
	{
		RemoveNoNullDir(m_ProjectPath + L"\\res\\UI");
	}

	if (GetFileAttributes(m_ProjectPath + L"\\Res_Extend") & FILE_ATTRIBUTE_DIRECTORY)
	{
		RemoveNoNullDir(m_ProjectPath + L"\\Res_Extend");
	}

	CString strCommand = _T("-quit -batchmode -executeMethod BuildUI.GenerateAllUIScene -projectPath \"") + m_ProjectPath + L"\"";
	strCommand += " -logFile ";
	strCommand += "\"";
	strCommand += m_CurrntWorkDir;
	strCommand += "\\log\\BuildUI.log\"";
	ExceUnityCommand(strCommand);

	WriteLog(L"Build UI End ");
}

void CAutoBuilderDlg::BuildAtlas()
{
	WriteLog(L" Begin Build Atlas ");

	if (GetFileAttributes(m_ProjectPath + L"\\res\\UI\\Atlas") & FILE_ATTRIBUTE_DIRECTORY)
	{
		RemoveNoNullDir(m_ProjectPath + L"\\res\\UI\\Atlas");
	}

	CString strCommand = _T("-quit -batchmode -executeMethod GenerateAtlas.Generate_Atlas -projectPath \"") + m_ProjectPath + L"\"";
	strCommand += L" -logFile ";
	strCommand += L"\"";
	strCommand += m_CurrntWorkDir;
	strCommand += L"\\log\\BuildAtlas.log\"";
	ExceUnityCommand(strCommand);

	WriteLog(L" Build Atlas End");
}

void CAutoBuilderDlg::BuildStage()
{
	WriteLog(L" Begin Build Stage ");

	if (GetFileAttributes(m_ProjectPath + L"\\res\\Scenes") & FILE_ATTRIBUTE_DIRECTORY)
	{
		RemoveNoNullDir(m_ProjectPath + L"\\res\\Scenes");
	}

	CString strCommand = _T("-quit -batchmode -executeMethod GenerateStage.Generate_Stage -projectPath \"") + m_ProjectPath + L"\"";
	strCommand += L" -logFile ";
	strCommand += L"\"";
	strCommand += m_CurrntWorkDir;
	strCommand += L"\\log\\BuildStage.log\"";
	ExceUnityCommand(strCommand);

	WriteLog(L" Build Stage End");
}

void  CAutoBuilderDlg::LoadData()
{
	WCHAR cValue[MAX_PATH];
	GetPrivateProfileString(L"Data", L"ParterName", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_ParterName = cValue;
	m_Bundleversioncode = GetPrivateProfileInt(L"Data", L"Bundleversioncod", 0, L".\\AutoBuild.ini");

	GetPrivateProfileString(L"Data", L"UnityPath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_strUnityPath = cValue;

	GetPrivateProfileString(L"Data", L"ProjectPath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_ProjectPath = cValue;

	GetPrivateProfileString(L"Data", L"SettingPath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_SettingPath = cValue;

	GetPrivateProfileString(L"Data", L"Thrid", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_thridPath = cValue;

	GetPrivateProfileString(L"Data", L"ConfigPath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_strConfigPath = cValue;

	GetPrivateProfileString(L"Data", L"BundleVersion", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_strbundleversion = cValue;

	GetPrivateProfileString(L"Data", L"SVNExePath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_SVNExePath = cValue;

	GetPrivateProfileString(L"Data", L"ResPath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_ResPath = cValue;

	GetPrivateProfileString(L"Data", L"WinRarPath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_Rar = cValue;

	GetPrivateProfileString(L"Data", L"ProductName", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_strProductName = cValue;

	m_ResourceVersion = GetPrivateProfileInt(L"Data", L"ResourceVersion", 0, L".\\AutoBuild.ini");

	GetPrivateProfileString(L"Data", L"SplashScreen", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_DefaultSplashScreen = cValue;

	GetPrivateProfileString(L"Data", L"AndroidKeyPath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_DefaultAndroidKeyPath = cValue;

	m_DefaultAndroidAPILevel = GetPrivateProfileInt(L"Data", L"AndroidAPILevel", 0, L".\\AutoBuild.ini");

	int nTemp = GetPrivateProfileInt(L"Config", L"BuildUI", 0, L".\\AutoBuild.ini");
	m_bUI = nTemp > 0;
	nTemp = GetPrivateProfileInt(L"Config", L"BuildAtlas", 0, L".\\AutoBuild.ini");
	m_batlas = nTemp > 0;
	nTemp = GetPrivateProfileInt(L"Config", L"BuildStage", 0, L".\\AutoBuild.ini");
	m_bStage = nTemp > 0;
	nTemp = GetPrivateProfileInt(L"Config", L"BuildSmallPack", 0, L".\\AutoBuild.ini");
	m_bIsSmallPack = nTemp > 0;
	nTemp = GetPrivateProfileInt(L"Config", L"SvnUp", 0, L".\\AutoBuild.ini");
	m_bSvnUp = nTemp > 0;
	nTemp = GetPrivateProfileInt(L"Config", L"GenerateDynamicPrefab", 0, L".\\AutoBuild.ini");
	m_bGenDynamicPrefab = nTemp > 0;
	nTemp = GetPrivateProfileInt(L"Config", L"DynDownlad", 0, L".\\AutoBuild.ini");
	m_bDynDownlad = nTemp > 0;

	GetPrivateProfileString(L"Data", L"MaterialsPath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_strMaterialsPath = cValue;
	GetPrivateProfileString(L"Data", L"MaterialsDiffPath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_strMaterialsDiffPath = cValue;
	GetPrivateProfileString(L"Data", L"ItemlistPath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_strItemlistPath = cValue;
	GetPrivateProfileString(L"Data", L"MusicPath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_strMusicPath = cValue;
	GetPrivateProfileString(L"Data", L"MusiclistPath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_strMusiclistPath = cValue;
	GetPrivateProfileString(L"Data", L"EffectPath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_strEffectPath = cValue;
	GetPrivateProfileString(L"Data", L"EffectlistPath", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_strEffectlistPath = cValue;

	GetPrivateProfileString(L"Data", L"PreBuildScript", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_strPreBuildScipt = cValue;
	GetPrivateProfileString(L"Data", L"AfterBuildScript", L"", cValue, MAX_PATH, L".\\AutoBuild.ini");
	m_strAfterBuildScipt = cValue;
}
void CAutoBuilderDlg::SaveSelect()
{
	WritePrivateProfileStringW(NULL, NULL, NULL, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"ParterName", m_ParterName, L".\\AutoBuild.ini");
	CString strv;
	strv.Format(L"%d", m_Bundleversioncode);
	WritePrivateProfileString(L"Data", L"Bundleversioncod", strv.GetBuffer(), L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"UnityPath", m_strUnityPath, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"ProjectPath", m_ProjectPath, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"SettingPath", m_SettingPath, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"BundleVersion", m_strbundleversion, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"Thrid", m_thridPath, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"ConfigPath", m_strConfigPath, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"SVNExePath", m_SVNExePath, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"ResPath", m_ResPath, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"WinRarPath", m_Rar, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"ProductName", m_strProductName, L".\\AutoBuild.ini");
	strv.Format(L"%d", m_ResourceVersion);
	WritePrivateProfileString(L"Data", L"ResourceVersion", strv.GetBuffer(), L".\\AutoBuild.ini");

	WritePrivateProfileString(L"Data", L"MaterialsPath", m_strMaterialsPath, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"MaterialsDiffPath", m_strMaterialsDiffPath, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"ItemlistPath", m_strItemlistPath, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"MusicPath", m_strMusicPath, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"MusiclistPath", m_strMusiclistPath, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"EffectPath", m_strEffectPath, L".\\AutoBuild.ini");
	WritePrivateProfileString(L"Data", L"EffectlistPath", m_strEffectlistPath, L".\\AutoBuild.ini");
}
void CAutoBuilderDlg::ExceCommand(CString Processpath, CString strCommand, HANDLE output)
{
	CString strLog;
	strLog.Format(L"[ExceCommand]%s %s", Processpath, strCommand);
	WriteLog(strLog);

	STARTUPINFO si;
	PROCESS_INFORMATION pi;

	CString p = L"\"";
	p += Processpath;
	p += L"\"";
	ZeroMemory(&si, sizeof(si));
	si.cb = sizeof(si);
	ZeroMemory(&pi, sizeof(pi));
	BOOL  bInheritHandles = FALSE;
	if (INVALID_HANDLE_VALUE != output)
	{
		si.dwFlags = STARTF_USESTDHANDLES;
		si.hStdError = output;
		si.hStdOutput = output;
		bInheritHandles = TRUE;
	}
	// Start the child process. 
	if (!CreateProcess(Processpath,   // No module name (use command line). 
		strCommand.GetBuffer(), // Command line. 
		NULL,             // Process handle not inheritable. 
		NULL,             // Thread handle not inheritable. 
		bInheritHandles,  // . 
		0,                // No creation flags. 
		NULL,             // Use parent's environment block. 
		NULL,             // Use parent's starting directory. 
		&si,              // Pointer to STARTUPINFO structure.
		&pi)             // Pointer to PROCESS_INFORMATION structure.
		)
	{
		printf("CreateProcess failed (%d).\n", GetLastError());
		return;
	}

	// Wait until child process exits.
	WaitForSingleObject(pi.hProcess, INFINITE);

	// Close process and thread handles. 
	CloseHandle(pi.hProcess);
	CloseHandle(pi.hThread);

}
void CAutoBuilderDlg::ExceUnityCommand(CString strCommand)
{
	ExceCommand(m_strUnityPath, strCommand, INVALID_HANDLE_VALUE);
}
void CAutoBuilderDlg::Closelog()
{
	if (m_LogFile != NULL)
	{
		m_LogFile->Flush();
		m_LogFile->Close();
		delete m_LogFile;
		m_LogFile = NULL;
	}
}

struct ItemInfo
{
	CString m_strItemID;
	CString m_strItemName;
	CString m_icon;
	BOOL m_bIsBeginner;
	BOOL m_bIsCloth;
};

CString GetExcelDriver()
{
	USES_CONVERSION;
	WCHAR szBuf[2001];
	WORD cbBufMax = 2000;
	WORD cbBufOut;
	WCHAR *pszBuf = szBuf;
	CString sDriver;

	if (!SQLGetInstalledDrivers(pszBuf, cbBufMax, &cbBufOut))
		return L"";

	do
	{
		if (wcsstr(pszBuf, L"Microsoft Excel Driver") != 0)
			//if (strstr(pszBuf, L"Microsoft Excel Driver") != 0)
			//if (wcsstr(pszBuf, L"xlsx") != 0)
		{
			//发现 !
			sDriver = CString(pszBuf);
			//break;
		}
		pszBuf = wcsrchr(pszBuf, L'\0') + 1;
	} while (pszBuf[1] != '\0');

	return sDriver;
}

void GetItemFileName(const CString &strItemID, CString &astrFileName)
{
	astrFileName.Format(L"%s.clh", strItemID);
}

BOOL IsItemFileName(const CString &strFileName, CString &astrFileName)
{
	if (strFileName.CompareNoCase(astrFileName) == 0)
	{
		return TRUE;
	}

	return FALSE;
}

void GetIconFileName(const CString &icon, CString &astrFileName)
{
	astrFileName.Format(L"%s.texture", icon);
}

BOOL CAutoBuilderDlg::FilterIcon(CFile *pLogFile, BOOL resall)
{
	USES_CONVERSION;
	CString strLog;
	CString sDriver;
	CString sDsn;
	CDatabase database;
	CString sSql;
	CArray<ItemInfo> aItemInfo;

	int nErr = 0;
	int nOK = 0;

	sDriver = GetExcelDriver();
	if (sDriver.IsEmpty())
	{
		//没有发现Excel驱动
		strLog += L"没有安装Excel驱动!\r\n";
		return FALSE;
	}

	//创建进行存取的字符串
	sDsn.Format(L"ODBC;DRIVER={%s};DSN=' ';DBQ=%s", sDriver, m_strItemlistPath);
	TRY
	{
		//打开数据库(即Excel文件)
		database.Open(NULL, false, true, sDsn);
		CRecordset recset(&database);

		//设置读取的查询语句.
		//where 三级子类 != 15
		sSql = L"SELECT 物品ID,物品名称,新手道具,ICON FROM \"服饰$\" where 三级子类<>15";
		recset.Open(CRecordset::forwardOnly, sSql, CRecordset::readOnly);
		ItemInfo temp;
		while (!recset.IsEOF())
		{
			recset.GetFieldValue((short)0, temp.m_strItemID);
			CStringA strItemID = W2A(temp.m_strItemID);
			int nItemId = atoi(strItemID.GetBuffer());
			temp.m_strItemID.Format(L"%d", nItemId);
			recset.GetFieldValue(1, temp.m_strItemName);
			CString strIsBeginner;
			recset.GetFieldValue(2, strIsBeginner);
			temp.m_bIsBeginner = _ttoi(strIsBeginner);
			recset.GetFieldValue(3, temp.m_icon);
			temp.m_bIsCloth = TRUE;
			aItemInfo.Add(temp);

			recset.MoveNext();
		}
		recset.Close();

		sSql = L"SELECT 物品ID,物品名称,新手道具,ICON FROM \"旧特效服饰$\" where 三级子类<>15";
		recset.Open(CRecordset::forwardOnly, sSql, CRecordset::readOnly);
		while (!recset.IsEOF())
		{
			recset.GetFieldValue((short)0, temp.m_strItemID);
			CStringA strItemID = W2A(temp.m_strItemID);
			int nItemId = atoi(strItemID.GetBuffer());
			temp.m_strItemID.Format(L"%d", nItemId);
			recset.GetFieldValue(1, temp.m_strItemName);
			CString strIsBeginner;
			recset.GetFieldValue(2, strIsBeginner);
			temp.m_bIsBeginner = _ttoi(strIsBeginner);
			recset.GetFieldValue(3, temp.m_icon);
			temp.m_bIsCloth = TRUE;
			aItemInfo.Add(temp);

			recset.MoveNext();
		}
		recset.Close();

		sSql = L"SELECT 物品ID,物品名称,新手道具,ICON FROM \"道具$\"";
		recset.Open(CRecordset::forwardOnly, sSql, CRecordset::readOnly);
		while (!recset.IsEOF())
		{
			recset.GetFieldValue((short)0, temp.m_strItemID);
			CStringA strItemID = W2A(temp.m_strItemID);
			int nItemId = atoi(strItemID.GetBuffer());
			temp.m_strItemID.Format(L"%d", nItemId);
			recset.GetFieldValue(1, temp.m_strItemName);
			CString strIsBeginner;
			recset.GetFieldValue(2, strIsBeginner);
			temp.m_bIsBeginner = _ttoi(strIsBeginner);
			recset.GetFieldValue(3, temp.m_icon);
			temp.m_bIsCloth = FALSE;
			aItemInfo.Add(temp);

			recset.MoveNext();
		}
		recset.Close();

		sSql = L"SELECT 物品ID,物品名称,新手道具,ICON FROM \"徽章$\"";
		recset.Open(CRecordset::forwardOnly, sSql, CRecordset::readOnly);
		while (!recset.IsEOF())
		{
			recset.GetFieldValue((short)0, temp.m_strItemID);
			CStringA strItemID = W2A(temp.m_strItemID);
			int nItemId = atoi(strItemID.GetBuffer());
			temp.m_strItemID.Format(L"%d", nItemId);
			recset.GetFieldValue(1, temp.m_strItemName);
			CString strIsBeginner;
			recset.GetFieldValue(2, strIsBeginner);
			temp.m_bIsBeginner = _ttoi(strIsBeginner);
			recset.GetFieldValue(3, temp.m_icon);
			temp.m_bIsCloth = FALSE;
			aItemInfo.Add(temp);

			recset.MoveNext();
		}
		recset.Close();
	}
		CATCH(CDBException, e)
	{
		//数据库操作产生异常时...
		strLog += L"数据库错误: " + e->m_strError + L"\r\n";
		return FALSE;
	}
	END_CATCH;

	CString iconPath = m_ResPath + L"\\Icon";
	CString astrFileName;
	CFileFind finder;
	CString strTemp = iconPath + L"\\*.texture";
	CArray<CString> astrAllFile;
	BOOL bHave = finder.FindFile(strTemp);

	while (bHave)
	{
		bHave = finder.FindNextFile();
		strTemp = finder.GetFileName();
		astrAllFile.Add(strTemp);
	}

	for (int i = 0; i < aItemInfo.GetSize(); i++)
	{
		ItemInfo &temp = aItemInfo[i];

		if (!temp.m_bIsBeginner)
			continue;

		GetIconFileName(temp.m_icon, astrFileName);

		bool bOK = false;
		for (int j = 0; j < astrAllFile.GetCount(); j++)
		{
			BOOL bCopy = IsItemFileName(astrAllFile[j], astrFileName);
			if (bCopy)
			{
				CString path = L"\\xuanqu\\lwts\\res\\Icon\\";
				if (resall)
				{
					path = L"\\xuanqu\\lwts\\res_all\\Icon\\";
				}
				BOOL bSucees = CopyFile(iconPath + L"\\" + astrAllFile[j], m_CurrntWorkDir + path + astrAllFile[j], FALSE);
				if (!bSucees)
				{
					DWORD dwErrorCode = GetLastError();
					strTemp.Format(L"Copy file Failed::%s\r\n", iconPath + L"\\" + astrAllFile[j]);
					strLog += strTemp;
				}
				else
				{
					CString a;
					a.Format(L"物品[%s,%s][%s]的资源文件复制成功\r\n", temp.m_strItemName, temp.m_strItemID, astrAllFile[j]);
					strLog += a;

					//astrAllFile.RemoveAt(j);
					//j--;
					bOK = true;
				}
			}
		}

		if (bOK)
		{
			nOK++;
		}
		else
		{
			nErr++;

			CString a;
			a.Format(L"物品[%s,%s]的资源文件复制失败\r\n", temp.m_strItemName, temp.m_strItemID);
			strLog += a;
		}
	}

	CString a;
	a.Format(L"复制完成，总共筛选出%d个文件，发现%d个错误\r\n", nOK, nErr);
	strLog += a;

	const int UNICODE_TXT_FLG = 0xFEFF;
	pLogFile->Write(&UNICODE_TXT_FLG, 2);
	pLogFile->Write(strLog.GetBuffer(), strLog.GetLength() * 2);

	return nErr == 0;
}

BOOL CAutoBuilderDlg::FilterMaterials(CFile *pLogFile, BOOL resall)
{
	USES_CONVERSION;
	CString strLog;
	CString sDriver;
	CString sDsn;
	CDatabase database;
	CString sSql;
	CArray<ItemInfo> aItemInfo;

	int nErr = 0;
	int nOK = 0;

	sDriver = GetExcelDriver();
	if (sDriver.IsEmpty())
	{
		//没有发现Excel驱动
		strLog += L"没有安装Excel驱动!\r\n";
		return FALSE;
	}

	//创建进行存取的字符串
	sDsn.Format(L"ODBC;DRIVER={%s};DSN=' ';DBQ=%s", sDriver, m_strItemlistPath);
	TRY
	{
		//打开数据库(即Excel文件)
		database.Open(NULL, false, true, sDsn);
		CRecordset recset(&database);

		//设置读取的查询语句.
		//where 三级子类 != 15
		sSql = L"SELECT 物品ID,物品名称,新手道具,Atlas FROM \"服饰$\" where 三级子类<>15";
		recset.Open(CRecordset::forwardOnly, sSql, CRecordset::readOnly);
		ItemInfo temp;
		while (!recset.IsEOF())
		{
			recset.GetFieldValue((short)0, temp.m_strItemID);
			CStringA strItemID = W2A(temp.m_strItemID);
			int nItemId = atoi(strItemID.GetBuffer());
			temp.m_strItemID.Format(L"%d", nItemId);
			recset.GetFieldValue(1, temp.m_strItemName);
			CString strIsBeginner;
			recset.GetFieldValue(2, strIsBeginner);
			temp.m_bIsBeginner = _ttoi(strIsBeginner);
			recset.GetFieldValue(3, temp.m_icon);
			temp.m_bIsCloth = TRUE;
			aItemInfo.Add(temp);

			recset.MoveNext();
		}
		recset.Close();

		sSql = L"SELECT 物品ID,物品名称,新手道具,Atlas FROM \"旧特效服饰$\" where 三级子类<>15";
		recset.Open(CRecordset::forwardOnly, sSql, CRecordset::readOnly);
		while (!recset.IsEOF())
		{
			recset.GetFieldValue((short)0, temp.m_strItemID);
			CStringA strItemID = W2A(temp.m_strItemID);
			int nItemId = atoi(strItemID.GetBuffer());
			temp.m_strItemID.Format(L"%d", nItemId);
			recset.GetFieldValue(1, temp.m_strItemName);
			CString strIsBeginner;
			recset.GetFieldValue(2, strIsBeginner);
			temp.m_bIsBeginner = _ttoi(strIsBeginner);
			recset.GetFieldValue(3, temp.m_icon);
			temp.m_bIsCloth = TRUE;
			aItemInfo.Add(temp);

			recset.MoveNext();
		}
		recset.Close();

		sSql = L"SELECT 物品ID,物品名称,新手道具,Atlas FROM \"小孩服饰$\" where 三级子类<>4";
		recset.Open(CRecordset::forwardOnly, sSql, CRecordset::readOnly);
		while (!recset.IsEOF())
		{
			recset.GetFieldValue((short)0, temp.m_strItemID);
			CStringA strItemID = W2A(temp.m_strItemID);
			int nItemId = atoi(strItemID.GetBuffer());
			temp.m_strItemID.Format(L"%d", nItemId);
			recset.GetFieldValue(1, temp.m_strItemName);
			CString strIsBeginner;
			recset.GetFieldValue(2, strIsBeginner);
			temp.m_bIsBeginner = _ttoi(strIsBeginner);
			recset.GetFieldValue(3, temp.m_icon);
			temp.m_bIsCloth = TRUE;
			aItemInfo.Add(temp);

			recset.MoveNext();
		}
		recset.Close();
	}
		CATCH(CDBException, e)
	{
		//数据库操作产生异常时...
		strLog += L"数据库错误: " + e->m_strError + L"\r\n";
		return FALSE;
	}
	END_CATCH;

	CString astrFileName;
	CFileFind finder;
	CString strTemp = m_strMaterialsPath + L"\\*.clh";
	CArray<CString> astrAllFile;
	CArray<CString> diffAllFile;
	BOOL bHave = finder.FindFile(strTemp);

	while (bHave)
	{
		bHave = finder.FindNextFile();
		strTemp = finder.GetFileName();
		astrAllFile.Add(strTemp);
	}

	if (!m_strMaterialsDiffPath.IsEmpty())
	{
		strTemp = m_strMaterialsDiffPath + L"\\*.clh";
		bHave = finder.FindFile(strTemp);
		while (bHave)
		{
			bHave = finder.FindNextFile();
			strTemp = finder.GetFileName();
			diffAllFile.Add(strTemp);
		}
	}

	for (int i = 0; i < aItemInfo.GetSize(); i++)
	{
		ItemInfo &temp = aItemInfo[i];

		if ((m_bIsSmallPack || m_bDynDownlad) && !resall && !temp.m_bIsBeginner)
			continue;

		GetItemFileName(temp.m_icon, astrFileName);

		bool bOK = false;
		for (int j = 0; j < astrAllFile.GetCount(); j++)
		{
			BOOL bCopy = IsItemFileName(astrAllFile[j], astrFileName);
			if (bCopy)
			{
				CString path = L"\\xuanqu\\lwts\\res\\Materials\\";
				if (resall)
				{
					path = L"\\xuanqu\\lwts\\res_all\\Materials\\";
				}
				BOOL bSucees = CopyFile(m_strMaterialsPath + L"\\" + astrAllFile[j], m_CurrntWorkDir + path + astrAllFile[j], FALSE);
				if (!bSucees)
				{
					DWORD dwErrorCode = GetLastError();
					strTemp.Format(L"Copy file Failed::%s\r\n", m_strMaterialsPath + L"\\" + astrAllFile[j]);
					strLog += strTemp;
				}
				else
				{
					CString a;
					a.Format(L"物品[%s,%s][%s]的资源文件复制成功\r\n", temp.m_strItemName, temp.m_strItemID, astrAllFile[j]);
					strLog += a;

					//astrAllFile.RemoveAt(j);
					//j--;
					bOK = true;
				}
			}
		}
		for (int j = 0; j < diffAllFile.GetCount(); j++)
		{
			int nIndex = IsItemFileName(diffAllFile[j], astrFileName);
			if (nIndex >= 0)
			{
				CString path = L"\\xuanqu\\lwts\\res\\Materials\\";
				if (resall)
				{
					path = L"\\xuanqu\\lwts\\res_all\\Materials\\";
				}
				BOOL bSucees = CopyFile(m_strMaterialsDiffPath + L"\\" + diffAllFile[j], m_CurrntWorkDir + path + diffAllFile[j], FALSE);
				if (!bSucees)
				{
					DWORD dwErrorCode = GetLastError();
					strTemp.Format(L"Copy file Failed::%s\r\n", m_strMaterialsDiffPath + L"\\" + diffAllFile[j]);
					strLog += strTemp;
				}
				else
				{
					CString a;
					a.Format(L"物品[%s,%s][%s]的差异资源文件复制成功\r\n", temp.m_strItemName, temp.m_strItemID, diffAllFile[j]);
					strLog += a;

					diffAllFile.RemoveAt(j);
					j--;
					bOK = true;
				}
				//break;
			}
		}

		if (bOK)
		{
			nOK++;
		}
		else
		{
			nErr++;

			CString a;
			a.Format(L"物品[%s,%s]的资源文件复制失败\r\n", temp.m_strItemName, temp.m_strItemID);
			strLog += a;
		}
	}

	CString a;
	a.Format(L"复制完成，总共筛选出%d个文件，发现%d个错误\r\n", nOK, nErr);
	strLog += a;

	const int UNICODE_TXT_FLG = 0xFEFF;
	pLogFile->Write(&UNICODE_TXT_FLG, 2);
	pLogFile->Write(strLog.GetBuffer(), strLog.GetLength() * 2);

	return nErr == 0;
}

struct MusicInfo
{
	CString m_strMusicID;
	CString m_strMusicName;
	BOOL m_bIsBeginner;
};

struct EffectInfo
{
	CString m_strEffectName;
};

BOOL CAutoBuilderDlg::FilterMusic(CFile *pLogFile, BOOL resall)
{
	USES_CONVERSION;
	CString strLog;
	CString sDriver;
	CString sDsn;
	CDatabase database;
	CString sSql;
	CArray<MusicInfo> aMusicInfo;

	int nErr = 0;
	int nOK = 0;

	sDriver = GetExcelDriver();
	if (sDriver.IsEmpty())
	{
		//没有发现Excel驱动
		strLog += L"没有安装Excel驱动!\r\n";
		return FALSE;
	}

	//创建进行存取的字符串
	sDsn.Format(L"ODBC;DRIVER={%s};DSN=' ';DBQ=%s", sDriver, m_strMusiclistPath);
	TRY
	{
		//打开数据库(即Excel文件)
		database.Open(NULL, false, true, sDsn);
		CRecordset recset(&database);

		//设置读取的查询语句.
		//where 三级子类 != 15
		sSql = L"SELECT 歌曲ID,音乐资源名称,新手音乐 FROM \"音乐表$\"";
		recset.Open(CRecordset::forwardOnly, sSql, CRecordset::readOnly);
		MusicInfo temp;
		while (!recset.IsEOF())
		{
			recset.GetFieldValue((short)0, temp.m_strMusicID);
			CStringA strMusicID = W2A(temp.m_strMusicID);
			int nMusicID = atoi(strMusicID.GetBuffer());
			temp.m_strMusicID.Format(L"%d", nMusicID);
			CString strMusicName;
			recset.GetFieldValue(1, strMusicName);
			temp.m_strMusicName.Format(L"%s.smp", strMusicName);
			CString strIsBeginner;
			recset.GetFieldValue(2, strIsBeginner);
			temp.m_bIsBeginner = _ttoi(strIsBeginner);
			aMusicInfo.Add(temp);

			recset.MoveNext();
		}
		recset.Close();
	}
		CATCH(CDBException, e)
	{
		//数据库操作产生异常时...
		strLog += L"数据库错误: " + e->m_strError + L"\r\n";
		return FALSE;
	}
	END_CATCH;

	CFileFind finder;
	CString strTemp = m_strMusicPath + L"\\*.smp";
	CArray<CString> astrAllFile;
	BOOL bHave = finder.FindFile(strTemp);

	while (bHave)
	{
		bHave = finder.FindNextFile();
		strTemp = finder.GetFileName();
		astrAllFile.Add(strTemp);
	}

	//copy ground
	for (int i = 0; i < astrAllFile.GetCount(); i++)
	{
		if (astrAllFile[i].Find(L"song") == -1 && astrAllFile[i].Find(L"Song"))
		{
			CString path = L"\\xuanqu\\lwts\\res\\Music\\";
			if (resall)
			{
				path = L"\\xuanqu\\lwts\\res_all\\Music\\";
			}
			BOOL bSucees = CopyFile(m_strMusicPath + L"\\" + astrAllFile[i], m_CurrntWorkDir + path + astrAllFile[i], FALSE);
			if (!bSucees)
			{
				DWORD dwErrorCode = GetLastError();
				strTemp.Format(L"Copy file Failed::%s\r\n", m_strMusicPath + L"\\" + astrAllFile[i]);
				strLog += strTemp;
			}
		}
	}

	for (int i = 0; i < aMusicInfo.GetSize(); i++)
	{
		MusicInfo &temp = aMusicInfo[i];

		if (m_bIsSmallPack && !temp.m_bIsBeginner)
			continue;

		bool bOK = false;
		for (int j = 0; j < astrAllFile.GetCount(); j++)
		{
			if (astrAllFile[j].CompareNoCase(temp.m_strMusicName) == 0)
			{
				CString path = L"\\xuanqu\\lwts\\res\\Music\\";
				if (resall)
				{
					path = L"\\xuanqu\\lwts\\res_all\\Music\\";
				}
				BOOL bSucees = CopyFile(m_strMusicPath + L"\\" + astrAllFile[j], m_CurrntWorkDir + path + astrAllFile[j], FALSE);
				if (!bSucees)
				{
					DWORD dwErrorCode = GetLastError();
					strTemp.Format(L"Copy file Failed::%s\r\n", m_strMusicPath + L"\\" + astrAllFile[j]);
					strLog += strTemp;
				}
				else
				{
					astrAllFile.RemoveAt(j);
					j--;
					bOK = true;
				}
				//break;
			}
		}
		if (bOK)
		{
			nOK++;
		}
		else
		{
			nErr++;

			CString a;
			a.Format(L"音乐[%s,%s]的资源文件复制失败\r\n", temp.m_strMusicID, temp.m_strMusicName);
			strLog += a;
		}
	}

	CString a;
	a.Format(L"复制完成，总共筛选出%d个文件，发现%d个错误\r\n", nOK, nErr);
	strLog += a;

	const int UNICODE_TXT_FLG = 0xFEFF;
	pLogFile->Write(&UNICODE_TXT_FLG, 2);
	pLogFile->Write(strLog.GetBuffer(), strLog.GetLength() * 2);

	return nErr == 0;
}

BOOL CAutoBuilderDlg::FilterEffect(CFile *pLogFile)
{
	USES_CONVERSION;
	CString strLog;
	CString sDriver;
	CString sDsn;
	CDatabase database;
	CString sSql;
	CArray<EffectInfo> aEffectInfo;

	int nErr = 0;
	int nOK = 0;

	sDriver = GetExcelDriver();
	if (sDriver.IsEmpty())
	{
		//没有发现Excel驱动
		strLog += L"没有安装Excel驱动!\r\n";
		return FALSE;
	}

	//创建进行存取的字符串
	sDsn.Format(L"ODBC;DRIVER={%s};DSN=' ';DBQ=%s", sDriver, m_strEffectlistPath);
	TRY
	{
		//打开数据库(即Excel文件)
		database.Open(NULL, false, true, sDsn);
		CRecordset recset(&database);

		//设置读取的查询语句.
		//where 三级子类 != 15
		sSql = L"SELECT 特效名称 FROM \"特效$\"";
		recset.Open(CRecordset::forwardOnly, sSql, CRecordset::readOnly);
		EffectInfo temp;
		while (!recset.IsEOF())
		{
			CString strEffectName;
			recset.GetFieldValue((short)0, strEffectName);
			temp.m_strEffectName.Format(L"%s.pre", strEffectName);

			bool bExist = false;
			for (int i = 0; i < aEffectInfo.GetSize(); i++)
			{
				if (aEffectInfo[i].m_strEffectName.CompareNoCase(temp.m_strEffectName) == 0)
				{
					bExist = true;
					break;
				}
			}

			if (!bExist)
			{
				aEffectInfo.Add(temp);
			}

			recset.MoveNext();
		}
		recset.Close();

		sSql = L"SELECT 特效名称 FROM \"舞团团徽特效$\"";
		recset.Open(CRecordset::forwardOnly, sSql, CRecordset::readOnly);
		while (!recset.IsEOF())
		{
			CString strEffectName;
			recset.GetFieldValue((short)0, strEffectName);
			temp.m_strEffectName.Format(L"%s.pre", strEffectName);

			bool bExist = false;
			for (int i = 0; i < aEffectInfo.GetSize(); i++)
			{
				if (aEffectInfo[i].m_strEffectName.CompareNoCase(temp.m_strEffectName) == 0)
				{
					bExist = true;
					break;
				}
			}

			if (!bExist)
			{
				aEffectInfo.Add(temp);
			}

			recset.MoveNext();
		}
		recset.Close();

		sSql = L"SELECT 特效名称 FROM \"结婚戒指特效$\"";
		recset.Open(CRecordset::forwardOnly, sSql, CRecordset::readOnly);
		while (!recset.IsEOF())
		{
			CString strEffectName;
			recset.GetFieldValue((short)0, strEffectName);
			temp.m_strEffectName.Format(L"%s.pre", strEffectName);

			bool bExist = false;
			for (int i = 0; i < aEffectInfo.GetSize(); i++)
			{
				if (aEffectInfo[i].m_strEffectName.CompareNoCase(temp.m_strEffectName) == 0)
				{
					bExist = true;
					break;
				}
			}

			if (!bExist)
			{
				aEffectInfo.Add(temp);
			}

			recset.MoveNext();
		}
		recset.Close();

		sSql = L"SELECT 特效名称 FROM \"结婚场景特效$\"";
		recset.Open(CRecordset::forwardOnly, sSql, CRecordset::readOnly);
		while (!recset.IsEOF())
		{
			CString strEffectName;
			recset.GetFieldValue((short)0, strEffectName);
			temp.m_strEffectName.Format(L"%s.pre", strEffectName);

			bool bExist = false;
			for (int i = 0; i < aEffectInfo.GetSize(); i++)
			{
				if (aEffectInfo[i].m_strEffectName.CompareNoCase(temp.m_strEffectName) == 0)
				{
					bExist = true;
					break;
				}
			}

			if (!bExist)
			{
				aEffectInfo.Add(temp);
			}

			recset.MoveNext();
		}
		recset.Close();
	}
		CATCH(CDBException, e)
	{
		//数据库操作产生异常时...
		strLog += L"数据库错误: " + e->m_strError + L"\r\n";
		return FALSE;
	}
	END_CATCH;

	CFileFind finder;
	CString strTemp = m_strEffectPath + L"\\*.pre";
	CArray<CString> astrAllFile;
	BOOL bHave = finder.FindFile(strTemp);

	while (bHave)
	{
		bHave = finder.FindNextFile();
		strTemp = finder.GetFileName();
		astrAllFile.Add(strTemp);
	}

	for (int i = 0; i < aEffectInfo.GetSize(); i++)
	{
		EffectInfo &temp = aEffectInfo[i];

		bool bOK = false;
		for (int j = 0; j < astrAllFile.GetCount(); j++)
		{
			if (astrAllFile[j].CompareNoCase(temp.m_strEffectName) == 0)
			{
				BOOL bSucees = CopyFile(m_strEffectPath + L"\\" + astrAllFile[j], m_CurrntWorkDir + L"\\xuanqu\\lwts\\res\\Effect\\" + astrAllFile[j], FALSE);
				if (!bSucees)
				{
					DWORD dwErrorCode = GetLastError();
					strTemp.Format(L"Copy file Failed::%s\r\n", m_strEffectPath + L"\\" + astrAllFile[j]);
					strLog += strTemp;
				}
				else
				{
					astrAllFile.RemoveAt(j);
					j--;
					bOK = true;
				}
				//break;
			}
		}
		if (bOK)
		{
			nOK++;
		}
		else
		{
			nErr++;

			CString a;
			a.Format(L"特效[%s]的资源文件复制失败\r\n", temp.m_strEffectName);
			strLog += a;
		}
	}

	CString a;
	a.Format(L"复制完成，总共筛选出%d个文件，发现%d个错误\r\n", nOK, nErr);
	strLog += a;

	const int UNICODE_TXT_FLG = 0xFEFF;
	pLogFile->Write(&UNICODE_TXT_FLG, 2);
	pLogFile->Write(strLog.GetBuffer(), strLog.GetLength() * 2);

	return nErr == 0;
}

void CAutoBuilderDlg::OnCancel()
{
	// TODO: Add your specialized code here and/or call the base class
	Closelog();
	CDialog::OnCancel();
}


void CAutoBuilderDlg::OnBnClickedUicompile()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData();
	CString strTemp = m_bUI ? L"1" : L"0";
	WritePrivateProfileString(L"Config", L"BuildUI", strTemp, L".\\AutoBuild.ini");
}


void CAutoBuilderDlg::OnBnClickedatlascompile2()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData();
	CString strTemp = m_batlas ? L"1" : L"0";
	WritePrivateProfileString(L"Config", L"BuildAtlas", strTemp, L".\\AutoBuild.ini");
}


void CAutoBuilderDlg::OnBnClickedatlascompile3()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData();
	CString strTemp = m_bStage ? L"1" : L"0";
	WritePrivateProfileString(L"Config", L"BuildStage", strTemp, L".\\AutoBuild.ini");
}


void CAutoBuilderDlg::OnBnClickedChecksmallpack()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData();
	CString strTemp = m_bIsSmallPack ? L"1" : L"0";
	WritePrivateProfileString(L"Config", L"BuildSmallPack", strTemp, L".\\AutoBuild.ini");
}


void CAutoBuilderDlg::OnBnClickedSvnUp()
{
	// TODO:  在此添加控件通知处理程序代码
	UpdateData();
	CString strTemp = m_bSvnUp ? L"1" : L"0";
	WritePrivateProfileString(L"Config", L"SvnUp", strTemp, L".\\AutoBuild.ini");
}


void CAutoBuilderDlg::OnBnClickedCheckgenprefab()
{
	// TODO:  在此添加控件通知处理程序代码
	UpdateData();
	CString strTemp = m_bGenDynamicPrefab ? L"1" : L"0";
	WritePrivateProfileString(L"Config", L"GenerateDynamicPrefab", strTemp, L".\\AutoBuild.ini");
}


void CAutoBuilderDlg::OnBnClickedCheckdyndownload()
{
	// TODO:  在此添加控件通知处理程序代码
	UpdateData();
	CString strTemp = m_bDynDownlad ? L"1" : L"0";
	WritePrivateProfileString(L"Config", L"DynDownlad", strTemp, L".\\AutoBuild.ini");
}

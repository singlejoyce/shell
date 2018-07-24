// AutoBuilderDlg.h : header file
//

#pragma once
#include "afxwin.h"
#include <fstream>
using namespace std;

// CAutoBuilderDlg dialog
class CAutoBuilderDlg : public CDialog
{
// Construction
public:
	CAutoBuilderDlg(CWnd* pParent = NULL);	// standard constructor
// Dialog Data
	enum { IDD = IDD_AUTOBUILDER_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support

private:
	void BuildUI();
	void GenerateDynamicPrefab();
	void BuildAtlas();
	void BuildStage();
	void ExceUnityCommand( CString strCommand );
	void ExceCommand( CString Processpath,CString strCommand,HANDLE output );
	void SaveSelect();
	void LoadData();
	void BuildForParter( CString strParterName );
	void BuildForALL( );
	BOOL ReplaceThrid(CString strThridName,BOOL bAll);
	void RebuildProjectSetting(CString strPARTER_ID ,CString strBundle_identifie,int nVersioncode ,CString SubVersin,CString Parterpath,CString SplashScreen,int nAPILevel);
	void SetWorkDir();
	void BuildAPK(CString strIcon,CString strKeyFile,CString strKeyaliasName,CString strKeyaliasPass,CString strKeystorePass);
	void RebuildScriptDefine(CString strDefine);
	void ClearScriptDefine();
	void SetOpenLog();
	void WriteLog(CString strContent);
	BOOL RemoveNoNullDir( CString path );
	BOOL CopyDir( CString pathScr,CString pathDes );
	bool _GetLine( char * pContent ,int& nlen,int nTotalLen,char* pResult  );
	void MoveApkToParterFolder( CString path,CString strParterName ,BOOL IsBig);
	void GetNew(CString szDir,CString LogName,int nReversion);
	void Revert(CString szDir);
	void MakeConfigDir();
	void MakeAllZip(CString pack);
	void MakeAllZipforDynDownlad(CString pack);
	void CopyAllZipToThrid();
	void OutputMd5ForFile(CString strFileName);
	void SVNClear( CString szDir );
	void Closelog();
	BOOL FilterIcon(CFile *pLogFile, BOOL resall);
	BOOL FilterMaterials(CFile *pLogFile, BOOL resall);
	BOOL FilterMusic(CFile *pLogFile, BOOL resall);
	BOOL FilterEffect(CFile *pLogFile);
	//BOOL CheckDecryptContent();
	void CopyIcon(CString strthridr, CString strIcon);
	void DoChangeProductName(CString strPath, CString strProductName);
	void DoChangeScriptDefine(CString strPath);
	void GenerateJson();
	void GenerateJson(ofstream &json, CString path, BOOL includeSubdir);
	BOOL DeletePath(CString strPath);
	void GeneratePack(BOOL isSmallPack, BOOL isDynDownlad);
	void GenerateAllPack();
	void SvnUpdate();
	void BuildRes();
	void CopyResExtend();
// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();
	CString m_ParterName;
	int m_Bundleversioncode;
	BOOL m_bUI;
	BOOL m_batlas;
	BOOL m_bStage;
	CString m_strPreBuildScipt;
	CString m_strAfterBuildScipt;
	CString m_strUnityPath;
	CString m_ProjectPath;
	CComboBox m_ParterSelect;
	CString m_DefaultSplashScreen;
	CString m_DefaultAndroidKeyPath;
	int m_DefaultAndroidAPILevel;
	CString m_thridPath;

	CString m_CurrntWorkDir;

	CFile *m_LogFile;
protected:
	virtual void OnCancel();
public:
	CString m_strbundleversion;
	CString m_SVNExePath;
	CString m_ResPath;
	int m_ResourceVersion;
	CString m_Rar;

	int m_ProjectReVersion;
	int m_ThridReVersion;
	int m_ResReVersion;

	CString m_strConfigPath;
	int m_nConfigVersion;
	CString m_strResPackName;
	CComboBox m_ResPackNameSelect;

	CString m_SettingPath;
	int m_SettingReVersion;
	CString m_strMaterialsPath;
	CString m_strItemlistPath;
	afx_msg void OnBnClickedUicompile();
	afx_msg void OnBnClickedatlascompile2();
	afx_msg void OnBnClickedatlascompile3();
	BOOL m_bIsSmallPack;
	afx_msg void OnBnClickedChecksmallpack();
	int m_nMaterialsVersion;
	CString m_strMusiclistPath;
	CString m_strMusicPath;
	int m_nMusicVersion;
	CString m_strProductName;
	CString m_strMaterialsDiffPath;
	BOOL m_bSvnUp;
	afx_msg void OnBnClickedSvnUp();
	CString m_strEffectPath;
	CString m_strEffectlistPath;
	int m_nEffectVersion;
	afx_msg void OnBnClickedCheckgenprefab();
	BOOL m_bGenDynamicPrefab;
	BOOL m_bDynDownlad;
	afx_msg void OnBnClickedCheckdyndownload();
};

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace StageTester
{
    public partial class MainUI : Form
    {
        private EStageMode m_StageMode = EStageMode.None;

        public MainUI()
        {
            InitializeComponent();
        }

        private void OnBtnChooseDirClick(object sender, EventArgs e)
        {
            FolderBrowserDialog dirFilter = new FolderBrowserDialog();
            dirFilter.RootFolder = Environment.SpecialFolder.MyComputer;
            dirFilter.ShowNewFolderButton = false;

            if (dirFilter.ShowDialog() == DialogResult.OK)
            {
                m_edt_StageDir.Text = dirFilter.SelectedPath;
            }
        }

        private void OnChangeResultSelected(object sender, EventArgs e)
        {
            m_dgv_Detail.Rows.Clear();

            if (m_lst_Result.SelectedItem != null)
            {
                TestDetail detailInfo = (TestDetail)(m_lst_Result.SelectedItem);
                _ShowStageDetail(detailInfo);
            }
        }

        private void OnBtnTestClick(object sender, EventArgs e)
        {
            _RefreshStage();
            _TestStage();
        }

        void _GetAllPrefectScore(TestDetail detailInfo)
        {
            if (m_StageMode != EStageMode.None)
            {
                int m_strSongMode = 0;
                int m_strSongLevel = 0;

                for (EStageLevel stageLevel = EStageLevel.Easy; stageLevel < EStageLevel.Max; ++stageLevel)
                {
                    //模式转换为id
                    if (detailInfo.m_eStageMode == EStageMode.Taiko)
                    {
                        m_strSongMode = 1;
                    }
                    else if (detailInfo.m_eStageMode == EStageMode.Tradition)
                    {
                        m_strSongMode = 2;
                    }
                    else if (detailInfo.m_eStageMode == EStageMode.Osu)
                    {
                        m_strSongMode = 3;
                    }
                    else if (detailInfo.m_eStageMode == EStageMode.Audition)
                    {
                        m_strSongMode = 4;
                    }
                    else if (detailInfo.m_eStageMode == EStageMode.HeartBeats)
                    {
                        m_strSongMode = 21;
                    }

                    //难度转换
                    if (stageLevel == EStageLevel.Easy)
                    {
                        m_strSongLevel = 1;
                    }
                    else if (stageLevel == EStageLevel.Normal)
                    {
                        m_strSongLevel = 2;
                    }
                    else if (stageLevel == EStageLevel.Hard)
                    {
                        m_strSongLevel = 3;
                    }
                    else if (stageLevel == EStageLevel.SuperEasy)
                    {
                        m_strSongLevel = 11;
                    }
                    else if (stageLevel == EStageLevel.SuperNormal)
                    {
                        m_strSongLevel = 12;
                    }
                    else if (stageLevel == EStageLevel.SuperHard)
                    {
                        m_strSongLevel = 13;
                    }

                    DebugExcel.Write(detailInfo.m_strStageFile.Remove(0, 4) + "\t" + m_strSongMode +
                        "\t" + m_strSongLevel + "\t" + detailInfo.m_TestFeatures.Find(Test).m_strValue[(int)stageLevel],
                        LogLevel.ERROR);
                }
            }
        }

        private bool Test(TestFeature feature)
        {
            return feature != null && feature.m_strFeature == "PERFECTSCORE";
        }

        private void _RefreshStage()
        {
            m_lab_StageDir.Text = "";
            m_StageMode = EStageMode.None;

            m_lab_StageMode.Text = "";
            m_lab_StageCount.Text = "";
            m_lab_SucCount.Text = "";
            m_lab_FailCount.Text = "";

            m_lst_Result.Items.Clear();
            m_dgv_Detail.Rows.Clear();

            if (m_edt_StageDir.TextLength > 0)
            {
                if (Directory.Exists(m_edt_StageDir.Text))
                {
                    string stageDir = m_edt_StageDir.Text.Substring(m_edt_StageDir.Text.LastIndexOf('\\') + 1);
                    if (stageDir == "taigu")
                    {
                        m_StageMode = EStageMode.Taiko;
                    }
                    else if (stageDir == "tradition")
                    {
                        m_StageMode = EStageMode.Tradition;
                    }
                    else if (stageDir == "osu")
                    {
                        m_StageMode = EStageMode.Osu;
                    }
                    else if (stageDir == "au")
                    {
                        m_StageMode = EStageMode.Audition;
                    }
                    else if (stageDir == "rhythm")
                    {
                        m_StageMode = EStageMode.Rhythm;
                    }
                    else if (stageDir == "heartbeats")
                    {
                        m_StageMode = EStageMode.HeartBeats;
                    }

                    if (m_StageMode != EStageMode.None)
                    {
                        m_lab_StageDir.Text = m_edt_StageDir.Text;
                        m_lab_StageMode.Text = CommonFunc.GetStageName(m_StageMode);
                    }
                    else
                    {
                        MessageBox.Show("关卡模式解析失败 " + m_edt_StageDir.Text + " 不合法!", "错误", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("文件夹 " + m_edt_StageDir.Text + " 不存在!", "错误", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("文件夹为空!", "错误", MessageBoxButtons.OK);
            }
        }

        private void _TestStage()
        {
            if (m_StageMode != EStageMode.None)
            {
                int nSucCount = 0;
                int nFailCount = 0;

                Dictionary<string, EStageMode> allStage = _GetAllStage(m_StageMode);
                foreach (KeyValuePair<string, EStageMode> kvp in allStage)
                {
                    TestDetail detailInfo = new TestDetail();
                    detailInfo.m_strStageFile = kvp.Key;
                    detailInfo.m_eStageMode = kvp.Value;

                    StageInfo_Base[] arStageInfo = new StageInfo_Base[(int)EStageLevel.Max];
                    arStageInfo[(int)EStageLevel.Easy] = _GetStageInfo(detailInfo.m_strStageFile, detailInfo.m_eStageMode, EStageLevel.Easy, false);
                    arStageInfo[(int)EStageLevel.Normal] = _GetStageInfo(detailInfo.m_strStageFile, detailInfo.m_eStageMode, EStageLevel.Normal, false);
                    arStageInfo[(int)EStageLevel.Hard] = _GetStageInfo(detailInfo.m_strStageFile, detailInfo.m_eStageMode, EStageLevel.Hard, false);
                    arStageInfo[(int)EStageLevel.SuperEasy] = _GetStageInfo(detailInfo.m_strStageFile, detailInfo.m_eStageMode, EStageLevel.SuperEasy, true);
                    arStageInfo[(int)EStageLevel.SuperNormal] = _GetStageInfo(detailInfo.m_strStageFile, detailInfo.m_eStageMode, EStageLevel.SuperNormal, true);
                    arStageInfo[(int)EStageLevel.SuperHard] = _GetStageInfo(detailInfo.m_strStageFile, detailInfo.m_eStageMode, EStageLevel.SuperHard, true);

                    if (detailInfo.m_eStageMode == EStageMode.Taiko)
                    {
                        _CheckTaikoFeature(arStageInfo, detailInfo);
                    }
                    else if (detailInfo.m_eStageMode == EStageMode.Tradition)
                    {
                        _CheckTraditionFeature(arStageInfo, detailInfo);
                    }
                    else if (detailInfo.m_eStageMode == EStageMode.Osu)
                    {
                        _CheckOsuFeature(arStageInfo, detailInfo);
                    }
                    else if (detailInfo.m_eStageMode == EStageMode.Audition)
                    {
                        _CheckAuditionFeature(arStageInfo, detailInfo);
                    }
                    else if (detailInfo.m_eStageMode == EStageMode.Rhythm)
                    {
                        _CheckRhythmFeature(arStageInfo, detailInfo);
                    }
                    else if (detailInfo.m_eStageMode == EStageMode.HeartBeats)
                    {
                        _CheckHeartBeatsFeature(arStageInfo, detailInfo);
                    }

                    if (detailInfo.m_nTestError > 0)
                    {
                        ++nFailCount;
                    }
                    else
                    {
                        ++nSucCount;
                    }

                    m_lst_Result.Items.Add(detailInfo);
                    _GetAllPrefectScore(detailInfo);
                }

                m_lab_StageCount.Text = allStage.Count.ToString();
                m_lab_SucCount.Text = nSucCount.ToString();
                m_lab_FailCount.Text = nFailCount.ToString();
            }
        }

        void _ShowStageDetail(TestDetail detailInfo)
        {
            List<TestFeature> allTestFeature = detailInfo.m_TestFeatures;
            foreach (TestFeature testFeature in allTestFeature)
            {
                if (testFeature != null)
                {
                    string[] rowContent = new string[] {
                        testFeature.m_strFeature, 
                        testFeature.m_strValue[(int)EStageLevel.Easy],
                        testFeature.m_strValue[(int)EStageLevel.Normal],
                        testFeature.m_strValue[(int)EStageLevel.Hard],
                        testFeature.m_strValue[(int)EStageLevel.SuperEasy],
                        testFeature.m_strValue[(int)EStageLevel.SuperNormal],
                        testFeature.m_strValue[(int)EStageLevel.SuperHard]};
                    int rowIndex = m_dgv_Detail.Rows.Add(rowContent);

                    if (testFeature.m_nErrorNo > 0)
                    {
                        m_dgv_Detail.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }

            m_dgv_Detail.ClearSelection();
        }

        private Dictionary<string, EStageMode> _GetAllStage(EStageMode stageMode)
        {
            Dictionary<string, EStageMode> allStage = new Dictionary<string, EStageMode>();

            if (stageMode != EStageMode.None)
            {
                string[] stagePath = Directory.GetFiles(m_lab_StageDir.Text);
                foreach (string path in stagePath)
                {
                    string stageFile = path.Substring(path.LastIndexOf('\\') + 1);
                    stageFile = stageFile.Remove(stageFile.IndexOf('.'));

                    if (!allStage.ContainsKey(stageFile))
                    {
                        allStage.Add(stageFile, stageMode);
                    }
                }
            }

            return allStage;
        }

        private StageInfo_Base _GetStageInfo(string stageFile, EStageMode stageMode, EStageLevel stageLevel, bool isSuper)
        {
            StageInfo_Base stageInfo = null;

            string stagePath = m_lab_StageDir.Text + "/" + stageFile + CommonFunc.GetStageExtension(stageMode, stageLevel, isSuper);
            if (File.Exists(stagePath))
            {
                if (stageMode == EStageMode.Taiko)
                {
                    stageInfo = new StageInfo_Taiko();
                }
                else if (stageMode == EStageMode.Tradition)
                {
                    stageInfo = new StageInfo_Tradition();
                }
                else if (stageMode == EStageMode.Osu)
                {
                    stageInfo = new StageInfo_Osu();
                }
                else if (stageMode == EStageMode.Audition)
                {
                    stageInfo = new StageInfo_Audition();
                }
                else if (stageMode == EStageMode.Rhythm)
                {
                    stageInfo = new StageInfo_Rhythm();
                }
                else if (stageMode == EStageMode.HeartBeats)
                {
                    stageInfo = new StageInfo_Rhythm();
                }

                if (stageInfo != null)
                {
                    stageInfo.LoadStageInfo(stagePath, stageLevel);
                }
            }

            return stageInfo;
        }

        void _CheckTaikoFeature(StageInfo_Base[] arStageInfo, TestDetail detailInfo)
        {
            string[] arFilterFeature = new string[] { "CONTENT", "BPM", "MEASURE", "OFFSET", 
                "KSPEED", "MATCHTIME", "DANGCE", "SHOWTIME", "ROUNDHEAD", "ROUNDEXTRA", "PERFECTSCORE" };
            foreach (string filterFeature in arFilterFeature)
            {
                bool bCheckEqual = true;
                TestFeature testFeature = new TestFeature();
                testFeature.m_strFeature = filterFeature;

                for (EStageLevel stageLevel = EStageLevel.Easy; stageLevel < EStageLevel.Max; ++stageLevel)
                {
                    if (arStageInfo[(int)stageLevel] != null)
                    {
                        StageInfo_Taiko taikoStage = (StageInfo_Taiko)arStageInfo[(int)stageLevel];

                        if (testFeature.m_strFeature == "CONTENT")
                        {
                            string strValue = "无非法符号";
                            if (arStageInfo[(int)stageLevel].m_bInvalidFlag)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidFlag);
                                strValue = "含非法符号";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "BPM")
                        {
                            testFeature.m_strValue[(int)stageLevel] = taikoStage.mBPM.ToString();
                        }
                        else if (testFeature.m_strFeature == "MEASURE")
                        {
                            string strValue = taikoStage.mBeatN.ToString() + "/" + taikoStage.mBeatD.ToString();
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "OFFSET")
                        {
                            testFeature.m_strValue[(int)stageLevel] = taikoStage.mOffset.ToString();
                        }
                        else if (testFeature.m_strFeature == "KSPEED")
                        {
                            testFeature.m_strValue[(int)stageLevel] = taikoStage.mKSpeed.ToString();
                        }
                        else if (testFeature.m_strFeature == "MATCHTIME")
                        {
                            string strValue = taikoStage.mMatchTime.ToString();
                            if (taikoStage.mMatchTime < 10f)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidValue);
                                strValue += "(?)";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "DANGCE")
                        {
                            testFeature.m_strValue[(int)stageLevel] = taikoStage.mDanceTime.ToString();
                        }
                        else if (testFeature.m_strFeature == "SHOWTIME")
                        {
                            string strValue = "";
                            foreach (CTaiguShowTime showTime in taikoStage.ShowTimeList)
                            {
                                strValue += "<";
                                strValue += showTime.BeginTime.ToString();
                                strValue += ",";
                                strValue += showTime.EndTime.ToString();
                                strValue += ">";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "ROUNDHEAD")
                        {
                            bool bExisted = false;
                            bool bInvalid = false;
                            Dictionary<int, string> allNode = new Dictionary<int, string>();
                            foreach (string nodeHead in taikoStage.NodeHeadList)
                            {
                                int nodeNo = 0;
                                int.TryParse(nodeHead, out nodeNo);
                                if (nodeNo == 0 || nodeHead.Length != 3)
                                {
                                    bInvalid = true;
                                }

                                if (nodeNo != 0)
                                {
                                    if (allNode.ContainsKey(nodeNo))
                                    {
                                        bExisted = true;
                                    }
                                    else
                                    {
                                        allNode.Add(nodeNo, nodeHead);
                                    }
                                }

                                if (bInvalid && bExisted)
                                {
                                    break;
                                }
                            }

                            string strValue = (bInvalid ? "有错误" : "无错误");
                            strValue += "/";
                            strValue += (bExisted ? "有重复" : "无重复");
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "ROUNDEXTRA")
                        {
                            bCheckEqual = false;

                            string strValue = taikoStage.NodeHeadList[taikoStage.NodeHeadList.Count - 1];
                            strValue += "/";
                            strValue += taikoStage.NodeHeadList.Count;
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "PERFECTSCORE")
                        {
                            bCheckEqual = false;
                            testFeature.m_strValue[(int)stageLevel] = taikoStage.MaxScore(stageLevel).ToString();
                        }
                    }
                    else
                    {
                        CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.FileMissing);
                        testFeature.m_strValue[(int)stageLevel] = "--";
                    }
                }

                if (bCheckEqual)
                {
                    if (testFeature.m_strValue[(int)EStageLevel.Easy] != testFeature.m_strValue[(int)EStageLevel.Normal]
                        || testFeature.m_strValue[(int)EStageLevel.Easy] != testFeature.m_strValue[(int)EStageLevel.Hard]
                        || testFeature.m_strValue[(int)EStageLevel.Normal] != testFeature.m_strValue[(int)EStageLevel.Hard])
                    {
                        CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.NoEqualValue);
                    }
                }

                CommonFunc.MergeTestError(ref detailInfo.m_nTestError, testFeature.m_nErrorNo);
                detailInfo.m_TestFeatures.Add(testFeature);
            }
        }

        void _CheckTraditionFeature(StageInfo_Base[] arStageInfo, TestDetail detailInfo)
        {
            string[] arFilterFeature = new string[] { "CONTENT", "BPM", "MEASURE", "OFFSET", 
                "KSPEED", "MATCHTIME", "DANGCE", "SHOWTIME", "ROUNDHEAD", "ROUNDEXTRA", "PERFECTSCORE" };
            foreach (string filterFeature in arFilterFeature)
            {
                bool bCheckEqual = true;
                TestFeature testFeature = new TestFeature();
                testFeature.m_strFeature = filterFeature;

                for (EStageLevel stageLevel = EStageLevel.Easy; stageLevel < EStageLevel.Max; ++stageLevel)
                {
                    if (arStageInfo[(int)stageLevel] != null)
                    {
                        StageInfo_Tradition traditionStage = (StageInfo_Tradition)arStageInfo[(int)stageLevel];

                        if (testFeature.m_strFeature == "CONTENT")
                        {
                            string strValue = "无非法符号";
                            if (arStageInfo[(int)stageLevel].m_bInvalidFlag)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidFlag);
                                strValue = "含非法符号";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "BPM")
                        {
                            testFeature.m_strValue[(int)stageLevel] = traditionStage.mBPM.ToString();
                        }
                        else if (testFeature.m_strFeature == "MEASURE")
                        {
                            string strValue = traditionStage.mBeatN.ToString() + "/" + traditionStage.mBeatD.ToString();
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "OFFSET")
                        {
                            testFeature.m_strValue[(int)stageLevel] = traditionStage.mOffset.ToString();
                        }
                        else if (testFeature.m_strFeature == "KSPEED")
                        {
                            testFeature.m_strValue[(int)stageLevel] = traditionStage.mKSpeed.ToString();
                        }
                        else if (testFeature.m_strFeature == "MATCHTIME")
                        {
                            string strValue = traditionStage.mMatchTime.ToString();
                            if (traditionStage.mMatchTime < 10f)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidValue);
                                strValue += "(?)";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "DANGCE")
                        {
                            testFeature.m_strValue[(int)stageLevel] = traditionStage.mDanceTime.ToString();
                        }
                        else if (testFeature.m_strFeature == "SHOWTIME")
                        {
                            string strValue = "";
                            foreach (CTranditionShowTime showTime in traditionStage.ShowTimeList)
                            {
                                strValue += "<";
                                strValue += showTime.BeginTime.ToString();
                                strValue += ",";
                                strValue += showTime.EndTime.ToString();
                                strValue += ">";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "ROUNDHEAD")
                        {
                            bool bExisted = false;
                            bool bInvalid = false;
                            Dictionary<int, string> allNode = new Dictionary<int, string>();
                            foreach (string nodeHead in traditionStage.NodeHeadList)
                            {
                                int nodeNo = 0;
                                int.TryParse(nodeHead, out nodeNo);
                                if (nodeNo == 0 || nodeHead.Length != 4)
                                {
                                    bInvalid = true;
                                }

                                if (nodeNo != 0)
                                {
                                    if (allNode.ContainsKey(nodeNo))
                                    {
                                        bExisted = true;
                                    }
                                    else
                                    {
                                        allNode.Add(nodeNo, nodeHead);
                                    }
                                }

                                if (bInvalid && bExisted)
                                {
                                    break;
                                }
                            }

                            string strValue = (bInvalid ? "有错误" : "无错误");
                            strValue += "/";
                            strValue += (bExisted ? "有重复" : "无重复");
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "ROUNDEXTRA")
                        {
                            bCheckEqual = false;

                            string strValue = traditionStage.NodeHeadList[traditionStage.NodeHeadList.Count - 1];
                            strValue += "/";
                            strValue += traditionStage.NodeHeadList.Count;
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "PERFECTSCORE")
                        {
                            bCheckEqual = false;
                            testFeature.m_strValue[(int)stageLevel] = traditionStage.MaxScore().ToString();
                        }
                    }
                    else
                    {
                        CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.FileMissing);
                        testFeature.m_strValue[(int)stageLevel] = "--";
                    }
                }

                if (bCheckEqual)
                {
                    if (testFeature.m_strValue[(int)EStageLevel.Easy] != testFeature.m_strValue[(int)EStageLevel.Normal]
                        || testFeature.m_strValue[(int)EStageLevel.Easy] != testFeature.m_strValue[(int)EStageLevel.Hard]
                        || testFeature.m_strValue[(int)EStageLevel.Normal] != testFeature.m_strValue[(int)EStageLevel.Hard])
                    {
                        CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.NoEqualValue);
                    }
                }

                CommonFunc.MergeTestError(ref detailInfo.m_nTestError, testFeature.m_nErrorNo);
                detailInfo.m_TestFeatures.Add(testFeature);
            }
        }

        void _CheckOsuFeature(StageInfo_Base[] arStageInfo, TestDetail detailInfo)
        {
            string[] arFilterFeature = new string[] { "CONTENT", "BPM", "MEASURE", "OFFSET", "MATCHTIME", "SHOWROUND", "PERFECTSCORE" };
            foreach (string filterFeature in arFilterFeature)
            {
                TestFeature testFeature = new TestFeature();
                testFeature.m_strFeature = filterFeature;

                bool bTestEqual = true;

                for (EStageLevel stageLevel = EStageLevel.Easy; stageLevel < EStageLevel.Max; ++stageLevel)
                {
                    if (arStageInfo[(int)stageLevel] != null)
                    {
                        StageInfo_Osu osuStage = (StageInfo_Osu)arStageInfo[(int)stageLevel];

                        if (testFeature.m_strFeature == "CONTENT")
                        {
                            string strValue = "无非法符号";
                            if (arStageInfo[(int)stageLevel].m_bInvalidFlag)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidFlag);
                                strValue = "含非法符号";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "BPM")
                        {
                            string strValue = osuStage.mBPM.ToString();
                            if (osuStage.mBPM == 0f)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidValue);
                                strValue += "(?)";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "MEASURE")
                        {
                            string strValue = osuStage.mBeatN.ToString() + "/-";
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "OFFSET")
                        {
                            string strValue = osuStage.mOffset.ToString();
                            if (osuStage.mOffset == 0f)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidValue);
                                strValue += "(?)";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "MATCHTIME")
                        {
                            string strValue = osuStage.mMatchTime.ToString();
                            if (osuStage.mMatchTime < 10f)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidValue);
                                strValue += "(?)";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "SHOWROUND")
                        {
                            string strValue = "";
                            if (osuStage.mShowtimeRound != null)
                            {
                                foreach (int showRound in osuStage.mShowtimeRound)
                                {
                                    strValue += "<";
                                    strValue += showRound.ToString();
                                    strValue += ">";
                                }
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "PERFECTSCORE")
                        {
                            bTestEqual = false;
                            testFeature.m_strValue[(int)stageLevel] = osuStage.MaxScore().ToString();
                        }
                    }
                    else
                    {
                        CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.FileMissing);
                        testFeature.m_strValue[(int)stageLevel] = "--";
                    }
                }

                if (bTestEqual)
                {
                    if (testFeature.m_strValue[(int)EStageLevel.Easy] != testFeature.m_strValue[(int)EStageLevel.Normal]
                        || testFeature.m_strValue[(int)EStageLevel.Easy] != testFeature.m_strValue[(int)EStageLevel.Hard]
                        || testFeature.m_strValue[(int)EStageLevel.Normal] != testFeature.m_strValue[(int)EStageLevel.Hard])
                    {
                        CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.NoEqualValue);
                    }
                }

                CommonFunc.MergeTestError(ref detailInfo.m_nTestError, testFeature.m_nErrorNo);
                detailInfo.m_TestFeatures.Add(testFeature);
            }
        }

        void _CheckRhythmFeature(StageInfo_Base[] arStageInfo, TestDetail detailInfo)
        {
            string[] arFilterFeature = new string[] { "CONTENT", "BPM", "MEASURE", "OFFSET", 
                "KSPEED", "MATCHTIME", "DANGCE", "SHOWTIME", "ROUNDHEAD", "ROUNDEXTRA", "PERFECTSCORE"};
            foreach (string filterFeature in arFilterFeature)
            {
                bool bCheckEqual = true;
                TestFeature testFeature = new TestFeature();
                testFeature.m_strFeature = filterFeature;

                for (EStageLevel stageLevel = EStageLevel.Easy; stageLevel < EStageLevel.Max; ++stageLevel)
                {
                    if (arStageInfo[(int)stageLevel] != null)
                    {
                        StageInfo_Rhythm rhythmStage = (StageInfo_Rhythm)arStageInfo[(int)stageLevel];

                        if (testFeature.m_strFeature == "CONTENT")
                        {
                            string strValue = "无非法符号";
                            if (arStageInfo[(int)stageLevel].m_bInvalidFlag)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidFlag);
                                strValue = "含非法符号";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "BPM")
                        {
                            testFeature.m_strValue[(int)stageLevel] = rhythmStage.mBPM.ToString();
                        }
                        else if (testFeature.m_strFeature == "MEASURE")
                        {
                            string strValue = rhythmStage.mBeatN.ToString() + "/" + rhythmStage.mBeatD.ToString();
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "OFFSET")
                        {
                            testFeature.m_strValue[(int)stageLevel] = rhythmStage.mOffset.ToString();
                        }
                        else if (testFeature.m_strFeature == "KSPEED")
                        {
                            testFeature.m_strValue[(int)stageLevel] = rhythmStage.mKSpeed.ToString();
                        }
                        else if (testFeature.m_strFeature == "MATCHTIME")
                        {
                            string strValue = rhythmStage.mMatchTime.ToString();
                            if (rhythmStage.mMatchTime < 10f)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidValue);
                                strValue += "(?)";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "DANGCE")
                        {
                            testFeature.m_strValue[(int)stageLevel] = rhythmStage.mDanceTime.ToString();
                        }
                        else if (testFeature.m_strFeature == "SHOWTIME")
                        {
                            string strValue = "";
                            foreach (CRhythmShowTime showTime in rhythmStage.ShowTimeList)
                            {
                                strValue += "<";
                                strValue += showTime.BeginTime.ToString();
                                strValue += ",";
                                strValue += showTime.EndTime.ToString();
                                strValue += ">";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "ROUNDHEAD")
                        {
                            bool bExisted = false;
                            bool bInvalid = false;
                            Dictionary<int, string> allNode = new Dictionary<int, string>();
                            foreach (string nodeHead in rhythmStage.NodeHeadList)
                            {
                                int nodeNo = 0;
                                int.TryParse(nodeHead, out nodeNo);
                                if (nodeNo == 0 || nodeHead.Length != 3)
                                {
                                    bInvalid = true;
                                }

                                if (nodeNo != 0)
                                {
                                    if (allNode.ContainsKey(nodeNo))
                                    {
                                        bExisted = true;
                                    }
                                    else
                                    {
                                        allNode.Add(nodeNo, nodeHead);
                                    }
                                }

                                if (bInvalid && bExisted)
                                {
                                    break;
                                }
                            }

                            string strValue = (bInvalid ? "有错误" : "无错误");
                            strValue += "/";
                            strValue += (bExisted ? "有重复" : "无重复");
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "ROUNDEXTRA")
                        {
                            bCheckEqual = false;

                            string strValue = rhythmStage.NodeHeadList[rhythmStage.NodeHeadList.Count - 1];
                            strValue += "/";
                            strValue += rhythmStage.NodeHeadList.Count;
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "PERFECTSCORE")
                        {
                            bCheckEqual = false;
                            testFeature.m_strValue[(int)stageLevel] = rhythmStage.MaxScore().ToString();
                        }
                    }
                    else
                    {
                        CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.FileMissing);
                        testFeature.m_strValue[(int)stageLevel] = "--";
                    }
                }

                if (bCheckEqual)
                {
                    if (testFeature.m_strValue[(int)EStageLevel.Easy] != testFeature.m_strValue[(int)EStageLevel.Normal]
                        || testFeature.m_strValue[(int)EStageLevel.Easy] != testFeature.m_strValue[(int)EStageLevel.Hard]
                        || testFeature.m_strValue[(int)EStageLevel.Normal] != testFeature.m_strValue[(int)EStageLevel.Hard])
                    {
                        CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.NoEqualValue);
                    }
                }

                CommonFunc.MergeTestError(ref detailInfo.m_nTestError, testFeature.m_nErrorNo);
                detailInfo.m_TestFeatures.Add(testFeature);
            }
        }


        void _CheckAuditionFeature(StageInfo_Base[] arStageInfo, TestDetail detailInfo)
        {
            string[] arFilterFeature = new string[] { "CONTENT", "BPM", "MEASURE", "OFFSET", 
                "KSPEED", "MATCHTIME", "DANGCE", "SHOWTIME", "ROUNDHEAD", "ROUNDEXTRA", "PERFECTSCORE"};
            foreach (string filterFeature in arFilterFeature)
            {
                bool bCheckEqual = true;
                TestFeature testFeature = new TestFeature();
                testFeature.m_strFeature = filterFeature;

                for (EStageLevel stageLevel = EStageLevel.Easy; stageLevel < EStageLevel.Max; ++stageLevel)
                {
                    if (arStageInfo[(int)stageLevel] != null)
                    {
                        StageInfo_Audition auditionStage = (StageInfo_Audition)arStageInfo[(int)stageLevel];

                        if (testFeature.m_strFeature == "CONTENT")
                        {
                            string strValue = "无非法符号";
                            if (arStageInfo[(int)stageLevel].m_bInvalidFlag)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidFlag);
                                strValue = "含非法符号";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "BPM")
                        {
                            testFeature.m_strValue[(int)stageLevel] = auditionStage.mBPM.ToString();
                        }
                        else if (testFeature.m_strFeature == "MEASURE")
                        {
                            string strValue = auditionStage.mBeatN.ToString() + "/" + auditionStage.mBeatD.ToString();
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "OFFSET")
                        {
                            testFeature.m_strValue[(int)stageLevel] = auditionStage.mOffset.ToString();
                        }
                        else if (testFeature.m_strFeature == "KSPEED")
                        {
                            testFeature.m_strValue[(int)stageLevel] = auditionStage.mKSpeed.ToString();
                        }
                        else if (testFeature.m_strFeature == "MATCHTIME")
                        {
                            string strValue = auditionStage.mMatchTime.ToString();
                            if (auditionStage.mMatchTime < 10f)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidValue);
                                strValue += "(?)";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "DANGCE")
                        {
                            testFeature.m_strValue[(int)stageLevel] = auditionStage.mDanceTime.ToString();
                        }
                        else if (testFeature.m_strFeature == "SHOWTIME")
                        {
                            string strValue = "";
                            foreach (AuditionShowTime showTime in auditionStage.ShowTimeList)
                            {
                                strValue += "<";
                                strValue += showTime.BeginTime.ToString();
                                strValue += ",";
                                strValue += showTime.EndTime.ToString();
                                strValue += ">";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "ROUNDHEAD")
                        {
                            bool bExisted = false;
                            bool bInvalid = false;
                            Dictionary<int, string> allNode = new Dictionary<int, string>();
                            foreach (string nodeHead in auditionStage.NodeHeadList)
                            {
                                int nodeNo = 0;
                                int.TryParse(nodeHead, out nodeNo);
                                if (nodeNo == 0 || nodeHead.Length != 3)
                                {
                                    bInvalid = true;
                                }

                                if (nodeNo != 0)
                                {
                                    if (allNode.ContainsKey(nodeNo))
                                    {
                                        bExisted = true;
                                    }
                                    else
                                    {
                                        allNode.Add(nodeNo, nodeHead);
                                    }
                                }

                                if (bInvalid && bExisted)
                                {
                                    break;
                                }
                            }

                            string strValue = (bInvalid ? "有错误" : "无错误");
                            strValue += "/";
                            strValue += (bExisted ? "有重复" : "无重复");
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "ROUNDEXTRA")
                        {
                            bCheckEqual = false;

                            string strValue = auditionStage.NodeHeadList[auditionStage.NodeHeadList.Count - 1];
                            strValue += "/";
                            strValue += auditionStage.NodeHeadList.Count;
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "PERFECTSCORE")
                        {
                            testFeature.m_strValue[(int)stageLevel] = auditionStage.MaxScore().ToString();
                            bCheckEqual = false;
                        }
                    }
                    else
                    {
                        CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.FileMissing);
                        testFeature.m_strValue[(int)stageLevel] = "--";
                    }
                }

                if (bCheckEqual)
                {
                    if (testFeature.m_strValue[(int)EStageLevel.Easy] != testFeature.m_strValue[(int)EStageLevel.Normal]
                        || testFeature.m_strValue[(int)EStageLevel.Easy] != testFeature.m_strValue[(int)EStageLevel.Hard]
                        || testFeature.m_strValue[(int)EStageLevel.Normal] != testFeature.m_strValue[(int)EStageLevel.Hard])
                    {
                        CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.NoEqualValue);
                    }
                }

                CommonFunc.MergeTestError(ref detailInfo.m_nTestError, testFeature.m_nErrorNo);
                detailInfo.m_TestFeatures.Add(testFeature);
            }

            // 检测错误：
            for (EStageLevel stageLevel = EStageLevel.Easy; stageLevel < EStageLevel.Max; ++stageLevel)
            {
                if (arStageInfo[(int)stageLevel] != null)
                {
                    StageInfo_Audition auditionStage = (StageInfo_Audition)arStageInfo[(int)stageLevel];

                    CommonFunc.MergeTestError(ref detailInfo.m_nTestError, auditionStage.SpecialTest());
                }
            }
        }

        void _CheckHeartBeatsFeature(StageInfo_Base[] arStageInfo, TestDetail detailInfo)
        {
            string[] arFilterFeature = new string[] { "CONTENT", "BPM", "MEASURE", "OFFSET", 
                "KSPEED", "MATCHTIME", "DANGCE", "SHOWTIME", "ROUNDHEAD", "ROUNDEXTRA", "PERFECTSCORE"};
            foreach (string filterFeature in arFilterFeature)
            {
                bool bCheckEqual = true;
                TestFeature testFeature = new TestFeature();
                testFeature.m_strFeature = filterFeature;

                for (EStageLevel stageLevel = EStageLevel.Easy; stageLevel < EStageLevel.Max; ++stageLevel)
                {
                    if (arStageInfo[(int)stageLevel] != null)
                    {
                        StageInfo_Taiko stage = (StageInfo_Taiko)arStageInfo[(int)stageLevel];

                        if (testFeature.m_strFeature == "CONTENT")
                        {
                            string strValue = "无非法符号";
                            if (arStageInfo[(int)stageLevel].m_bInvalidFlag)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidFlag);
                                strValue = "含非法符号";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "BPM")
                        {
                            testFeature.m_strValue[(int)stageLevel] = stage.mBPM.ToString();
                        }
                        else if (testFeature.m_strFeature == "MEASURE")
                        {
                            string strValue = stage.mBeatN.ToString() + "/" + stage.mBeatD.ToString();
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "OFFSET")
                        {
                            testFeature.m_strValue[(int)stageLevel] = stage.mOffset.ToString();
                        }
                        else if (testFeature.m_strFeature == "KSPEED")
                        {
                            testFeature.m_strValue[(int)stageLevel] = stage.mKSpeed.ToString();
                        }
                        else if (testFeature.m_strFeature == "MATCHTIME")
                        {
                            string strValue = stage.mMatchTime.ToString();
                            if (stage.mMatchTime < 10f)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidValue);
                                strValue += "(?)";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "DANGCE")
                        {
                            testFeature.m_strValue[(int)stageLevel] = stage.mDanceTime.ToString();
                        }
                        else if (testFeature.m_strFeature == "SHOWTIME")
                        {
                            string strValue = "";
                            foreach (CTaiguShowTime showTime in stage.ShowTimeList)
                            {
                                strValue += "<";
                                strValue += showTime.BeginTime.ToString();
                                strValue += ",";
                                strValue += showTime.EndTime.ToString();
                                strValue += ">";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "ROUNDHEAD")
                        {
                            bool bExisted = false;
                            bool bInvalid = false;
                            Dictionary<int, string> allNode = new Dictionary<int, string>();
                            foreach (string nodeHead in stage.NodeHeadList)
                            {
                                int nodeNo = 0;
                                int.TryParse(nodeHead, out nodeNo);
                                if (nodeNo == 0)
                                {
                                    bInvalid = true;
                                }

                                if (nodeNo != 0)
                                {
                                    if (allNode.ContainsKey(nodeNo))
                                    {
                                        bExisted = true;
                                    }
                                    else
                                    {
                                        allNode.Add(nodeNo, nodeHead);
                                    }
                                }

                                if (bInvalid && bExisted)
                                {
                                    break;
                                }
                            }

                            string strValue = (bInvalid ? "有错误" : "无错误");
                            strValue += "/";
                            strValue += (bExisted ? "有重复" : "无重复");
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "ROUNDEXTRA")
                        {
                            bCheckEqual = false;

                            string strValue = stage.NodeHeadList[stage.NodeHeadList.Count - 1];
                            strValue += "/";
                            strValue += stage.NodeHeadList.Count;
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "PERFECTSCORE")
                        {
                            bCheckEqual = false;
                            testFeature.m_strValue[(int)stageLevel] = "--";
                        }
                    }
                    else
                    {
                        CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.FileMissing);
                        testFeature.m_strValue[(int)stageLevel] = "--";
                    }
                }

                if (bCheckEqual)
                {
                    if (testFeature.m_strValue[(int)EStageLevel.Easy] != testFeature.m_strValue[(int)EStageLevel.Normal]
                        || testFeature.m_strValue[(int)EStageLevel.Easy] != testFeature.m_strValue[(int)EStageLevel.Hard]
                        || testFeature.m_strValue[(int)EStageLevel.Normal] != testFeature.m_strValue[(int)EStageLevel.Hard])
                    {
                        CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.NoEqualValue);
                    }
                }

                CommonFunc.MergeTestError(ref detailInfo.m_nTestError, testFeature.m_nErrorNo);
                detailInfo.m_TestFeatures.Add(testFeature);
            }
        }

        void _CheckRainbowFeature(StageInfo_Base[] arStageInfo, TestDetail detailInfo)
        {
            string[] arFilterFeature = new string[] { "CONTENT", "BPM", "MEASURE", "OFFSET", 
                "KSPEED", "MATCHTIME", "DANGCE", "SHOWTIME", "ROUNDHEAD", "ROUNDEXTRA", "PERFECTSCORE"};
            foreach (string filterFeature in arFilterFeature)
            {
                bool bCheckEqual = true;
                TestFeature testFeature = new TestFeature();
                testFeature.m_strFeature = filterFeature;

                for (EStageLevel stageLevel = EStageLevel.Easy; stageLevel < EStageLevel.Max; ++stageLevel)
                {
                    if (arStageInfo[(int)stageLevel] != null)
                    {
                        StageInfo_Rainbow rainbowStage = (StageInfo_Rainbow)arStageInfo[(int)stageLevel];

                        if (testFeature.m_strFeature == "CONTENT")
                        {
                            string strValue = "无非法符号";
                            if (arStageInfo[(int)stageLevel].m_bInvalidFlag)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidFlag);
                                strValue = "含非法符号";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "BPM")
                        {
                            testFeature.m_strValue[(int)stageLevel] = rainbowStage.mBPM.ToString();
                        }
                        else if (testFeature.m_strFeature == "MEASURE")
                        {
                            string strValue = rainbowStage.mBeatN.ToString() + "/" + rainbowStage.mBeatD.ToString();
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "OFFSET")
                        {
                            testFeature.m_strValue[(int)stageLevel] = rainbowStage.mOffset.ToString();
                        }
                        else if (testFeature.m_strFeature == "KSPEED")
                        {
                            testFeature.m_strValue[(int)stageLevel] = rainbowStage.mKSpeed.ToString();
                        }
                        else if (testFeature.m_strFeature == "MATCHTIME")
                        {
                            string strValue = rainbowStage.mMatchTime.ToString();
                            if (rainbowStage.mMatchTime < 10f)
                            {
                                CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.InvalidValue);
                                strValue += "(?)";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "DANGCE")
                        {
                            testFeature.m_strValue[(int)stageLevel] = rainbowStage.mDanceTime.ToString();
                        }
                        else if (testFeature.m_strFeature == "SHOWTIME")
                        {
                            string strValue = "";
                            foreach (RainbowShowTime showTime in rainbowStage.ShowTimeList)
                            {
                                strValue += "<";
                                strValue += showTime.BeginTime.ToString();
                                strValue += ",";
                                strValue += showTime.EndTime.ToString();
                                strValue += ">";
                            }

                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "ROUNDHEAD")
                        {
                            bool bExisted = false;
                            bool bInvalid = false;
                            Dictionary<int, string> allNode = new Dictionary<int, string>();
                            foreach (string nodeHead in rainbowStage.NodeHeadList)
                            {
                                int nodeNo = 0;
                                int.TryParse(nodeHead, out nodeNo);
                                if (nodeNo == 0 || nodeHead.Length != 3)
                                {
                                    bInvalid = true;
                                }

                                if (nodeNo != 0)
                                {
                                    if (allNode.ContainsKey(nodeNo))
                                    {
                                        bExisted = true;
                                    }
                                    else
                                    {
                                        allNode.Add(nodeNo, nodeHead);
                                    }
                                }

                                if (bInvalid && bExisted)
                                {
                                    break;
                                }
                            }

                            string strValue = (bInvalid ? "有错误" : "无错误");
                            strValue += "/";
                            strValue += (bExisted ? "有重复" : "无重复");
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "ROUNDEXTRA")
                        {
                            bCheckEqual = false;

                            string strValue = rainbowStage.NodeHeadList[rainbowStage.NodeHeadList.Count - 1];
                            strValue += "/";
                            strValue += rainbowStage.NodeHeadList.Count;
                            testFeature.m_strValue[(int)stageLevel] = strValue;
                        }
                        else if (testFeature.m_strFeature == "PERFECTSCORE")
                        {
                            bCheckEqual = false;
                            testFeature.m_strValue[(int)stageLevel] = rainbowStage.MaxScore().ToString();
                        }
                    }
                    else
                    {
                        CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.FileMissing);
                        testFeature.m_strValue[(int)stageLevel] = "--";
                    }
                }

                if (bCheckEqual)
                {
                    if (testFeature.m_strValue[(int)EStageLevel.Easy] != testFeature.m_strValue[(int)EStageLevel.Normal]
                        || testFeature.m_strValue[(int)EStageLevel.Easy] != testFeature.m_strValue[(int)EStageLevel.Hard]
                        || testFeature.m_strValue[(int)EStageLevel.Normal] != testFeature.m_strValue[(int)EStageLevel.Hard])
                    {
                        CommonFunc.SetTestError(ref testFeature.m_nErrorNo, ETestError.NoEqualValue);
                    }
                }

                CommonFunc.MergeTestError(ref detailInfo.m_nTestError, testFeature.m_nErrorNo);
                detailInfo.m_TestFeatures.Add(testFeature);
            }

            // 检测错误：
            for (EStageLevel stageLevel = EStageLevel.Easy; stageLevel < EStageLevel.Max; ++stageLevel)
            {
                if (arStageInfo[(int)stageLevel] != null)
                {
                    StageInfo_Rainbow rainbowStage = (StageInfo_Rainbow)arStageInfo[(int)stageLevel];

                    CommonFunc.MergeTestError(ref detailInfo.m_nTestError, rainbowStage.SpecialTest());
                }
            }
        }

    }
}

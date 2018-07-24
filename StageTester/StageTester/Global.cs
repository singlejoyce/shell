using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StageTester
{
    public enum EStageMode
    {
        None,

        Taiko = 1,
        Tradition = 2,
        Osu = 3,
        Audition = 4,
        Rhythm = 5,

        SuperTaiko = 11,
        SuperTradition = 12,
        SuperOsu = 13,
        SuperAudition = 14,

        HeartBeats = 21,

        Max,
    }

    public enum EStageLevel
    {
        Easy,
        Normal,
        Hard,
        SuperEasy,
        SuperNormal,
        SuperHard,

        Max,
    }

    public enum ETestError
    {
        FileMissing = 0x0001,               // 文件丢失
        InvalidFlag = 0x0002,
        InvalidValue = 0x0004,
        NoEqualValue = 0x0008,
        EvenRoundNotZero = 0x0010,
        ShowTimeNotZero = 0x0020,
    }

    public static class CommonFunc
    {
        public static string GetStageName(EStageMode stageMode)
        {
            string stageName = "";

            if (EStageMode.Taiko == stageMode)
            {
                stageName = "经典模式";
            }
            else if (EStageMode.Tradition == stageMode)
            {
                stageName = "恋舞模式";
            }
            else if (EStageMode.Osu == stageMode)
            {
                stageName = "泡泡模式";
            }
            else if (EStageMode.Audition == stageMode)
            {
                stageName = "劲舞模式";
            }
            else if (EStageMode.Rhythm == stageMode)
            {
                stageName = "节奏模式";
            }
            else if (EStageMode.SuperTaiko == stageMode)
            {
                stageName = "超级经典模式";
            }
            else if (EStageMode.SuperTradition == stageMode)
            {
                stageName = "超级恋舞模式";
            }
            else if (EStageMode.SuperOsu == stageMode)
            {
                stageName = "超级泡泡模式";
            }
            else if (EStageMode.SuperAudition == stageMode)
            {
                stageName = "超级劲舞模式";
            }
            else if (EStageMode.HeartBeats == stageMode)
            {
                stageName = "心动模式";
            }
            return stageName;
        }

        public static string GetStageExtension(EStageMode stageMode, EStageLevel stageLevel, bool isSuper)
        {
            string stageExtension = ".";

            if (stageMode == EStageMode.Taiko)
            {
                if (isSuper)
                {
                    stageExtension += "sg";
                }
                else
                {
                    stageExtension += "tg";
                }
            }
            else if (stageMode == EStageMode.Tradition)
            {
                if (isSuper)
                {
                    stageExtension += "sd";
                }
                else
                {
                    stageExtension += "td";
                }
            }
            else if (stageMode == EStageMode.Osu)
            {
                if (isSuper)
                {
                    stageExtension += "so";
                }
                else
                {
                    stageExtension += "os";
                }
            }
            else if (EStageMode.Audition == stageMode)
            {
                if (isSuper)
                {
                    stageExtension += "su";
                }
                else
                {
                    stageExtension += "au";
                }
            }
            else if (stageMode == EStageMode.Rhythm)
            {
                stageExtension += "rh";
            }
            else if (EStageMode.HeartBeats == stageMode)
            {
                stageExtension += "hb";
            }

            if (stageLevel == EStageLevel.Easy)
            {
                stageExtension += "e";
            }
            else if (stageLevel == EStageLevel.Normal)
            {
                stageExtension += "n";
            }
            else if (stageLevel == EStageLevel.Hard)
            {
                stageExtension += "h";
            }
            else if (stageLevel == EStageLevel.SuperEasy)
            {
                stageExtension += "e";
            }
            else if (stageLevel == EStageLevel.SuperNormal)
            {
                stageExtension += "n";
            }
            else if (stageLevel == EStageLevel.SuperHard)
            {
                stageExtension += "h";
            }

            return stageExtension;
        }

        public static void SetTestError(ref int testError, ETestError errorFlag)
        {
            testError |= (int)errorFlag;
        }

        public static void MergeTestError(ref int testError, int mergeError)
        {
            testError |= mergeError;
        }

        public static bool HasTestError(int testError, ETestError errorFlag)
        {
            int res = (testError & (int)errorFlag);
            return (res != 0);
        }
    }

    public class TestDetail
    {
        public string m_strStageFile = "";
        public EStageMode m_eStageMode = EStageMode.None;
        public int m_nTestError = 0;

        public List<TestFeature> m_TestFeatures = new List<TestFeature>();

        public override string ToString()
        {
            string errorInfo = "";
            if (CommonFunc.HasTestError(m_nTestError, ETestError.FileMissing))
            {
                errorInfo += "<文件缺失>";
            }
            if (CommonFunc.HasTestError(m_nTestError, ETestError.InvalidFlag))
            {
                errorInfo += "<非法符号>";
            }
            if (CommonFunc.HasTestError(m_nTestError, ETestError.InvalidValue))
            {
                errorInfo += "<非法值>";
            }
            if (CommonFunc.HasTestError(m_nTestError, ETestError.NoEqualValue))
            {
                errorInfo += "<不一致值>";
            }
            if (CommonFunc.HasTestError(m_nTestError, ETestError.EvenRoundNotZero))
            {
                errorInfo += "<偶数节不为0>";
            }
            if (CommonFunc.HasTestError(m_nTestError, ETestError.ShowTimeNotZero))
            {
                errorInfo += "<Show Time不为0>";
            }

            string briefInfo = m_strStageFile + "\t";
            if (errorInfo.Length > 0)
            {
                briefInfo += errorInfo;
            }
            else
            {
                briefInfo += "Pass";
            }

            return briefInfo;
        }
    }

    public class TestFeature
    {
        public string m_strFeature = "";
        public int m_nErrorNo = 0;
        public string[] m_strValue = new string[(int)EStageLevel.Max];
    }
}

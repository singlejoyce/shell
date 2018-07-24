using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StageTester
{
    public enum RhythmBallType
    {
        Begin = 0,

        Click,
        DoubleClick,
        BevelTR,
        BevelBL,
        LongPress,
        DoubleLongPress,
        SlideByArc,
        Batter,

        End,
    }

    public class CRhythmShowTime
    {
        public int BeginTime = 0;
        public int EndTime = 0;
    }

    public class CRhythmRoundInfo
    {
        public int RoundNO = 0;
        public List<RhythmBallType> BallList = new List<RhythmBallType>();
    }

    public class StageInfo_Rhythm : StageInfo_Base
    {
        public string mMusicFile = "";

        public int mBeatN = 4;		//分子
        public int mBeatD = 4;		//分母

        public float mBPM = 0f;
        public float mOffset = 0f;
        public float mKSpeed = 1f;

        public float mMatchTime = 0f;
        public float mDanceTime = 0f;
        public List<int[]> mShowRounds = new List<int[]>();

        public List<string> NodeHeadList = new List<string>();
        public List<CRhythmRoundInfo> RoundInfoList = new List<CRhythmRoundInfo>();
        public List<CRhythmShowTime> ShowTimeList = new List<CRhythmShowTime>();

        public override void LoadStageInfo(string stagePath, EStageLevel stageLevel)
        {
            using (StreamReader sr = new StreamReader(stagePath, Encoding.GetEncoding("gb18030")))
            {
                char[] invalidFlag = { '：' };
                char[] trimStart = { ' ', '\t' };
                char[] trimEnd = { ' ', '\r', '\n', '\t' };
                Dictionary<int, string> roundNote = new Dictionary<int, string>();

                string stringLine = null;
                while ((stringLine = sr.ReadLine()) != null)
                {
                    if (stringLine != "")
                    {
                        if (!m_bInvalidFlag && ContainsFlag(stringLine, invalidFlag))
                        {
                            m_bInvalidFlag = true;
                        }

                        stringLine.TrimEnd(trimEnd);
                        stringLine.TrimStart(trimStart);

                        if (BeginWithFlag(stringLine, '#'))
                        {
                            StageTag tag = AnalyseTag(ref stringLine, '#');
                            if (tag == StageTag.Round)
                            {
                                stringLine = stringLine.Replace(" ", "");
                                string strHead = "";
                                string strNote = "";
                                SeparateString(stringLine, ':', ref strHead, ref strNote);
                                NodeHeadList.Add(strHead);

                                int nHead = Convert.ToInt32(strHead);
                                if (!roundNote.ContainsKey(nHead))
                                {
                                    roundNote.Add(nHead, strNote);
                                }
                                else
                                {
                                    DebugLog.Write("CRhythmStageInfo LoadStageInfo Error: Round NO is duplicate " + strHead, LogLevel.ERROR);
                                }
                            }
                            else if (tag == StageTag.PatEnd)
                            {
                                AnalysisRoundNote(roundNote);
                                roundNote.Clear();
                            }
                        }
                        else if (BeginWithFlag(stringLine, '@'))
                        {
                            stringLine = stringLine.Replace(" ", "");
                            string strHead = "";
                            string strBegin = "";
                            string strEnd = "";
                            SeparateString(stringLine, ':', ref strHead, ref strBegin, ref strEnd);

                            int nBegin = Convert.ToInt32(strBegin);
                            int nEnd = Convert.ToInt32(strEnd);
                            if (nBegin > 0 && nBegin < nEnd)
                            {
                                CRhythmShowTime showTime = new CRhythmShowTime();
                                showTime.BeginTime = nBegin;
                                showTime.EndTime = nEnd;
                                ShowTimeList.Add(showTime);
                            }
                        }
                        else
                        {
                            string key = "";
                            string value = "";
                            SeparateString(stringLine, ':', ref key, ref value);

                            key = key.ToUpper();
                            SetMatchValue(key, value);
                        }
                    }
                }

                sr.Close();
            }
        }

        void AnalysisRoundNote(Dictionary<int, string> roundNote)
        {
            int longPressCount = 0;
            int batterCount = 0;
            int endCount = 0;
            foreach (KeyValuePair<int, string> kvp in roundNote)
            {
                CRhythmRoundInfo roundInfo = new CRhythmRoundInfo();
                roundInfo.RoundNO = kvp.Key;
                string stringNote = kvp.Value;
                for (int index = 0; index < stringNote.Length; ++index)
                {
                    RhythmBallType ballType = (RhythmBallType)int.Parse(stringNote.Substring(index, 1));
                    if (IsBallValid(ballType) || ballType == RhythmBallType.Begin || ballType == RhythmBallType.End)
                    {
                        roundInfo.BallList.Add(ballType);
                        if (ballType == RhythmBallType.End)
                            endCount++;
                        else if (ballType == RhythmBallType.LongPress)
                            longPressCount++;
                        else if (ballType == RhythmBallType.Batter)
                            batterCount++;
                    }
                    else
                    {
                        DebugLog.Write("CRhythmStageInfo.AnalysisRoundNote Error: Ball type = " + ballType.ToString(), LogLevel.ERROR);
                    }
                }

                RoundInfoList.Add(roundInfo);
            }
            if (longPressCount + batterCount != endCount)
            {
                DebugLog.Write("CRhythmStageInfo.AnalysisRoundNote Error: stage(" + mMusicFile + ") "
                + "LongPressCount(" + longPressCount + ") + BatterCount(" + batterCount + ") != EndCount(" + endCount + ")",
                    LogLevel.ERROR);
            }
        }

        void SetMatchValue(string key, string value)
        {
            if (string.Compare(key, "TITLE") == 0)
            {
                mMusicFile = value;
            }
            else if (string.Compare(key, "BPM") == 0)
            {
                mBPM = Convert.ToSingle(value);
            }
            else if (string.Compare(key, "MEASURE") == 0)
            {
                string beatN = "";
                string beatD = "";
                SeparateString(value, '/', ref beatN, ref beatD);

                Int32.TryParse(beatN, out mBeatN);
                Int32.TryParse(beatD, out mBeatD);
            }
            else if (string.Compare(key, "OFFSET") == 0)
            {
                mOffset = Convert.ToSingle(value);
            }
            else if (string.Compare(key, "KSPEED") == 0)
            {
                mKSpeed = Convert.ToSingle(value);
            }
            else if (string.Compare(key, "MATCHTIME") == 0)
            {
                mMatchTime = Convert.ToSingle(value);
            }
            else if (string.Compare(key, "DANGCE") == 0)
            {
                mDanceTime = Convert.ToSingle(value);
            }
            else if (key.StartsWith("SHOWTIME"))
            {
                string beginRound = "";
                string endRound = "";
                SeparateString(value, '/', ref beginRound, ref endRound);

                int[] showRound = new int[2];
                if (Int32.TryParse(beginRound, out showRound[0]) && Int32.TryParse(endRound, out showRound[1]))
                {
                    mShowRounds.Add(showRound);
                }
            }
        }

        bool IsBallValid(RhythmBallType nBallType)
        {
            return (nBallType > RhythmBallType.Begin && nBallType < RhythmBallType.End
                && nBallType != RhythmBallType.SlideByArc && nBallType != RhythmBallType.DoubleLongPress);
            // 去除SlideByArc和DoubleLongPress的两种Ball，降低模式难度 2017-08-03
        }

        private int GetRankBaseScore(RhythmBallType ballType, BeatResultRank rank)
        {
            int[] arrScoreDefault = 
            {
			    0,			// miss
			    250,		// bad
			    300,		// good
			    400,		// cool
			    500,		// perfect
            };

            int[] arrScoreDoubleClick = 
            {
			    0,			// miss
			    300,		// bad
			    400,		// good
			    600,		// cool
			    800,		// perfect
		    };

            int[] arrScoreBevelTR = 
            {
			    0,			// miss
			    300,		// bad
			    400,		// good
			    500,		// cool
			    600,		// perfect
		    };

            int[] arrScoreBevelBL = 
            {
			    0,			// miss
			    300,		// bad
			    400,		// good
			    500,		// cool
			    600,		// perfect
    		};

            int[] arrScoreDoubleLongPress = 
            {
			    0,			// miss
			    300,		// bad
			    400,		// good
			    600,		// cool
			    800,		// perfect
    		};

            int[] arrScoreBatter = 
            {
			    0,			// miss
			    300,		// bad
			    400,		// good
			    600,		// cool
			    800,		// perfect
    		};

            switch (ballType)
            {
                case RhythmBallType.DoubleClick:
                    return arrScoreDoubleClick[(int)rank - 1];
                case RhythmBallType.BevelTR:
                    return arrScoreBevelTR[(int)rank - 1];
                case RhythmBallType.BevelBL:
                    return arrScoreBevelBL[(int)rank - 1];
                case RhythmBallType.DoubleLongPress:
                    return arrScoreDoubleLongPress[(int)rank - 1];
                case RhythmBallType.Batter:
                    return arrScoreBatter[(int)rank - 1];
                default:
                    return arrScoreDefault[(int)rank - 1];
            }
        }

        protected int GetBonus(RhythmBallType ballType)
        {
            if (RhythmBallType.DoubleClick == ballType
                || RhythmBallType.DoubleLongPress == ballType
                || RhythmBallType.BevelBL == ballType
                || RhythmBallType.BevelTR == ballType)
                return 15;
            else
                return 10;
        }


        public int MaxScore()
        {
            int combo = 0;
            int ret = 0;

            RhythmBallType prevBallType = RhythmBallType.Begin;

            foreach (CRhythmRoundInfo rinfo in RoundInfoList)
            {
                foreach (RhythmBallType ball in rinfo.BallList)
                {
                    if (RhythmBallType.Begin == ball)
                        continue;

                    combo++;
                    RhythmBallType ballRealType = ball;
                    if (RhythmBallType.End == ballRealType)
                        ballRealType = prevBallType;

                    if (!IsBallValid(ballRealType))
                        continue; // 错误的文件格式

                    prevBallType = ball;

                    int comboLevel = (combo > 100 ? 10 : combo / 10);
                    int rankScore = GetRankBaseScore(ballRealType, BeatResultRank.Perfect);
                    int thisScore = rankScore + rankScore * comboLevel * GetBonus(ballRealType) / 100;

                    ret += thisScore;
                }
            }

            return ret;
        }

    }
}

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StageTester
{
    public class AuditionShowTime
	{
		public int BeginTime = 0;
		public int EndTime = 0;
	};

    public class AuditionBallInfo
    {
        public AuditionBallType m_BallType;
        public bool m_bIsReverse = false;
    }

    public class AuditionRoundInfo
    {
        public int RoundNO = 0;
        public List<AuditionBallInfo> BallList = new List<AuditionBallInfo>();
        public bool m_bAllEmpty = true;
    }

    public class StageInfo_Audition : StageInfo_Base
	{
		public int MaxBallNum
		{
			get
			{
				return 6;
			}
		}

        int[] mBasePercent = 
	        {
		        0,			// miss
		        30,			// bad
		        50,			// good
		        70,			// cool
		        100			// perfect
	        };



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
        public Dictionary<int, AuditionRoundInfo> RoundInfoMap = new Dictionary<int, AuditionRoundInfo>();
        public List<AuditionShowTime> ShowTimeList = new List<AuditionShowTime>();

        public override void LoadStageInfo(string stagePath, EStageLevel stageLevel)
		{
			using ( StreamReader sr = new StreamReader( stagePath, Encoding.GetEncoding( "gb18030" ) ) )
			{
				char[] invalidFlag = { '：' };
				char[] trimStart = { ' ', '\t' };
				char[] trimEnd = { ' ', '\r', '\n', '\t' };
				Dictionary<int, string> roundNote = new Dictionary<int, string>();

				string stringLine = null;
				while ( ( stringLine = sr.ReadLine() ) != null )
				{
					if ( stringLine != "" )
					{
                        // test every line is valid
						if ( !m_bInvalidFlag && ContainsFlag( stringLine, invalidFlag ) )
						{
							m_bInvalidFlag = true; // true is invalid.
						}

						stringLine.TrimEnd( trimEnd );
						stringLine.TrimStart( trimStart );

						if ( BeginWithFlag( stringLine, '#' ) )
						{
							StageTag tag = AnalyseTag( ref stringLine, '#' );
							if ( tag == StageTag.Round )
							{
								stringLine = stringLine.Replace( " ", "" );

								string strHead = "";
								string strNote = "";
								SeparateString( stringLine, ':', ref strHead, ref strNote );
								NodeHeadList.Add (strHead);

								int nHead = Convert.ToInt32( strHead );

								if ( !roundNote.ContainsKey( nHead ) )
								{
									roundNote.Add( nHead, strNote );
								}
                                else
                                {
                                    DebugLog.Write("CAuditionStageInfo LoadStageInfo Error: Round note duplicate " + nHead + "  " + stagePath, LogLevel.ERROR);
                                }
							}
							else if ( tag == StageTag.PatEnd )
							{ // add round end.
								AnalysisRoundNote( roundNote, stageLevel);
								roundNote.Clear();
							}
						}
						else if ( BeginWithFlag( stringLine, '@' ) )
						{
							//Show time
							stringLine = stringLine.Replace( " ", "" );
							string strHead = "";
							string strBegin = "";
							string strEnd = "";
							SeparateString( stringLine, ':', ref strHead, ref strBegin, ref strEnd );

							int nBegin = Convert.ToInt32( strBegin );
							int nEnd = Convert.ToInt32( strEnd );
							if ( nBegin > 0 && nBegin < nEnd )
							{
                                AuditionShowTime o = new AuditionShowTime();
								o.BeginTime = nBegin;
								o.EndTime = nEnd;
								ShowTimeList.Add( o );
							}
						}
						else
						{
							string key = "";
							string value = "";
							SeparateString( stringLine, ':', ref key, ref value );

							key = key.ToUpper();
							SetMatchValue( key, value );
						}
					}
				}

				sr.Close();
			}
		}

		void AnalysisRoundNote(Dictionary<int, string> roundNote, EStageLevel stageLevel)
		{
            foreach (KeyValuePair<int, string> kvp in roundNote)
            {
                AuditionRoundInfo roundInfo = new AuditionRoundInfo();
                roundInfo.RoundNO = kvp.Key;
                string stringNote = kvp.Value;
                bool IsReverse = false;
                for (int index = 0; index < stringNote.Length; ++index)
                {
                    string note = stringNote.Substring(index, 1);
                    AuditionBallInfo ballInfo = new AuditionBallInfo();
                    if (note[0] == '*')
                    {
                        IsReverse = true;

                        if (stageLevel > EStageLevel.Hard)
                        {
                            if (note.Contains("5"))
                            {
                                roundNote.Remove(kvp.Key);
                            }
                        }
                    }
                    else
                    {
                        int ballIndex = 0;
                        int.TryParse(note, out ballIndex);

                        AuditionBallType ballType = CAuditionBallType.GetBallType(ballIndex);
                        if (IsReverse)
                        {
                            ballInfo.m_bIsReverse = true;
                            IsReverse = false;
                        }
                        ballInfo.m_BallType = ballType;
                        roundInfo.BallList.Add(ballInfo);
                        if (CAuditionBallType.IsValid(ballType))
                        {
                            if (roundInfo.m_bAllEmpty)
                            {
                                roundInfo.m_bAllEmpty = false;
                            }
                        }
                    }
                }

                RoundInfoMap.Add(kvp.Key, roundInfo);
            }
        }

		public AuditionRoundInfo GetRoundInfoByIndex(int nIndex)
		{
			if ( !RoundInfoMap.ContainsKey( nIndex ) )
			{
				return null;
			}

			return RoundInfoMap[nIndex];
		}

		public void SetMatchValue(string key, string value)
		{
			if ( string.Compare( key, "TITLE" ) == 0 )
			{
				mMusicFile = value;
			}
			else if ( string.Compare( key, "BPM" ) == 0 )
			{
				mBPM = Convert.ToSingle( value );
			}
			else if ( string.Compare( key, "MEASURE" ) == 0 )
			{
				string beatN = "";
				string beatD = "";
				SeparateString( value, '/', ref beatN, ref beatD );

				Int32.TryParse( beatN, out mBeatN );
				Int32.TryParse( beatD, out mBeatD );
			}
			else if ( string.Compare( key, "OFFSET" ) == 0 )
			{
				mOffset = Convert.ToSingle( value );
			}
			else if ( string.Compare( key, "KSPEED" ) == 0 )
			{
				mKSpeed = Convert.ToSingle( value );
			}
			else if ( string.Compare( key, "MATCHTIME" ) == 0 )
			{
				mMatchTime = Convert.ToSingle( value );
			}
			else if ( string.Compare( key, "DANGCE" ) == 0 )
			{
				mDanceTime = Convert.ToSingle( value );
			}
			else if ( key.StartsWith( "SHOWTIME" ) )
			{
				string beginRound = "";
				string endRound = "";
				SeparateString( value, '/', ref beginRound, ref endRound );

				int[] showRound = new int[2];
				if ( Int32.TryParse( beginRound, out showRound[0] ) && Int32.TryParse( endRound, out showRound[1] ) )
				{
					mShowRounds.Add( showRound );
				}
			}
		}

        private bool IsShowTimeRound(int roundNo)
        {
            foreach (AuditionShowTime showTime in ShowTimeList)
            {
                if (roundNo >= showTime.BeginTime && roundNo <= showTime.EndTime)
                    return true;
            }

            return false;
        }
        
        public int SpecialTest()
        {
            int retValue = 0;
            foreach (KeyValuePair<int, AuditionRoundInfo> kvp in RoundInfoMap)
            {
                AuditionRoundInfo au = kvp.Value;
                if (au.RoundNO % 2 == 0)
                {
                    if (!au.m_bAllEmpty)
                        CommonFunc.SetTestError(ref retValue, ETestError.EvenRoundNotZero);
                        
                }
                else if (IsShowTimeRound(au.RoundNO))
                {
                    if (!au.m_bAllEmpty)
                        CommonFunc.SetTestError(ref retValue, ETestError.ShowTimeNotZero);
                }
                else
                {
                    if (!au.m_bAllEmpty)
                    { // 非空节不能存在0.
                        foreach (AuditionBallInfo bt in au.BallList)
                        {
                            if (bt.m_BallType == AuditionBallType.None)
                                CommonFunc.SetTestError(ref retValue, ETestError.InvalidValue);
                        }
                    }
                }
            }

            return retValue;
        }

        public int MaxScore()
        {
            int combo = 0;
            int ret = 0;
            const int mContinuousPBonus = 50;
            foreach (KeyValuePair<int, AuditionRoundInfo> rinfo in RoundInfoMap)
            {
                int ballCount = 0;
                foreach (AuditionBallInfo ballInfo in rinfo.Value.BallList)
                {
                    if (AuditionBallType.None < ballInfo.m_BallType && ballInfo.m_BallType < AuditionBallType.End)
                        ++ballCount;
                }

                if (0 == ballCount)
                    continue;

                int baseMark = 50 * ballCount * (ballCount - 1) + 500;

                int percentIndex = (int)BeatResultRank.Perfect - 1;
                int rankMark = baseMark * mBasePercent[percentIndex] / 100;
                int thisScore = rankMark + rankMark * ++combo * mContinuousPBonus / 100;

                ret += thisScore;
            }

            return ret;
        }

	}
}

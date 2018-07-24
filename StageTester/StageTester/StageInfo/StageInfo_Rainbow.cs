using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StageTester
{
    public class RainbowShowTime
	{
		public int BeginTime = 0;
		public int EndTime = 0;
	};

    public class RainbowRoundInfo
    {
        public int RoundNO = 0;
        public List<int> BallList = new List<int>();
        public bool m_bAllEmpty = true;
    }

    public class StageInfo_Rainbow : StageInfo_Base
	{
		public int MaxBallNum
		{
			get
			{
				return 6;
			}
		}

        int[] mBaseMark = 
	    {
		    0,			// miss
		    250,		// bad
		    300,		// good
		    400,		// cool
		    500			// perfect
	    };
        const int mComboBonus = 10;		// percent

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
        public Dictionary<int, RainbowRoundInfo> RoundInfoMap = new Dictionary<int, RainbowRoundInfo>();
        public List<RainbowShowTime> ShowTimeList = new List<RainbowShowTime>();

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
								AnalysisRoundNote( roundNote );
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
                                RainbowShowTime o = new RainbowShowTime();
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

		void AnalysisRoundNote(Dictionary<int, string> roundNote)
		{
            foreach (KeyValuePair<int, string> kvp in roundNote)
            {
                RainbowRoundInfo roundInfo = new RainbowRoundInfo();
                roundInfo.RoundNO = kvp.Key;
                string stringNote = kvp.Value;

                for (int index = 0; index < stringNote.Length; ++index)
                {
                    string note = stringNote.Substring(index, 1);
                    int ballIndex = 0;
                    int.TryParse(note, out ballIndex);

                    roundInfo.BallList.Add(ballIndex);
                }

                RoundInfoMap.Add(kvp.Key, roundInfo);
            }
        }

		public RainbowRoundInfo GetRoundInfoByIndex(int nIndex)
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
            foreach (RainbowShowTime showTime in ShowTimeList)
            {
                if (roundNo >= showTime.BeginTime && roundNo <= showTime.EndTime)
                    return true;
            }

            return false;
        }
        
        public int SpecialTest()
        {
            int retValue = 0;
            foreach (KeyValuePair<int, RainbowRoundInfo> kvp in RoundInfoMap)
            {
                RainbowRoundInfo au = kvp.Value;
                
                if (IsShowTimeRound(au.RoundNO))
                {
                    foreach (int ball in au.BallList)
                    {
                        if (0 != ball)
                            CommonFunc.SetTestError(ref retValue, ETestError.ShowTimeNotZero);
                    }
                }
                else
                {
                    foreach (int ball in au.BallList)
                    {
                        if (ball < 0 || ball > 7)
                            CommonFunc.SetTestError(ref retValue, ETestError.InvalidValue);
                    }
                }
            }

            return retValue;
        }

        public int MaxScore()
        {
            int ret = 0;
            int combo = 0;

            foreach (KeyValuePair<int, RainbowRoundInfo> kvp in RoundInfoMap)
            {
                RainbowRoundInfo rinfo = kvp.Value;
//                int ballcount = 0;
                foreach (int ball in rinfo.BallList)
                {
                    if (0 != ball)
                    {
                        combo++; // 多一次 Perfect.
                        int comboLevel = combo > 100 ? 10 : (combo / 10);
                        int markIndex = (int)BeatResultRank.Perfect - 1;
                        int rankMark = mBaseMark[markIndex];
                        int thisscore = rankMark + rankMark * comboLevel * mComboBonus / 100;

                        ret += thisscore;
                    }
                }


            }

            return ret;
        }

	}
}

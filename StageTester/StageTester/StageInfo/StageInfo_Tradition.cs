using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StageTester
{
	public class CTranditionShowTime
	{
		public int BeginTime = 0;
		public int EndTime = 0;
	};

	public class CTranditionalRoundInfo
	{
		public List<CTranditionalSubNote[]> BallList = new List<CTranditionalSubNote[]>();
	}

	public class CTranditionalSubNote
	{
		public bool Double = false;
		public int[] Position = new int[2];
	}

	public class StageInfo_Tradition : StageInfo_Base
	{
		public int MaxBallNum
		{
			get
			{
				return 6;
			}
		}

        const int mContinuousPBonus = 50;		// percent

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
		public Dictionary<int, CTranditionalRoundInfo> RoundInfoMap = new Dictionary<int, CTranditionalRoundInfo>();
		public List<CTranditionShowTime> ShowTimeList = new List<CTranditionShowTime>();

        public override void LoadStageInfo(string stagePath, EStageLevel stageLevel)
		{
			using ( StreamReader sr = new StreamReader( stagePath, Encoding.GetEncoding( "gb18030" ) ) )
			{
				char[] invalidFlag = { '：' };
				char[] trimStart = { ' ', '\t' };
				char[] trimEnd = { ' ', '\r', '\n', '\t' };
				Dictionary<int, string[]> roundNote = new Dictionary<int, string[]>();

				string stringLine = null;
				while ( ( stringLine = sr.ReadLine() ) != null )
				{
					if ( stringLine != "" )
					{
						if ( !m_bInvalidFlag && ContainsFlag( stringLine, invalidFlag ) )
						{
							m_bInvalidFlag = true;
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
								NodeHeadList.Add( strHead );

								int nHead = Convert.ToInt32( strHead );
								int nPos = nHead % 10;
								nHead = nHead / 10;
								nPos = ( nPos <= 1 ? 0 : 1 );

								if ( !roundNote.ContainsKey( nHead ) )
								{
									roundNote.Add( nHead, new string[2] );
								}

								if ( roundNote[nHead][nPos] == null )
								{
									roundNote[nHead][nPos] = strNote;
								}
								else
								{
									DebugLog.Write( "CTrandionStageInfo LoadStageInfo Error: Round note duplicate " + nHead + "  " + stagePath, LogLevel.ERROR );
								}
							}
							else if ( tag == StageTag.PatEnd )
							{
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
								CTranditionShowTime o = new CTranditionShowTime();
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

        void AnalysisRoundNote(Dictionary<int, string[]> roundNote, EStageLevel stageLevel)
		{
			foreach ( KeyValuePair<int, string[]> kvp in roundNote )
			{
				if ( kvp.Value[0].Length == MaxBallNum && kvp.Value[1].Length == MaxBallNum )
				{
					CTranditionalRoundInfo roundInfo = new CTranditionalRoundInfo();
					List<CTranditionalSubNote> LineNote = new List<CTranditionalSubNote>();

					for ( int x = 0; x < MaxBallNum; x++ )
					{
						for ( int y = 0; y < 2; y++ )
						{
							if ( kvp.Value[y][x] == '8' )
							{
								CTranditionalSubNote Note = new CTranditionalSubNote();
								Note.Position[0] = x;
								Note.Position[1] = y;
								LineNote.Add( Note );
							}
							else if ( kvp.Value[y][x] == '9' )
							{
								CTranditionalSubNote Note = new CTranditionalSubNote();
								Note.Position[0] = x;
								Note.Position[1] = y;
								LineNote.Add( Note );
								roundInfo.BallList.Add( LineNote.ToArray() );
								LineNote.Clear();
							}
							else if ( kvp.Value[y][x] != '0' )
							{
								CTranditionalSubNote Note = new CTranditionalSubNote();
								Note.Position[0] = x;
								Note.Position[1] = y;
                                if(stageLevel < EStageLevel.SuperEasy)
                                {
                                    if (kvp.Value[y][x] != '1')
                                    {
                                        Note.Double = true;
                                    }
                                }
								LineNote.Add( Note );
								roundInfo.BallList.Add( LineNote.ToArray() );
								LineNote.Clear();
							}
						}
					}

					RoundInfoMap.Add( kvp.Key, roundInfo );
				}
			}
		}

		public CTranditionalRoundInfo GetRoundInfoByIndex(int nIndex)
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


        int _CalcuBaseMark(int ballCount)
        {
            int mark = 0;

            if (ballCount > 0)
            {
                mark = 50 * ballCount * (ballCount - 1) + 500;
            }

            return mark;
        }

        public int MaxScore()
        {
            int combo = 0;
            int ret = 0;

            foreach (KeyValuePair<int, CTranditionalRoundInfo> kvp in RoundInfoMap)
            {
                int cnt = 0;
                foreach (CTranditionalSubNote[] tsn in kvp.Value.BallList)
                {
                    if (tsn.Length == 1)
                        cnt += (tsn[0].Double ? 2 : 1);
                    else if (tsn.Length > 1)
                    {
                        foreach (CTranditionalSubNote t in tsn)
                        {
                            if (t.Position[0] < MaxBallNum && t.Position[0] >= 0
                                && t.Position[1] < MaxBallNum && t.Position[1] >= 0)
                            {
                                cnt++;
                            }
                        }
                    }
                }

                if (0 == cnt)
                    continue;

                combo++;

                // cnt:
                int baseMark = _CalcuBaseMark(cnt);
                int percentIndex = (int)BeatResultRank.Perfect - 1;
                int rankMark = baseMark * mBasePercent[percentIndex] / 100;
                int thisScore = rankMark + rankMark * combo * mContinuousPBonus / 100;

                ret += thisScore;
            }

            return ret;
        }
    }
}

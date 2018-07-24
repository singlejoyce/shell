using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StageTester
{
	public enum TaiguBallType
	{
		None,

		Red,
		Blue,
		RBMix,

		Max,
	};

	public class CTaiguShowTime
	{
		public int BeginTime = 0;
		public int EndTime = 0;
	}

	public class CTaiguRoundInfo
	{
		public List<TaiguBallType> BallList = new List<TaiguBallType>();
	}

	public class StageInfo_Taiko : StageInfo_Base
	{
        const int mComboBonus = 10;		// percent

        int[] mBaseMark = 
	    {
		    0,			// miss
		    250,		// bad
		    300,		// good
		    400,		// cool
		    500			// perfect
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
		public List<CTaiguRoundInfo> RoundInfoList = new List<CTaiguRoundInfo>();
		public List<CTaiguShowTime> ShowTimeList = new List<CTaiguShowTime>();

        public override void LoadStageInfo(string stagePath, EStageLevel stageLevel)
		{
			using ( StreamReader sr = new StreamReader( stagePath, Encoding.GetEncoding( "gb18030" ) ) )
			{
				char[] invalidFlag = { '：' };
				char[] trimStart = { ' ', '\t' };
				char[] trimEnd = { ' ', '\r', '\n', '\t' };
				List<string> roundNote = new List<string>();

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

								roundNote.Add( strNote );
							}
							else if ( tag == StageTag.PatEnd )
							{
								AnalysisRoundNote( roundNote, stageLevel);
								roundNote.Clear();
							}
						}
						else if ( BeginWithFlag( stringLine, '@' ) )
						{
							stringLine = stringLine.Replace( " ", "" );
							string strHead = "";
							string strBegin = "";
							string strEnd = "";
							SeparateString( stringLine, ':', ref strHead, ref strBegin, ref strEnd );

							int nBegin = Convert.ToInt32( strBegin );
							int nEnd = Convert.ToInt32( strEnd );
							if ( nBegin > 0 && nBegin < nEnd )
							{
								CTaiguShowTime showTime = new CTaiguShowTime();
								showTime.BeginTime = nBegin;
								showTime.EndTime = nEnd;
								ShowTimeList.Add( showTime );
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

        void AnalysisRoundNote(List<string> roundNote, EStageLevel stageLevel)
        {
            for (int i = 0; i < roundNote.Count; ++i)
            {
                string stringNote = roundNote[i];
                CTaiguRoundInfo roundInfo = new CTaiguRoundInfo();
                if (stageLevel > EStageLevel.Hard)
                {
                    for (int j = 0; j < stringNote.Length; j++)
                    {
                        if (stringNote[j] == '8')
                        {
                            if (!stringNote.Contains('9'))
                            {
                                roundNote.Remove(stringNote);
                            }
                            else
                            {
                                IncludeZero(j, stringNote, roundNote);
                            }
                        }
                    }
                }

                for (int index = 0; index < stringNote.Length; ++index)
                {
                    if (stringNote[index] == '1')
                    {
                        roundInfo.BallList.Add(TaiguBallType.Red);
                    }
                    else if (stringNote[index] == '2')
                    {
                        roundInfo.BallList.Add(TaiguBallType.Blue);
                    }
                    else if (stringNote[index] == '3')
                    {
                        roundInfo.BallList.Add(TaiguBallType.RBMix);
                    }
                    else
                    {
                        roundInfo.BallList.Add(TaiguBallType.None);
                    }
                }

                RoundInfoList.Add(roundInfo);
            }
        }

        public void IncludeZero(int j, string stringNote, List<string> roundNote)
        {
            int index = stringNote.IndexOf('9');
            if ((index - j) > 1)
            {
                for (int n = j + 1; n < index; n++)
                {
                    if (stringNote[n] != '0')
                    {
                        roundNote.Remove(stringNote);
                        break;
                    }
                }
            }
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


        public int MaxScore(EStageLevel stageLevel)
        {
            int combo = 0;
            int ret = 0;
            int comboLevel = 0;

            foreach (CTaiguRoundInfo rinfo in this.RoundInfoList)
            {
                foreach (TaiguBallType ball in rinfo.BallList)
                {
                    if (ball <= TaiguBallType.None || ball >= TaiguBallType.Max)
                        continue;

                    if(stageLevel > EStageLevel.Hard)
                    {
                        comboLevel = 10;
                    }
                    else
                    {
                        combo++; // 
                        comboLevel = (combo > 100 ? 10 : combo / 10);
                    }
                    int rankMark = mBaseMark[(int)BeatResultRank.Perfect - 1];
                    int thisScore = rankMark + rankMark * comboLevel * mComboBonus / 100;

                    ret += thisScore;
                }
            }

            return ret;
        }

	}
}

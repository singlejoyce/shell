using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StageTester
{
	public class StageInfo_Osu : StageInfo_Base
	{
		enum EOSU_SECTION
		{
			EOSU_SECTION_NON,
			EOSU_SECTION_HITOBJECTS,
			EOSU_SECTION_TIMEPOINT,
			EOSU_SECTION_DIFFICULTY,
			EOSU_SECTION_METADATA
		}

        int[,] mBaseMark =
	    {
		    {300,400,500},
		    {330,440,550},
		    {360,480,600},
		    {390,520,650},
		    {420,560,700},
		    {450,600,750},
		    {480,640,800},
		    {510,680,850},
		    {540,720,900},
		    {570,760,950},
		    {600,800,1000}
	    };

		public int mBeatN = 4;		//分子
		public int mBeatD = 4;		//分母

		public float mBPM = 0f;
		public float mOffset = 0f;

		public float mMatchTime = 0f;
		public int[] mShowtimeRound = null;

		public int m_nRoundCount = 1;
		public int m_nCurrentRoundOperatorCount = 0;

        public int m_nAllOperCount = 0;

        public override void LoadStageInfo(string stagePath, EStageLevel stageLevel)
		{
			using ( StreamReader sr = new StreamReader( stagePath, Encoding.GetEncoding( "gb18030" ) ) )
			{
				char[] invalidFlag = { '，' };
				char[] trimStart = { ' ', '\t' };
				char[] trimEnd = { ' ', '\r', '\n', '\t' };
				string stringLine = null;
				EOSU_SECTION curSection = EOSU_SECTION.EOSU_SECTION_NON;
				EOSU_SECTION Section = EOSU_SECTION.EOSU_SECTION_NON;
				while ( ( stringLine = sr.ReadLine() ) != null )
				{
					if ( !m_bInvalidFlag && ContainsFlag( stringLine, invalidFlag ) )
					{
						m_bInvalidFlag = true;
					}

					stringLine = stringLine.TrimEnd( trimEnd );
					stringLine = stringLine.TrimStart( trimStart );
					if ( IsComment( stringLine ) )
					{
						continue;
					}
					if ( GetSection( stringLine, ref Section ) )
					{
						curSection = Section;
					}
					else
					{
						if ( BeginWithNum( stringLine ) )
						{
							if ( Section == EOSU_SECTION.EOSU_SECTION_HITOBJECTS )
							{
								AddNewOperator( stringLine );
							}

							if ( Section == EOSU_SECTION.EOSU_SECTION_TIMEPOINT )
							{
								string[] splitResult = stringLine.Split( ',' );
								if ( splitResult.Length >= 3 )
								{
									float offset = 0;
									float.TryParse( splitResult[0], out offset );
									mOffset = offset / 1000;

									float bpm = 0;
									float.TryParse( splitResult[1], out bpm );
									mBPM = 60000 / bpm;

									int nbeat = 0;
									int.TryParse( splitResult[2], out nbeat );
									if ( nbeat == 3 )
									{
										mBeatN = 3;
									}
								}
							}
						}
						else
						{
							string strkey = null;
							string strvalue = null;
							if ( Section == EOSU_SECTION.EOSU_SECTION_METADATA )
							{
								SeparateString( stringLine, ':', ref strkey, ref strvalue );
								if ( strkey == "Tags" )
								{
									float fv = 0;
									float.TryParse( strvalue, out fv );
									mMatchTime = fv;
								}
								else if ( strkey == "Source" )
								{
									//for show time;
									string[] ShowTomes = strvalue.Split( ',' );
									mShowtimeRound = new int[ShowTomes.Length];
									for ( int i = 0; i < ShowTomes.Length; i++ )
									{
										int nv = 0;
										int.TryParse( ShowTomes[i], out nv );
										mShowtimeRound[i] = nv;
									}
								}
							}
							else if ( Section == EOSU_SECTION.EOSU_SECTION_DIFFICULTY )
							{
								SeparateString( stringLine, ':', ref strkey, ref strvalue );
							}
						}
					}
				}

				sr.Close();
			}
		}

		bool GetSection(string strLine, ref EOSU_SECTION Section)
		{
			if ( strLine.Length >= 2 && strLine[0] == '[' && strLine[strLine.Length - 1] == ']' )
			{
				Section = EOSU_SECTION.EOSU_SECTION_NON;
				string v = strLine.Replace( "[", "" );
				v = v.Replace( "]", "" );

				if ( v == "HitObjects" )
				{
					Section = EOSU_SECTION.EOSU_SECTION_HITOBJECTS;
				}
				else if ( v == "Metadata" )
				{
					Section = EOSU_SECTION.EOSU_SECTION_METADATA;
				}
				else if ( v == "TimingPoints" )
				{
					Section = EOSU_SECTION.EOSU_SECTION_TIMEPOINT;
				}
				else if ( v == "Difficulty" )
				{
					Section = EOSU_SECTION.EOSU_SECTION_DIFFICULTY;
				}
				return true;
			}
			return false;
		}

		void AddOperatorToList()
		{
			m_nCurrentRoundOperatorCount++;
		}

		void AddNewOperator(string strLine)
		{
			string[] splitResult = strLine.Split( ',' );
			if ( splitResult.Length == 6 )
			{
				//click
				AddOperatorToList();
			}
			else if ( splitResult.Length == 7 )
			{
				// Round end & Round Begin count;
				AddOperatorToList();

				m_nRoundCount++;
				m_nCurrentRoundOperatorCount = 0;
			}
			else if ( splitResult.Length == 8 )
			{
				string[] subsplitResult = splitResult[5].Split( '|' );
				if ( subsplitResult[0] == "L" )
				{
					if ( subsplitResult.Length == 2 )
					{
						AddOperatorToList();
					}
				}
				else if ( subsplitResult[0] == "B" )
				{
					AddOperatorToList();
				}
			}

            if (splitResult.Length >= 6 && splitResult[3] != "12")
                m_nAllOperCount++;
		}

        public int MaxScore()
        {
            int ret = 0;
            int combo = 0;

            for (int i = 0; i < m_nAllOperCount; ++i)
            {
                combo++;
                //calu mark;
                int x = combo / 10;
                if (x > 10)
                    x = 10;

                int thisScore = mBaseMark[x, (int)BeatResultRank.Perfect - (int)BeatResultRank.Good];

                ret += thisScore;
            }

            return ret;
        }

	}
}

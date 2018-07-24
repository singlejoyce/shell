using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StageTester
{
	public enum StageTag
	{
		None,

		Round,
		PatStart,
		PatEnd,
	};


    public enum BeatResultRank
    {
        None,

        Miss,
        Bad,
        Good,
        Cool,
        Perfect,

        Max,
    }

	public class StageInfo_Base
	{
		public bool m_bInvalidFlag = false;

		public virtual void LoadStageInfo(string stagePath, EStageLevel stageLevel) {}

		public bool ContainsFlag(string srcString, char[] arFlag)
		{
			string[] arSplit = srcString.Split( arFlag );
			if ( arSplit.Length > 1 )
			{
				return true;
			}

			return false;
		}

		public void SeparateString(string srcString, char flag, ref string firstPart, ref string secondPart)
		{
			if ( srcString != null && srcString.Length > 0 )
			{
				string[] splitResult = srcString.Split( flag );
				if ( splitResult.Length >= 2 )
				{
					firstPart = splitResult[0];
					secondPart = splitResult[1];
				}
			}
		}

		public void SeparateString(string srcString, char flag, ref string firstPart, ref string secondPart, ref string thirdPart)
		{
			if ( srcString != null && srcString.Length > 0 )
			{
				string[] splitResult = srcString.Split( flag );
				if ( splitResult.Length >= 3 )
				{
					firstPart = splitResult[0];
					secondPart = splitResult[1];
					thirdPart = splitResult[2];
				}
			}
		}

		public bool BeginWithNum(string srcString)
		{
			if ( srcString != null && srcString.Length > 0 )
			{
				char ch = srcString[0];
				if ( ch >= '0' && ch <= '9' )
				{
					return true;
				}
			}

			return false;
		}

		public bool BeginWithFlag(string srcString, char flag)
		{
			if ( srcString != null && srcString.Length > 0 && srcString[0] == flag )
			{
				return true;
			}

			return false;
		}

		public StageTag AnalyseTag(ref string srcString, char flag)
		{
			if ( BeginWithFlag( srcString, flag ) )
			{
				srcString = srcString.Substring( 1, srcString.Length - 1 );
				if ( BeginWithNum( srcString ) )
				{
					return StageTag.Round;
				}
				else
				{
					srcString.ToUpper();
					if ( srcString.CompareTo( "PATSTART" ) == 0 )
					{
						return StageTag.PatStart;
					}
					else if ( srcString.CompareTo( "PATEND" ) == 0 )
					{
						return StageTag.PatEnd;
					}
				}
			}

			return StageTag.None;
		}

		public bool IsComment(string srcString)
		{
			if ( srcString.Length >= 2 && srcString[0] == '/' && srcString[1] == '/' )
			{
				return true;
			}
			return false;
		}

//        public int MaxScore() { return 0;  }
	}
}

using System;
using System.IO;

namespace StageTester
{
	public enum LogLevel
	{
		WARNING,
		ERROR,
		SYSTEM,
	}

	public class DebugLog
	{
		public static string LogPath
		{
			get
			{
				return "StageTester.log";
			}
		}

		public static void Write(string strLog, LogLevel logLv)
		{
			using ( StreamWriter swWriter = new StreamWriter( LogPath, true ) )
			{
				swWriter.WriteLine( "[" + logLv.ToString() + " " + DateTime.Now.ToString() + "]" + strLog );
				swWriter.Close();
			}
		}
	}

    public class DebugExcel
    {
        public static string ExcelPath
        {
            get
            {
                return "AllMaxScore.xls";
            }
        }

        public static void Write(string strLog, LogLevel logLv)
        {
            using (StreamWriter swWriter = new StreamWriter(ExcelPath, true))
            {
                swWriter.WriteLine(strLog);
                swWriter.Close();
            }
        }
    }
}

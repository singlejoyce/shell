using System.Collections;


public enum AuditionBallType
{
	None = 0,
	Up,
	Down,
	Left ,
	Right,
	End,
}

// Define the result of ball range 
public enum AuditionBallRange
{
	Right = -1,
	Range = 0,
	Left = 1,
}

public enum AuditionBeatType
{
	Up,
	Down,
	Left,
	Right,
	Space,
}

// Class define about ball type 
public class CAuditionBallType
{
	public static bool IsValid(AuditionBallType nBallType)
	{
		if (nBallType > AuditionBallType.None && nBallType < AuditionBallType.End)
		{
			return true;
		}
		return false;
	}

	public static AuditionBallType GetBallType(int index)
	{
		if (index == 1 || index == 2)
			return AuditionBallType.Down;
		else if (index == 3 || index == 4)
			return AuditionBallType.Left;
		else if (index == 6 || index == 7)
			return AuditionBallType.Right;
		else if (index == 8 || index == 9)
			return AuditionBallType.Up;

		return AuditionBallType.None;
	}
}

public class CAuditionMode
{
	public static float BallDiameter = 47f;
}

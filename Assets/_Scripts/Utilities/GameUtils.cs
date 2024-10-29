using System.Globalization;
using System.Linq;

public static class GameUtils
{
	public static bool IsOutdoorScene(GameEnums.Scene scene)
	{
		GameEnums.Scene[] outdoorScenes = { GameEnums.Scene.Lake, GameEnums.Scene.Forest };
		return outdoorScenes.Contains(scene);
	}

	public static string FormatNumberString(int num)
	{
		if (num > 999999999)
		{
			return num.ToString("0,,,.###B");
		}
		else if (num > 999999)
		{
			return num.ToString("0,,.##M");
		}
		else if (num > 999)
		{
			return num.ToString("0,.#K");
		}

		return num.ToString();
	}
}
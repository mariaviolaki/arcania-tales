using System.Linq;

public static class GameUtils
{
	public static bool IsOutdoorScene(GameEnums.Scene scene)
	{
		GameEnums.Scene[] outdoorScenes = { GameEnums.Scene.Lake, GameEnums.Scene.Forest };
		return outdoorScenes.Contains(scene);
	}
}
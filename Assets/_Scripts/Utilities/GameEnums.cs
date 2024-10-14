public static class GameEnums
{
	public enum BodyPart
	{
		None, Body, Hair, Top, Bottom
	}

	public enum MoveDirection
	{
		None, Horizontal, Vertical
	}

	public enum LastMoveDirection
	{
		None, LastHorizontal, LastVertical
	}

	public enum Season
	{
		Spring = 1, Summer, Fall, Winter
	}

	public enum WeekDay
	{
		Monday = 1, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
	}

	public enum WeekDayShort
	{
		Mon = 1, Tue, Wed, Thu, Fri, Sat, Sun
	}

	public enum ItemType
	{
		None, Mushroom, Pumpkin
	}

	public enum Scene
	{
		None, Lake, Forest
	}

	public enum Direction
	{
		Up, Down, Left, Right
	}

	public enum NpcExpression
	{
		Neutral, Happy, Angry
	}

	public enum UIState
	{
		None, ItemSelection, Toolbar, Inventory, Storage, Dialogue
	}
}